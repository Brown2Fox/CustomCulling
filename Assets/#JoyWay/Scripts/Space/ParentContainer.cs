using UnityEngine;

public class ParentContainer : MonoBehaviour {
	private Transform parent;

	private void Awake() {
		ReSetParent();
	}

	public void ReturnToParent() {
		transform.parent = parent;
	}

	public void ReSetParent() {
		parent = transform.parent;
	}
}
