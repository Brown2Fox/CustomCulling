using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(SkinnedMeshRendererBoneEditor))]
public class SkinnedMeshRendererBoneEditorEditor : Editor {
	public override void OnInspectorGUI() {
		SkinnedMeshRendererBoneEditor theScript = (SkinnedMeshRendererBoneEditor)target;
		if (theScript.target == null)
			theScript.currentBones = null;
		else
			theScript.currentBones = theScript.target.bones;
		base.OnInspectorGUI();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Auto fill using \"Root Bone\"")) {
			theScript.newBones = new Transform[theScript.currentBones.Length];
			Transform
				rootToFind = theScript.target.rootBone,
				candidate;
			if (rootToFind != null)
				for (int i = theScript.currentBones.Length - 1; i >= 0; --i) {
					if (theScript.currentBones[i] == null) continue;
					candidate = RecFind(rootToFind, theScript.currentBones[i].name);
					if (candidate == null) continue;
					theScript.newBones[i] = candidate;
				}
		}
		if (GUILayout.Button("Repeat current bones")) {
			theScript.newBones = new Transform[theScript.currentBones.Length];
			Array.Copy(theScript.currentBones, 0, theScript.newBones, 0, theScript.currentBones.Length);
		}
		if (GUILayout.Button("Update bones"))
			theScript.target.bones = theScript.newBones;
		GUILayout.EndHorizontal();
	}

	private Transform RecFind(Transform _target, string _name) {
		if (_target.name == _name)
			return _target;
		Transform candidate;
		for (int i = _target.childCount - 1; i >= 0; --i) {
			candidate = RecFind(_target.GetChild(i), _name);
			if (candidate != null)
				return candidate;
		}
		return null;
	}
}
