using System;
using UnityEngine;

public class GhostControllers : Singleton<GhostControllers> {
	public Action<ControllerPair> onUpdate;
	public Controllers controllers;

	public ControllerPair current;
	private static readonly int idleAnimID = Animator.StringToHash("idle");

	private Transform moveRoot, rotatingRoot;

	protected override void Awake() {
		base.Awake();
		current = controllers.oculus;
		HMDVarietyHandler.instance.OnRightControllerDefined += UpdateControllers;
		HMDVarietyHandler.instance.OnLeftControllerDefined += UpdateControllers;
	}

	private void Start() {
		UpdateControllers();
		SetControllersAnimated(idleAnimID);
		rotatingRoot = transform.parent;
		moveRoot = rotatingRoot.parent;
	}

	public void UpdateControllers() {
		ControllerPair newpair = current;

		switch (HMDVarietyHandler.right) {
			case ControllerType.ViveController:
				newpair = controllers.vive;
				break;
			case ControllerType.OculusController:
				newpair = controllers.oculus;
				break;
			case ControllerType.ViveCosmosController:
				newpair = controllers.oculus;
				break;
			case ControllerType.Knuckles:
				newpair = controllers.knuckles;
				break;
			case ControllerType.WMRController:
				newpair = controllers.wmr;
				break;
		}

		HandType active = HandType.None;

		active |= current.left.gameobject.activeSelf ? HandType.Left : HandType.None;
		active |= current.right.gameobject.activeSelf ? HandType.Right : HandType.None;

		Activate(HandType.None);
		current = newpair;
		Activate(active);

		onUpdate?.Invoke(current);
	}


	public void Activate(HandType _handType = HandType.Any) {
		current.left.gameobject.SetActive((_handType & HandType.Left) != HandType.None);
		current.right.gameobject.SetActive((_handType & HandType.Right) != HandType.None);
	}

	public void SetButtonAnimation(int _animationTriggerID, HandType _handType = HandType.Any) {
		if ((_handType & HandType.Left) != HandType.None)
			current.left.animator.SetTrigger(_animationTriggerID);
		if ((_handType & HandType.Right) != HandType.None)
			current.right.animator.SetTrigger(_animationTriggerID);
	}


	public void SetControllersFollow(bool _state = true) {
		controllers.leftPoser.enabled = _state;
		controllers.rightPoser.enabled = _state;
		controllers.leftAnim.enabled = !_state;
		controllers.rightAnim.enabled = !_state;
	}

	public void SetControllersAnimated(int _animationTriggerID) {
		SetControllersFollow(false);
		controllers.leftAnim.SetTrigger(_animationTriggerID);
		controllers.rightAnim.SetTrigger(_animationTriggerID);
	}


	public void SetUnderRorationRoot() {
		transform.parent = rotatingRoot;
		transform.LocalReset();
	}

	public void SetUnderMoveRoot() {
		transform.parent = moveRoot;
		transform.localPosition = Vector3.zero;
	}

	public void SetUnderNullRoot() {
		transform.parent = null;
	}


	public void Disable() {
		Activate(HandType.None);
		controllers.leftAnimScript.ClipActionReset();
		controllers.rightAnimScript.ClipActionReset();
	}

	private void OnValidate() {
		controllers.OnValidate();
	}
}


[Serializable]
public struct Controllers {
	public Transform leftRoot;
	public Transform rightRoot;
	[DisableEditing]
	public LateCopyPosRot leftPoser, rightPoser;

	[DisableEditing]
	public Animator leftAnim, rightAnim;
	[DisableEditing]
	public GhostControllerActionButton leftAnimScript, rightAnimScript;


	public ControllerPair wmr;
	public ControllerPair vive;
	public ControllerPair oculus;
	public ControllerPair knuckles;


	public void OnValidate() {
		if (leftRoot) {
			leftPoser = leftRoot.GetComponent<LateCopyPosRot>();
			leftAnim = leftRoot.GetComponent<Animator>();
			leftAnimScript = leftRoot.GetComponent<GhostControllerActionButton>();
		}

		if (rightRoot) {
			rightPoser = rightRoot.GetComponent<LateCopyPosRot>();
			rightAnim = rightRoot.GetComponent<Animator>();
			rightAnimScript = rightRoot.GetComponent<GhostControllerActionButton>();
		}


		wmr.OnValidate();
		vive.OnValidate();
		oculus.OnValidate();
		knuckles.OnValidate();
	}
}

[Serializable]
public struct ControllerPair {
	public GhostController left;
	public GhostController right;

	public void SetAnimationBool(int _animationBoolID, bool _state, HandType _handType = HandType.Any) {
		if ((_handType & HandType.Left) != HandType.None)
			left.animator.SetBool(_animationBoolID, _state);
		if ((_handType & HandType.Right) != HandType.None)
			right.animator.SetBool(_animationBoolID, _state);
	}

	public void OnValidate() {
		left.OnValidate();
		right.OnValidate();
	}
}


[Serializable]
public struct GhostController {
	public GameObject gameobject;
	public Animator animator;

	public void OnValidate() {
		if (gameobject) {
			animator = gameobject.GetComponent<Animator>();
			Debug.Assert(animator, $"no animator attached to {gameobject}", gameobject);
		}
	}
}