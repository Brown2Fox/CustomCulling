using JoyWay.Systems.InputSystem;
using UnityEngine;

public class PlayerMoveAbility : MoveAbilityBase {
	
	public CharacterControllerExtention characterControllerExtention;
	public Transform headCam;
	public Transform trackingSpace;

	public bool checkHands;
	public Transform leftParkurer, rightParkurer;
	private CapsuleCollider leftParkurerCapsule, rightParkurerCapsule;

	[HideInInspector]
	public float d_rad = -.005f, d_h = -.05f, d_s = .03f;

	public bool CantMove() {
		return blocked;
		//return (characterControllerExtention.isDashing > 0 || !characterControllerExtention.isGrounded && !characterControllerExtention.isFlying);
	}


	protected override void InitBehaviour() {
		base.InitBehaviour();
		SetWalkState(false);

		invMaxMin = 1f / (maxMagnitude - minMagnitude);
		characterControllerExtention.onSetFlight += SetWalkState;
		lastCamPosition = headCam.localPosition;

		leftParkurerCapsule = leftParkurer.GetComponent<CapsuleCollider>();
		rightParkurerCapsule = rightParkurer.GetComponent<CapsuleCollider>();
	}


	public float
		maxMagnitude = 0.6f,
		minMagnitude = 0.1f;
	public AnimationCurve magnitudeCurve;

	private float
		invMaxMin = 1f;


	private float curVelocityMagnitude;
	private Vector3
		inputVelocityVector,
		lastVelocityDerectionForRun,
		lastCamPosition;

	public override void CalculateCurrentSpeed() {
		if (!isWalkingState) {
			base.CalculateCurrentSpeed();
			return;
		}

		newVelocityVector += desiredDirection.normalized * currentVelocityParameters.acceleration * Time.deltaTime;
		currentVelocity = newVelocityVector.magnitude;

		Vector3 flatRBVelo = body.velocity;
		flatRBVelo.y = 0f;
		Vector3 sumVelocity = newVelocityVector + flatRBVelo;
		if (sumVelocity.magnitude > currentVelocityParameters.maxVelocity)
			currentVelocity = Mathf.Clamp(currentVelocity - sumVelocity.magnitude + currentVelocityParameters.maxVelocity, 0f, float.PositiveInfinity);

		//if (isWalkingState) {
		//	Vector3 revertDesiredDirection = newVelocityVector / currentVelocityParameters.maxVelocity;
		//	desiredDirection += revertDesiredDirection;
		//	desiredDirection.Normalize();
		//}

		//float desiredSpeed = Mathf.Clamp01(desiredDirection.magnitude) * currentVelocityParameters.maxVelocity;
		//if (blocked || desiredSpeed < currentVelocity)
		//	currentVelocity -= currentVelocityParameters.stopAcceleration * Time.fixedDeltaTime;
		//else {
		//	newVelocityVector += desiredDirection.normalized * currentVelocityParameters.acceleration * Time.fixedDeltaTime;
		//	currentVelocity = newVelocityVector.magnitude;
		//}

		//currentVelocity = Mathf.Clamp(currentVelocity, 0f, desiredSpeed);

	}

	protected override void FixedUpdate() {
		desiredDirection = Vector3.zero;
		if (!CantMove()) {

			// Вариант инертного передвижения по воздуху, реализованный через переопределение данных из инпутов

			//// изменяем "раскладку" на инерционную для полёта
			//if (isWalkingState) {
			//	//lastVelocityDerectionForRun = currentDirection / currentVelocityParameters.maxVelocity;
			//	Vector3 t_curVelocityDirection = new Vector3(InputSystem.moveVector.x, 0f, InputSystem.moveVector.y);

			//	curVelocityDirection = Vector3.Lerp(lastVelocityDerectionForRun + t_curVelocityDirection, t_curVelocityDirection, t_curVelocityDirection.magnitude);
			//	lastVelocityDerectionForRun = curVelocityDirection;

			//	//Debug.Log(
			//	//	$"({curVelocityDirection.x:0.00}, {curVelocityDirection.z:0.00} ({curVelocityDirection.magnitude:0.00})) = " +
			//	//	$"({lastVelocityDerectionForRun.x:0.00}, {lastVelocityDerectionForRun.z:0.00} ({lastVelocityDerectionForRun.magnitude:0.00})) - " +
			//	//	$"({t_curVelocityDirection.x:0.00}, {t_curVelocityDirection.z:0.00} ({t_curVelocityDirection.magnitude:0.00}))");
			//} else
			//	// и используем обычную для ходьбы
			
			inputVelocityVector = new Vector3(InputSystem.moveVector.x, 0f, InputSystem.moveVector.y);

			curVelocityMagnitude = inputVelocityVector.magnitude;

			if (curVelocityMagnitude > minMagnitude) {
				if (curVelocityMagnitude > maxMagnitude) curVelocityMagnitude = maxMagnitude;
				curVelocityMagnitude = magnitudeCurve.Evaluate((curVelocityMagnitude - minMagnitude) * invMaxMin);

				desiredDirection = headCam.TransformDirection(inputVelocityVector);
				desiredDirection.y = 0f;
				desiredDirection.Normalize();
				desiredDirection *= curVelocityMagnitude;
			}
		}

		base.FixedUpdate();

		//for physical move irl
		Vector3 camOffset = trackingSpace.TransformVector(headCam.localPosition - lastCamPosition);
		camOffset.y = 0;
		body.MovePosition(body.position + camOffset);
		lastCamPosition = headCam.localPosition;


		if (characterControllerExtention.isGrounded != grounded ||
			characterControllerExtention.isFlying && grounded) {
			characterControllerExtention.SetGrounded(grounded);
		}

		TrackingSpaceToMovable();
	}

	private void LateUpdate() {
		TrackingSpaceToMovable();
	}

	private void TrackingSpaceToMovable() {
		PosRot result;
		result.pos = trackingSpace.position - headCam.position;
		result.pos.y = 0;
		result.rot = Quaternion.identity;
		result = body.transform.Global() + result;

		trackingSpace.CopyGlobalPosRot(result);
	}


	protected override void DestroyBehaviour() {
		characterControllerExtention.onSetFlight -= SetWalkState;
	}

	// Проверяет возможность двигаться на _shift для тела и рук. Совершает минимально возможное движение. Убирает тряску.
	protected override void MakeTransition(Vector3 _shift) {
		Vector3
			bodyShift = CapsuleCaster.GetCastShift(true, body.transform, ccExtension.physicsCollider, _shift, (int)FlaggedLayer.Vision, .2f, d_rad, d_h, d_s),
			leftHandShift = CapsuleCaster.GetCastShift(false, leftParkurer.transform, leftParkurerCapsule, _shift, (int)FlaggedLayer.Vision, 0f, d_rad * .3f, d_rad * .6f, d_s),
			rightHandShift = CapsuleCaster.GetCastShift(false, rightParkurer.transform, rightParkurerCapsule, _shift, (int)FlaggedLayer.Vision, 0f, d_rad * .3f, d_rad * .6f, d_s),
			moveShift = Vector3.zero;

		if (!checkHands) {
			body.MovePosition(bodyShift + body.position);
			return;
		}

		// TODO: Проверять в соответствии с направлением _shift, а не просто на минимальную длину.
		if (bodyShift.sqrMagnitude < leftHandShift.sqrMagnitude)
			if (bodyShift.sqrMagnitude < rightHandShift.sqrMagnitude)
				moveShift = bodyShift;
			else
				moveShift = rightHandShift;
		else if (leftHandShift.sqrMagnitude < rightHandShift.sqrMagnitude)
			moveShift = leftHandShift;
		else
			moveShift = rightHandShift;
		body.MovePosition(moveShift + body.position);
		characterControllerExtention.SetMoveShift(moveShift);
	}

	public override void ResetAbility() { }
}
