using UnityEngine;

public class CapsuleCasterTool : MonoBehaviour {
	// TODO: remove {
	private static CapsuleCasterTool inst;
	public Transform support;

	public bool d_tool;
	public float d_maxDistance;
	public LayerMask d_layerMask;
	private Vector3 d_result;
	// }

	// Technical variables
	private static Vector3 result;
	private static float hitRadius;
	private static RaycastHit hit;

	private void Awake() {
		if (inst == null)
			inst = this;
		else
			Destroy(gameObject);
	}

	private void FixedUpdate() {
		if (d_tool) return;
		d_result = MakeCast(transform.position + transform.up * transform.localScale.y / 2f, transform.position - transform.up * transform.localScale.y / 2f, transform.localScale.x / 2f, transform.forward, d_maxDistance, d_layerMask);
	}

	public static Vector3 MakeCast(Vector3 _p1, Vector3 _p2, float _radius, Vector3 _direction, float _distance, LayerMask _layerMask) {
		inst.support.position = (_p1 + _p2) / 2f;
		inst.support.LookAt(inst.support.position + _direction, _p2 - _p1);

		if (!Physics.CapsuleCast(_p1, _p2, _radius, _direction, out hit, _distance, _layerMask))
			return (_p1 + _p2) / 2f + _direction * _distance;

		result = hit.point;
		result = inst.support.InverseTransformPoint(result);
		hitRadius = Mathf.Abs(result.y) - (_p2 - _p1).magnitude / 2f;
		if (hitRadius < 0)
			hitRadius = _radius;
		else
			hitRadius = Mathf.Sqrt(_radius * _radius - hitRadius * hitRadius);
		result = new Vector3(0f, 0f, result.z - Mathf.Sqrt(hitRadius * hitRadius - result.x * result.x));
		result = inst.support.TransformPoint(result);
		return result;
	}

	private void OnDrawGizmos() {
		if (inst == null || d_tool) return;
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(hit.point, 0.03f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(d_result + transform.up * transform.localScale.y / 2f, transform.localScale.x / 2f);
		Gizmos.DrawWireSphere(d_result, transform.localScale.x / 2f);
		Gizmos.DrawWireSphere(d_result - transform.up * transform.localScale.y / 2f, transform.localScale.x / 2f);
	}
}
