using UnityEngine;

public class ElementalGroundingEffect {
	public HeroGroundingAbility ability;
	public PoolObject vfx;
	public AudioAsset sfx;

	public void Init(HeroGroundingAbility _ability) {
		ability = _ability;
		ability.effectsList.Add(this);
	}




	public void RemoveEffect() {
		ability.effectsList.Remove(this);
	}
}