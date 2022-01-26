using JoyWay.Systems.InputSystem;
using UnityEngine;

[AbilityType(AbilityType.Telekinesis)]
public class ExtractObject : AbilityHandPart<ExtractorAbility> {
	public float detectionTime = 20f;
	public float distanceDetection = 0.15f;
	public float startImpulse = 20f;

	[DisableEditing]
	public TelekinesisHandPart telekinesisHand;

	private VibrationSettings vibrationSettings;

	private float
		minVibrationAmplitude = 0.5f,
		maxVibrationAmplitude = 3f;

	private bool
		isDetecting;

	private float
		detectionTimeLeft,
		detectionSqrMagnitude;

	private Vector3
		detectionPoint,
		newHandVector;

	private RaycastHit hit;


	public ExtractObject() {
		AbilityTriggerID = Animator.StringToHash("Extract");
		AbilityEndTriggerID = Animator.StringToHash("ExtractEnded");
	}


	public override void Init(HandSplitAbility _ability) {
		base.Init(_ability);
		telekinesisHand = GetComponent<TelekinesisHandPart>();
		detectionSqrMagnitude = distanceDetection;
	}


	public override void StartCast() {
		base.StartCast();
		TryExtract();
	}

	protected override bool CastCheck() {
		return false;
	}

	protected override void SuccessCast() {
		Debug.LogError("Not implemented yet");
	}

	public override void EndCast() {
		base.EndCast();
		if (isDetecting) {
			isDetecting = false;
		}
	}


	private void TryExtract() {
		if (Physics.Raycast(hand.magicPoint.position, hand.magicPoint.forward, out RaycastHit hitInfo, ability.extractDistance, (int) FlaggedLayer.Vision, QueryTriggerInteraction.Ignore)) {
			//Label tempLabel = hitInfo.collider.GetComponent<Label>();
			//if (tempLabel != null && tempLabel.Check(LabelType.StoneSource)) {
			CreateTrashFromWall(hitInfo);
			//}
		}
	}


	public void CreateTrashFromWall(RaycastHit hit) {
		this.hit = hit;
		isDetecting = true;
		detectionTimeLeft = detectionTime;
		detectionPoint = hand.transform.position;


		Transform effect = ability.effectPool.Pop<Transform>();
		effect.position = hit.point + hit.normal * .02f;
		effect.rotation = Quaternion.LookRotation(hit.normal);
		effect.gameObject.SetActive(true);

		//create crack
		//GameObject newEffect = Instantiate(extractionEffect, hit.point + hit.normal * .02f, Quaternion.LookRotation(hit.normal));

		//vibro
		vibrationSettings.amplitude = 3f;
		InputSystem.instance.Vibrate(vibrationSettings, hand.type);
	}

	public void NewObject() {
		onNoReturn?.Invoke(this);

		Transform stone = ability.stonePool.Pop<Transform>();
		stone.position = hit.point;
		stone.rotation = Quaternion.identity;
		stone.gameObject.SetActive(true);


		//GameObject newStone = Instantiate(extractableObject, hit.point, Quaternion.identity);
		Label tempLabel = stone.GetComponent<Label>();
		Rigidbody rb = stone.GetComponent<Rigidbody>();
		rb.AddForce(hit.normal * (rb.mass * startImpulse), ForceMode.Impulse);
		telekinesisHand.Hold(tempLabel);
		hand.SetActiveOther(this, telekinesisHand);
		telekinesisHand.casting = true;
	}

	protected override void Update() {
		base.Update();
		if (isDetecting) {
			vibrationSettings.duration = Time.deltaTime;
			vibrationSettings.amplitude = Mathf.Clamp(newHandVector.sqrMagnitude * 10, minVibrationAmplitude, maxVibrationAmplitude);
			InputSystem.instance.Vibrate(vibrationSettings, telekinesisHand.hand.type);
		}
	}

	public void FixedUpdate() {
		if (isDetecting) {
			if (detectionTimeLeft <= 0) {
				isDetecting = false;
				return;
			}

			detectionTimeLeft -= Time.fixedDeltaTime;

			newHandVector = hand.transform.position - detectionPoint;
			if (newHandVector.sqrMagnitude > detectionSqrMagnitude && CorrectDirection(newHandVector)) {
				NewObject();
				isDetecting = false;
			}

			vibrationSettings.amplitude = Mathf.Clamp(newHandVector.sqrMagnitude * 10, minVibrationAmplitude, maxVibrationAmplitude);
			InputSystem.instance.Vibrate(vibrationSettings, hand.type);
		}
	}


	public bool CorrectDirection(Vector3 handVector) {
		if (Vector3.Angle(hit.normal, handVector) < 30f)
			return true;
		return false;
	}

	protected override int PriorityModifiers() {
		Vector3 v3 = Player.instance.head.localPosition;
		v3.y *= .85f;
		v3 -= hand.transform.localPosition;
		float m = v3.sqrMagnitude;

		var p = 0;


		if (Physics.Raycast(hand.magicPoint.position, hand.magicPoint.forward, out RaycastHit hitInfo, ability.extractDistance, (int) FlaggedLayer.Vision, QueryTriggerInteraction.Ignore)) {
			p += 30;
			if (m < ability.sqrBlockRange)
				p -= 15;
		}

		return p;
	}
}