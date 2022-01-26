using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Snapper : MonoBehaviour {
	[DisableEditing]
	public Owner owner;
	public SnapZoneType type;

	public Transform snapLooker;

	public AudioAsset snapSound;
	public AudioAsset unSnapSound;

	public float radius = 0.01f;
	public UnityEvent onSnap;
	public UnityEvent onUnSnap;

	private Collider[] colliders;

	public List<Component> blockers = new List<Component>();

	[DisableEditing]
	public SnapZone snapZone;
	private SnapZone lastSnapZone;

	private Rigidbody rb;
	private SimpleGrabbable grabbable;



	private void Awake() {
		rb = GetComponent<Rigidbody>();
		grabbable = GetComponent<SimpleGrabbable>();
	}

	public void TrySnap() {
		if (IsBlocked()) return;

		colliders = Physics.OverlapSphere(snapLooker.position, radius);

		SnapZone priorityZone = null;
		int min = int.MaxValue;
		for (int i = colliders.Length - 1; i >= 0; --i) {
			var zone = colliders[i].GetComponent<SnapZone>();
			if (zone && (zone.possibleTags & type) != SnapZoneType.None && zone.priority <= min) {
				priorityZone = zone;	
				min = priorityZone.priority;
			}
		}

		if (priorityZone && priorityZone.TrySnap(this)) {
			Snap(priorityZone);
		}
	}

	public void SnapTo(SnapZone _zone) {
		if (_zone.TrySnap(this)) {
			Snap(_zone);
		}
	}

	public bool IsBlocked() {
		for (int i = blockers.Count - 1; i >= 0; --i)
			if (blockers[i] == null || blockers[i].gameObject == null)
				blockers.RemoveAt(i);
		return blockers.Count > 0;
	}

	public void Snap(SnapZone _zone) {
		snapZone = _zone;
		lastSnapZone = snapZone;
		if (rb) {
			rb.useGravity = false;
			rb.isKinematic = true;
		}

		owner.RemoveOwner();
		owner.SetOwner(snapZone.owner);

		onSnap?.Invoke();
	}


	public void TryUnSnap() {
		if (snapZone)
			UnSnap();
	}


	public void UnSnap() {
		snapZone.OnUnSnap(this);
		snapZone = null;
		owner.RemoveOwner();


		if (rb) {
			rb.useGravity = true;
			rb.isKinematic = false;
		}

		onUnSnap?.Invoke();
	}

	public void SafeUnSnap() {

		if (!snapZone) return;
		if (grabbable.grabType == GrabType.Joint) {
			//transform.parent = null;
			if (rb) {
				rb.useGravity = true;
				rb.isKinematic = false;
			}
		}

		if (snapZone is SnapZonePocket)
			grabbable.muteOne = true;	  // Мьютим одно проигрывание звука, чтобы проигрался звук карманов а не хватания

		snapZone.OnUnSnap(this);
		snapZone = null;
	}

	public SnapZone GetLastSnapZone() {
		return lastSnapZone;
    }


#if UNITY_EDITOR
	private void OnValidate() {
		if (!owner) owner = GetComponentInParent<Owner>();
	}
#endif
}