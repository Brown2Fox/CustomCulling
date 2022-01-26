using UnityEngine;
using UnityEngine.Events;

public class EnableEvent : MonoBehaviour {
    public UnityEvent onEnable;

    private void OnEnable() {
        onEnable?.Invoke();
    }
}
