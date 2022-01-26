using UnityEngine;
using UnityEngine.Serialization;

public abstract class HeroGroundAsset : ScriptableObject {
	[FormerlySerializedAs("effect")]
	public PoolObject vfx;
	[FormerlySerializedAs("effectAudio")]
	public AudioAsset sfx;

	public abstract ElementalGroundingEffect Init(Owner _owner);
}


public class HeroGroundAsset<EffectType> : HeroGroundAsset where EffectType : ElementalGroundingEffect, new() {
	public override ElementalGroundingEffect Init(Owner _owner) => SetUpEffect(_owner);

	public virtual EffectType SetUpEffect(Owner _owner) {
		
		return null;
	}
}