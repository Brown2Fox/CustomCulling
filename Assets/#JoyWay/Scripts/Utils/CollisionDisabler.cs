using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDisabler : MonoBehaviour {
	public Collider collider;
	public List<Collider> toDisable;


	private void Awake() {
		foreach (Collider collider1 in toDisable) {
			Physics.IgnoreCollision(collider, collider1);
		}
	}
}	