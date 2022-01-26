using UnityEngine;

public class Destroyer : MonoBehaviour {
	public void DestroyDow() {
		Destroy(gameObject);
	}

	public void DestroyWithDelay(float _time) {
		Destroy(gameObject, _time);
	}
}
