#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(LevelLoadTrigger))]
public class LevelLoadTriggerEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();


		if (GUILayout.Button("Trigger")) {
			var me = target as LevelLoadTrigger;
			me.Trigger();
		}
	}
}


#endif


public class LevelLoadTrigger : BaseAreaTrigger {
	public ScenesList sceneToLoad;

	public override void Trigger() {
		LevelManager.Load(sceneToLoad);
	}
}