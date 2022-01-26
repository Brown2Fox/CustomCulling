using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsAbility : Ability {

	public MoveAbilityBase moveAbilityBase;
	public Transform ground;

	public float cdWalkSteps;
	public float cdRunSteps;

	public SoundFromMaterialAsset materialAsset;


	protected override void InitBehaviour() {
		settings.cooldown = cdWalkSteps;
	}


	private void FixedUpdate() {

		if (moveAbilityBase 
			&& moveAbilityBase.grounded
			&& moveAbilityBase.newVelocityVector != Vector3.zero){

			if (CooldownLeft <= 0) PlayFootstep();

		}
	}

	private void PlayFootstep() {

		lastUseTime = Time.time;
		PlaySound(GetClip());
	}

	private AudioClip GetClip() {
		

		return null;
	}

	protected override void DestroyBehaviour() { }

}
