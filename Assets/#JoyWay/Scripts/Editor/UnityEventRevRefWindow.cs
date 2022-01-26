using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class UnityEventRevRefWindow : EditorWindow {
	private MonoBehaviour[] mbs;
	private bool
		isActive = false,
		notButton = false;
	private int
		notButtonCount = 0,
		globalIndex;

	private void Awake() {
		Clear();
		//GetReflections();
	}
	private void OnDestroy() {
		Clear();
	}

	[MenuItem("Window/JoyWayTools/UnityEvent's Revert References", priority = 100)]
	public static void ShowWindow() {
		GetWindow<UnityEventRevRefWindow>("Revert References");
	}

	private void OnGUI() {
		GUILayout.Label(
			"Обратные ссылки на UnityEvent'ы в RevRef'ах.\n" +
			"Перед билдом или коммитом отчищайте проект.\n" +
			"Не забывайте обновлять разметку.");

		if (notButtonCount > 0) {
			GUI.color = Color.yellow;
			notButton = GUILayout.Button("Это не кнопка.");
			--notButtonCount;
		} else if (isActive) {
			GUI.color = Color.red;
			notButton = GUILayout.Button("Проект размечен.");
		} else {
			GUI.color = Color.green;
			notButton = GUILayout.Button("Проект чист.");
		}
		GUI.color = Color.white;
		if (notButton)
			notButtonCount = 5;

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Разметить/Обновить")) {
			Clear();
			GetReflections();
		}
		if (GUILayout.Button("Отчистить"))
			Clear();
		EditorGUILayout.EndHorizontal();

	}

	private void GetReflections() {
		mbs = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
		for (globalIndex = mbs.Length - 1; globalIndex >= 0; --globalIndex) {
			System.Reflection.FieldInfo[] fields = mbs[globalIndex].GetType().GetFields();
			for (int j = fields.Length - 1; j >= 0; --j)
				if (fields[j].FieldType.IsSubclassOf(typeof(UnityEventBase)))
					ProcessUnityEventBase((UnityEventBase)fields[j].GetValue(mbs[globalIndex]), mbs[globalIndex]);
		}
		isActive = true;
	}

	public void ProcessUnityEventBase(UnityEventBase _event, MonoBehaviour _mb) {
		for (int i = _event.GetPersistentEventCount() - 1; i >= 0; --i) {
			GameObject targetObject;
			Object tempObject = _event.GetPersistentTarget(i);
			if (tempObject == null)
				continue;
			if (tempObject is Component)
				targetObject = ((Component)tempObject).gameObject;
			else if (tempObject is GameObject)
				targetObject = (GameObject)tempObject;
			else {
				Debug.Log($"Unknown target! From {_mb.name}, PersistentEvent #{i}.", _mb);
				continue;
			}

			RevRef rr = targetObject.GetComponent<RevRef>();
			if (!rr)
				rr = targetObject.AddComponent<RevRef>();
			if (!rr.references.Contains(_mb))
				rr.references.Add(_mb);
		}
	}

	public void Clear() {
		RevRef[] objs = Resources.FindObjectsOfTypeAll<RevRef>();
		for (int i = objs.Length - 1; i >= 0; --i)
			DestroyImmediate(objs[i], true);
		mbs = new MonoBehaviour[0];
		isActive = false;
	}
}
