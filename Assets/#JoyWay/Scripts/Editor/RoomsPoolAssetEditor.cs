using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomsPoolAsset))]
public class RoomsPoolAssetEditor : Editor {
	private RoomsPoolAsset theScript;
	private List<bool> showWay = new List<bool>();

	private Vector2Int tempV2I;
	private RoomType tempRT;
	private SceneAsset tempS;
	private RotateMeasure tempRM;

	public override void OnInspectorGUI() {
		theScript = (RoomsPoolAsset)target;
		if (showWay.Count != theScript.variants.Count) {
			showWay = new List<bool>();
			for (int i = theScript.variants.Count; i > 0; --i)
				showWay.Add(true);
		}

		tempV2I = EditorGUILayout.Vector2IntField("Size", theScript.size);
		if (theScript.size != tempV2I) {
			theScript.size = tempV2I;
			EditorUtility.SetDirty(theScript);
		}

		tempRT = (RoomType)EditorGUILayout.EnumPopup(theScript.roomType);
		if (theScript.roomType != tempRT) {
			theScript.roomType = tempRT;
			for (int i = theScript.variants.Count - 1; i >= 0; --i)
				theScript.variants[i].roomType = tempRT;
			EditorUtility.SetDirty(theScript);
		}

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Hide all ways")) {
			for (int i = showWay.Count - 1; i >= 0; --i)
				showWay[i] = false;
		}
		if (GUILayout.Button("Show all ways")) {
			for (int i = showWay.Count - 1; i >= 0; --i)
				showWay[i] = true;
		}
		GUILayout.EndHorizontal();

		for (int i = 0; i < theScript.variants.Count; ++i) {
			GUILayout.BeginHorizontal();

			GUILayout.BeginVertical(GUILayout.Width(250));
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("X", GUILayout.Width(38f), GUILayout.Height(38f))) {
				theScript.variants.RemoveAt(i);
				showWay.RemoveAt(i);
				return;
			}
			GUILayout.BeginVertical();
			tempS = (SceneAsset)EditorGUILayout.ObjectField(theScript.variants[i].roomScene, typeof(SceneAsset), false);
			if (theScript.variants[i].roomScene != tempS) {
				theScript.variants[i].roomScene = tempS;
				EditorUtility.SetDirty(theScript);
			}

			GUILayout.BeginHorizontal();
			GUILayout.Label("Rotation");
			tempRM = (RotateMeasure)EditorGUILayout.EnumPopup(theScript.variants[i].modelRotation);
			if (theScript.variants[i].modelRotation != tempRM) {
				theScript.variants[i].modelRotation = tempRM;
				EditorUtility.SetDirty(theScript);
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();

			if (showWay[i]) {
				if (GUILayout.Button("0", GUILayout.Width(38f), GUILayout.Height(38f))) showWay[i] = false;
			} else {
				if (GUILayout.Button("I", GUILayout.Width(38f), GUILayout.Height(38f))) showWay[i] = true;
			}

			GUILayout.EndHorizontal();

			GUILayout.Label(theScript.variants[i].roomSceneName);
			GUILayout.EndVertical();

			if (showWay[i]) {
				for (int j = 0; j < theScript.size.x + 2; ++j) {
					GUILayout.BeginVertical(GUILayout.Width(17f));
					for (int k = theScript.size.y + 1; k >= 0; --k) {
						if (j == 0 && (k == 0 || k == theScript.size.y + 1) || j == theScript.size.x + 1 && (k == 0 || k == theScript.size.y + 1) ||
							j > 0 && k > 0 && j < theScript.size.x + 1 && k < theScript.size.y + 1) {
							SetEditable(false);
							EditorGUILayout.Toggle(false);
						} else {
							SetEditable(true);
							WayConnection wc = MakeWayByToPoint(j - 1, k - 1);
							if (theScript.variants[i].ways.Contains(wc)) {
								if (!EditorGUILayout.Toggle(true)) {
									theScript.variants[i].ways.Remove(wc);
									EditorUtility.SetDirty(theScript);
								}
							} else {
								if (EditorGUILayout.Toggle(false)) {
									theScript.variants[i].ways.Add(wc);
									EditorUtility.SetDirty(theScript);
								}
							}
						}
					}
					GUILayout.EndVertical();
				}
			}
			SetEditable(true);

			GUILayout.EndHorizontal();
			if (showWay[i])
				GUILayout.Space(5f);
		}
		if (GUILayout.Button("+")) {
			theScript.variants.Add(new RoomData(theScript.size, theScript.roomType));
			showWay.Add(false);
			EditorUtility.SetDirty(theScript);
		}
	}

	private bool editable = false;
	private void SetEditable(bool _state) {
		if (_state == editable) return;
		editable = _state;
		if (editable)
			EditorGUI.EndDisabledGroup();
		else
			EditorGUI.BeginDisabledGroup(true);
	}

	private bool CheckWay(List<WayConnection> _ways, int _i, int _j) {
		return _ways.Contains(MakeWayByToPoint(_i - 1, _j - 1));
	}

	private WayConnection MakeWayByToPoint(int _i, int _j) {
		Vector2Int
			wayFrom,
			wayTo = new Vector2Int(_i, _j);
		if (_i == -1)
			wayFrom = new Vector2Int(0, _j);
		else if (_j == -1)
			wayFrom = new Vector2Int(_i, 0);
		else if (_i == theScript.size.x)
			wayFrom = new Vector2Int(_i - 1, _j);
		else
			wayFrom = new Vector2Int(_i, _j - 1);

		return new WayConnection(wayFrom, wayTo);
	}
}
