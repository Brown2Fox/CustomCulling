using UnityEngine;

[CreateAssetMenu(fileName = "ClimbAbilityAsset", menuName = "JoyWay/Ability/ClimbAbilityAsset", order = 0)]
public class ClimbAbilityAsset : HandSplitAbilityAsset<ClimbAbility> {
	public float dashVelocityMultiplier = 1f;
	public float dashVelocityMax = 1f;
	//public ClimbSettings ClimbSettings;
}