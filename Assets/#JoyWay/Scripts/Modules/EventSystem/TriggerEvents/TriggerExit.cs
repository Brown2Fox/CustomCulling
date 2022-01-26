using UnityEngine;

public class TriggerExit : MonoBehaviour {
	public UnityEventCollider onTriggerExit;

	private void OnTriggerExit(Collider _collider) {
		onTriggerExit?.Invoke(_collider);
	}
}
