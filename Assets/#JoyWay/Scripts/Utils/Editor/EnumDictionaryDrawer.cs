// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
//
// // IngredientDrawer
// [CustomPropertyDrawer(typeof(EnumDictionaryAttribute))]
// public class EnumDictionaryDrawer : PropertyDrawer {
// 	// Draw the property inside the given rect
// 	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
// 		var att = attribute as EnumDictionaryAttribute;
// 		int count = att.values.Length;
// 		int full_width = count + 2;
//
// 		var me = property;
//
// 		var obj = property.serializedObject;
//
//
// 		position = new Rect(position.x, position.y, position.width, y_offset * full_width);
//
// 		EditorGUI.BeginProperty(position, label, property);
//
// 		// Draw label
// 		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
//
// 		// Don't make child fields be indented
// 		var indent = EditorGUI.indentLevel;
// 		EditorGUI.indentLevel = 0;
//
//
// 		//set names in top
// 		for (int i = 0; i < count; i++) {
// 			// EditorGUI.LabelField();
// 		}
//
// 		for (int i = 0; i < count; i++) {
// 			for (int j = 0; j < count; j++) {
// 				// var asset = EditorGUI.ObjectField(GetPosition(i+1,j+1), )
// 			}
// 		}
//
// 		//
// 		// // Draw fields - passs GUIContent.none to each so they are drawn without labels
// 		// EditorGUI.PropertyField(type, property.FindPropertyRelative("type"), GUIContent.none);
// 		// EditorGUI.PropertyField(asset, property.FindPropertyRelative("asset"), GUIContent.none);
//
// 		// Set indent back to what it was
// 		EditorGUI.indentLevel = indent;
//
// 		EditorGUI.EndProperty();
// 	}
//
//
// 	private int x_offset, y_offset = 20;
//
// 	private Rect GetPosition(int i, int j) {
// 		return default;
// 	}
// }