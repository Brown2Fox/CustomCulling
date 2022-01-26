using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InGameLogFormer))]
public class InGameLogFormerEditor : Editor {
	private string text;
	private Color color = Color.white;

	private InGameLogFormer theSctipt;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		theSctipt = (InGameLogFormer)target;

		GUILayout.BeginHorizontal();

		text = EditorGUILayout.TextField(text);
		color = EditorGUILayout.ColorField(color, GUILayout.Width(60f));

		if (GUILayout.Button("Log it", GUILayout.Width(50f)))
			theSctipt.AddLine(text, color);
		GUILayout.EndHorizontal();
	}
}
