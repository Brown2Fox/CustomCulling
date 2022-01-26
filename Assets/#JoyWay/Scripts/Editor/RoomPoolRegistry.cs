using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomPoolRegistry", menuName = "JoyWay/MapGeneration/Room pool registry")]
public class RoomPoolRegistry : ScriptableObject {
	public SceneAsset[] constScenes = new SceneAsset[Enum.GetNames(typeof(ScenesList)).Length];
	public RoomsPoolAsset[] pools;
}

[CustomEditor(typeof(RoomPoolRegistry))]
public class RoomPoolRegistryEditor : Editor {
	private List<string> scenesNamesList;
	List<EditorBuildSettingsScene> buildScenes;
	private RoomPoolRegistry theScript;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		theScript = (RoomPoolRegistry)target;
		if (GUILayout.Button("Подготовить сцены"))
			PrepareScenes();
	}

	private void PrepareScenes() {
		scenesNamesList = new List<string>();
		for (int i = 0; i < theScript.constScenes.Length; ++i)
			if (theScript.constScenes[i] == null)
				scenesNamesList.Add("");
			else
				scenesNamesList.Add(AssetDatabase.GetAssetPath(theScript.constScenes[i]));

		for (int i = 0; i < theScript.pools.Length; ++i)
			for (int j = 0; j < theScript.pools[i].variants.Count; ++j)
				if (!scenesNamesList.Contains(theScript.pools[i].variants[j].roomSceneName))
					scenesNamesList.Add(theScript.pools[i].variants[j].roomSceneName);


		buildScenes = new List<EditorBuildSettingsScene>();
		for (int i = 0; i < scenesNamesList.Count; ++i)
			buildScenes.Add(new EditorBuildSettingsScene(scenesNamesList[i], true));

		EditorBuildSettings.scenes = buildScenes.ToArray();
	}
}
