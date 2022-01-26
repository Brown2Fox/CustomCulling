using UnityEngine;

public class HubTrash : MonoBehaviour {
	public static HubTrash instance;
	private void Awake() {
		if (instance != null)
			Destroy(this);
		else
			instance = this;
	}
}
