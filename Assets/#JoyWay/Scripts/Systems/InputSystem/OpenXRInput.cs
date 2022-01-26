using UnityEngine;
using UnityEngine.InputSystem;
using InputDevice = UnityEngine.InputSystem.InputDevice;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using Debug = UnityEngine.Debug;
using Pose = UnityEngine.XR.OpenXR.Input.Pose;

namespace JoyWay.Systems.InputSystem {
	public class OpenXRInput : InputSystem {
		[SerializeField]
		private HandType mainHand = HandType.Right;
		private HandType OffHand => mainHand ^ HandType.Any;
		public static InputDevice left, right;

		public static InputWrapper wrapper;
		public static PlayerControlsInput PlayerControls;
		public static AbilityInput AbilityInput;
		public static UIInput UIInput;


		protected override void Awake() {
			base.Awake();

			UnityEngine.InputSystem.InputSystem.onDeviceChange += CheckNewDevice;
			SetUpDevices();

			wrapper = new InputWrapper();
			PlayerControls = new PlayerControlsInput();
			wrapper.PlayerControls.SetCallbacks(PlayerControls);
			AbilityInput = new AbilityInput();
			wrapper.Abilities.SetCallbacks(AbilityInput);
			UIInput = new UIInput();
			wrapper.MenuInteractions.SetCallbacks(UIInput);
			


			wrapper.MainInteractions.Pause.performed += _context => PauseManager.instance.TogglePause(true);


			wrapper.Enable();
		}

		private void CheckNewDevice(InputDevice _inputDevice, InputDeviceChange _inputDeviceChange) {
			if (_inputDeviceChange == InputDeviceChange.Added || _inputDeviceChange == InputDeviceChange.Removed) {
				if (_inputDevice is XRController) {
					Debug.Log(_inputDevice.GetType());
					SetUpDevices();
				}
			}
		}


		private void SetUpDevices() {
			var devices = UnityEngine.InputSystem.InputSystem.devices;

			foreach (InputDevice inputDevice in devices) {
				if (inputDevice is XRController device) {
					var capabilities = device.description.capabilities;
					var deviceDescriptor = XRDeviceDescriptor.FromJson(capabilities);

					if (deviceDescriptor != null) {
						if ((deviceDescriptor.characteristics & InputDeviceCharacteristics.Left) != 0) {
							left = device;
						} else if ((deviceDescriptor.characteristics & InputDeviceCharacteristics.Right) != 0) {
							right = device;
						}
					}
				}
			}
		}


		public static void PerformInputAction(CustomInputAction _action, InputAction.CallbackContext _context) {
			HandType hand;
			if (_context.control.device.IsLeft()) {
				hand = HandType.Left;
			} else {
				hand = HandType.Right;
			}

			bool state = _context.control.IsPressed(.5f);
			// switch (_context.phase) {
			// 	case InputActionPhase.Performed:
			// 		state = true;
			// 		break;
			// 	case InputActionPhase.Canceled:
			// 		state = false;
			// 		break;
			// 	// default:
			// 	// 	return;
			// }

			Debug.Log($"hand is {hand} and state is {state}\n" +
					  $"raw data is:\n" +
					  $"device - {_context.control.device}\n" +
					  $"phase - {_context.phase}");

			_action.Trigger(state, hand);
		}


		public override Vector3 GetHandPosition(HandType _hand) {
			if (_hand == HandType.Left)
				return wrapper.DeviceBase.LeftControllerPose.ReadValue<Pose>().position;
			else
				return wrapper.DeviceBase.RightControllerPose.ReadValue<Pose>().position;
		}

		public override Quaternion GetHandRotation(HandType _hand) {
			if (_hand == HandType.Left)
				return wrapper.DeviceBase.LeftControllerPose.ReadValue<Pose>().rotation;
			else
				return wrapper.DeviceBase.RightControllerPose.ReadValue<Pose>().rotation;
		}


		public override float GetTriggerSquezee(HandType _hand) {
			if (_hand == HandType.Left)
				return wrapper.DeviceBase.LeftTriggerAxis.ReadValue<float>();
			else
				return wrapper.DeviceBase.RightTriggerAxis.ReadValue<float>();
		}

		public override float GetGripSquezee(HandType _hand) {
			if (_hand == HandType.Left)
				return wrapper.DeviceBase.LeftGripAxis.ReadValue<float>();
			else
				return wrapper.DeviceBase.RightGripAxis.ReadValue<float>();
		}

		public override void Vibrate(VibrationSettings _settings, HandType _hand) {
			Debug.Log("Vibration not implemented yet");
		}

		public override void VibrateProcess() {
			Debug.Log("Vibration not implemented yet");
		}
	}
}