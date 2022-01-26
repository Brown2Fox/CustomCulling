using System;
using JoyWay.Systems.InputSystem;
using UnityEngine;

public abstract class BaseAbilityAsset : PowerupAsset {
	public AbilitySettings settings;


	public virtual void SetUpAbilityBase(Ability _ability) {
		_ability.settings = settings;
		_ability.AbilityID = entryID;
	}

	public abstract Ability InitBaseAbility(Owner _to);
}


public abstract class ActiveAbilityAsset : BaseAbilityAsset {
	public CastAudio castAudio;
	
}

[Serializable]
public abstract class AbilityAsset<A> : BaseAbilityAsset where A : Ability {
	public override void InitAsset(Owner _to, Owner _from) => InitAbility(_to);
	public override Ability InitBaseAbility(Owner _to) => InitAbility(_to);


	public A InitAbility(Owner _to) {
		A ability = _to.aContainer.GetOrAddAbility<A>(this);
		SetUpAbility(ability);
		ability.Init(_to.aContainer);
		return ability;
	}

	public abstract void SetUpAbility(A _ability);

	public override void SetUpAbilityBase(Ability _ability) {
		base.SetUpAbilityBase(_ability);
		SetUpAbility((A) _ability);
	}

	public override string GetDescription() {
		return $"this is the asset of {typeof(A)} type";
	}
}

[Serializable]
public abstract class ActiveAbilityAsset<A> : ActiveAbilityAsset where A : ActiveAbility {
	public override void InitAsset(Owner _to, Owner _from) => InitAbility(_to);
	public override Ability InitBaseAbility(Owner _to) => InitAbility(_to);


	public A InitAbility(Owner _to) {
		A ability = _to.aContainer.GetOrAddAbility<A>(this);
		SetUpAbility(ability);
		ability.Init(_to.aContainer);
		return ability;
	}

	public virtual void SetUpAbility(A _ability) {
		base.SetUpAbilityBase(_ability);
		_ability.castAudio = castAudio;
	}

	public override void SetUpAbilityBase(Ability _ability) {
		SetUpAbility((A) _ability);
	}

	public override string GetDescription() {
		return $"this is the asset of {typeof(A)} type";
	}
}


public abstract class HandSplitAbilityAsset<A> : ActiveAbilityAsset<A> where A : HandSplitAbility {
	public PoolObject castChargeEffect;
	public HandSplitAbilityVibrationSettings vibration;

	public AbilityInput.Type input;


	public override void SetUpAbility(A _ability) {
		base.SetUpAbility(_ability);
		_ability.input = input;
		_ability.vibrations = vibration;
		if (castChargeEffect)
			_ability.chargingPool = Pool.GetOrCreate(castChargeEffect);
	}
}
