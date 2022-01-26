using UnityEngine;

public class VelocityCounter : MonoBehaviour {
	public float accuracy = 10f;

	public Vector3 velocity, localVelocity;
	private Vector3 lastPosition, lastLocalPosition;

	public Vector3 angularVelocity;
	private Quaternion lastRotation;

	private void OnEnable() {
		Reset();
	}

	public void Reset() {
		velocity = Vector3.zero;
		localVelocity = Vector3.zero;
		lastPosition = transform.position;
		lastLocalPosition = transform.localPosition;
	}

	private void Update() {
		if (Time.deltaTime != 0) {
			velocity = Vector3.Lerp(velocity, (transform.position - lastPosition) / Time.deltaTime, accuracy * Time.deltaTime);
			localVelocity = Vector3.Lerp(localVelocity, (transform.localPosition - lastLocalPosition) / Time.deltaTime, accuracy * Time.deltaTime);
		}
		lastPosition = transform.position;
		lastLocalPosition = transform.localPosition;

		if (Time.deltaTime != 0) {
			angularVelocity = (Quaternion.Inverse(lastRotation) * transform.rotation).eulerAngles;
			for (int i = 0; i < 3; ++i)
				if (angularVelocity[i] > 180f)
					angularVelocity[i] -= 360f;
			angularVelocity = angularVelocity / Time.deltaTime * Mathf.Deg2Rad;
		}
		lastRotation = transform.rotation;
	}
}
