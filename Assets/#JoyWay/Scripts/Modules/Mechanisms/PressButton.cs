using System;
using UnityEngine;
using UnityEngine.Events;

public class PressButton : MonoBehaviour, IActivatable {
	public UnityEvent OnPress;

	public float returnSpeed;
	public float returnDelay;
	public float nextReturn;


	public void Activate() {
		OnPress?.Invoke();
	}


	private void OnCollisionStay(Collision other) {
		nextReturn = Time.time + returnDelay;
	}


	private void LateUpdate() { }

	private void FixedUpdate() {
		if (Time.time - nextReturn > 0) { }
	}
}