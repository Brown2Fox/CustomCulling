using System;
using System.Collections;
using JoyWay.Systems.InputSystem;
using UnityEngine;

[AbilityType(AbilityType.Grab)]
public class GrabHandPart : AbilityHandPart<GrabAbility> {
	public TriggerPool triggerPool;
	public Transform grabRoot;
	public VelocityCounter velocityCounter;

	private ConfiguratableJointsFields handJoint;

	// TODO: Remove telekinesis or not?)
	public TelekinesisHandPart telekinesis;

	[DisableEditing]
	public Label curTarget;

	private SimpleGrabbable curGrabbable, lastGrabbableToTry;
	public bool isFree => curGrabbable == null;

	private ConfigurableJoint joint;
	private Coroutine resetJoint;
	//TODO
	//заменить это
	private GameObject curGrabbedObject;
	private Owner curGrabOwner;
	private GrabType curGrabType;


	public AudioSource aSource;
	public AudioAsset grabClips;

	public Action<GameObject, GrabHandPart> onGrab, onDrop;

	public GrabHandPart() {
		AbilityTriggerID = Animator.StringToHash("Grab");
		AbilityEndTriggerID = Animator.StringToHash("GrabEnded");
	}

	private void OnEnable() {
		triggerPool.SetCondition(PoolCondition);

		triggerPool.OnActiveTargetHasChanged += ChangeTarget;
		triggerPool.gameObject.SetActive(true);
	}

	private bool PoolCondition(Label _label) {
		SimpleGrabbable sg = _label.GetComponent<SimpleGrabbable>();
		if (!sg) return false;
		return sg.CanBeGrabed(this);
		//return _label.GetComponent<IGrabbable>().CanGrab(this);
	}

	private void OnDisable() {
		TryDrop();
		ChangeTarget(null);
		triggerPool.gameObject.SetActive(false);
		triggerPool.OnActiveTargetHasChanged -= ChangeTarget;
	}

	public override void Init(HandSplitAbility _ability) {
		base.Init(_ability);
		handJoint = hand.GetComponent<ConfigurableJoint>().MakeCopy();
	}

	public override void StartCast() {
		base.StartCast();
		TryGrab();
	}

	protected override bool CastCheck() {
		return false;
	}

	protected override void SuccessCast() {
		Debug.LogError("Not Implemented");
	}

	private bool longholding;

	public override void EndCast() {
		if (GameMain.settings.serializable.input.longHolds && !longholding) {
			longholding = true;
		} else {
			base.EndCast();
			longholding = false;
			TryDrop();
		}
	}

	public void ForceGrab(SimpleGrabbable _grabbable) {
		base.StartCast();
		if (curGrabbable) Drop();
		Grab(_grabbable);
	}


	public void TryGrab() {
		if (curGrabbable) return;

		if (curTarget) {
			lastGrabbableToTry = curTarget.GetComponent<SimpleGrabbable>();

			if (lastGrabbableToTry.CanBeGrabed(this))
				Grab();
		} else if (triggerPool.colliders.Count > 0) {
			StartPredict();
		}
	}


	public override void StartPredict() {
		base.StartPredict();
		StartCoroutine(Prediction());
	}


	public IEnumerator Prediction() {
		predictionTimeLeft = 1f;
		while (predictionTimeLeft > 0) {
			predictionTimeLeft -= Time.deltaTime;

			if (curTarget) {
				PredictionComplete();
				break;
			}

			yield return null;
		}

		StopPredict();
	}


	public void TryDrop(bool _forced = false) {
		if (curGrabbable != null && (_forced || curGrabbable.TryDrop(this)))
			Drop();
	}

	private void Grab(SimpleGrabbable _grabbable = null) {
		onNoReturn?.Invoke(this);
		if (_grabbable) {
			lastGrabbableToTry = _grabbable;
		}

// Debug.Log("grab");
		curGrabType = lastGrabbableToTry.grabType;
		curGrabbedObject = lastGrabbableToTry.GetObject();
		switch (curGrabType) {
			case GrabType.Parent:
				Reparent(true);
				break;
			case GrabType.Joint:
				JointSetUp(true);
				HandCopyPosSetUp(true);
				break;
			case GrabType.Transform:
				grabbingByTransform = true;
				transform.Global().GetLocalOfChild(lastGrabbableToTry.Offset + lastGrabbableToTry.transform.Global());
				break;
			case GrabType.JointHandle:
				JointSetUp(true);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		//curGrabOwner = curGrabbedObject.GetComponent<Owner>();
		//if (curGrabOwner != null) {
		//	curGrabOwner.RemoveOwner();
		//	curGrabOwner.SetOwner(hand.owner);
		//}

		curGrabbable = lastGrabbableToTry;
		curGrabbable.Grab(this);
		curGrabbable.AddBlocker(this);
		onGrab?.Invoke(curGrabbedObject, this);

		if (curGrabbable.muteOne) curGrabbable.muteOne = false;
		else aSource.PlayOneShot((curGrabbable.grabSound) ? curGrabbable.grabSound : grabClips);
		
		InputSystem.AddAbilityListener(InputAbilityNames.GrabApply, ActivateHoldedObject, hand.type);
		InputSystem.AddAbilityListener(InputAbilityNames.GrabApply, ActivateHoldedObject, hand.type);
		var grabposer = curGrabbedObject.GetComponent<IGrabPose>();
		if (grabposer != null) {
			hand.SetAnimationTrigger(grabposer.GetPoseID());
		}
	}

	private static readonly int GrabSquezee = Animator.StringToHash("GrabSquezee");

	private bool activating;

	public void ActivateHoldedObject(bool _state) {
		if (_state) {
			IActivatable[] activatables = curGrabbedObject.GetComponentsInChildren<IActivatable>();
			foreach (IActivatable activatable in activatables) {
				if (activatable is ILongActivatable Ilong) {
					Ilong.StartActivation();
					hand.animator.SetBool(GrabSquezee, true);
				} else
					activatable.Activate();
			}
		} else {
			ILongActivatable[] activatables = curGrabbedObject.GetComponentsInChildren<ILongActivatable>();
			foreach (ILongActivatable activatable in activatables)
				activatable.Interrupt();


			hand.animator.SetBool(GrabSquezee, false);
		}

		activating = _state;
	}


	private void Drop() {
// Debug.Log("drop");
		switch (curGrabType) {
			case GrabType.Parent:
				Reparent(false);
				break;
			case GrabType.Joint:
				JointSetUp(false);
				HandCopyPosSetUp(false);
				break;
			case GrabType.Transform:
				grabbingByTransform = false;
				offset = transform.Global().GetLocalOfChild(lastGrabbableToTry.Offset + lastGrabbableToTry.transform.Global());
				break;
			case GrabType.JointHandle:
				JointSetUp(false);
				curTarget = null;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		if (activating)
			ActivateHoldedObject(false);
		curGrabbable.RemoveBlocker(this);
		curGrabbable.Drop();
		curGrabbable = null;
		if (telekinesis)
			telekinesis.enabled = curTarget == null;

		onDrop?.Invoke(curGrabbedObject, this);
		curGrabbedObject = null;
		InputSystem.RemoveAbilityListener(InputAbilityNames.GrabApply, ActivateHoldedObject, hand.type);
	}

// TODO: Добавить смещения (можно взять PosRot от Ирека)
	private void Reparent(bool _state) {
		ParentContainer pc = curGrabbedObject.GetComponent<ParentContainer>();
		if (_state) {
			if (!pc)
				curGrabbedObject.AddComponent<ParentContainer>();
			curGrabbedObject.transform.parent = grabRoot;
			curGrabbedObject.transform.CopyLocalPosRot(lastGrabbableToTry.Offset);
			curGrabbedObject.layer = (int) Layer.GrabbedObject;
		} else {
			if (pc != null)
				pc.ReturnToParent();
			else
				curGrabbedObject.transform.parent = DefaultSceneParent.lastParent;
			curGrabbedObject.layer = (int) Layer.Default;
		}
	}

	public void JointSetUp(bool _state) {
		if (_state) {
			ParentContainer pc = curGrabbedObject.GetComponent<ParentContainer>();
			if (!pc) {
				pc = curGrabbedObject.AddComponent<ParentContainer>();
				pc.ReSetParent();
			}

			curGrabbedObject.transform.parent = Player.instance.trackingSpace;
			curGrabbedObject.layer = (int) Layer.GrabbedObject;


			lastGrabbableToTry.transform.rotation = handJoint.connectedBody.rotation * Quaternion.Inverse(lastGrabbableToTry.Offset.localRotation) * hand.offset.currentOffset.rot;
			joint = curGrabbedObject.AddComponent<ConfigurableJoint>();
			joint.Copy(handJoint);
			joint.connectedAnchor = Vector3.zero;
			Vector3 grabAnchor = lastGrabbableToTry.offset.localPosition;
			if (hand.type == HandType.Left)
				grabAnchor.x = -grabAnchor.x;
			joint.anchor = grabAnchor;
			resetJoint = StartCoroutine(ResetJoint());
			//PosRot handPosRot = new PosRot() { pos = joint.connectedBody.transform.position, rot = joint.connectedBody.transform.rotation };
			//PosRot gunsPosRot = new PosRot() { pos = lastGrabbableToTry.offset.position, rot = lastGrabbableToTry.offset.rotation };

			//joint.targetRotation = Quaternion.Inverse(Quaternion.Inverse(lastGrabbableToTry.transform.rotation) * Quaternion.Inverse(lastGrabbableToTry.offset.rotation) * joint.connectedBody.transform.rotation);

			//joint.targetRotation = Quaternion.FromToRotation(lastGrabbableToTry.offset.forward, joint.connectedBody.transform.forward);

			//joint.connectedBody = hand.GetComponent<Rigidbody>();

			//JointDrive drive = new JointDrive {
			//	positionSpring = 10000f,
			//	positionDamper = 200f,
			//	maximumForce = float.MaxValue
			//};

			//joint.xDrive = drive;
			//joint.yDrive = drive;
			//joint.zDrive = drive;

			//joint.rotationDriveMode = RotationDriveMode.Slerp;
			//drive.positionSpring = 4000f;
			//drive.positionDamper = 100f;
			//drive.maximumForce = float.MaxValue;

			//joint.slerpDrive = drive;
			//joint.connectedMassScale = 1000f;

			////setting grabbable
			//offset = transform.Global().GetGlobalOfChild(lastGrabbableToTry.offset);
			//curGrabbedObject.transform.CopyGlobalPosRot(offset);
			//ConfigurableJoint joint = curGrabbedObject.AddComponent<ConfigurableJoint>();
			//joint.connectedBody = hand.GetComponent<ConfigurableJoint>().connectedBody;
			//joint.xMotion = ConfigurableJointMotion.Limited;
			//joint.yMotion = ConfigurableJointMotion.Limited;
			//joint.zMotion = ConfigurableJointMotion.Limited;
			//JointDrive drive = new JointDrive();
			//drive.positionSpring = 10000f;
			//drive.positionDamper = 200f;
			//drive.maximumForce = float.MaxValue;
			//joint.xDrive = drive;
			//joint.yDrive = drive;
			//joint.zDrive = drive;
			//joint.rotationDriveMode = RotationDriveMode.Slerp;
			//drive.positionSpring = 4000f;
			//drive.positionDamper = 100f;
			//drive.maximumForce = float.MaxValue;
			//joint.slerpDrive = drive;
			//joint.connectedMassScale = lastGrabbableToTry.rb.mass;
		} else {
			ParentContainer pc = curGrabbedObject.GetComponent<ParentContainer>();
			if (pc != null)
				pc.ReturnToParent();
			else
				curGrabbedObject.transform.parent = DefaultSceneParent.lastParent;
			curGrabbedObject.layer = (int) Layer.Default;
			if (joint)
				Destroy(joint);
			if (resetJoint != null)
				StopCoroutine(resetJoint);
		}
	}

	private IEnumerator ResetJoint() {
		yield return new WaitForFixedUpdate();
		if (joint)
			Destroy(joint);
		lastGrabbableToTry.transform.rotation = handJoint.connectedBody.rotation * Quaternion.Inverse(lastGrabbableToTry.Offset.localRotation);
		joint = curGrabbedObject.AddComponent<ConfigurableJoint>();
		joint.Copy(handJoint);
		joint.connectedAnchor = Vector3.zero;
		Vector3 grabAnchor = curGrabbable.offset.localPosition;
		if (hand.type == HandType.Left)
			grabAnchor.x = -grabAnchor.x;
		joint.anchor = grabAnchor;
	}

	public void HandCopyPosSetUp(bool _state) {
		if (_state) {
			//setting hand
			Destroy(hand.GetComponent<ConfigurableJoint>());
			Rigidbody rb = hand.GetComponent<Rigidbody>();
			rb.isKinematic = true;
			rb.detectCollisions = false;
			LateCopyPosRot lateCopy = hand.gameObject.AddComponent<LateCopyPosRot>();

			Vector3 offset = hand.offset.currentOffset.pos;
			offset = new Vector3((hand.type == HandType.Left ? 1 : 0) * offset.x, offset.y, offset.z);

			if (hand.type == HandType.Left) {
				Vector3 leftSocket = lastGrabbableToTry.Offset.localPosition;
				leftSocket.x = -leftSocket.x;
				lastGrabbableToTry.Offset.localPosition = leftSocket;
			}

			lastGrabbableToTry.Offset.localPosition -= offset;
			lastGrabbableToTry.Offset.localRotation *= Quaternion.Inverse(hand.offset.currentOffset.rot);

			lateCopy.target = lastGrabbableToTry.Offset;
			lateCopy.smooth = float.PositiveInfinity;
			//setting parkur capsule
			//hand.parkurCapsule.radius *= 4;
		} else {
			Vector3 offset = hand.offset.currentOffset.pos;
			offset = new Vector3((hand.type == HandType.Left ? 1 : 0) * offset.x, offset.y, offset.z);

			lastGrabbableToTry.Offset.localPosition += offset;
			if (hand.type == HandType.Left) {
				Vector3 leftSocket = lastGrabbableToTry.Offset.localPosition;
				leftSocket.x = -leftSocket.x;
				lastGrabbableToTry.Offset.localPosition = leftSocket;
			}

			lastGrabbableToTry.Offset.localRotation *= hand.offset.currentOffset.rot;

			ConfigurableJoint jointHandFollow = hand.gameObject.AddComponent<ConfigurableJoint>();
			jointHandFollow.Copy(handJoint);
			jointHandFollow.connectedAnchor = Vector3.zero;
			jointHandFollow.targetRotation = Quaternion.Inverse(jointHandFollow.connectedBody.transform.rotation) * jointHandFollow.transform.rotation;

			Rigidbody rb = jointHandFollow.GetComponent<Rigidbody>();
			rb.isKinematic = false;
			rb.detectCollisions = true;

			//hand.parkurCapsule.radius /= 4;

			Destroy(GetComponentInParent<LateCopyPosRot>());
		}
	}

	private bool grabbingByTransform;
	private PosRot offset;

	protected override void Update() {
		base.Update();
		if (grabbingByTransform)
			curGrabbable.transform.CopyGlobalPosRot(transform.Global().GetGlobalOfChild(offset));
	}

//private void OnDrawGizmos() {
//	PosRot posRot = new PosRot();
//	posRot.pos = rb.centerOfMass;
//	Gizmos.DrawSphere(transform.Global().GetGlobalOfChild(posRot).pos, 0.1f);
//}

	public void ChangeTarget(Label _label) {
		if (telekinesis)
			telekinesis.enabled = _label == null && curGrabbable == null;
		curTarget = _label;
	}

	protected override int PriorityModifiers() {
		if (triggerPool.CurrentTarger == null)
			return 0;
		else
			return 100;
	}
}


public struct GrabInfo {
	public SimpleGrabbable grabbable;
	public Owner grabbableOwner;
	public GameObject grabbableGO;
	public GrabType grabType;
	public Label label;
}