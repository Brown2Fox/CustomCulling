using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(LoadingScreenController))]
public class LoadingScreenControllerEditor : Editor
{
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		LoadingScreenController the = (LoadingScreenController)target;

		// Возможно я перепутал Vector3.right и Vector3.forward
		if (GUILayout.Button("Start"))
			the.StartLoading();

		if (GUILayout.Button("Stop"))
			the.EndLoading();
	}
}
#endif
*/


public class LoadingScreenController : MonoBehaviour
{
	public Animation loadingAnimation;
	public Camera camera;
	
	public GameObject loadingPlane;

	private void Awake() {

		LevelManager.SoonLoading += StartLoading;
		LevelManager.EndLoading += EndLoading;
		
		loadingPlane.transform.localPosition = new Vector3(0, 0, camera.nearClipPlane + 0.001f);
	}

	public void StartLoading() {
		loadingAnimation.Play("Fade Forward 1");
	}

	public void EndLoading() {
		loadingAnimation.Play("Fade Back 1");
	}

}

