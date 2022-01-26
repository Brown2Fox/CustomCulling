using UnityEngine;
using UnityEditor;

public class MassObjectReplacerWindow : EditorWindow {
	public Transform[] replacebles;
	private GameObject replacer, newReplacer;
	private Transform source;
	private Vector2 scrollPos;

	[MenuItem("Window/JoyWayTools/Mass Object Replacer", priority = 101)]
	public static void ShowWindow() {
		GetWindow<MassObjectReplacerWindow>("Mass Object Replacer");
	}

	public void OnGUI() {
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		SerializedObject replaceblesArray = new SerializedObject(this);
		EditorGUILayout.PropertyField(replaceblesArray.FindProperty("replacebles"), true);
		replaceblesArray.ApplyModifiedProperties();
		EditorGUILayout.EndScrollView();

		EditorGUILayout.BeginHorizontal();
		replacer = (GameObject)EditorGUILayout.ObjectField(replacer, typeof(GameObject), false);
		if (GUILayout.Button("Replace objects"))
			Replace();
		EditorGUILayout.EndHorizontal();
	}

	private void Replace() {
		for (int i = 0; i < replacebles.Length; ++i) {
			source = replacebles[i];
			if (replacer.scene.name == null)
				newReplacer = PrefabUtility.InstantiatePrefab(replacer) as GameObject;
			else
				newReplacer = Instantiate(replacer);
			newReplacer.transform.position = source.position;
			newReplacer.transform.rotation = source.rotation;
			newReplacer.transform.parent = source.parent;
			newReplacer.transform.localScale = source.localScale;
			newReplacer.transform.SetSiblingIndex(source.GetSiblingIndex());
			newReplacer.name = replacer.name;
			newReplacer.SetActive(replacebles[i].gameObject.activeSelf);
			DestroyImmediate(source.gameObject);
		}
		replacebles = new Transform[0];
	}
}
