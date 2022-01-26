using System.Collections;
using System.Collections.Generic;
using JoyWay.Systems.InputSystem;
using UnityEngine;

public class HeroGroundingAbility : ActiveAbility, IInterruptible {
	public Transform groundedRoot;


	public List<ElementalGroundingEffect> effectsList = new List<ElementalGroundingEffect>();

	private HashSet<Owner> triggerred = new HashSet<Owner>();
	public CharacterControllerExtention ccExtension;

	private MoveAbilityBase moveAbility;
	private DashAbility dashAbility;


	protected override void InitBehaviour() {
		ccExtension = Player.instance.ccExtention;
		ccExtension.onSetClimb += Interrupt;
		InputSystem.SuperHeroGrounding.AddListener(StartCast);
		moveAbility = container.GetEntryOfType<MoveAbilityBase>();
		dashAbility = container.GetEntryOfType<DashAbility>();
	}


	public void StartCast(bool _state, HandType _hand) {
		if (_state) {
			StartCast();

			moveAbility.blockSources.Add(this);
			dashAbility.blockSources.Add(this);


			triggerred.Clear();
		}
	}


	private Vector3 startPoint;

	[HideInInspector]
	public Vector3 direction;

	public void RemoveEffect(ElementalGroundingEffect _groundingEffect) {
		effectsList.Remove(_groundingEffect);
	}

	public void Enable() {
		startPoint = groundedRoot.position;

		

		//effects from gems
		foreach (var groundingEffect in effectsList) {
			var effect = Pool.Pop<Transform>(groundingEffect.vfx);
			effect.transform.CopyGlobalPosRot(groundedRoot.Global());
			effect.gameObject.SetActive(true);
			PlaySound(groundingEffect.sfx);
		}


		PlaySound();
		EndCast();
	}


	public IEnumerator HeroFall() {
		startTime = Time.time;
		while (!ccExtension.isGrounded) {
			ccExtension.rigidBody.velocity = Vector3.down * (heroLandingForce * (1 + Time.time - startTime));
			yield return null;
		}

		Enable();
	}


	public override void StartCast() {
		StartCoroutine(HeroFall());
	}


	public void EndCast() {
		moveAbility.blockSources.Remove(this);
		dashAbility.blockSources.Remove(this);
	}


	[HideInInspector]
	public float startTime;
	private float heroLandingForce = 20f;


	public override void ResetAbility() {
		effectsList.Clear();
	}


	protected override void DestroyBehaviour() {
		ccExtension.onSetClimb -= Interrupt;
	}

	public void Interrupt(bool _b) {
		if (_b) Interrupt();
	}

	public void Interrupt() {
		StopAllCoroutines();
		EndCast();
	}
}