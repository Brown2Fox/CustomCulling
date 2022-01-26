using System;
using JoyWay.Systems.InputSystem;
using UnityEngine;
using InputAction = UnityEngine.InputSystem.InputAction;

public class ThirdPersonController : MonoBehaviour, InputWrapper.IThirdPersonSwitcherActions {
	public static bool active;
	[SerializeField]
	private Animator animator;

	private void Start() {
		OpenXRInput.wrapper.ThirdPersonSwitcher.SetCallbacks(this);
	}


	public void ActivateThirdPerson(bool _state) {
		animator.enabled = _state;
		active = _state;
	}

	public void OnThirdPerson(InputAction.CallbackContext context) {
		if (context.performed)
			ActivateThirdPerson(true);
	}

	public void OnFirstPerson(InputAction.CallbackContext context) {
		if (context.performed)
			ActivateThirdPerson(false);
	}
}