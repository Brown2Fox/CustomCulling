using UnityEngine;

[CreateAssetMenu(fileName = "TelekinesisAbilityAsset", menuName = "JoyWay/Ability/TelekinesisAbilityAsset", order = 0)]
public class TelekinesisAbilityAsset : HandSplitAbilityAsset<TelekinesisAbility> {
	public TelekinesisSettings telekinesisSettings;

	public override void SetUpAbility(TelekinesisAbility _ability) {
		base.SetUpAbility(_ability);
		_ability.defaultSettings = telekinesisSettings;
		_ability.SetupSize(telekinesisSettings.maxDist);
	}
}