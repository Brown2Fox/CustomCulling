using System;
using System.Collections.Generic;
using JoyWay.Systems.InputSystem;
using UnityEngine;

public class DashAbility : HandSplitAbility<DashDetector>, IShowingStat {
	[DisableEditing]
	public int currentDashes;
	public int additionalDashes;

	public Transform
		leftWhiteArrow,
		rightWhiteArrow,
		leftRedArrow,
		rightRedArrow;
	public ParticleSystem
		leftParticles, rightParticles;

	public ParticleSystem speedParticles;

	public CastAudio groundDashAudio;
	public CastAudio airDashAudio;

	public dashSettings dashSettings;

	public CharacterControllerExtention ccExtension;

	[Header("Show stats")]
	public bool show_dash_byDefault = true;

	protected override void InitBehaviour() {
		base.InitBehaviour();
		ccExtension = Player.instance.ccExtention;
		ccExtension.onSetGrounded += OnGrounded;
		ResetDashCount();

		left.InitUI(leftWhiteArrow, leftRedArrow, leftParticles);
		right.InitUI(rightWhiteArrow, rightRedArrow, rightParticles);

		if (owner) owner.showingStats.Add(this);
	}

	protected override void DestroyBehaviour() {
		ccExtension.isDashing = 0;
		currentDashes = 0;
		ccExtension.onSetGrounded -= OnGrounded;
		ccExtension.isDashing = 0;
	}

	private void OnGrounded(bool _state) {
		if (_state) {
			ResetDashCount();
		}
	}


	public void ResetDashCount() {
		currentDashes = dashSettings.baseCount + additionalDashes;
	}


	private void FixedUpdate() {
		if (!blocked && !ccExtension.isGrounded && (left.isDashing && left.DirectionDown() || right.isDashing && right.DirectionDown())) {
			InputSystem.SuperHeroGrounding.Trigger(true, HandType.Any);
		}
	}


	public override void ResetAbility() {
		additionalDashes = 0;
	}

	public List<ShowingStatsUI.NameDiscr> GetStatsInfo() {
		List<ShowingStatsUI.NameDiscr> nameDiscrs = new List<ShowingStatsUI.NameDiscr>();

		ShowingStatsUI.NameDiscr dashCount = new ShowingStatsUI.NameDiscr();
		dashCount.Fill("", "Dash count", (dashSettings.baseCount + additionalDashes).ToString(), 
			(GameMain.instance.developerSettingsAsset.showAllStats) || show_dash_byDefault);
		nameDiscrs.Add(dashCount);

		return nameDiscrs;
	}
}

[Serializable]
public struct dashSettings {
	public int baseCount;

	public Vector2 velocityBounds;

	public float detectionRadius;
	public float dropRadius;
	public float detectionTime;
	public float detectionVelocityMultiplier;

	public Vector2 dashVelocityBounds;

	public Vector2 dashTimeBounds;

	public float exitVelocity;
	public float jumpImpact;
}