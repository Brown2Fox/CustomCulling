using UnityEngine;

public class TriggerEnter : MonoBehaviour {
	public UnityEventCollider onTriggerEnter;

	private void OnTriggerEnter(Collider _collider) {
		onTriggerEnter?.Invoke(_collider);
	}
}
