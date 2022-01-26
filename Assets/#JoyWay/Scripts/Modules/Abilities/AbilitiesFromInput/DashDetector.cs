using UnityEngine;

public class DashDetector : AbilityHandPart<DashAbility> {
	[SerializeField]
	private VelocityCounter velocityCounter;
	private Transform trackingSpace;

	public dashSettings settings;

	private float detectionSqrMagnitude;
	private float detectionTimeLeft;
	private float velocityRadiusMultiplier;
	private float dashTimeLeft;
	private float dashVelocityRatio;
	private float sqrDetectionRadius;

	private float velocityRatio;
	private float invVelocityRatio;

	public bool isDashing;
	public bool isDetecting;

	public Transform uiArrowWhite, uiArrowRed;
	public ParticleSystem particles;
	private Transform cameraPoint;
	private const float MINIMUM_UI_SCALE = .001f, ARROW_SCALE_MULTIPLIER = .3f;

	private Vector3 detectionPoint;
	private Vector3 dashVector;
	private Vector3 newHandVector;
	private Vector3 prevHandVector;
	private Vector3 moveHandVector;
	private float velocityMultiplier;
	public float accuracy = 1f;
	public float timeDescrese = 1f;

	public DashDetector() {
		AbilityTriggerID = Animator.StringToHash("Dash");
		AbilityEndTriggerID = Animator.StringToHash("DashEnded");
	}

	public override void Init(HandSplitAbility _ability) {
		base.Init(_ability);
		cameraPoint = Player.instance.head;
		settings = ability.dashSettings;
		// velocityCounter = GetComponent<VelocityCounter>();
		trackingSpace = Player.instance.trackingSpace;

		velocityMultiplier = settings.detectionVelocityMultiplier / Time.fixedDeltaTime;
		velocityRatio = (settings.velocityBounds.y - settings.velocityBounds.x);
		invVelocityRatio = 1 / velocityRatio;
		sqrDetectionRadius = settings.detectionRadius * settings.detectionRadius;
	}

	public void InitUI(Transform _whiteArrow, Transform _redArrow, ParticleSystem _particles) {
		uiArrowWhite = _whiteArrow;
		uiArrowRed = _redArrow;

		particles = _particles;
	}

	public override void StartCast() {
		base.StartCast();
		if (isDashing) return;
		isDetecting = true;

		if (LevelManager.currentScene == ScenesList.Tutorial && ability.currentDashes > 0) {
			uiArrowWhite.gameObject.SetActive(true);
			uiArrowWhite.localScale = Vector3.zero;
		}

		prevHandVector = Vector3.zero;
		detectionTimeLeft = settings.detectionTime;
		detectionPoint = transform.localPosition;
	}

	public override bool TryCast() {
		if (blocked) return false;
		StartCast();
		return true;
	}

	protected override bool CastCheck() {
		return false;
	}

	// protected override void SuccessCast() {
	// 	Debug.LogError("Not Implemented");
	// }

	public override void EndCast() {
		base.EndCast();
		if (isDetecting) {
			isDetecting = false;
			if (newHandVector.magnitude < settings.dropRadius) {
				Drop();
				if (!ability.ccExtension.isGrounded)
					--ability.currentDashes;
			}
			else
				SuccessCast();
		}
	}


	private void Drop() {
		uiArrowWhite.gameObject.SetActive(false);
		ability.ccExtension.SetFlight(false);
	}

	protected override void SuccessCast() {
		--ability.currentDashes;

		if (velocityCounter.localVelocity.magnitude < settings.velocityBounds.x) {
			Drop();
			return;
		}

		onNoReturn?.Invoke(this);

		dashVector = newHandVector;

		dashVector.Normalize();

		dashVelocityRatio = (Mathf.Clamp(velocityCounter.localVelocity.magnitude, settings.velocityBounds.x, settings.velocityBounds.y) - settings.velocityBounds.x) * invVelocityRatio;

		float dashAngle = Mathf.Clamp(Vector3.Angle(newHandVector, Vector3.up), 0f, 90f);
		//																	                    1/(90f*90f)
		dashVector *= (1 - settings.jumpImpact + settings.jumpImpact * dashAngle * dashAngle * 0.00012345678F) * Mathf.Lerp(settings.dashVelocityBounds.x, settings.dashVelocityBounds.y, dashVelocityRatio);
		dashVector = trackingSpace.TransformVector(dashVector);

		// Debug.Log($"" +
		// 	$"velocity = {dashVector.magnitude} " +
		// 	$"impact = {1 - settings.jumpImpact + settings.jumpImpact * dashAngle / 90f * dashAngle / 90f} " +
		// 	$"angle = {dashAngle} " +
		// 	$"dashVelocityRatio = {dashVelocityRatio}");

		ability.ccExtension.SetFlight(true);

		isDashing = true;
		//ability.speedParticles.Play();

		if (ability.currentDashes >= 0) {
			particles.transform.position = transform.position;
			particles.transform.LookAt(transform.parent.TransformPoint(detectionPoint), cameraPoint.position - transform.position);
			particles.Play();
			if (LevelManager.currentScene == ScenesList.Tutorial && uiArrowWhite.gameObject.activeInHierarchy) {
				uiArrowWhite.gameObject.SetActive(false);

				uiArrowRed.gameObject.SetActive(false);
				uiArrowRed.CopyGlobalPosRot(particles.transform);
				uiArrowRed.localScale = ARROW_SCALE_MULTIPLIER * newHandVector.magnitude * Vector3.one;
				uiArrowRed.gameObject.SetActive(true);
			}

			if (ability.ccExtension.isGrounded)
				hand.HandPlaySound(ability.groundDashAudio.endCast);
			else
				hand.HandPlaySound(ability.airDashAudio.endCast);
		}

		++ability.ccExtension.isDashing;
		dashTimeLeft = Mathf.Lerp(settings.dashTimeBounds.x, settings.dashTimeBounds.y, dashVelocityRatio);
	}

	private void StopDash() {
		isDashing = false;
		moveHandVector = Vector3.zero;
		--ability.ccExtension.isDashing;
		ability.ccExtension.rigidBody.velocity = ability.ccExtension.rigidBody.velocity.normalized * Mathf.Clamp(ability.ccExtension.rigidBody.velocity.magnitude, 0f, settings.exitVelocity);
	}

	public void FixedUpdate() {
		if (isDetecting) {
			if (detectionTimeLeft <= 0) {
				EndCast();
				return;
			}

			detectionTimeLeft -= Time.fixedDeltaTime;

			newHandVector = detectionPoint - transform.localPosition;

			if (ability.currentDashes > 0) {
				moveHandVector = moveHandVector + newHandVector - prevHandVector;
				ability.ccExtension.rigidBody.velocity = velocityMultiplier * trackingSpace.TransformVector(moveHandVector / accuracy);
			}

			prevHandVector = newHandVector;
			moveHandVector = moveHandVector - moveHandVector * Time.deltaTime * 1.0f / timeDescrese;

			if (LevelManager.currentScene == ScenesList.Tutorial) {
				uiArrowWhite.position = transform.position;
				if (newHandVector.magnitude < MINIMUM_UI_SCALE)
					uiArrowWhite.localScale = Vector3.zero;
				else {
					uiArrowWhite.LookAt(transform.parent.TransformPoint(detectionPoint), cameraPoint.position - transform.position);
					uiArrowWhite.localScale = ARROW_SCALE_MULTIPLIER * newHandVector.magnitude * Vector3.one;
				}
			}

			velocityRadiusMultiplier = Mathf.Lerp(1f, 2f, (Mathf.Clamp(velocityCounter.localVelocity.magnitude, settings.velocityBounds.x, settings.velocityBounds.y) - settings.velocityBounds.x) * invVelocityRatio);

			detectionSqrMagnitude = sqrDetectionRadius * velocityRadiusMultiplier * velocityRadiusMultiplier;
			if (newHandVector.sqrMagnitude > detectionSqrMagnitude && ability.currentDashes > 0) {
				SuccessCast();
				isDetecting = false;
			}
		}

		if (isDashing) {
			dashTimeLeft -= Time.fixedDeltaTime;
			if (dashTimeLeft <= 0)
				StopDash();
		}
	}

	public bool DirectionDown() {
		Vector3 direction = detectionPoint - transform.localPosition;
		if (Vector3.Angle(Vector3.down, direction) < 30f)
			return true;
		return false;
	}

	public bool CheckCommonDashing() {
		if (isDetecting && settings.detectionTime - detectionTimeLeft < 0.1f) {
			return true;
		}

		return false;
	}
}