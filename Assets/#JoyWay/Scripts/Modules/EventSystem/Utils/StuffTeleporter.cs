using UnityEngine;

public class StuffTeleporter : MonoBehaviour, IActivatable {
	public Transform obj;

	public void Activate() {
		obj.position = transform.position;
	}
}
