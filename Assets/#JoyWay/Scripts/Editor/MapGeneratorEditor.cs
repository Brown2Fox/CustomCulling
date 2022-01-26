using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(MapGenerator), true)]
public class MapGeneratorEditor : Editor {
	private MapGenerator theScript;
	public override void OnInspectorGUI() {
		theScript = (MapGenerator)target;

		GUILayout.BeginHorizontal();
		theScript.randomSeed = EditorGUILayout.ToggleLeft("Random seed", theScript.randomSeed, GUILayout.Width(100f));
		if (theScript.randomSeed)
			EditorGUI.BeginDisabledGroup(true);
		theScript.seed = EditorGUILayout.IntField(theScript.seed);
		if (GUILayout.Button("Roll"))
			theScript.seed = Random.Range(int.MinValue, int.MaxValue);
		if (theScript.randomSeed)
			EditorGUI.EndDisabledGroup();
		GUILayout.EndHorizontal();

		base.OnInspectorGUI();

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Clear")) {
			ClearLog();
			theScript.ResetAll();
			if (!Application.isPlaying)
				EditorSceneManager.MarkSceneDirty(theScript.gameObject.scene);
		}
		if (GUILayout.Button("Start")) {
			ClearLog();
			theScript.ResetAll();
			theScript.StartGeneration();
			theScript.Place();
			if (theScript.autoNavmesh)
				theScript.NavMeshIt();
			if (!Application.isPlaying)
				EditorSceneManager.MarkSceneDirty(theScript.gameObject.scene);
		}
		if (GUILayout.Button("+point")) {
			theScript.ClearScene();
			theScript.AddPoint();
			theScript.Place();
			if (theScript.autoNavmesh)
				theScript.NavMeshIt();
			if (!Application.isPlaying)
				EditorSceneManager.MarkSceneDirty(theScript.gameObject.scene);
		}
		if (GUILayout.Button("Finish")) {
			theScript.ClearScene();
			theScript.FinishGeneration();
			theScript.Place();
			theScript.FillTheMap();
			if (theScript.autoNavmesh)
				theScript.NavMeshIt();
			if (!Application.isPlaying)
				EditorSceneManager.MarkSceneDirty(theScript.gameObject.scene);
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Clear & Generate & Place")) {
			ClearLog();
			theScript.ResetAll();
			theScript.Generate();
			theScript.Place();
			theScript.FillTheMap();
			if (theScript.autoNavmesh)
				theScript.NavMeshIt();
			if (!Application.isPlaying)
				EditorSceneManager.MarkSceneDirty(theScript.gameObject.scene);
		}
		theScript.autoNavmesh = EditorGUILayout.ToggleLeft("Auto NavMesh", theScript.autoNavmesh, GUILayout.Width(100f));
		if (GUILayout.Button("Bake NavMesh")) {
			theScript.NavMeshIt();
			if (!Application.isPlaying)
				EditorSceneManager.MarkSceneDirty(theScript.gameObject.scene);
		}
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("Collect stats"))
			theScript.MakeStatistics();

		EditorGUILayout.BeginHorizontal();
		theScript.mapAsset = (MapAsset)EditorGUILayout.ObjectField(theScript.mapAsset, typeof(MapAsset), false);
		if (GUILayout.Button("Save") && theScript.mapAsset)
			SaveAsset();
		EditorGUILayout.EndHorizontal();
	}

	private void SaveAsset() {
		theScript.mapAsset.mapTable = new char[theScript.mapSize.x, theScript.mapSize.y];
		for (int i = theScript.mapSize.x - 1; i >= 0; --i) {
			for (int j = theScript.mapSize.y - 1; j >= 0; --j)
				theScript.mapAsset.mapTable[i, j] = theScript.theMap[i, j];
		}

		theScript.mapAsset.serrialized = new string[theScript.mapSize.y];
		for (int i = theScript.mapSize.y - 1; i >= 0; --i)
			for (int j = theScript.mapSize.x - 1; j >= 0; --j)
				theScript.mapAsset.serrialized[i] += theScript.mapAsset.mapTable[theScript.mapSize.x - 1 - j, theScript.mapSize.y - 1 - i];

		EditorUtility.SetDirty(theScript.mapAsset);
		AssetDatabase.SaveAssets();
	}

	public void ClearLog() {
		Assembly assembly = Assembly.GetAssembly(typeof(Editor));
		System.Type type = assembly.GetType("UnityEditor.LogEntries");
		MethodInfo method = type.GetMethod("Clear");
		method.Invoke(new object(), null);
	}
}
