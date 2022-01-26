using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JoyWay.Systems.InputSystem {
	[Obsolete]
	public class DesktopInput : MonoBehaviour {
		[SerializeField]
		private HandType mainHand = HandType.Right;
		private HandType offHand => mainHand ^ HandType.Any;

		public static InputWrapper wrapper;
		public static InputWrapper.PlayerControlsActions playerControls;


		private void Awake() {
			wrapper = new InputWrapper();
			wrapper.PlayerControls.Move.performed += OnMove;
			wrapper.Enable();

			wrapper.DebugKeys.LoadHub.performed += context => LevelManager.LoadHub();
			wrapper.MainInteractions.Pause.performed += context => PauseManager.instance.TogglePause(true);
		}


		private void OnDestroy() {
			wrapper.Disable();
		}


		private void Update() {
			InputSystem.moveVector += wrapper.PlayerControls.Move.ReadValue<Vector2>();
		}

		public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context) { }

		public void OnLook(UnityEngine.InputSystem.InputAction.CallbackContext context) { }

		public void OnFire(UnityEngine.InputSystem.InputAction.CallbackContext context) {
			switch (context.phase) {
				case InputActionPhase.Started:
					InputSystem.Shoot.Trigger(true, mainHand);
					break;
				case InputActionPhase.Canceled:
					InputSystem.Shoot.Trigger(false, mainHand);
					break;
			}
		}

		public void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context) {
			if (context.performed)
				InputSystem.Jump.Trigger(true, mainHand);
		}

		public void OnRotateSmooth(UnityEngine.InputSystem.InputAction.CallbackContext context) { }

		public void OnRotateRight(UnityEngine.InputSystem.InputAction.CallbackContext context) {
			if (context.performed)
				InputSystem.onTurnRight?.Invoke(true);
			if (context.canceled)
				InputSystem.onTurnRight?.Invoke(false);
		}

		public void OnRotateLeft(UnityEngine.InputSystem.InputAction.CallbackContext context) {
			if (context.performed)
				InputSystem.onTurnLeft?.Invoke(true);
			if (context.canceled)
				InputSystem.onTurnLeft?.Invoke(false);
		}
	}
}