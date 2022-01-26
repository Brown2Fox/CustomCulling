using UnityEngine;

public class UpdateCopyPosRot : MonoBehaviour {
	public Transform target;
	public float smooth = 10f;

	private void Update() {
		transform.position = Vector3.Lerp(transform.position, target.position, smooth * Time.unscaledDeltaTime);
		transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, smooth * Time.unscaledDeltaTime);
	}
}