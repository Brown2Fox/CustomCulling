using UnityEngine;

public class FixedCopyPosRot : MonoBehaviour {
	public Transform target;
	public float smooth = 10f;
	private void FixedUpdate() {
		transform.position = Vector3.Lerp(transform.position, target.position, smooth * Time.deltaTime);
		transform.rotation = Quaternion.Lerp(transform.rotation,target.rotation, smooth * Time.deltaTime);
	}
}
