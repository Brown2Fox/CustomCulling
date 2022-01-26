using UnityEngine;

public class CoppyFlatDirection : MonoBehaviour {
	public Transform source;
	private Vector3 forwardDir;

	private void LateUpdate() {
		forwardDir = source.forward;
		forwardDir.y = 0;
		if (forwardDir.sqrMagnitude > 0)
			transform.forward = forwardDir;
	}
}
