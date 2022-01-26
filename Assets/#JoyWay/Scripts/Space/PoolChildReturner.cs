using UnityEngine;

public class PoolChildReturner : MonoBehaviour {
	private Transform trueParent;

	public void Return(Transform _trueParent) {
		trueParent = _trueParent;
		Invoke(nameof(Return), 0.05f);
	}
	private void Return() {
		transform.parent = trueParent;
		gameObject.SetActive(false);
	}
}
