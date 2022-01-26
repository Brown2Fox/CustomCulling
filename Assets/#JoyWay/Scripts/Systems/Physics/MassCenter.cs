using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassCenter : MonoBehaviour {
	public Vector3 point;
	public Transform target;

	private void Awake() {
		if (target == null)
			GetComponent<Rigidbody>().centerOfMass = point;
		else
			GetComponent<Rigidbody>().centerOfMass = transform.InverseTransformPoint(target.position);
	}
}
