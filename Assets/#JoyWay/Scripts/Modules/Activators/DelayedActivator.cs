using System;
using UnityEngine;

public class DelayedActivator : Activator, IProgressBarUI {
	public float delay = 5f;

	private float activateTime;

	private void OnEnable() {
		StartTimer();
	}


	public void StartTimer() {
		activateTime = Time.time + delay;
	}


	public void Update() {
		if (Time.time > activateTime) {
			Activate();
			activateTime = float.PositiveInfinity;
		}
	}

	public float GetMaxValue() => delay;

	public float GetCurrentValue() => (activateTime - Time.time) / delay;
}
