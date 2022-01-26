using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JoyWay.Systems.InputSystem {
#if UNITY_EDITOR
	[CustomEditor(typeof(InputAsset), true)]
	public class InputAssetEditor : Editor {
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			GUILayout.Space(5);
			if (GUILayout.Button("Reset to default")) {
				(target as InputAsset)?.SetDefault();
			}
		}
	}
#endif


	public abstract class InputAsset : ScriptableObject {
		public abstract void SetDefault();
	}
}
