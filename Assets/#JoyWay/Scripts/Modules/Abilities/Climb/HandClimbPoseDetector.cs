using System;
using UnityEngine;

public class HandClimbPoseDetector : MonoBehaviour {
	public RaycastHit hitDownFinger, hitDownPalm, hitFwdFinger, hitFwdPalm;
	public bool hitDF, hitDP, hitFF, hitFP;
	public Transform fingerPoint, palmPoint;
	public Vector3 fingerDown, palmDown;

	public float topDownCheckDist;

	public Animator animator;

	// private void LateUpdate() {
	// 	CheckTopDown();
	// }


	private PosRot detectorRootOffset;

	private void OnEnable() {
		detectorRootOffset = transform.Global().GetLocalOfChild(animator.transform.Global());
		CheckTopDown();
	}


	public void CheckTopDown() {
		fingerDown = -fingerPoint.up;
		palmDown = -palmPoint.up;
		hitDF = Physics.Raycast(fingerPoint.position, fingerDown, out hitDownFinger, topDownCheckDist, (int) FlaggedLayer.Vision);
		hitDP = Physics.Raycast(palmPoint.position, palmDown, out hitDownPalm, topDownCheckDist, (int) FlaggedLayer.Vision);
		if (!hitDF && !hitDP) {
			SetHandFree();
			return;
		}

		if (hitDF && hitDP) {
			CheckDoubleTop();
			return;
		}

		if (!hitDP) {
			GetPalmSideCast();
			return;
		}

		if (!hitDF) {
			GetFingerSideCast();
		}
	}


	public void SetAngleTo(float _angle) {
		animator.SetFloat("angle", _angle);
	}


	public void SetHandFree() {
		SetAngleTo(0f);
	}

	public void CheckDoubleTop() {
		if (Vector3.Angle(hitDownFinger.normal, hitDownPalm.normal) < 5f) {
			SetHandFree();
			return;
		}

		GetPalmSideCast();
	}

	public Vector3 vr, x, x2;
	public float fingerLength = .1f;
	public RaycastHit palmsideHit, fingerSideHit;
	public Vector3 palmPseudopoint;

	public void GetPalmSideCast() {
		vr = Vector3.Cross(fingerPoint.forward, hitDownFinger.normal);
		x = Vector3.Cross(vr, hitDownFinger.normal);
		palmPseudopoint = hitDownFinger.point + (x * fingerLength) + (hitDownFinger.normal * .1f);

		hitFP = Physics.Raycast(palmPseudopoint, -hitDownFinger.normal, out palmsideHit, .2f, (int) FlaggedLayer.Vision);

		if (!hitFP) {
			palmPseudopoint = hitDownFinger.point + (x * fingerLength) - (hitDownFinger.normal * .1f);
			hitFP = Physics.Raycast(palmPseudopoint, -x, out palmsideHit, fingerLength * .5f, (int) FlaggedLayer.Vision);
			if (!hitFP) {
				palmsideHit.normal = x;
				palmsideHit.point = palmPseudopoint + (x * (fingerLength * .5f));
			}
		}


		palmtofingerfwd = hitDownFinger.point - palmsideHit.point;
		palmfwd = Vector3.ProjectOnPlane(palmtofingerfwd, palmsideHit.normal);


		angle = Vector3.Angle(hitDownFinger.normal, palmsideHit.normal);

		SetAngleTo(angle);


		PosRot snapPoint;
		snapPoint.pos = palmsideHit.point;
		var fwd = Vector3.ProjectOnPlane(palmfwd, palmsideHit.normal);
		snapPoint.rot = Quaternion.LookRotation(fwd, palmsideHit.normal);

		animator.transform.CopyGlobalPosRot(snapPoint.GetGlobalOfChild(detectorRootOffset));
	}

	public float angle;
	Vector3 palmtofingerfwd, fingertofingerfwd;
	Vector3 palmfwd, fingerfwd;


	public void GetFingerSideCast() {
		vr = Vector3.Cross(palmPoint.forward, hitDownPalm.normal);
		x = Vector3.Cross(hitDownPalm.normal, vr);
		palmPseudopoint = hitDownPalm.point + (x * fingerLength) + (hitDownPalm.normal * .1f);

		hitFF = Physics.Raycast(palmPseudopoint, -hitDownPalm.normal, out fingerSideHit, .2f, (int) FlaggedLayer.Vision);

		if (!hitFF) {
			palmPseudopoint = hitDownPalm.point + (x * fingerLength) - (hitDownPalm.normal * .1f);
			hitFF = Physics.Raycast(palmPseudopoint, -x, out fingerSideHit, fingerLength * .5f, (int) FlaggedLayer.Vision);
			if (!hitFF) {
				fingerSideHit.normal = x;
				fingerSideHit.point = palmPseudopoint + (x * (fingerLength * .5f));
			}
		}


		fingertofingerfwd = hitDownPalm.point - fingerSideHit.point;
		fingerfwd = Vector3.ProjectOnPlane(fingertofingerfwd, fingerSideHit.normal);


		angle = Vector3.Angle(hitDownPalm.normal, fingerSideHit.normal);

		SetAngleTo(angle);
	}


	private void OnDisable() {
		animator.transform.LocalReset();
	}


	public float gizmoSize = .1f;

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(fingerPoint.position, fingerPoint.position + (fingerDown * topDownCheckDist));

		Gizmos.color = Color.green;
		Gizmos.DrawLine(hitDownFinger.point, hitDownFinger.point + hitDownFinger.normal * gizmoSize);

		Gizmos.color = Color.white;
		Gizmos.DrawLine(hitDownFinger.point, hitDownFinger.point + (vr * fingerLength));

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(hitDownFinger.point, hitDownFinger.point + (x * fingerLength));


		Gizmos.color = Color.red;
		Gizmos.DrawLine(palmPseudopoint, palmPseudopoint - palmfwd * gizmoSize);

		Gizmos.color = Color.green;
		Gizmos.DrawLine(palmPseudopoint, palmPseudopoint - palmsideHit.normal * gizmoSize);

		// Gizmos.color = Color.white;
		// Gizmos.DrawLine(hitDownFinger.point, hitDownFinger.point + (vr * fingerLength));
		//
		// Gizmos.color = Color.blue;
		// Gizmos.DrawLine(hitDownFinger.point, hitDownFinger.point + (x * fingerLength));

		// Gizmos.color = Color.magenta;
		// Gizmos.DrawLine(palmPseudopoint, palmsideHit.point);
	}
}