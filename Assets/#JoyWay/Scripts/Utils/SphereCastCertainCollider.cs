using UnityEngine;

public class SphereCastCertainCollider {
	public static RaycastHit Catch(Collider _collider, Vector3 _prevPos, Vector3 _newPos, float _radius, float _safeAdd = 0.5f) {
		Vector3 dif = _newPos - _prevPos;
		if (_safeAdd > 0f) {
			_safeAdd += _radius;
			_prevPos -= dif.normalized * _safeAdd;
			//_radius += _safeAdd;
		}
		RaycastHit[] hits = Physics.SphereCastAll(_prevPos, _radius, dif, dif.magnitude + 2f * _safeAdd, 1 << _collider.gameObject.layer, QueryTriggerInteraction.Collide);
		for (int i = hits.Length - 1; i >= 0; --i)
			if (hits[i].collider == _collider)
				return hits[i];

		return new RaycastHit();
	}
}
