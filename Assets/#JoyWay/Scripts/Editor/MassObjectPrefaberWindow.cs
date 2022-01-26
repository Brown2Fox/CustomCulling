using UnityEngine;
using UnityEditor;

public class MassObjectPrefaberWindow : EditorWindow {
	public GameObject[] targets;
	private GameObject t_target;
	private Vector2 scrollPos;
	private string path;
	private GameObject pathSource;

	[MenuItem("Window/JoyWayTools/Mass Object Prefaber", priority = 102)]
	public static void ShowWindow() {
		GetWindow<MassObjectPrefaberWindow>("Mass Object Prefaber");
	}

	public void OnGUI() {
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		SerializedObject replaceblesArray = new SerializedObject(this);
		EditorGUILayout.PropertyField(replaceblesArray.FindProperty("targets"), true);
		replaceblesArray.ApplyModifiedProperties();
		EditorGUILayout.EndScrollView();

		EditorGUILayout.LabelField("Example: Assets/#JoyWay/Prefabs/PrefabPrefix_");
		path = EditorGUILayout.TextField(path);

		EditorGUILayout.BeginHorizontal();
		pathSource = (GameObject)EditorGUILayout.ObjectField(pathSource, typeof(GameObject), false);
		if (GUILayout.Button("Get the object's path"))
			path = AssetDatabase.GetAssetPath(pathSource);
		EditorGUILayout.EndHorizontal();
		if (GUILayout.Button("Prefab objects"))
			Prefab();
	}

	private void Prefab() {
		for (int i = 0; i < targets.Length; ++i) {
			t_target = targets[i];
			PrefabUtility.SaveAsPrefabAsset(t_target, path + t_target.name + ".prefab");
		}
	}
}
