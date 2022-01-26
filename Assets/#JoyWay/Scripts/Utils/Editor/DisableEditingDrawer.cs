using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DisableEditingAttribute))]
public class DisableEditingDrawer : PropertyDrawer {
	/// <summary>
	/// Display attribute and his value in inspector depending on the type
	/// Fill attribute needed
	/// </summary>
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginDisabledGroup(true);
		EditorGUI.PropertyField(position, property, label);
		EditorGUI.EndDisabledGroup();
	}
}