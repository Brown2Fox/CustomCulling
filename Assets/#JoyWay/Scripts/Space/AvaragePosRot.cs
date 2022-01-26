using UnityEngine;

public class AvaragePosRot : MonoBehaviour {
	public Transform target1, target2;

	private void LateUpdate() {
		UpdatePosition();
	}

	public void UpdatePosition() {
		transform.position = (target1.position + target2.position) * .5f;
		transform.rotation = Quaternion.Lerp(target1.rotation, target2.rotation, .5f);
	}
}
