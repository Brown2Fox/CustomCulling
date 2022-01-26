using System;
using UnityEngine;

public class SnapZone : MonoBehaviour {
	[DisableEditing]
	public Owner owner;
	public SnapZoneType possibleTags;
	public int priority;
	public Transform[] points;

	public AudioSource aSource;
	public AudioAsset snapSound;
	public AudioAsset unSnapSound;

	public Action<SnapZone> onUnSnap;

	public Snapper currentSnapper;

	private void Awake() {
		Init();
	}

	public void Init() {
		owner = GetComponentInParent<Owner>();
	}


	public virtual bool TrySnap(Snapper snapper) {
		if ((possibleTags & snapper.type) == SnapZoneType.None) return false;

		if (!TryGetPlace(out Transform place)) return false;

		snapper.transform.parent = place;
		snapper.transform.LocalReset();
		gameObject.layer = (int) Layer.GrabbedObject;

		OnSnap(snapper);
		return true;
	}

	// Заглушает звук прицепления предмета
	public bool muteOne = false;
	public virtual void OnSnap(Snapper snapper) {
		if (muteOne)
			muteOne = false;
		else {
			AudioClip clip = (snapper.snapSound)? snapper.snapSound : snapSound;
			aSource.PlayOneShot(clip);
		}
		currentSnapper = snapper;
	}

	public virtual void OnUnSnap(Snapper snapper) {


		AudioClip clip = (snapper.unSnapSound) ? snapper.unSnapSound : unSnapSound;
		aSource.PlayOneShot(clip);
			
		onUnSnap?.Invoke(this);
	}


	public bool TryGetPlace(out Transform place) {
		place = null;
		for (int i = 0; i < points.Length; ++i)
			if (points[i].childCount <= 0) {
				place = points[i];
				return true;
			}

		return false;
	}

	public void DropItems() {
		for (int i = points.Length - 1; i >= 0; --i)
			foreach (Transform child in points[i]) {
				child.parent = null;
				Snapper snapper = child.GetComponent<Snapper>();
				snapper.TryUnSnap();
			}
	}


	public virtual void ResetSnapZone() {
		DropItems();
	}

	public void ForceResnap() {
		currentSnapper.SnapTo(this);
	}


#if UNITY_EDITOR
	private void OnValidate() {
		if (!owner) owner = GetComponentInParent<Owner>();
	}
#endif
}

[Flags]
public enum SnapZoneType {
	None,            // 0000000000000000
	Gem = 1 << 0,    // 0000000000000001
	Weapon = 1 << 1, // 0000000000000010
	Any = ~0         // 1111111111111111
}