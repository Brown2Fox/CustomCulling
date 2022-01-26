using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(SnapPointSpawner))]
public class SnapPointSpawnerEditor : Editor
{
	public int itemCount;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		SnapPointSpawner the = (SnapPointSpawner)target;

		// Возможно я перепутал Vector3.right и Vector3.forward
		if (GUILayout.Button("Spawn"))
			the.Spawn(itemCount);
	}
}
#endif





public class SnapPointSpawner : MonoBehaviour
{
    public AnimationCurve lineY;
    public AnimationCurve lineZ;

    public float lenght;

	public GameObject pref;
	List<GameObject> spawned = new List<GameObject>();
	

	public List<SnapZone> Spawn(int giftsCount) {

		List<SnapZone> snapZones = new List<SnapZone>();
		
		// Уничтожаем заспавненые ранее точки (если такие были)
		if (spawned.Count > 0) foreach (GameObject one in spawned) Destroy(one);
		spawned = new List<GameObject>();

		float half = lenght / 2;
		float stepForLine = (lenght / (giftsCount + 1)) / lenght;
		float step = (lenght / (giftsCount + 1));


		int iStart = 0;
		int iEnd = giftsCount;

		if (giftsCount % 2 == 0) {
			iStart++;
			iEnd ++;
		}

		for (int i = iStart; i < iEnd; i++) {

			// Получаем Х			
			float xLine = 0.5f;
			float xTrue = 0;
			if (i > 0) {
				float m = Mathf.Ceil(((float)i) / 2) * Mathf.Pow(-1, i);
				xLine += (stepForLine * m);
				xTrue += (step * m);
			}

			// Получаем Y 
			float y = lineY.Evaluate(xLine);

			// Получаем Z
			float z = lineZ.Evaluate(xLine);

			Transform newObj = Instantiate(pref).transform;
			newObj.SetParent(transform);
			newObj.localEulerAngles = Vector3.zero;
			newObj.localPosition = new Vector3(xTrue, y, z);

			spawned.Add(newObj.gameObject);
			snapZones.Add(newObj.GetComponentInChildren<SnapZone>());
		}

		foreach(SnapZone one in snapZones) one.Init();

		return snapZones;

	}

}
