using UnityEngine;

[CreateAssetMenu(fileName = "DashAbilityAsset", menuName = "JoyWay/Ability/DashAbilityAsset", order = 0)]
public class DashAbilityAsset : HandSplitAbilityAsset<DashAbility> {
	public dashSettings DashSettings;


	public override void SetUpAbility(DashAbility _ability) {
		base.SetUpAbility(_ability);
		_ability.dashSettings = DashSettings;
	}
}