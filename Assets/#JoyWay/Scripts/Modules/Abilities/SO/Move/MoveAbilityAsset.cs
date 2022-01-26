using UnityEngine;

public abstract class MoveAbilityAsset<MA> : AbilityAsset<MA> where MA : MoveAbilityBase {
	public MoveVelocityParameters runVelocityParameters;
	public MoveVelocityParameters walkVelocityParameters;

	public override void SetUpAbility(MA _ability) {
		_ability.runVelocityParameters = runVelocityParameters;
		_ability.walkVelocityParameters = walkVelocityParameters;
	}
}