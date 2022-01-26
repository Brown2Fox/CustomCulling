using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace JoyWay.Systems.InputSystem {
	public abstract class InputSystem : Singleton<InputSystem> {
		public static Vector2 moveVector;

		public static CustomInputAction Shoot = new CustomInputAction();
		public static CustomInputAction Reload = new CustomInputAction();
		public static CustomInputAction Jump = new CustomInputAction();
		public static CustomInputAction SuperHeroGrounding = new CustomInputAction();

		public static readonly Dictionary<InputAbilityNames, CustomInputAction> AbilityActions = new Dictionary<InputAbilityNames, CustomInputAction>();

		protected override void Awake() {
			base.Awake();

			foreach (InputAbilityNames a in Enum.GetValues(typeof(InputAbilityNames))) {
				CustomInputAction action = new CustomInputAction();
				action.ignoreOnPause = true;
				AbilityActions.Add(a, action);
			}

			Shoot.ignoreOnPause = true;
			Reload.ignoreOnPause = true;
			Jump.ignoreOnPause = true;
			SuperHeroGrounding.ignoreOnPause = true;
		}

		public virtual Vector3 GetHandPosition(HandType _hand) {
			return Vector3.zero;
		}

		public virtual Quaternion GetHandRotation(HandType _hand) {
			return Quaternion.identity;
		}

		public virtual Vector3 GetHeadPosition() {
			return Vector3.zero;
		}


		public abstract float GetTriggerSquezee(HandType _hand);
		public abstract float GetGripSquezee(HandType _hand);


		public virtual Quaternion GetHeadRotation() {
			return Quaternion.identity;
		}

		public static void AddAbilityListener(InputAbilityNames _button, Action<bool> _action, HandType _hand) {
			var iaction = AbilityActions[_button];
			iaction.AddListener(_hand, _action);
		}

		public static void RemoveAbilityListener(InputAbilityNames _button, Action<bool> _action, HandType _hand) {
			var iaction = AbilityActions[_button];
			iaction.RemoveListener(_hand, _action);
		}

		//vibration
		//delay dont work
		protected float tLeft = 0;
		protected float tRight = 0;
		protected VibrationSettings vibrationSettingsLeft;
		protected VibrationSettings vibrationSettingsRight;

		public abstract void Vibrate(VibrationSettings _settings, HandType _hand);
		public abstract void VibrateProcess();

		public virtual void StopVibrate(HandType _hand) {
			if (_hand == HandType.Left)
				tLeft = 0;
			else if (_hand == HandType.Right)
				tRight = 0;
		}
	}

	public class CustomInputAction {
		public Action<bool> rightAction;
		public Action<bool> leftAction;
		public Action<bool, HandType> action;
		public bool state;
		public bool ignoreOnPause;

		public void AddListener(HandType _hand, Action<bool> _action) {
			if ((_hand & HandType.Right) != 0)
				rightAction += _action;
			if ((_hand & HandType.Left) != 0)
				leftAction += _action;
		}

		public void AddListener(Action<bool, HandType> _action) {
			action += _action;
		}

		public void RemoveListener(HandType _hand, Action<bool> _action) {
			if ((_hand & HandType.Right) != 0)
				rightAction -= _action;
			if ((_hand & HandType.Left) != 0)
				leftAction -= _action;
		}

		public void RemoveListener(Action<bool, HandType> _action) {
			this.action -= _action;
		}

		public void Trigger(bool _state, HandType _hand) {
			if (PauseManager.instance.state && ignoreOnPause) return;

			if ((_hand & HandType.Right) != 0)
				rightAction?.Invoke(_state);

			if ((_hand & HandType.Left) != 0)
				leftAction?.Invoke(_state);

			action?.Invoke(_state, _hand);
			// Debug.Log("triggered");

			this.state = _state;
		}
	}


	[Serializable]
	public struct VibrationSettings {
		public float amplitude;
		public float frequency;
		public float duration;
		public float delay;


		public static VibrationSettings Lerp(VibrationSettings _a, VibrationSettings _b, float _t) {
			_t = Mathf.Clamp01(_t);
			_a.delay = Mathf.Lerp(_a.delay, _b.delay, _t);
			_a.frequency = Mathf.Lerp(_a.frequency, _b.frequency, _t);
			_a.duration = Mathf.Lerp(_a.duration, _b.duration, _t);
			_a.amplitude = Mathf.Lerp(_a.amplitude, _b.amplitude, _t);
			return _a;
		}
	}

	[Serializable]
	public struct WeaponVibrationSettings {
		public VibrationSettings shootVibration;
		public VibrationSettings emptyShootVibration;
		public VibrationSettings reloadVibration;
	}

	[Serializable]
	public struct HandSplitAbilityVibrationSettings {
		public VibrationSettings startCast;
		public VibrationSettings loopCast;
		public VibrationSettings endCast;
	}
}