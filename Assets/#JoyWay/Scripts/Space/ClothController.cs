using UnityEngine;

public class ClothController : MonoBehaviour {
	public Transform
		rootPoint, midPoint, endPoint,
		midCenter, endCenter;
	public float midDist = .05f, endDist = .1f;

	public bool mathLock;

	public ClothBehaviour upCloth, downCloth;

	private Vector3
		flatNormal,
		rootNormal, midNormal, endNormal,
		t_pos;

	private void FixedUpdate() {
		if (mathLock) {
			t_pos = midPoint.position - midCenter.position;
			if (t_pos.magnitude > midDist)
				midPoint.position = midCenter.position + t_pos.normalized * midDist;

			t_pos = endPoint.position - endCenter.position;
			if (t_pos.magnitude > endDist)
				endPoint.position = endCenter.position + t_pos.normalized * endDist;
		}

		t_pos = midPoint.transform.localPosition;
		t_pos.x = 0;
		midPoint.transform.localPosition = t_pos;

		t_pos = endPoint.transform.localPosition;
		t_pos.x = 0;
		endPoint.transform.localPosition = t_pos;

		flatNormal = Vector3.Cross(rootPoint.position - midPoint.position, midPoint.position - endPoint.position);
		//flatNormal = rootPoint.right;
		rootNormal = Vector3.Cross(flatNormal, rootPoint.position - midPoint.position);
		endNormal = Vector3.Cross(flatNormal, midPoint.position - endPoint.position);
		midNormal = (rootNormal + endNormal) * .5f;

		upCloth.MakeCurveFromNormals(rootNormal, midNormal);
		downCloth.MakeCurveFromNormals(midNormal, endNormal);
	}

	private void OnDrawGizmos() {
		if (rootPoint == null || midPoint == null || endPoint == null) return;
		Gizmos.color = Color.red;
		Gizmos.DrawLine(rootPoint.position, rootPoint.position + rootNormal);
		Gizmos.DrawLine(midPoint.position, midPoint.position + midNormal);
		Gizmos.DrawLine(endPoint.position, endPoint.position + endNormal);
	}
}
