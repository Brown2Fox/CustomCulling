using UnityEngine;

public class LateCopyPosRot : MonoBehaviour {
	public Transform target;
	public float smooth = 10f;
	private float lastSmooth;

	private void LateUpdate() {
		if (smooth == float.PositiveInfinity)
			lastSmooth = smooth;
		else
			lastSmooth = smooth * Time.deltaTime;
		transform.position = Vector3.Lerp(transform.position, target.position, lastSmooth);
		transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, lastSmooth);
	}
}