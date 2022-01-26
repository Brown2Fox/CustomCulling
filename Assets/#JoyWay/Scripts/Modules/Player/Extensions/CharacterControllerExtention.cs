using System;
using JoyWay.Systems.InputSystem;
using UnityEngine;

public class CharacterControllerExtention : MonoBehaviour {
	public CapsuleCollider physicsCollider;
	public CapsuleCollider deleteMe;
	public Rigidbody rigidBody;


	private Transform groundedRoot;
	private Transform head;

	public bool
		isGrounded,
		isFlying,
		isGroundClose,
		inNearestGround;
	private bool
		isClimbing,
		isShort,
		wasShort,
		inGroundMove;

	public float
		capsulState = 1f,
		stepHeight = 0.25f,
		countStepSolution = 5f,
		timeReset = 0.2f;

	private float
		poseY,
		t,
		lastMoveHitPointY;

	public int
		isDashing;

	private Vector3
		groundedCenter,
		velocityComponent,
		sphereCast,
		sphereCast2;

	public Action<bool>
		onSetGrounded, onSetFlight, onSetClimb;


	public AudioAsset groundingSound;
	public AudioSource groundingSource;

	private void Awake() {
		groundedRoot = Player.instance.groundedRoot;
		head = Player.instance.head;
		health = GetComponentInParent<Health>();
		health.onDeath.AddListener(StopMoving);
	}

	private void Start() {
		InputSystem.Jump.AddListener(HandType.Any, Jump);
	}


	public void Jump(bool state) {
		if (state)
			rigidBody.AddForce(Vector3.up * (10 * rigidBody.mass), ForceMode.Impulse);
	}

	private void Update() {
		inNearestGround = HeightCheck();
		inGroundMove = MoveCheck();
		isGroundClose = HeightCheckClose();
		//InGameLogFormer.Log($"isClimbing = {isClimbing}", Color.red);
		//InGameLogFormer.Log($"isGrounded = {isGrounded}", Color.yellow);
		//InGameLogFormer.Log($"isGroundClose = {isGroundClose}", Color.green);
		//InGameLogFormer.Log($"willSphere = {isClimbing || !isGrounded && !isGroundClose}", Color.cyan);

		isShort = isClimbing || !isGrounded && !inNearestGround;
		if (isShort) {
			capsulState = 0;
			t = 0;
		}
		else if (wasShort) {
			if (Math.Abs(head.localPosition.y) > float.Epsilon)
				capsulState = (head.localPosition.y / 2 - physicsCollider.radius) / head.localPosition.y;
			else
				capsulState = 0;
			poseY = moveHitPoint.point.y;
			t = 0;
		}
		else if (inGroundMove) {
			if (Mathf.Abs(moveHitPoint.point.y - poseY) > stepHeight) {
				poseY = moveHitPoint.point.y;
			}
			else if (Mathf.Abs(moveHitPoint.point.y - poseY) > 0.001f) {
				if (Mathf.Abs(moveHitPoint.point.y - lastMoveHitPointY) > 0.001f || Vector3.Angle(moveHitPoint.normal, Vector3.up) > 5f) {
					capsulState = 1f;
					t = 1f;
				}
				else {
					sphereCast = CastSphere(head.position + moveShift, physicsCollider.radius, Vector3.down, 100);
					capsulState = (Mathf.Abs(sphereCast.y) + physicsCollider.radius) / head.localPosition.y;
					t = 0f;
					if (moveHitPoint.point.y > poseY) {
						poseY = moveHitPoint.point.y;
					}
					else {
						sphereCast2 = CastSphere(head.position, physicsCollider.radius, Vector3.down, 100);
						if (Mathf.Abs(sphereCast.y) > Mathf.Abs(sphereCast2.y))
							capsulState = (Mathf.Abs(sphereCast2.y) + physicsCollider.radius) / head.localPosition.y;
						else {
							capsulState = (Mathf.Abs(sphereCast.y) + physicsCollider.radius) / head.localPosition.y;
							poseY = moveHitPoint.point.y;
						}
					}
				}
			}
		}
		wasShort = isShort;
		float standH = head.localPosition.y * capsulState + physicsCollider.radius;
		if (inNearestGround) {
			t += Time.deltaTime;
		}
		float h = Mathf.Lerp(standH, head.localPosition.y + physicsCollider.radius, t / timeReset);

		physicsCollider.height = h;
		physicsCollider.center = Vector3.up * (head.localPosition.y + physicsCollider.radius - Mathf.Max(h * .5f, physicsCollider.radius));

		groundedCenter = physicsCollider.center;
		groundedCenter.y = 0f;
		groundedRoot.localPosition = groundedCenter;

		deleteMe.radius = physicsCollider.radius * .95f;
		deleteMe.height = physicsCollider.height * .95f;
		deleteMe.center = physicsCollider.center;
		lastMoveHitPointY = moveHitPoint.point.y;
	}

	private float ungroundStartTime;
	private float ungroundDelay = 0.5f;

	public void SetMoveShift(Vector3 moveShift) {
		this.moveShift = moveShift * countStepSolution;
	}

	public void SetGrounded(bool _state) {
		isGrounded = _state;
		if (_state) {
			SetFlight(false);
			if (ungroundStartTime + ungroundDelay < Time.time)
				groundingSource.PlayOneShot(groundingSound, .5f);
		}
		else {
			ungroundStartTime = Time.time;
		}

		onSetGrounded?.Invoke(_state);
	}

	public void SetFlight(bool _state) {
		if (_state != isFlying) {
			isFlying = _state;
			onSetFlight?.Invoke(_state);
		}
	}

	public void SetClimb(bool _state) {
		isClimbing = _state;
		if (_state)
			SetFlight(false);

		onSetClimb?.Invoke(_state);
		rigidBody.useGravity = !_state;
	}

	public void AddVelocityInCap(Vector3 _velocity) {
		velocityComponent = Vector3.Project(rigidBody.velocity, _velocity);
		if (Vector3.Angle(velocityComponent, _velocity) > 100f) {
			rigidBody.velocity += (_velocity - velocityComponent);
		}
		else {
			if (velocityComponent.magnitude < _velocity.magnitude)
				rigidBody.velocity += _velocity - velocityComponent;
		}
	}

	private RaycastHit hit;
	private bool HeightCheck() {
		//InGameLogFormer.Log(Physics.Raycast(head.position, Vector3.down, Mathf.Abs(head.localPosition.y) - 0.05f, (int)FlaggedLayer.Vision, QueryTriggerInteraction.Ignore).ToString(), Physics.Raycast(head.position, Vector3.down, Mathf.Abs(head.localPosition.y) - 0.05f, (int)FlaggedLayer.Vision, QueryTriggerInteraction.Ignore) ? Color.green : Color.red);
		return Physics.Raycast(head.position, Vector3.down, out hit, Mathf.Abs(head.localPosition.y) + stepHeight, (int)FlaggedLayer.Vision, QueryTriggerInteraction.Ignore);
	}
	private bool HeightCheckClose() {
		return Physics.Raycast(head.position, Vector3.down, out hit, Mathf.Abs(head.localPosition.y) - 0.05f, (int)FlaggedLayer.Vision, QueryTriggerInteraction.Ignore);
	}

	private Vector3 moveShift;
	private RaycastHit moveHitPoint;
	private bool MoveCheck() {
		return Physics.Raycast(head.position + moveShift, Vector3.down, out moveHitPoint, Mathf.Abs(head.localPosition.y) + stepHeight, (int)FlaggedLayer.Vision, QueryTriggerInteraction.Ignore);
	}

	private Vector3 CastSphere(Vector3 startPoint, float radius, Vector3 dir, float maxDist) {
		RaycastHit hitSphere;
		if (Physics.SphereCast(startPoint, radius, dir, out hitSphere, maxDist, (int)FlaggedLayer.Vision, QueryTriggerInteraction.Ignore)) {
			float b = Mathf.Sqrt(Mathf.Pow(startPoint.x - hitSphere.point.x, 2) + Mathf.Pow(startPoint.z - hitSphere.point.z, 2));
			float a = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(b, 2)) - radius;
			return dir * hitSphere.distance - dir * a;
		}
		else {
			return maxDist * dir - dir * radius;
		}
	}

	private Health health;
	private void StopMoving()
	{
		rigidBody.velocity = Vector3.zero;
	}
	
	private void OnDestroy()
	{
		health.onDeath.RemoveListener(StopMoving);
	}
}