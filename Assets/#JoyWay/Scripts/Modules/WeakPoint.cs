using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

public class WeakPoint : MonoBehaviour {
	[SerializeField]
	private Owner owner;

	public Action onDestroy;

	public void Restore(Owner _owner) {
		owner.health.Reset();
		owner.RemoveOwner();
		owner.SetOwner(_owner);
		gameObject.SetActive(true);
	}

	public void Destroyed() {
		onDestroy?.Invoke();
		gameObject.SetActive(false);
	}

	private void OnDisable() {
		onDestroy = null;
	}
}


#if UNITY_EDITOR

[CustomEditor(typeof(WeakPoint))]
public class WeakPointEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		if (GUILayout.Button("CopyRendeder")) {
			CopyParent();
		}
	}

	private void CopyParent() {
		var wp = target as WeakPoint;

		var parent = wp.transform.parent;

		wp.gameObject.GetComponent<MeshRenderer>().sharedMaterial = parent.gameObject.GetComponent<MeshRenderer>().sharedMaterial;

		wp.gameObject.GetComponent<MeshCollider>().sharedMesh = wp.gameObject.GetComponent<MeshFilter>().sharedMesh = parent.gameObject.GetComponent<MeshFilter>().sharedMesh;
	}
}


#endif