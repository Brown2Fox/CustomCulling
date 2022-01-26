// using System;
// using UnityEditor;
// using UnityEngine;
//
// [CustomEditor(typeof(AbilityMergeController))]
// public class AbilityCrossTableEditor : Editor {
// 	private AbilityMergeController theScript;
// 	private int size;
// 	private string[] labels;
// 	private MergedAbilityAsset tempGameObject;
//
// 	public MergedAbilityAssetsArray[] table;
//
//
// 	private void OnEnable() {
// 		labels = Enum.GetNames(typeof(GemAbilityType));
// 	}
//
//
// 	public override void OnInspectorGUI() {
// 		base.OnInspectorGUI();
// 		theScript = (AbilityMergeController) target;
// 		size = labels.Length;
// 		if (table == null || table.Length != size) {
// 			table = ReinitTable();
// 			EditorUtility.SetDirty(target);
// 			AssetDatabase.SaveAssets();
// 		}
//
// 		// Draw top labels
// 		GUILayout.BeginHorizontal();
// 		GUILayout.Label("", GUILayout.Width(70f));
// 		for (int i = 0; i < size; ++i)
// 			GUILayout.Label(labels[i], GUILayout.Width((EditorGUIUtility.currentViewWidth - 80f) / size));
// 		GUILayout.EndHorizontal();
//
// 		// Draw the table
// 		for (int i = 0; i < size; ++i) {
// 			GUILayout.BeginHorizontal();
// 			GUILayout.Label(labels[i], GUILayout.Width(70f));
// 			for (int j = 0; j < size; ++j) {
// 				// AbilityType_Extension a = ()
//
//
// 				tempGameObject = (MergedAbilityAsset) EditorGUILayout.ObjectField(table[i].array[j], typeof(MergedAbilityAsset), false);
// 				if (tempGameObject != table[i].array[j]) {
// 					table[i].array[j] = tempGameObject;
// 					EditorUtility.SetDirty(target);
// 					AssetDatabase.SaveAssets();
// 				}
// 			}
//
// 			GUILayout.EndHorizontal();
// 		}
// 	}
//
//
// 	private MergedAbilityAssetsArray[] ReinitTable() {
// 		MergedAbilityAssetsArray[] newTable = new MergedAbilityAssetsArray[size];
// 		for (int i = size - 1; i >= 0; --i)
// 			newTable[i] = new MergedAbilityAssetsArray {
// 														   array = new MergedAbilityAsset[size]
// 													   };
//
// 		if (table != null) {
// 			int oldSize = Math.Min(table.Length, size);
// 			for (int i = 0; i < oldSize; ++i)
// 			for (int j = 0; j < oldSize; ++j)
// 				newTable[i].array[j] = table[i].array[j];
// 		}
//
// 		return newTable;
// 	}
// }