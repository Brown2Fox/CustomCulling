using UnityEngine;

public class DistanceAllLog : MonoBehaviour {
	public Transform target;

	private void FixedUpdate() {
		Vector3 dist = target.position - transform.position;
		Debug.Log($"{Time.time.ToString("0.000")}: {dist.x.ToString("0.000")}, {dist.y.ToString("0.000")}, {dist.z.ToString("0.000")}: FixedUpdate");
	}

	private void OnTriggerStay(Collider _collider) {
		Vector3 dist = target.position - transform.position;
		Debug.Log($"{Time.time.ToString("0.000")}: {dist.x.ToString("0.000")}, {dist.y.ToString("0.000")}, {dist.z.ToString("0.000")}: OnTriggerStay");
	}

	private void Update() {
		Vector3 dist = target.position - transform.position;
		Debug.Log($"{Time.time.ToString("0.000")}: {dist.x.ToString("0.000")}, {dist.y.ToString("0.000")}, {dist.z.ToString("0.000")}: Update");
	}

	private void LateUpdate() {
		Vector3 dist = target.position - transform.position;
		Debug.Log($"{Time.time.ToString("0.000")}: {dist.x.ToString("0.000")}, {dist.y.ToString("0.000")}, {dist.z.ToString("0.000")}: LateUpdate");
	}
}
