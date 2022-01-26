using System;
using System.Collections.Generic;
using UnityEngine;

[AbilityType(AbilityType.Climb)]
public class ClimbDetector : AbilityHandPart<ClimbAbility> {
	public float
		distance = .3f,
		vDistance = .45f,
		tilt = 60f;

	public Transform
		firstCast, secondCast;

	public bool isTouching;
	private bool currentState;
	private float castStoppedTime = float.NegativeInfinity;
	private const float untouchableTime = .5f;

	[DisableEditing]
	public Vector3 normal;

	public List<Collider> touchers = new List<Collider>();

	private RaycastHit centerHit;
	private RaycastHit forwardHit;
	private RaycastHit downHit;
	private RaycastHit fingerHit, palmHit;


	public Transform
		handFollowPoint,
		frictioner;


	public PosRot handOffset;


	public ClimbDetector() {
		AbilityTriggerID = Animator.StringToHash("Climb");
		AbilityEndTriggerID = Animator.StringToHash("ClimbEnded");
	}


	public override void Init(HandSplitAbility _ability) {
		base.Init(_ability);
		handOffset = transform.Global().GetLocalOfChild(hand.transform);
	}

	protected override void Update() {
		base.Update();

		UpdateTouchState();

		if (predictionTimeLeft <= 0) return;

		if (CanClimb()) {
			PredictionComplete();
		}
		
	}

	public override void StartCast() {
		casting = true;
		if (CanClimb()) {
			base.StartCast();
			SetState(true);
		}
	}

	protected override bool CastCheck() {
		return false;
	}

	protected override void SuccessCast() {
		ability.PlaySound(ability.castAudio.successCast);
		BeautifyHandPose();
	}

	public override void EndCast() {
		base.EndCast();
		SetState(false);
	}

	public void TiggerEntered(Collider _collider) {
		if (!touchers.Contains(_collider)) {
			touchers.Add(_collider);
			UpdateTouchState();
			ability.UpdateClimbState();
		}
	}

	public void TriggerExited(Collider _collider) {
		touchers.Remove(_collider);
		UpdateTouchState();
	}

	private void UpdateTouchState() {
		if (Time.time < castStoppedTime + untouchableTime) {
			isTouching = false;
			return;
		}

		for (int i = touchers.Count - 1; i >= 0; --i) {
			if (touchers[i] == null || !touchers[i].gameObject.activeInHierarchy) {
				touchers.RemoveAt(i);
				continue;
			}

			if (CanTouch(touchers[i])) {
				if (!isTouching) {
					isTouching = true;
					snapPoint.pos = frictioner.position;
					ability.UpdateClimbState();
				}

				return;
			}
		}

		if (isTouching) {
			isTouching = false;
			ability.UpdateClimbState();
		}
	}


	private bool CanTouch(Collider _collider) {
		Vector3 closestPoint = _collider.ClosestPointOnBounds(frictioner.position);
		if (!Physics.Raycast(frictioner.position, closestPoint - frictioner.position, out RaycastHit hit, 1f, 1 << _collider.gameObject.layer, QueryTriggerInteraction.Ignore))
			return false;
		if (hit.normal.y < tilt) return false;
		if (hit.collider.material != null && hit.collider.material.dynamicFriction <= .01f) return false;
		return true;
	}


	public bool TiltCheck() => centerHit.normal.y > tilt;
	public bool TiltCheck(RaycastHit _hit) => _hit.normal.y > tilt;

	private bool CanClimb() {
		if (!FirstCast()) {
			if (!SecondCast(transform.position)) {
				return false;
			}
		}


		SuccessCast();

		return true;
	}

	private Collider[] overlappedColliders = new Collider[20];
	private int k;

	Collider nearestGood;

	public bool CheckOverlapped() {
		nearestGood = null;
		Vector3 pos = transform.position;
		float dist2 = Single.PositiveInfinity;
		RaycastHit nearestHit = default;
		for (int i = 0; i < k; i++) {
			var col = overlappedColliders[i];
			Vector3 point = col.ClosestPoint(pos);
			var v = point - pos;


			if (!Physics.Raycast(pos, v, out centerHit, distance, (int) FlaggedLayer.Vision)) continue;
			forwardHit = centerHit;

			if (!TiltCheck(centerHit)) {
				deltaPos = -Vector3.Project(v, centerHit.normal);
				if (!SecondCast(centerHit.point)) continue;
			}

			var newdist = (centerHit.point - pos).sqrMagnitude;
			if (dist2 > newdist) {
				nearestGood = col;
				nearestHit = centerHit;
				dist2 = newdist;
			}
		}
		if (nearestGood != null) centerHit = nearestHit;
		return nearestGood != null;
	}


	private bool FirstCast() {
		Vector3 pos = transform.position;
		if (isTouching) {
			k = Physics.OverlapSphereNonAlloc(pos, distance, overlappedColliders, (int) FlaggedLayer.Vision, QueryTriggerInteraction.Ignore);
			if (k > 0) {
				if (CheckOverlapped()) {
					return true;
				}
			}
		}

		var fwd = firstCast.forward;
		if (Physics.SphereCast(pos, distance * .5f, fwd, out forwardHit, distance * 1.5f, (int) FlaggedLayer.Vision, QueryTriggerInteraction.Ignore)) {
			if (FindNearest(pos, ref forwardHit)) {
				centerHit = forwardHit;
				return true;
			} else {
				fwd = forwardHit.point - pos;

				deltaPos = -Vector3.Project(fwd, forwardHit.normal);
				if (SecondCast(forwardHit.point))
					return true;
			}
		}

		deltaPos = Vector3.zero;

		return SecondCast(firstCast.position);
	}

	private Vector3 deltaPos;

	private bool SecondCast(Vector3 _startPoint) {
		_startPoint += vDistance * Vector3.up;
		if (Physics.SphereCast(_startPoint, distance * .5f, Vector3.down, out downHit, vDistance * 1.5f, (int) FlaggedLayer.Vision, QueryTriggerInteraction.Ignore)) {
			if (TiltCheck(downHit)) {
				centerHit = downHit;
				return true;
			}

			// var pos = _startPoint + deltaPos;
			// if (FindNearest(pos, ref downHit)) {
			// 	centerHit = downHit;
			// 	return true;
			// }
		}

		return false;
	}


	public bool FindNearest(Vector3 _pos, ref RaycastHit _hit) {
		Vector3 point = _hit.collider.ClosestPoint(_pos);
		var v = point - _pos;
		if (Physics.Raycast(_pos, v, out RaycastHit _hit2, vDistance * 1.5f, (int) FlaggedLayer.Vision)) {
			if (TiltCheck(_hit2)) {
				_hit = _hit2;
				return true;
			}
		}

		return false;
	}

	bool hitDF, hitDP;

	public void BeautifyHandPose() {
		Vector3 cPos = centerHit.point;
		Vector3 fwd;
		//если каст был в стену, то берем нормаль стены в качестве форварда
		fwd = (!TiltCheck(forwardHit) && forwardHit.normal != Vector3.zero) ? -forwardHit.normal : Vector3.ProjectOnPlane(transform.forward, centerHit.normal);

		Vector3 fingerPoint = cPos + fwd * .1f + centerHit.normal * .1f;
		Vector3 palmPoint = cPos - fwd * .1f + centerHit.normal * .1f;
		Vector3 pointToCast = centerHit.point - centerHit.normal * 0.1f;

		Vector3 fingerD = pointToCast - fingerPoint;
		Vector3 palmD = pointToCast - palmPoint;

		hitDF = Physics.Raycast(fingerPoint, fingerD, out fingerHit, distance, (int) FlaggedLayer.Vision);
		hitDP = Physics.Raycast(palmPoint, palmD, out palmHit, distance, (int) FlaggedLayer.Vision);


		angle = Vector3.Angle(fingerHit.normal, palmHit.normal);
		hand.animator.SetFloat(angleFloatID, angle);

		// hitDF = Physics.Raycast(fingerPoint.position, fingerDown, out hitDownFinger, topDownCheckDist, (int) FlaggedLayer.Vision);
		// hitDP = Physics.Raycast(palmPoint.position, palmDown, out hitDownPalm, topDownCheckDist, (int) FlaggedLayer.Vision);

#if UNITY_EDITOR
		// if (hitDF) {
		// 	d_fingerPos.position = fingerHit.point;
		// 	d_fingerNormal.position = fingerHit.normal * .1f + fingerHit.point;
		// }
		//
		// if (hitDP) {
		// 	d_palmPos.position = palmHit.point;
		// 	d_palmNormal.position = palmHit.normal * .1f + palmHit.point;
		// }
		// d_centerPos.position = centerHit.point;
		// d_centerNormal.position = centerHit.normal * .1f + centerHit.point;
		// d_forwardPos.position = forwardHit.point;
		// d_forwardNormal.position = forwardHit.normal * .1f + forwardHit.point;
		// d_downPos.position = downHit.point;
		// d_downNormal.position = downHit.normal * .1f + downHit.point;
#endif
	}

	float angle;

	private void SetState(bool _state) {
		if (currentState == _state) return;

		currentState = _state;
		if (currentState) {
			UpdateSnapPoint();
			handFollowPoint.parent = Player.instance.transform;
			handFollowPoint.CopyGlobalPosRot(snapPoint.GetGlobalOfChild(handOffset));
			// hand.velocityCounter.transform.CopyGlobalPosRot(handFollowPoint.Global());
			// hand.velocityCounter.Reset();
			hand.parkurCapsule.enabled = false;

			onNoReturn?.Invoke(this);
		} else {
			handFollowPoint.parent = hand.velocityCounter.transform;
			handFollowPoint.LocalReset();
			hand.parkurCapsule.enabled = true;
			castStoppedTime = Time.time;
		}

		UpdateTouchState();
		ability.UpdateClimbState();
	}

	private Vector3 palmFwd;

	public PosRot snapPoint;


	public float forwardFix = -0.01f, distFix = 0.003f;

	public void UpdateSnapPoint() {
		Vector3 fwd;
		if (angle > 5f) {
			fwd = Vector3.ProjectOnPlane(fingerHit.normal, palmHit.normal);
		} else {
			fwd = (fingerHit.point - palmHit.point);
		}

		fwd.Normalize();


		snapPoint.pos = centerHit.point;
		snapPoint.pos += fwd * forwardFix;
		snapPoint.pos += palmHit.normal * distFix;
		// if (hitDF && hitDP) {
		// 	fwd = Vector3.ProjectOnPlane(fingerHit.point - palmHit.point, centerHit.normal);
		// } else {
		// 	fwd = Vector3.ProjectOnPlane(transform.forward, centerHit.normal);
		// }

		snapPoint.rot = Quaternion.LookRotation(fwd, palmHit.normal);
	}

	protected override int PriorityModifiers() {
		if (firstCast.up.y < 0) return 0;

		if (isTouching)
			return 90;

		if (CanClimb())
			return 70;

		return 0;
	}


	[Header("Debug tools")]
	public Transform d_PointNorm;
	public GameObject d_1stCast, d_2ndCast, d_canClimb;
	public bool d_showCasts;
	public Transform d_centerPos, d_centerNormal, d_forwardPos, d_forwardNormal, d_downPos, d_downNormal;
	public Transform d_fingerPos, d_fingerNormal, d_palmPos, d_palmNormal;
	private static readonly int angleFloatID = Animator.StringToHash("angle");
}