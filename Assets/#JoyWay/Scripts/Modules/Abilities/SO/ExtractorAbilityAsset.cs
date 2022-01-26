using UnityEngine;

[CreateAssetMenu(fileName = "ExtractorAbilityAsset", menuName = "JoyWay/Ability/ExtractorAbility", order = 0)]
public class ExtractorAbilityAsset : HandSplitAbilityAsset<ExtractorAbility> {
	public PoolObject effect;
	public PoolObject extractableObject;


	public override void SetUpAbility(ExtractorAbility _ability) {
		base.SetUpAbility(_ability);

		_ability.stonePool = Pool.AddToPool(extractableObject, 5);
		_ability.effectPool = Pool.AddToPool(effect, 5);
	}
}