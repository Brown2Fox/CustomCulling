using UnityEngine;

public class LookAtCam : MonoBehaviour {
	private Transform target;
	public bool flat = false;
	private Vector3 t_camVector;
	private void Awake() {
		target = Player.instance.head;
	}
	private void Update() {
		if (flat) {
			t_camVector = target.position - transform.position;
			t_camVector.y = 0f;
			transform.forward = t_camVector;
		} else
			transform.LookAt(target);
	}
}
