using UnityEngine;

public class ForwardByVelocity : MonoBehaviour {
	public Rigidbody rb;
	private VelocityCounter vc;

	private void Update() {
		Vector3 vel = Vector3.zero;
		if (rb)
			vel += rb.velocity;
		if (vc)
			vel += vc.velocity;

		if (vel.sqrMagnitude > 0.1f)
			transform.forward = vel;
	}
}