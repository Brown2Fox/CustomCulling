using System;
using UnityEngine;

public class AutoDisabler : MonoBehaviour {
	public float timeToDisable = 5f;

	private void OnEnable() {
		Invoke(nameof(Disable), timeToDisable);
	}


	private void Disable() {
		gameObject.SetActive(false);
	}
}