using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace JoyWay.Systems.InputSystem {
	public class PlayerControlsInput : InputWrapper.IPlayerControlsActions {
		public void OnMove(InputAction.CallbackContext _context) {
			InputSystem.moveVector = _context.ReadValue<Vector2>();
		}


		public void OnFire(InputAction.CallbackContext _context) {
			OpenXRInput.PerformInputAction(InputSystem.Shoot, _context);
		}


		public void OnJump(InputAction.CallbackContext _context) {
			if (_context.performed)
				InputSystem.Jump.Trigger(true, HandType.Any);
		}

		public void OnRotateSmooth(InputAction.CallbackContext _context) {
			throw new System.NotImplementedException();
		}

		public void OnRotate(InputAction.CallbackContext _context) { }
	}
}