using UnityEngine;

public class ClothBehaviour : MonoBehaviour {
	public Transform[] starters;
	private Transform[][] controlable;
	private Vector3[] initPoss;
	private Quaternion[] initRots;

	public int deepth = -1;
	public float shiftness = 1f;

	private void Awake() {
		Transform tempT;
		controlable = new Transform[starters.Length][];
		initPoss = new Vector3[starters.Length];
		initRots = new Quaternion[starters.Length];

		for (int i = starters.Length - 1; i >= 0; --i) {
			tempT = starters[i].GetChild(0);
			if (deepth < 0)
				for (deepth = 1; tempT.childCount > 0; ++deepth)
					tempT = tempT.GetChild(0);

			controlable[i] = new Transform[deepth];
			tempT = starters[i];
			for (int k = 0; k < deepth; ++k) {
				tempT = tempT.GetChild(0);
				controlable[i][k] = tempT;
			}

			initPoss[i] = transform.InverseTransformPoint(controlable[i][deepth - 1].position);
			initRots[i] = Quaternion.Inverse(controlable[i][deepth - 1].rotation) * controlable[i][deepth - 1].rotation;
		}
	}

	public void MakeCurveFromNormals(Vector3 _start, Vector3 _end) {
		for (int i = controlable.Length - 1; i >= 0; --i)
			for (int j = deepth - 1; j >= 0; --j)
				controlable[i][j].position = Vector3.Lerp(starters[i].position, transform.TransformPoint(initPoss[i]), (j + 1f) / deepth);

		Vector3 dir, shift;
		float centerness;
		for (int i = deepth - 2; i >= 0; --i) {
			dir = Vector3.Lerp(_start, _end, (i + 1f) / deepth);
			centerness = 1 - Mathf.Abs(i + 1 - deepth * .5f) * 2 / deepth;
			shift = dir.normalized * (_start - _end).magnitude * shiftness * centerness;
			for (int j = controlable.Length - 1; j >= 0; --j) {
				controlable[j][i].position += shift;
			}
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.green;
		if (controlable != null)
			for (int i = controlable.Length - 1; i >= 0; --i)
				for (int j = deepth - 1; j >= 0; --j)
					Gizmos.DrawSphere(controlable[i][j].position, .003f);
		for (int i = starters.Length - 1; i >= 0; --i)
			Gizmos.DrawSphere(starters[i].position, .005f);
	}
}
