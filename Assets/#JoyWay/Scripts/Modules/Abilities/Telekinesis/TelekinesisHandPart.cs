using System;
using System.Collections.Generic;
using System.Linq;
using JoyWay.Systems.InputSystem;
using UnityEngine;

[AbilityType(AbilityType.Telekinesis)]
public class TelekinesisHandPart : AbilityHandPart<TelekinesisAbility> {
	public struct HoldingObjectStuff {
		public Owner owner;
		public Rigidbody rigidbody;
		public ParentContainer parentContainer;
		public List<ITelekinesisable> telekineticables;
	}


	private Label currentTarget;
	public HoldingObjectStuff holdingObj;
	public Rigidbody holdedRigidBody => holdingObj.rigidbody;
	public Owner holdedOwner => holdingObj.owner;


	private Vector3 velocity;
	private Vector3 prevRoomPosition;
	private Vector3 prevLocalPosition;

	public Transform localPointToHold;
	public GrabHandPart grabber;

	public VelocityCounter velocityCounter, movableVelocity;

	private Transform roomTransform;
	public TriggerPool triggerPool;

	private Label grabbableLabel;

	public bool debugTrails;

	public Transform d_trail;
	public Transform d_trail2;

	public Action<TelekinesisHandPart, Owner> OnGrab;
	public Action<TelekinesisHandPart, Owner> OnDrop;

	public GameObject visualZone;
	public Transform visualAndCollider;


	public TelekinesisHandPart() {
		AbilityTriggerID = Animator.StringToHash("Telekinesis");
		AbilityEndTriggerID = Animator.StringToHash("TelekinesisEnded");
	}


	public override void Init(HandSplitAbility _ability) {
		base.Init(_ability);
		holdingObj = new HoldingObjectStuff();

		hand.grabber.onGrab += GrabLock;
		hand.grabber.onDrop += GrabUnlock;
	}

	private void GrabLock(GameObject _grabable, GrabHandPart _grabber) {
		blockSources.Add(_grabber);
		triggerPool.gameObject.SetActive(false);
	}

	private void GrabUnlock(GameObject _grabable, GrabHandPart _grabber) {
		blockSources.Remove(_grabber);

		triggerPool.gameObject.SetActive(true);
	}


	private void OnEnable() {
		//velocityCounter = GetComponentInParent<VelocityCounter>();
		roomTransform = Player.instance.trackingSpace;
		movableVelocity = Player.instance.movable.GetComponent<VelocityCounter>();

		triggerPool.SetCondition(PoolCondition);
		triggerPool.OnActiveTargetHasChanged += ChangeTarget;
		triggerPool.gameObject.SetActive(true);
	}

	private bool PoolCondition(Label _label) {
		Vector3 direction = _label.CheckPoint.position - transform.position;
		int layerMask = (int) FlaggedLayer.Vision;
		if (Physics.Raycast(transform.position, direction, out RaycastHit _hit, direction.magnitude, layerMask)) {
			return false;
		}

		return true;
	}


	public void Scale(Vector3 _scale) {
		visualAndCollider.localScale = _scale;
	}

	private void OnDisable() {
		EndCast();
		ChangeTarget(null);
		triggerPool.gameObject.SetActive(false);
		triggerPool.OnActiveTargetHasChanged -= ChangeTarget;
	}

	private void ChangeTarget(Label _label) {
		if (blocked) return;
		currentTarget = _label;
	}

	public override void StartCast() {
		base.StartCast();
		TryGrab();
	}

	protected override bool CastCheck() {
		return false;
	}

	protected override void SuccessCast() {
		Debug.LogError("Not Implemented");
	}

	public override void EndCast() {
		base.EndCast();
		TryDrop();
	}

	private void TryGrab() {
		if (currentTarget) {
			Hold(currentTarget);
		}
	}

	private void TryDrop() {
		if (holdedRigidBody)
			Drop();
	}

	public void Hold(Label _label) {
		onNoReturn?.Invoke(this);

		if (_label.Check(LabelType.Grabbable))
			grabbableLabel = _label;
		else
			grabbableLabel = null;

		holdingObj.rigidbody = _label.GetComponentInParent<Rigidbody>();
		holdingObj.owner = _label.GetComponentInParent<Owner>();

		if (holdingObj.owner != null) {
			holdingObj.owner.RemoveOwner();
			holdingObj.owner.SetOwner(hand.owner);
		}

		float angle = Vector3.Angle(holdingObj.rigidbody.transform.position - transform.position, localPointToHold.forward);
		localPointToHold.localPosition = new Vector3(0f, 0f, Mathf.Clamp((holdingObj.rigidbody.transform.position - transform.position).magnitude / ability.currentSettings.maxDist * Mathf.Cos(angle * Mathf.Deg2Rad), ability.currentSettings.minDist, 1f));
		prevRoomPosition = roomTransform.InverseTransformPoint(transform.position);

		holdingObj.telekineticables = holdingObj.rigidbody.GetComponents<ITelekinesisable>().ToList();

		foreach (ITelekinesisable t in holdingObj.telekineticables)
			t.Grabbed(this);

		holdingObj.parentContainer = holdingObj.rigidbody.GetComponent<ParentContainer>();


		// ParentContainer pc = holdedRigidBody.gameObject.GetComponent<ParentContainer>();
		if (!holdingObj.parentContainer) {
			holdingObj.parentContainer = holdingObj.rigidbody.gameObject.AddComponent<ParentContainer>();
			holdingObj.parentContainer.ReSetParent();
		}

		holdingObj.rigidbody.transform.parent = Player.instance.trackingSpace;

		grabber.enabled = false;
		ability.onSuccessCast?.Invoke();
		OnGrab?.Invoke(this, holdedOwner);
		triggerPool.gameObject.SetActive(false);
	}


	private void Drop() {
		if (holdingObj.parentContainer != null)
			holdingObj.parentContainer.ReturnToParent();
		else
			holdingObj.rigidbody.gameObject.transform.parent = DefaultSceneParent.lastParent;

		holdingObj.rigidbody.velocity = movableVelocity.velocity + (velocityCounter.velocity - movableVelocity.velocity) * ability.currentSettings.throwPower;

		foreach (ITelekinesisable t in holdingObj.telekineticables)
			t.Dropped();

		if (holdingObj.owner)
			holdingObj.owner.StartRemoveOwner();

		OnDrop?.Invoke(this, holdingObj.owner);

		holdingObj = new HoldingObjectStuff();
		localPointToHold.transform.localPosition = Vector3.zero;

		grabber.enabled = true;

		triggerPool.gameObject.SetActive(true);
	}


	protected override void Update() {
		base.Update();
		triggerPool.gameObject.SetActive(!ability.blocked);
		if (!holdingObj.rigidbody) return;

		// Перехват
		if (grabbableLabel) {
			var grabbable = grabbableLabel.GetComponent<SimpleGrabbable>();
			if (grabbable.CanBeGrabed(grabber) &&
				(holdedRigidBody.transform.position - transform.position).magnitude < ability.currentSettings.grabDistance) {
				hand.SetActiveOther(this, grabber);
				EndCast();
				grabber.curTarget = grabbableLabel;
				grabber.ForceGrab(grabbable);
				// if (currentRigidBodyEffectManager)
				// currentRigidBodyEffectManager.RemoveEffect(EffectType.HightlightTelekines, this);
				// currentRigidBodyEffectManager = null;
				return;
			}
		}

		velocity = holdedRigidBody.velocity;

		// Просчёт точки притяжения
		prevLocalPosition = transform.InverseTransformPoint(roomTransform.TransformPoint(prevRoomPosition));

		if (prevLocalPosition.z < 0)
			prevLocalPosition.z = 0;

		localPointToHold.localPosition = new Vector3(0f, 0f, Mathf.Clamp(localPointToHold.localPosition.z - (prevLocalPosition.z * ability.currentSettings.pullScale) - (ability.currentSettings.constantPull * Time.deltaTime), ability.currentSettings.minDist, 1f));

		// if (debugTrails) {
		// 	if (d_trail)
		// 		d_trail.position = localPointToHold.position;
		// 	if (d_trail2)
		// 		d_trail2.position = velocityCounter.transform.position;
		// }

		velocity += (localPointToHold.position - holdedRigidBody.transform.position) * ability.currentSettings.adjustVelocityMultiplier;
		velocity = velocity.normalized * Mathf.Clamp(velocity.magnitude, 0f, ability.currentSettings.rangeMaxVelocityMultiplier * (localPointToHold.position - holdedRigidBody.transform.position).magnitude + ability.currentSettings.maximumVelocityInPoint);
		holdedRigidBody.velocity = velocity;
		prevRoomPosition = roomTransform.InverseTransformPoint(transform.position);
	}

	protected override int PriorityModifiers() {
		Vector3 v3 = Player.instance.head.localPosition;
		v3.y *= .85f;
		v3 -= hand.transform.localPosition;
		float m = v3.sqrMagnitude;

		var p = 0;


		if (triggerPool.CurrentTarger)
			p += 50;

		if (m < ability.sqrBlockRange)
			p -= 40;

		return p;
	}
}