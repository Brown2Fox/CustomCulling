using System;
using System.Collections.Generic;
using JoyWay.Systems.InputSystem;
using UnityEngine;

public class SimpleGrabbable : MonoBehaviour {
	[DisableEditing]
	public Owner owner;

	[DisableEditing]
	public AudioAsset grabSound;
	public bool muteOne;				   // Блокируем одно проигрывание звука

	public Transform Offset {
		get {
			if (offset == null) {
				offset = new GameObject().transform;
				offset.parent = transform;
				offset.localPosition = Vector3.zero;
				offset.localRotation = Quaternion.identity;
			}

			return offset;
		}
	}

	public Transform offset;

	public GrabType grabType;
	public UnityEventGrabber onGrabbed, onDropped;
	public Action<SimpleGrabbable> onGrab, onDrop;

	public List<Component> blockSources = new List<Component>();

	protected GrabHandPart curGrabber;

	public GrabHandPart currentGrabber { get {return curGrabber; } }

	[HideInInspector]
	public Rigidbody rb;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		if (!owner)
			owner = GetComponent<Owner>();
		SetVibrationEvent();
	}

	// TODO: add parameter for safe check without triggering longHold
	public virtual bool CanBeGrabed(GrabHandPart _grabber) {
		return blockSources.Count == 0 && !curGrabber;
	}

	public virtual bool CanDrop(GrabHandPart _grabber) {
		return true;
	}

	public virtual bool TryGrab(GrabHandPart _grabber) {
		if (CanBeGrabed(_grabber)) {
			Grab(_grabber);
			return true;
		}

		return false;
	}

	public virtual bool TryDrop(GrabHandPart _grabber) {
		return true;
	}

	public virtual void Grab(GrabHandPart _grabber) {
		curGrabber = _grabber;
		if (rb)
			switch (grabType) {
				case GrabType.Parent:
					rb.useGravity = false;
					rb.isKinematic = true;
					break;
				case GrabType.Transform:
					break;
				case GrabType.Joint:
				case GrabType.JointHandle:
					rb.useGravity = false;
					rb.isKinematic = false;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}


		if (owner) {
			owner.RemoveOwner();
			owner.SetOwner(_grabber.owner);

		}

		onGrabbed.Invoke(_grabber);
		onGrab?.Invoke(this);
	}

	public virtual void Drop() {
		if (rb && grabType == GrabType.Parent) {
			rb.useGravity = true;
			rb.isKinematic = false;
		}

		if (rb && grabType == GrabType.Joint) {
			rb.useGravity = true;
		}

		var _grabber = curGrabber;
		curGrabber = null;


		rb.velocity = _grabber.velocityCounter.velocity * 1.1f;
		rb.angularVelocity = _grabber.velocityCounter.angularVelocity;

		if (owner.owner)
			owner.StartRemoveOwner();
		else
			Debug.Log($"has to be owned by someone", owner);
		onDropped.Invoke(_grabber);
		onDrop?.Invoke(this);
	}

	public void UpdateOffset() {
		switch (grabType) {
			case GrabType.Parent:
				if (curGrabber != null)
					transform.CopyLocalPosRot(offset);
				break;
			// Другие случаи не хочу пока заморачиваться. Может и не понадобится.
		}
	}

	public void AddBlocker(Component source) {
		blockSources.Add(source);
	}

	public void RemoveBlocker(Component source) {
		blockSources.Remove(source);
	}

	public GrabType GetGrabType() {
		return grabType;
	}

	public GameObject GetObject() {
		return gameObject;
	}

	public Vector3 GetOffset(GrabHandPart _grabber) {
		return Vector3.zero;
	}

	public GrabHandPart GetGrabber() {
		return curGrabber;
	}

	public void SetVibrationEvent() {
		IVibratable[] vibrations = GetComponentsInChildren<IVibratable>(true);
		foreach (IVibratable iVibratable in vibrations)
			iVibratable.OnVibrate += Vibrate;
		//vibration.EnableVibrationAction(Vibrate);
	}

	public void Vibrate(VibrationSettings settings) {
		if (curGrabber)
			curGrabber.Vibrate(settings);
	}
}