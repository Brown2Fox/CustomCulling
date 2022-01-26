using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(NavMeshLinkMarker))]
public class NavMeshLinkMarkerEditor : Editor {
	private GameObject theScriptObject;

	private static GameObject
		a, b, c;

	private static bool autoMode = true;

	public override void OnInspectorGUI() {
		theScriptObject = ((NavMeshLinkMarker)target).gameObject;

		if (autoMode) {
			if (a != theScriptObject && b != theScriptObject && c != theScriptObject)
				if (a == null)
					a = theScriptObject;
				else if (b == null)
					b = theScriptObject;
				else if (c == null)
					c = theScriptObject;
		}

		GUILayout.BeginHorizontal();
		{
			GUILayout.BeginVertical();
			{
				a = (GameObject)EditorGUILayout.ObjectField(a, typeof(GameObject), true);

				GUILayout.BeginHorizontal();
				if (GUILayout.Button("A"))
					a = theScriptObject;
				if (GUILayout.Button("X"))
					a = null;
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();

			GUILayout.BeginVertical();
			{
				b = (GameObject)EditorGUILayout.ObjectField(b, typeof(GameObject), true);

				GUILayout.BeginHorizontal();
				if (GUILayout.Button("B"))
					b = theScriptObject;
				if (GUILayout.Button("X"))
					b = null;
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();

			GUILayout.BeginVertical();
			{
				c = (GameObject)EditorGUILayout.ObjectField(c, typeof(GameObject), true);

				GUILayout.BeginHorizontal();
				if (GUILayout.Button("C"))
					c = theScriptObject;
				if (GUILayout.Button("X"))
					c = null;
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		autoMode = GUILayout.Toggle(autoMode, "Auto mode");
		if (GUILayout.Button("Объединить"))
			Unite();
		GUILayout.EndHorizontal();
	}

	private void Unite() {
		if (a == b || b == c || a == null || b == null || c == null)
			return;

		GameObject go = new GameObject("NewMeshLink", typeof(MeshRenderer), typeof(NavMeshLink));
		NavMeshLink link = go.GetComponent<NavMeshLink>();

		Vector3
			m = (a.transform.position + b.transform.position) * .5f,
			l = b.transform.position - a.transform.position;

		go.transform.parent = a.transform.parent;
		go.transform.SetSiblingIndex(a.transform.GetSiblingIndex());
		go.transform.position = m;

		Vector3 newForw = Vector3.Cross(l, Vector3.up);
		if (Vector3.Angle(newForw, c.transform.position - go.transform.position) > 90f)
			newForw *= -1;
		go.transform.rotation = Quaternion.LookRotation(newForw, Vector3.Cross(newForw, l));

		link.width = l.magnitude;
		link.startPoint = Vector3.zero;
		Vector3 e = go.transform.InverseTransformPoint(c.transform.position);
		e.x = 0f;
		link.endPoint = e;

		DestroyImmediate(a);
		DestroyImmediate(b);
		DestroyImmediate(c);

		Selection.objects = new GameObject[] { go };
	}
}
