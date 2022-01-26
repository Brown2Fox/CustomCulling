using System;
using UnityEngine;

public class Player : Singleton<Player> {
	public Transform head;
	public Transform leftHandT;
	public Transform rightHandT;
	public Transform movable;
	public Transform trackingSpace;
	public Transform groundedRoot;
	public Transform camRotate;

	public Owner owner;
	public HandController leftHand, rightHand;
	public Health health;
	public CharacterControllerExtention ccExtention;
	public SnapZone[] pockets;

	public Action onDeath;

	private void Start() {
		owner = GetComponent<Owner>();
		owner.onWarp += RotateAfterWarp;

		onDeath = LevelManager.LoadDeathScene;
	}

	private void RotateAfterWarp() {
		Quaternion q = Quaternion.Inverse(camRotate.localRotation);
		movable.rotation = q * movable.rotation;
	}


	public void Die() {
		leftHand.grabber.TryDrop();
		rightHand.grabber.TryDrop();
		for (int i = pockets.Length - 1; i >= 0; --i)
			pockets[i].ResetSnapZone();

		owner.aContainer.ResetAbilities();

		onDeath?.Invoke();
	}
	

	public void TotalReset() {
		owner.ResetOwner();
		for (int i = pockets.Length - 1; i >= 0; --i)
			pockets[i].ResetSnapZone();
		leftHand.TotalReset();
		rightHand.TotalReset();
		health.Reset();
	}

	public void LocalTutorReset()
    {
		for (int i = pockets.Length - 1; i >= 0; --i)
			pockets[i].ResetSnapZone();
		leftHand.telekinesis.EndCast();
		rightHand.telekinesis.EndCast();
    }
}