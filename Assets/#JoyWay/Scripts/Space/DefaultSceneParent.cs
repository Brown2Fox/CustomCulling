using UnityEngine;

public class DefaultSceneParent : MonoBehaviour {
	public static Transform lastParent;

	private void OnEnable() {
		lastParent = transform;
	}
}
