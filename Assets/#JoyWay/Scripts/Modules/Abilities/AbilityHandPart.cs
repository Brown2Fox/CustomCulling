using System;
using System.Collections.Generic;
using JoyWay.Systems.InputSystem;
using UnityEngine;

public abstract class AbilityHandPart : MonoBehaviour, IActiveAbility {
	public Owner owner => ability.owner;
	[DisableEditing]
	public HandSplitAbility ability;

	[DisableEditing]
	public bool casting;


	public bool blocked => blockSources.Count > 0;

	//[HideInInspector]
	public List<object> blockSources = new List<object>();

	protected Transform chargingPart;

	public Action onCastStart;
	public Action<AbilityHandPart> onCastEnd, onSuccessCast, onPrediction, onNoReturn, onCastCheck;

	public HandController hand;
	private int handPriority;

	public int Priority {
		get => ability.priority + handPriority + PriorityModifiers();
		set => handPriority = value;
	}

	public bool Predictable => ability.predictable;
	public float PredictTime => ability.predictTime;

	public float predictionTimeLeft;

	public virtual void Init(HandSplitAbility _ability) {
		ability = _ability;
		predictionTimeLeft = float.NegativeInfinity;
		if (!hand) hand = GetComponent<HandController>();
	}

	public virtual void StartPredict() {
		predictionTimeLeft = PredictTime;
	}

	public virtual void StopPredict() {
		predictionTimeLeft = float.NegativeInfinity;
	}

	public virtual void PredictionComplete() {
		onPrediction?.Invoke(this);
	}

	public abstract bool TryCast();

	public virtual bool CastStartCheck() => !blocked && ability.CastStartCheck();

	public virtual void StartCast() {
		SetUpCharging();
		casting = true;
		onCastStart?.Invoke();
		ability.onActivate?.Invoke();
	}

	public event Action<IActiveAbility> OnCast;

	protected virtual void Update() {
		predictionTimeLeft -= Time.deltaTime;
	}

	protected virtual void SetUpCharging() {
		//hand.animator.SetTrigger(AbilityTriggerAnimID);
		//hand.animator.SetTrigger(AbilityTriggerID);
		if (chargingPart) chargingPart.gameObject.SetActive(true);
		hand.HandPlaySound(ability.castAudio.startCast);
		hand.HandPlayLoopSound(ability.castAudio.loopCast);
		hand.Vibrate(ability.vibrations.loopCast);
	}

	public virtual void BreakCast() {
		casting = false;
		HideCharging();
		hand.ResetActive(this);
		hand.StopVibrate();
		VibrationSettings settings = ability.vibrations.endCast;
		settings.amplitude /= 5;
		hand.Vibrate(settings);
	}

	public virtual void HideCharging() {
		//hand.animator.SetTrigger(AbilityEndTriggerID);
		if (chargingPart) chargingPart.gameObject.SetActive(false);
		hand.HandStopSound();
		hand.HandPlaySound(ability.castAudio.endCast);
	}

	protected virtual bool CastCheck() {
		return casting;
	}

	protected virtual void SuccessCast() {
		onNoReturn?.Invoke(this);
		onSuccessCast?.Invoke(this);
		ability.onSuccessCast?.Invoke();
		hand.HandPlaySound(ability.castAudio.successCast);
		hand.StopVibrate();
		hand.Vibrate(ability.vibrations.endCast);
	}

	public virtual void EndCast() {
		onCastCheck?.Invoke(this);

		if (CastCheck())
			SuccessCast();
		else
			hand.StopVibrate();
		HideCharging();
		casting = false;
		hand.ResetActive(this);
		onCastEnd?.Invoke(this);
		ability.onDeactivate?.Invoke();
	}


	public abstract void Destroy();

	protected virtual int PriorityModifiers() {
		return 0;
	}


	private static readonly int AbilityTriggerAnimID = Animator.StringToHash("AbilityTrigger");
	public int AbilityTriggerID;
	public int AbilityEndTriggerID;
}

public abstract class AbilityHandPart<A> : AbilityHandPart where A : HandSplitAbility {
	public new A ability => base.ability as A;

	public override void Init(HandSplitAbility _ability) {
		base.Init(_ability);
		ability[hand.type] = this;
	}

	public override bool TryCast() {
		if (!CastStartCheck()) return false;
		// Debug.Log($"TryCast -{this} at Time = {Time.time}");
		if (hand.activeAbility == null) {
			hand.toActivateList.Add(this);
			return true;
		} 
		//else {
		//	if (hand.activeAbility != this && hand.activeAbility.ability is IGemAbility) {
		//		casting = AbilityMergeController.instance.InitMergeAsset(hand.activeAbility, this);
		//		return true;
		//	}
		//}
		return false;
	}


	public override void Destroy() {
		if (casting)
			EndCast();
		Destroy(this);
	}
}