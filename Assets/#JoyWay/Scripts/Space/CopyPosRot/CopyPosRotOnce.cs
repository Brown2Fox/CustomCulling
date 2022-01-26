using UnityEngine;

public class CopyPosRotOnce : MonoBehaviour {
	public void CopyPosRot(Transform _target) {
		_target.CopyGlobalPosRot(transform);
	}
}
