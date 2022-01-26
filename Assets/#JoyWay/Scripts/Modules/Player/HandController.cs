using System;
using System.Collections.Generic;
using JoyWay.Systems.InputSystem;
using UnityEngine;

public class HandController : MonoBehaviour {
	public Owner owner;
	public HandType type;
	public HandController other;
	public GrabHandPart grabber;
	public TelekinesisHandPart telekinesis;
	public VelocityCounter velocityCounter;
	public CapsuleCollider parkurCapsule;
	public GameObject punch;
	public HandOffset offset;

	public Transform magicPoint;
	public Transform magicForward;

	public AbilityHandPart activeAbility { get; private set; }

	public Animator animator;

	public AudioSource audioSource;

	public Action onSetActiveAbility;

	public List<AbilityHandPart> toActivateList = new List<AbilityHandPart>();
	private List<CastLaunch> castsList = new List<CastLaunch>();
	private UpdateCopyPosRot handPosUpdater;


	private void Start() {
		handPosUpdater = GetComponent<UpdateCopyPosRot>();
		PauseManager.instance.onPause += SetPauseMode;
	}

	private void SetPauseMode(bool _state) {
		handPosUpdater.enabled = _state;
		if (_state) {
			grabber.EndCast();
		}
	}

	[Serializable]
	private struct CastLaunch {
		public AbilityHandPart startedAbility;
		public List<AbilityHandPart> predictorsList;
	}

	public void TotalReset() {
		grabber.TryDrop();
		telekinesis.EndCast();
	}

	private void LateUpdate() {
		if (toActivateList.Count == 0) return;

		AbilityHandPart currentTop = null;
		int topPriority = 0;

		foreach (AbilityHandPart abilityPart in toActivateList) {
			if (abilityPart.Priority > topPriority) {
				currentTop = abilityPart;
				topPriority = abilityPart.Priority;
			}
		}

		// Отбор предсказывающих абилок
		List<AbilityHandPart> predictorsList = new List<AbilityHandPart>();
		foreach (AbilityHandPart abilityPart in toActivateList) {
			if (abilityPart == currentTop || !abilityPart.Predictable) continue;

			predictorsList.Add(abilityPart);
			abilityPart.StartPredict();
			abilityPart.onPrediction += PredictionComplete;
		}

		// Запуск наблюдений предсказывающих абилок
		if (predictorsList.Count > 0) {
			if (currentTop)
				currentTop.onNoReturn += AbilityCasted;
			castsList.Add(new CastLaunch() {predictorsList = predictorsList, startedAbility = currentTop});
		}

		if (currentTop) {
			StartCast(currentTop);
		}

		toActivateList.Clear();
	}


	public void StartCast(AbilityHandPart currentTop) {
		currentTop.StartCast();
		activeAbility = currentTop;
		onSetActiveAbility?.Invoke();
		animator.SetTrigger(currentTop.AbilityTriggerID);
		animator.SetBool(AbilityTriggerAnimID, true);
	}

	public void SetActiveOther(AbilityHandPart _cur, AbilityHandPart _new) {
		if (activeAbility == _cur) {
			activeAbility = _new;
			animator.SetTrigger(AbilityTriggerAnimID);
			animator.SetBool(_cur.AbilityTriggerID, false);
			animator.SetBool(_new.AbilityTriggerID, true);
		}
	}

	public void ResetActive(AbilityHandPart _cur) {
		if (!_cur) return;
		if (activeAbility == _cur) {
			activeAbility = null;
			animator.SetBool(_cur.AbilityTriggerID, false);
		}
	}


	private void AbilityCasted(AbilityHandPart _caster) {
		int castIndex = 0;
		for (; castIndex < castsList.Count && castsList[castIndex].startedAbility != _caster; ++castIndex) ;
		if (castIndex >= castsList.Count) return;

		RemoveCastAt(castIndex);
	}

	private void PredictionComplete(AbilityHandPart _predictor) {
		// Находим активные абилки и прерываем их
		foreach (KeyValuePair<int, Ability> one in owner.aContainer.entries) {
			if (one.Key != _predictor.ability.AbilityID
				&& one.Value is HandSplitAbility) {
				HandSplitAbility localHSA = ((HandSplitAbility) (one.Value));

				if (localHSA[type].casting) localHSA[type].BreakCast();
			}
		}

		Vector2Int predictorIndex = Vector2Int.zero;
		for (; predictorIndex.x < castsList.Count; ++predictorIndex.x) {
			for (predictorIndex.y = 0; predictorIndex.y < castsList[predictorIndex.x].predictorsList.Count && castsList[predictorIndex.x].predictorsList[predictorIndex.y] != _predictor; ++predictorIndex.y) ;
			if (predictorIndex.y < castsList[predictorIndex.x].predictorsList.Count) break;
		}

		if (predictorIndex.x >= castsList.Count) return;

		ResetActive(castsList[predictorIndex.x].startedAbility);
		StartCast(castsList[predictorIndex.x].predictorsList[predictorIndex.y]);

		_predictor.onPrediction -= PredictionComplete;
		RemoveCastAt(predictorIndex.x);
	}

	private void RemoveCastAt(int _i) {
		foreach (AbilityHandPart predicting in castsList[_i].predictorsList)
			predicting.StopPredict();
		if (castsList[_i].startedAbility != null)
			castsList[_i].startedAbility.onNoReturn -= AbilityCasted;
		castsList.RemoveAt(_i);
	}


	public void HandPlaySound(AudioAsset audioAsset, float volume = 1f) {
		if (audioSource && audioAsset)
			audioSource.PlayOneShot(audioAsset, volume);
	}

	public void HandPlayLoopSound(AudioAsset audioAsset, float volume = 1f) {
		if (audioSource && audioAsset) {
			audioSource.clip = audioAsset;
			audioSource.loop = true;
			audioSource.volume = volume;
			audioSource.Play();
		}
	}

	public void HandStopSound() {
		if (audioSource)
			audioSource.Stop();
	}

	public void Vibrate(VibrationSettings vibrationSettings) {
		InputSystem.instance.Vibrate(vibrationSettings, type);
	}

	public void StopVibrate() {
		InputSystem.instance.StopVibrate(type);
	}

	[SerializeField]
	private Renderer rendererToGlow;
	private static readonly int EmissionTint = Shader.PropertyToID("_EmissionTint");
	private static readonly int TriggerSquezee = Animator.StringToHash("TriggerSquezee");
	private static readonly int GripSquezee = Animator.StringToHash("GripSquezee");

	public void SetGlowColor(Color _equipColor) {
		var mat = rendererToGlow.material;
		mat.SetColor(EmissionTint, _equipColor);
	}

	public void SetAnimationTrigger(int _animationID) {
		animator.SetTrigger(_animationID);
	}


	public void Update() {
		UpdateAnimationParameters();
	}

	private void UpdateAnimationParameters() {
		UpdateWallCheck();
		UpdateOrientation();
		animator.SetFloat(TriggerSquezee, InputSystem.instance.GetTriggerSquezee(type));
		animator.SetFloat(GripSquezee, InputSystem.instance.GetGripSquezee(type));
	}

	private void UpdateWallCheck() { }

	private void UpdateOrientation() { }


	private static readonly int AbilityTriggerAnimID = Animator.StringToHash("AbilityTrigger");
}


[Flags]
public enum HandBlock {
	Trigger = 1 << 0,
	Grab = 1 << 1,
	Stick = 1 << 2,
	Button1 = 1 << 3,
	Button2 = 1 << 4
}