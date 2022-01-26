using UnityEngine;

public class ClimbAbility : HandSplitAbility<ClimbDetector> {
	private CharacterControllerExtention characterControllerExtention;
	private VelocityCounter leftHandVelocity, rightHandVelocity;

	private bool
		wasClimbing,
		isGrabbed,
		isTouching,
		isClimbing;

	private Vector3 sumVelocity;
	private Rigidbody rigidBody;
	private Transform head;

	//public float shiftMultiplier = 10f;

	private MoveAbilityBase moveAbility;


	protected override void InitBehaviour() {
		base.InitBehaviour();
		characterControllerExtention = Player.instance.movable.GetComponent<CharacterControllerExtention>();
		leftHandVelocity = Player.instance.leftHandT.GetComponent<VelocityCounter>();
		rightHandVelocity = Player.instance.rightHandT.GetComponent<VelocityCounter>();
		head = Player.instance.head;
		rigidBody = Player.instance.movable.GetComponent<Rigidbody>();
		moveAbility = container.GetEntryOfType<MoveAbilityBase>();
	}

	private void OnEnable() {
		wasClimbing = false;
	}

	public void UpdateClimbState() {
		isGrabbed = left.casting | right.casting;
		isTouching = (left.isTouching || right.isTouching) && !characterControllerExtention.isGroundClose;
		isClimbing = isGrabbed | isTouching;

		if (isClimbing ^ wasClimbing) {
			characterControllerExtention.SetClimb(isClimbing);
			wasClimbing = isClimbing;

			if (!isClimbing) {
				rigidBody.velocity = rigidBody.velocity.normalized * Mathf.Clamp(rigidBody.velocity.magnitude * ((ClimbAbilityAsset)asset).dashVelocityMultiplier, 0, ((ClimbAbilityAsset)asset).dashVelocityMax);
				moveAbility.blockSources.Remove(this);
			} else {
				moveAbility.blockSources.Add(this);
			}
		}
	}

	private void FixedUpdate() {
		UpdateClimbState();
		if (isGrabbed) {
			sumVelocity = Vector3.zero;


			if (left.casting) {
				// PosRot sp = left.snapPoint;
				// sp.rot = leftHandVelocity.transform.rotation;
				//
				// sumVelocity += sp.GetGlobalOfChild(left.handOffset).pos - leftHandVelocity.transform.position;
				//
				//
				sumVelocity += left.snapPoint.pos - leftHandVelocity.transform.position;
			}

			if (right.casting) {
				// PosRot sp = right.snapPoint;
				// sp.rot = rightHandVelocity.transform.rotation;
				//
				// sumVelocity += sp.GetGlobalOfChild(right.handOffset).pos - rightHandVelocity.transform.position;
				//
				//
				sumVelocity += right.snapPoint.pos - rightHandVelocity.transform.position;
			}


			sumVelocity /= Time.fixedDeltaTime * 2f;
			//sumVelocity *= shiftMultiplier;
			rigidBody.velocity = sumVelocity;
		} else if (isTouching && !blocked) {
			sumVelocity = Vector3.zero;
			sumVelocity.y = rigidBody.velocity.y;

			if (left.touchers.Count > 0) {
				if ((left.snapPoint.pos - left.frictioner.position).sqrMagnitude > .3f) {
					//Debug.LogError($"Too far for climbing: {leftHandVelocity.transform.position} to {left.point}");
					// left.snapPoint.pos = left.frictioner.position;
				} else {
					Vector3 handShift = left.snapPoint.pos - left.frictioner.position;
					handShift.y = 0f;
					//sumVelocity += handShift * shiftMultiplier;
					sumVelocity += handShift / Time.fixedDeltaTime * .5f;
				}
			}

			if (right.touchers.Count > 0) {
				if ((right.snapPoint.pos - right.frictioner.position).sqrMagnitude > .3f) {
					//Debug.LogError($"Too far for climbing: {right.frictioner.position} to {right.point}");
					// right.snapPoint.pos = right.frictioner.position;
				} else {
					Vector3 handShift = right.snapPoint.pos - right.frictioner.position;
					handShift.y = 0f;
					//sumVelocity += handShift * shiftMultiplier;
					sumVelocity += handShift / Time.fixedDeltaTime * .5f;
				}
			}

			rigidBody.velocity = sumVelocity;
		}
	}

	public override void ResetAbility() { }
}