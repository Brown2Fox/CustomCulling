using System;
using UnityEngine;
using UnityEngine.Events;

public class Semaphore : MonoBehaviour, IActivatable {
	[SerializeField]
	private int startCount;
	[SerializeField]
	public int count;


	public UnityEvent onTrigger;


	private void OnEnable() {
		ResetCounter();
	}

	public void Activate() {
		count--;

		if (count == 0) onTrigger?.Invoke();
	}


	public void ResetCounter() {
		count = startCount;
	}
}