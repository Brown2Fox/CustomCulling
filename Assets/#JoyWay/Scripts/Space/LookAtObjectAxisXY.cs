using UnityEngine;

public class LookAtObjectAxisXY : MonoBehaviour
{
	public Transform target;
	private void Awake() {
		if (!target)
			target = Player.instance.head;
	}

	public void Update() {
		Vector3 relativePos = transform.position - target.position;
		Quaternion rotaion = Quaternion.LookRotation(relativePos, transform.up);
		transform.rotation = rotaion;
	}
}
