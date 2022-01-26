using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class LayersObjectsSelectWindow : EditorWindow {
	private Layer layer;
	private int lastLayer = -1;
	private Vector2 scrollPos;
	private Transform[] transforms;
	public List<GameObject> objectsOnLayer = new List<GameObject>();
	private bool
		scenesOnly = true,
		autoGet = false;

	[MenuItem("Window/JoyWayTools/Layer's Object Selector", priority = 105)]
	public static void ShowWindow() {
		GetWindow<LayersObjectsSelectWindow>("Layer's Object Selector");
	}

	public void OnGUI() {
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Toggle(scenesOnly, "Искать только на сцене") != scenesOnly) {
			scenesOnly = !scenesOnly;
			if (autoGet)
				GetObjects();
		}

		EditorGUILayout.BeginHorizontal(GUILayout.Width(150f));
		GUILayout.Label("    Слой");
		layer = (Layer)EditorGUILayout.EnumPopup(layer);
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Toggle(autoGet, "Авто") != autoGet)
			autoGet = !autoGet;
		if (GUILayout.Button("Отыскать объекты"))
			GetObjects();

		if (GUILayout.Button("Выбрать объекты")) {
			GameObject[] toSelect = new GameObject[objectsOnLayer.Count];
			Array.Copy(objectsOnLayer.ToArray(), 0, toSelect, 0, objectsOnLayer.Count);
			Selection.objects = toSelect;
		}

		EditorGUILayout.EndHorizontal();


		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

		if (lastLayer != (int)layer)
			if (autoGet)
				GetObjects();

		SerializedObject objectsArray = new SerializedObject(this);
		EditorGUILayout.PropertyField(objectsArray.FindProperty("objectsOnLayer"), true);

		EditorGUILayout.EndScrollView();
	}

	private void GetObjects() {
		transforms = Resources.FindObjectsOfTypeAll<Transform>();

		objectsOnLayer.Clear();
		for (int i = transforms.Length - 1; i >= 0; --i) {
			if (transforms[i].gameObject.layer != (int)layer) continue;
			if (scenesOnly && transforms[i].gameObject.scene.buildIndex == -1) continue;
			objectsOnLayer.Add(transforms[i].gameObject);
		}

		lastLayer = (int)layer;
	}
}
