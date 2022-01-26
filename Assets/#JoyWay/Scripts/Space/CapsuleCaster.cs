using UnityEngine;
using UnityEngine.Profiling;

public class CapsuleCaster : MonoBehaviour {
	public static Vector3 GetCastShift(bool _fromFloor, Transform _t, CapsuleCollider _capsule, Vector3 _offset, LayerMask _layerMask, float _heightReduce = 0f, float _radiusAjust = 0f, float _heightAjust = 0f, float _stepBack = 0f) {
		Profiler.BeginSample("CapsuleCaster.MakeCastFromFloor");

		Vector3 p, p1, p2;

		_offset += _offset.normalized * _stepBack;
		Vector3 centerOffset = _capsule.transform.TransformVector(_capsule.center);
		p = _t.position + centerOffset - _offset.normalized * _stepBack;

		float
			ajustedRad = _capsule.radius + _radiusAjust,
			ajustedHeigh = _capsule.height + _heightAjust;
		p1 = p + _t.up * Mathf.Max(ajustedHeigh * .5f - ajustedRad, .01f);
		p2 = p - _t.up * Mathf.Max(ajustedHeigh * .5f - ajustedRad, .01f);

		// TODO: Соптимизировать это, переместив урезание высоты в поправку высоты
		float curHeight = (p1 - p2).magnitude;
		if (curHeight - .01f < _heightReduce)
			_heightReduce = curHeight - .01f;

		if (_fromFloor)
			p2 += _t.up * _heightReduce;
		else {
			p1 -= _heightReduce * .5f * _t.up;
			p2 += _heightReduce * .5f * _t.up;
		}

		p = MakeCast(p1, p2, ajustedRad, _offset.normalized, _offset.magnitude, _layerMask) - _capsule.center;

		Profiler.EndSample();

		return p - _t.position - centerOffset;
	}

	public static Vector3 MakeCast(Vector3 _p1, Vector3 _p2, float _radius, Vector3 _offset, float _distance, LayerMask _layerMask) {
		PosRot support, result;
		float hitRadius;

		support = new PosRot {
								 pos = (_p1 + _p2) * .5f,
								 rot = Quaternion.LookRotation(_offset.sqrMagnitude < .000001f ? Vector3.forward : _offset, _p2 - _p1)
							 };

		if (!Physics.CapsuleCast(_p1, _p2, _radius, _offset, out RaycastHit hit, _distance, _layerMask))
			return (_p1 + _p2) / 2f + _offset * _distance;

		result = new PosRot {
								pos = hit.point,
								rot = Quaternion.identity
							};
		result = support.GetLocalOfChild(result);
		hitRadius = Mathf.Abs(result.pos.y) - (_p2 - _p1).magnitude / 2f;
		if (hitRadius < 0)
			hitRadius = _radius;
		else
			hitRadius = Mathf.Sqrt(_radius * _radius - hitRadius * hitRadius);
		result.pos = new Vector3(0f, 0f, result.pos.z - Mathf.Sqrt(hitRadius * hitRadius - result.pos.x * result.pos.x));
		result = support.GetGlobalOfChild(result);
		return result.pos;
	}
}
