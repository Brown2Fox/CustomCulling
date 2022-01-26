using UnityEngine;
using UnityEngine.Events;

public class PlayerMathTrigger : MonoBehaviour {
	public UnityEvent onTriggerEnter, onTriggerExit;
	private bool isPlayerIn = false;
	
	private Transform target;
	private float minX, maxX, minY, maxY, minZ, maxZ;

	private void Start() {
		target = Player.instance.movable;

		Vector3 point1 = transform.TransformPoint(new Vector3(-.5f, -.5f, -.5f));
		Vector3 point2 = transform.TransformPoint(new Vector3(.5f, .5f, .5f));

		minX = Mathf.Min(point1.x, point2.x);
		maxX = Mathf.Max(point1.x, point2.x);
		minY = Mathf.Min(point1.y, point2.y);
		maxY = Mathf.Max(point1.y, point2.y);
		minZ = Mathf.Min(point1.z, point2.z);
		maxZ = Mathf.Max(point1.z, point2.z);
	}

	private void Update() {
		if (isPlayerIn) {
			if (target.position.x < minX || target.position.x > maxX ||
			target.position.z < minZ || target.position.z > maxZ ||
			target.position.y < minY || target.position.y > maxY) {
				isPlayerIn = false;
				onTriggerExit.Invoke();
			}
		} else {
			if (target.position.x > minX && target.position.x < maxX &&
			target.position.z > minZ && target.position.z < maxZ &&
			target.position.y > minY && target.position.y < maxY) {
				isPlayerIn = true;
				onTriggerEnter.Invoke();
			}
		}
	}
}
