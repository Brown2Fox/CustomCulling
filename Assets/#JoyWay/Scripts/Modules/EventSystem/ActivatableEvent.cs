using UnityEngine;
using UnityEngine.Events;

public class ActivatableEvent : MonoBehaviour, IActivatable {
	public UnityEvent onActivate;

	public void Activate() {
		onActivate?.Invoke();
	}
}