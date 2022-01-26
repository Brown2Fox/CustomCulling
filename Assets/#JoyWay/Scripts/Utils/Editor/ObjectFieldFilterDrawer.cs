using UnityEditor;
using UnityEngine;

/// <summary>
/// Allow to display an attribute in inspector without allow editing
/// </summary>
[CustomPropertyDrawer(typeof(ObjectFieldFilterAttribute))]
public class ObjectFieldFilterDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var att = attribute as ObjectFieldFilterAttribute;

        EditorGUI.ObjectField(position, property, att.T, label);
    }
}