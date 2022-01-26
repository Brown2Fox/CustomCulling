using UnityEngine;

[CreateAssetMenu(fileName = "HeroGroundingAbilityAsset", menuName = "JoyWay/Ability/HeroGroundingAbilityAsset", order = 0)]
public class HeroGroundingAbilityAsset : ActiveAbilityAsset<HeroGroundingAbility> {

	public override void SetUpAbility(HeroGroundingAbility _ability) {
		base.SetUpAbility(_ability);
	}
}