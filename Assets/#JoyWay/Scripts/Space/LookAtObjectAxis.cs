using UnityEngine;

public class LookAtObjectAxis : MonoBehaviour
{
	public Transform target;

	private float takeAngle;
	private float curAngle;
	private void Awake() {
		if (!target)
			target = Player.instance.head;
	}

	public void ResetAngle(GrabHandPart grabHand) {
		takeAngle = Vector3.SignedAngle(Vector3.ProjectOnPlane(target.position - transform.position, transform.right), transform.forward, transform.right);
	}

	public void Update() {
		float newAngle = Vector3.SignedAngle(Vector3.ProjectOnPlane(target.position - transform.position, transform.right), transform.forward, transform.right);
		curAngle += takeAngle - newAngle;
		transform.localRotation = Quaternion.Euler(curAngle, 0f, 0f);
	}
}
