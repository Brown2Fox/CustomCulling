using UnityEditor;
using UnityEngine;

public class StaticConstsWindow : EditorWindow {
	[MenuItem("Window/JoyWayTools/Some static constants", priority = 110)]
	public static void ShowWindow() {
		GetWindow<StaticConstsWindow>("Some static constants");
	}

	private void OnGUI() {
		RoomController.fontSizeMultiplier = EditorGUILayout.Slider("RoomControlle'r fontSize", RoomController.fontSizeMultiplier, 0, 20000);
		if (GUILayout.Button("Mark Chunks Scene Names")) MarkChunksSceneNames();
	}

	private void MarkChunksSceneNames() {
		RoomsPoolAsset[] roomsPoolAssets = Resources.FindObjectsOfTypeAll<RoomsPoolAsset>();
		for (int i = roomsPoolAssets.Length - 1; i >= 0; --i) {
			for (int j = roomsPoolAssets[i].variants.Count - 1; j >= 0; --j) {
				roomsPoolAssets[i].variants[j].UpdateSceneName();
				EditorUtility.SetDirty(roomsPoolAssets[i]);
			}
		}
	}
}
