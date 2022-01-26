using System;
using JoyWay.Systems.InputSystem;

public interface IVibratable {
	event Action<VibrationSettings> OnVibrate;
}

public static class VibrationHelper {
	//public static void Vibrate(this IVibratable vibrate, VibrationSettings vibrationSettings) {
	//	Action<VibrationSettings> action;
	//	action += vibrate.OnVibrate;
	//	vibrate.OnVibrate?.Invoke(vibrationSettings);
	//}

	public static void Vibrate(this AbilityHandPart vibrate, VibrationSettings vibrationSettings) {
		vibrate.hand.Vibrate(vibrationSettings);
	}
}
