using UnityEngine;
using UnityEngine.Events;

public class DisableEvent : MonoBehaviour {
    public UnityEvent onDisable;

    private void OnDisable() {
        onDisable?.Invoke();
    }
}
