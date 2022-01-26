using System;
using UnityEngine;

public class GhostControllerActionButton : MonoBehaviour {
	[SerializeField]
	private HandType type = HandType.Any;

	private ControllerPair current;
	private int currentButtonID;

	private GhostControllers ghost;

	private void Awake() {
		ghost = GetComponentInParent<GhostControllers>();
		ghost.onUpdate += _pair => current = _pair;
	}

	public void SetCurrentButtonGrab() {
		currentButtonID = grabButtonBoolID;
	}

	public void SetCurrentButtonTrigger() {
		currentButtonID = triggerButtonBoolID;
	}

	public void SetCurrentButtonDash() {
		currentButtonID = dashButtonBoolID;
	}


	public void SetButtonOn() {
		current.SetAnimationBool(currentButtonID, true, type);
	}

	public void SetButtonOff() {
		current.SetAnimationBool(currentButtonID, false, type);
	}

	public void AnimationEnded() {
		onClipEnd?.Invoke();
	}

	public void AnimationStarted() {
		onClipStart?.Invoke();
	}

	public void ClipActionReset() {
		onClipEnd = default;
		onClipStart = default;
	}


	public Action onClipEnd, onClipStart;


	private static readonly int grabButtonBoolID = Animator.StringToHash("grabButtonBool");
	private static readonly int triggerButtonBoolID = Animator.StringToHash("triggerButtonBool");
	private static readonly int dashButtonBoolID = Animator.StringToHash("dashButtonBool");
}