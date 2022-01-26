using System.Collections;
using UnityEngine;

public class LayerSwitcher : MonoBehaviour {
	public Layer
		mainLayer,
		alterLayer;

	public static float delay = 0.2f;

	public GameObject[] objects;

	public void Switch(bool _state) {
		StopAllCoroutines();
		StartCoroutine(SwichLayer(_state, 0));
	}

	public void SwithcWithDelay(bool _state) {
		StopAllCoroutines();
		if (gameObject.activeInHierarchy)
			StartCoroutine(SwichLayer(_state, delay));
		else
			ProcessSwitch(_state);
	}

	IEnumerator SwichLayer(bool _state, float delayTime) {
		yield return new WaitForSeconds(delayTime);
		//		Debug.Log($"switching to {_state}");
		ProcessSwitch(_state);
	}

	private void ProcessSwitch(bool _state) {
		if (_state)
			for (int i = objects.Length - 1; i >= 0; --i)
				objects[i].layer = (int)alterLayer;
		else
			for (int i = objects.Length - 1; i >= 0; --i)
				objects[i].layer = (int)mainLayer;
	}
}
