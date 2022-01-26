using JoyWay.Systems.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInput : InputWrapper.IMenuInteractionsActions {
	public static readonly CustomInputAction Click = new CustomInputAction();

	public void OnUIClick(InputAction.CallbackContext _context) {
		OpenXRInput.PerformInputAction(Click, _context);
	}

	public void OnUIScroll(InputAction.CallbackContext _context) {
		Debug.Log("Scroll not implemented yet");
	}
}