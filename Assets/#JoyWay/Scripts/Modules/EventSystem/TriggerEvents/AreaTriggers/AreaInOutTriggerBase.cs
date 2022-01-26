using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaInOutTriggerBase : MonoBehaviour {
	//public TargetCheckParams targetCheck;
	public Team affectedTeams;

	public List<Owner> currentAffected = new List<Owner>();

	protected virtual bool Check(Owner _owner) {
		if (!_owner) return false;
		if (currentAffected.Contains(_owner)) return false;
		if ((_owner.team & affectedTeams) == 0) return false;

		return true;
	}


	private void OnTriggerEnter(Collider other) {
		var newOwner = other.GetComponentInParent<Owner>();
		if (!Check(newOwner)) return;
		TriggerEnter(newOwner);
		currentAffected.Add(newOwner);
	}

	private void OnTriggerExit(Collider other) {
		var newOwner = other.GetComponentInParent<Owner>();
		if (!currentAffected.Contains(newOwner)) return;

		TriggerExit(newOwner);
		currentAffected.Remove(newOwner);
	}


	private void OnDisable() {
		foreach (Owner owner in currentAffected) {
			TriggerExit(owner);
		}

		currentAffected.Clear();
	}


	public abstract void TriggerEnter(Owner _owner);
	public abstract void TriggerExit(Owner _owner);
}