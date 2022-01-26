using JoyWay.Systems.InputSystem;
using UnityEngine.InputSystem;

public class UtilityHandInputs : InputWrapper.IUtilityActions {
	public static readonly CustomInputAction PedestalItemActivation = new CustomInputAction();
	public static readonly CustomInputAction GemEquip = new CustomInputAction();
	public static readonly CustomInputAction TelekinesisApplyGemEffect = new CustomInputAction();

	public void OnItemActivation(InputAction.CallbackContext _context) {
		OpenXRInput.PerformInputAction(PedestalItemActivation, _context);
	}

	public void OnGemEquip(InputAction.CallbackContext _context) {
		OpenXRInput.PerformInputAction(GemEquip, _context);
	}

	public void OnTelekinesisApplyGemEffect(InputAction.CallbackContext _context) {
		OpenXRInput.PerformInputAction(TelekinesisApplyGemEffect, _context);
	}
}