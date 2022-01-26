using System;
using System.Collections.Generic;
using Valve.Newtonsoft.Json.Utilities;

public class AbilityContainer : Container<Ability> {
	private int uniqID = -1;

	public void Init() {
		base.Init();

		foreach (var entry in entries) {
			entry.Value.Init(this);
		}
	}


	public override void PreInitEntry(Ability _entry) {
		if (_entry.asset)
			_entry.asset.SetUpAbilityBase(_entry);
		else
			_entry.AbilityID = uniqID--;
	}

	public override int GetID(Ability _entry) {
		return _entry.AbilityID;
	}

	public T GetOrAddAbility<T>(BaseAbilityAsset _asset) where T : Ability {
		var ability = GetEntryOfType<T>();
		if (!ability) ability = AddAbility<T>(_asset);
		return ability;
	}


	public T AddAbility<T>(BaseAbilityAsset _asset) where T : Ability {
		var ability = container.AddComponent<T>();
		ability.enabled = false;
		entries.Add(_asset.entryID, ability);
		onEntryAdd?.Invoke(ability);
		return ability;
	}

	public T GetAbility<T>(bool addIfNull = true) where T : Ability {
		return GetEntryOfType<T>();
	}

	// public Ability GetAbility(Type t, bool addIfNull = true) {
	// 	Ability ability;
	// 	if (!entries.TryGetValue(t, out ability)) {
	// 		if (!TryGetEntry(t, out ability))
	// 			if (addIfNull) { }
	// 	}
	//
	// 	return ability;
	// }


	// public Action<ShootingAbilityBase> OnShootingAbilityAdded;
	// public Action<ShootingAbilityBase> OnShootingAbilityRemoved;


	public void Remove(Ability _ability) {
		entries.Remove(_ability.AbilityID);
		// if (_ability is ShootingAbilityBase swb)
		// 	OnShootingAbilityRemoved?.Invoke(swb);
		Destroy(_ability);
	}


	public void ResetAbilities() {
		Ability[] list = new Ability[entries.Values.Count];
		entries.Values.CopyTo(list, 0);

		foreach (Ability bb in list)
			bb.ResetAbility();
	}
}