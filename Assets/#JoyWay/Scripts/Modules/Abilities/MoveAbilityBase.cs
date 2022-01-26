using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public abstract class MoveAbilityBase : Ability, IShowingStat {
	public Rigidbody body;

	public MoveVelocityParameters currentVelocityParameters;
	public MoveVelocityParameters runVelocityParameters;
	public MoveVelocityParameters walkVelocityParameters;
	protected bool isWalkingState;

	protected float[] angleSpeedMltiplier = new float[181];


	//public Collider[] groundColliders;

	public bool grounded {
		get { return _grounded; }
	}

	protected bool _grounded;
	protected float moveAngle;
	protected CharacterControllerExtention ccExtension;


	[Header("Show stats")]
	public bool show_velocity_byDefault = true;

	protected override void InitBehaviour() {
		//asset.SetUpAbilityBase(this);
		ccExtension = Player.instance.movable.GetComponent<CharacterControllerExtention>();
		grounderRadius = ccExtension.physicsCollider.radius * .95f;
		fromGroundCheckVector =
			ccExtension.physicsCollider.transform.position +
			ccExtension.physicsCollider.transform.TransformPoint(ccExtension.physicsCollider.center) -
			ccExtension.physicsCollider.transform.up * ccExtension.physicsCollider.height +
			Vector3.up * (grounderRadius * .8f);
		currentVelocity = 0;
		newVelocityVector = Vector3.zero;

		if (owner) owner.showingStats.Add(this);
	}

	public virtual void SetWalkState(bool _state) {
		isWalkingState = _state;
		var parameters = isWalkingState ? walkVelocityParameters : runVelocityParameters;
		SetParams(parameters);
	}


	protected virtual void SetParams(in MoveVelocityParameters _params) {
		currentVelocityParameters = _params;

		for (int i = 0; i < 181; i++) {
			angleSpeedMltiplier[i] = _params.moveAngleCurve.Evaluate(i);
		}
	}


	public virtual void Warp(Vector3 _point) {
		body.transform.position = _point;
	}

	public virtual void Warp(PosRot _point) {
		body.transform.CopyGlobalPosRot(_point);
	}

	// Обычно длинна 1, но для игрока может быть [0, 1].
	protected Vector3 desiredDirection;

	public Vector3 newVelocityVector;

	//public GameObject d_Capsule, d_sphere1, d_sphare2, d_p1, d_p2, d_p3;

	protected float currentVelocity;

	public virtual void CalculateCurrentSpeed() {
		float desiredSpeed = Mathf.Clamp01(desiredDirection.magnitude) * currentVelocityParameters.maxVelocity;
		if (blocked || desiredSpeed < currentVelocity)
			newVelocityVector = Vector3.MoveTowards(newVelocityVector, desiredDirection.normalized * desiredSpeed, currentVelocityParameters.stopAcceleration * Time.fixedDeltaTime);
		//currentVelocity = Mathf.Clamp(currentVelocity - currentVelocityParameters.stopAcceleration * Time.fixedDeltaTime, 0f, float.PositiveInfinity);
		else
			newVelocityVector = Vector3.MoveTowards(newVelocityVector, desiredDirection.normalized * desiredSpeed, currentVelocityParameters.acceleration * Time.fixedDeltaTime);
		//newVelocityVector += desiredDirection.normalized * currentVelocityParameters.acceleration * Time.fixedDeltaTime;

		currentVelocity = newVelocityVector.magnitude;
		//currentVelocity = Mathf.Clamp(currentVelocity, 0f, desiredSpeed);
	}


	protected virtual void FixedUpdate() {
		Profiler.BeginSample("MoveAbilityBase.FU");
		Vector3 force = Vector3.zero;
		if (!blocked) {
			CalculateCurrentSpeed();

			newVelocityVector = Vector3.ClampMagnitude(newVelocityVector.normalized * currentVelocity, currentVelocityParameters.maxVelocity * angleSpeedMltiplier[(int) moveAngle]);

			if (currentVelocity > 0.0001f) {
				force = newVelocityVector;
			}
		}

		Vector3 flatRBV = body.velocity;
		flatRBV.y = 0;
		//Debug.Log($"{newVelocityVector.magnitude:0.00} + {body.velocity.magnitude:0.00} ({flatRBV.magnitude:0.00}) = {(newVelocityVector + body.velocity).magnitude:0.00} ({(newVelocityVector + flatRBV).magnitude:0.00})");
		//InGameLogFormer.Log($"{newVelocityVector.magnitude:0.00} + {body.velocity.magnitude:0.00} ({flatRBV.magnitude:0.00}) = {(newVelocityVector + body.velocity).magnitude:0.00} ({(newVelocityVector + flatRBV).magnitude:0.00})");

		if (pushing) {
			force += pushForce;
		}


		force *= Time.fixedDeltaTime;

		if (force.sqrMagnitude > 0f)
			MakeTransition(force);

		fromGroundCheckVector =
			ccExtension.physicsCollider.transform.TransformPoint(ccExtension.physicsCollider.center) -
			ccExtension.physicsCollider.height * .5f * ccExtension.physicsCollider.transform.up +
			Vector3.up * (grounderRadius * .8f);

		_grounded = Physics.CheckSphere(fromGroundCheckVector, grounderRadius, (int) FlaggedLayer.Vision);

		//groundColliders = Physics.OverlapSphere(fromGroundCheckVector, grounderRadius, (int)FlaggedLayer.Vision);
		//_grounded = groundColliders.Length > 0;


		CalculateMoveAngle();
		CalculateLocalMoveVector();

		Profiler.EndSample();
	}

	protected virtual void MakeTransition(Vector3 _shift) {
		body.MovePosition(_shift + body.position);
		//if (isPlayer)
		//	body.MovePosition(CapsuleCaster.MakeCastFromFloor(body.transform, ccExtension.physicsCollider, _shift, (int) FlaggedLayer.Vision, d_rad, d_h, d_s));
		//else
		//	body.MovePosition(_shift + body.position);
	}


	protected virtual void CalculateMoveAngle() {
		var v1 = newVelocityVector;
		var v2 = body.transform.forward;
		v1.y = 0;
		v2.y = 0;
		var angle = Vector3.Angle(v1, v2);
		moveAngle = angle;
	}

	protected virtual void CalculateLocalMoveVector() {
		var v1 = newVelocityVector;
		var t = body.transform;
		v1.y = 0;
		localMoveVector = t.InverseTransformVector(newVelocityVector).normalized;
	}

	protected Vector3 localMoveVector;


	private Vector3 fromGroundCheckVector;
	private float grounderRadius;


	protected bool pushing;
	public Vector3 pushForce;

	public void AddPushForce(Vector3 _force) {
		pushForce += _force;
		pushing = pushForce.sqrMagnitude > 0.02f;
		if (!pushing) pushForce = Vector3.zero;
	}

	public List<ShowingStatsUI.NameDiscr> GetStatsInfo() {
		List<ShowingStatsUI.NameDiscr> nameDiscrs = new List<ShowingStatsUI.NameDiscr>();

		ShowingStatsUI.NameDiscr maxVelocity = new ShowingStatsUI.NameDiscr();
		maxVelocity.Fill("", "Velocity", currentVelocityParameters.maxVelocity.ToString(), (GameMain.instance.developerSettingsAsset.showAllStats) || show_velocity_byDefault);
		nameDiscrs.Add(maxVelocity);

		return nameDiscrs;
	}
}


[Serializable]
public struct MoveVelocityParameters {
	public float maxVelocity;
	public float acceleration;
	public float stopAcceleration;
	public AnimationCurve moveAngleCurve;


	public static MoveVelocityParameters operator *(MoveVelocityParameters _params, float _f) {
		_params.maxVelocity *= _f;
		_params.acceleration *= _f;
		return _params;
	}
}


[Serializable]
public struct CharCapsuleSettings {
	public float height;
	public float radius;
}