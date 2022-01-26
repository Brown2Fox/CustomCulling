using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshLinkActivate : MonoBehaviour {
	private NavMeshLink[] navMeshLinks;
	public void Awake() {
		navMeshLinks = GetComponentsInChildren<NavMeshLink>();
	}

	public void Start() {
		foreach (NavMeshLink link in navMeshLinks) {
			link.UpdateLink();
		}
	}
}
