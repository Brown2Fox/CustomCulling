using UnityEngine;

public class LookAtObject : MonoBehaviour
{
	public Transform target;

	public void Update() {
		transform.LookAt(target, Vector3.up);
	}
}
