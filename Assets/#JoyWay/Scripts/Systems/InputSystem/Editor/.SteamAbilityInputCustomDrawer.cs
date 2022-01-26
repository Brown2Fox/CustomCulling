using UnityEditor;
using UnityEngine;
using Valve.VR;

// IngredientDrawer
namespace JoyWay.Systems.InputSystem.Editor {
	[CustomPropertyDrawer(typeof(SteamToAbilityInput))]
	public class SteamAbilityInputCustomDrawer : PropertyDrawer {
		// Draw the property inside the given rect
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			// Using BeginProperty / EndProperty on the parent property means that
			// prefab override logic works on the entire property.
			EditorGUI.BeginProperty(position, label, property);

			// Draw label
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			// Don't make child fields be indented
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			float w5 = position.width * .5f;


			// Calculate rects
			var ability = new Rect(position.x, position.y, w5 - 5f, position.height);
			var action = new Rect(position.x + w5 + 5f, position.y, w5 - 5f, position.height);

			// Draw fields - passs GUIContent.none to each so they are drawn without labels
			EditorGUI.PropertyField(ability, property.FindPropertyRelative("abilityButton"), GUIContent.none);

			var defWidth = EditorGUIUtility.labelWidth;
			var svriabpd = new SteamVR_Input_Action_Boolean_PropertyEditor();

			EditorGUIUtility.labelWidth = 0;
			svriabpd.OnGUI_NoLabel(action, property.FindPropertyRelative("steamAction"), GUIContent.none);

			EditorGUIUtility.labelWidth = defWidth;
			// Set indent back to what it was
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}
	}
}