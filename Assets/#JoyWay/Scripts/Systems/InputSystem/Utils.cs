using UnityEngine.InputSystem;


namespace JoyWay.Systems.InputSystem {
	public static class InputUtils {
		public static bool IsLeft(this InputDevice _device) {
			return _device == OpenXRInput.left;
		}
	}
}