using System;

public class SettingsGameplay : SettingsUIPage {
	public UIEnumField turnTypeSwitcher, turnAngleSwitcher;
	public UIToggleButton damageNumbers, impactPoint, longhold;


	public override void Show() {
		turnTypeSwitcher.SetUp(typeof(TurnType), (int) GameMain.settings.serializable.input.turnType, OnTurnTypeUpdate);


		damageNumbers.SetUp(GameMain.settings.serializable.configs.damageNumbers, OnDamageNumbersChange);
		//impactPoint.SetUp(GameMain.settings.serializable.configs.impactPoints, OnImpactPointsChange);
		longhold.SetUp(GameMain.settings.serializable.input.longHolds, OnLongholdsChange);
		damageNumbers.SetDisabled(true);
	}


	public void OnTurnTypeUpdate(int _i) {
		TurnType tt = (TurnType) _i;
		GameMain.settings.serializable.input.turnType = tt;

		switch (tt) {
			case TurnType.Disabled:
				turnAngleSwitcher.SetDisabled();
				break;
			case TurnType.SnapTurn:
				turnAngleSwitcher.SetUp(typeof(SnapTurnDegrees), (int) GameMain.settings.serializable.input.snapTurnDegrees, OnSnapTurnDegreesChange);
				break;
			case TurnType.SmoothTurn:
				turnAngleSwitcher.SetUp(typeof(SmoothTurnSpeed), (int) GameMain.settings.serializable.input.smoothTurnSpeed, OnSmoothTurnSpeedChange);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}


	public void OnSnapTurnDegreesChange(int _i) {
		GameMain.settings.serializable.input.snapTurnDegrees = (SnapTurnDegrees) _i;
	}

	public void OnDamageNumbersChange(bool _state) {
		GameMain.settings.serializable.configs.damageNumbers = _state;
	}

	public void OnImpactPointsChange(bool _state) {
		GameMain.settings.serializable.configs.impactPoints = _state;
	}

	public void OnLongholdsChange(bool _state) {
		GameMain.settings.serializable.input.longHolds = _state;
	}


	public void OnSmoothTurnSpeedChange(int _i) {
		GameMain.settings.serializable.input.smoothTurnSpeed = (SmoothTurnSpeed) _i;
	}


	public override void Hide() { }
}