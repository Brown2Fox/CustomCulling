using System;
using UnityEngine;
using Valve.VR;

namespace JoyWay.Systems.InputSystem {
	[Obsolete]
	public class SteamInput : InputSystem {
		public SteamVR_Action_Boolean triggerButton;
		public SteamVR_Action_Boolean gripButton;
		public SteamVR_Action_Boolean teleport;
		public SteamVR_Action_Boolean dash;
		public SteamVR_Action_Boolean turnLeft;
		public SteamVR_Action_Boolean turnRight;
		public SteamVR_Action_Boolean pause;


		public SteamInputAsset inputAsset;

		public SteamVR_Action_Single trigger;
		public SteamVR_Action_Single grip;
		public SteamVR_Action_Vector2 movePad;

		public SteamVR_Action_Vibration vibration;

		public SteamVR_Action_Pose pose;

		private void OnEnable() {
			SteamVR_Input.actionSets[1].Activate();
			SteamVR_Input.actionSets[2].Activate();

			triggerButton.AddOnChangeListener(Shoot.ButtonChanged, SteamVR_Input_Sources.LeftHand);
			triggerButton.AddOnChangeListener(Shoot.ButtonChanged, SteamVR_Input_Sources.RightHand);
			triggerButton.AddOnChangeListener(UIClick.ButtonChanged, SteamVR_Input_Sources.LeftHand);
			triggerButton.AddOnChangeListener(UIClick.ButtonChanged, SteamVR_Input_Sources.RightHand);

			gripButton.AddOnChangeListener(Grab.ButtonChanged, SteamVR_Input_Sources.LeftHand);
			gripButton.AddOnChangeListener(Grab.ButtonChanged, SteamVR_Input_Sources.RightHand);

			// dash.AddOnChangeListener(Dash.ButtonChanged, SteamVR_Input_Sources.LeftHand);
			// dash.AddOnChangeListener(Dash.ButtonChanged, SteamVR_Input_Sources.RightHand);

			pause.AddOnChangeListener(Pause.ButtonChanged, SteamVR_Input_Sources.LeftHand);
			pause.AddOnChangeListener(Pause.ButtonChanged, SteamVR_Input_Sources.RightHand);

			turnLeft.AddOnChangeListener(ActionButtonChanged, SteamVR_Input_Sources.LeftHand);
			turnLeft.AddOnChangeListener(ActionButtonChanged, SteamVR_Input_Sources.RightHand);
			turnRight.AddOnChangeListener(ActionButtonChanged, SteamVR_Input_Sources.LeftHand);
			turnRight.AddOnChangeListener(ActionButtonChanged, SteamVR_Input_Sources.RightHand);


			foreach (var sta in inputAsset.abilityInputs) {
				sta.steamAction.AddOnChangeListener(abilityActions[sta.abilityButton].ButtonChanged, SteamVR_Input_Sources.LeftHand);
				sta.steamAction.AddOnChangeListener(abilityActions[sta.abilityButton].ButtonChanged, SteamVR_Input_Sources.RightHand);
			}
		}

		private void OnDisable() {
			triggerButton.RemoveOnChangeListener(Shoot.ButtonChanged, SteamVR_Input_Sources.LeftHand);
			triggerButton.RemoveOnChangeListener(Shoot.ButtonChanged, SteamVR_Input_Sources.RightHand);
			triggerButton.RemoveOnChangeListener(UIClick.ButtonChanged, SteamVR_Input_Sources.LeftHand);
			triggerButton.RemoveOnChangeListener(UIClick.ButtonChanged, SteamVR_Input_Sources.RightHand);

			gripButton.RemoveOnChangeListener(Grab.ButtonChanged, SteamVR_Input_Sources.LeftHand);
			gripButton.RemoveOnChangeListener(Grab.ButtonChanged, SteamVR_Input_Sources.RightHand);

			// dash.RemoveOnChangeListener(Dash.ButtonChanged, SteamVR_Input_Sources.LeftHand);
			// dash.RemoveOnChangeListener(Dash.ButtonChanged, SteamVR_Input_Sources.RightHand);

			pause.RemoveOnChangeListener(Pause.ButtonChanged, SteamVR_Input_Sources.LeftHand);
			pause.RemoveOnChangeListener(Pause.ButtonChanged, SteamVR_Input_Sources.RightHand);

			turnLeft.RemoveOnChangeListener(ActionButtonChanged, SteamVR_Input_Sources.LeftHand);
			turnLeft.RemoveOnChangeListener(ActionButtonChanged, SteamVR_Input_Sources.RightHand);
			turnRight.RemoveOnChangeListener(ActionButtonChanged, SteamVR_Input_Sources.LeftHand);
			turnRight.RemoveOnChangeListener(ActionButtonChanged, SteamVR_Input_Sources.RightHand);


			foreach (var sta in inputAsset.abilityInputs) {
				sta.steamAction.RemoveOnChangeListener(abilityActions[sta.abilityButton].ButtonChanged, SteamVR_Input_Sources.LeftHand);
				sta.steamAction.RemoveOnChangeListener(abilityActions[sta.abilityButton].ButtonChanged, SteamVR_Input_Sources.RightHand);
			}


			SteamVR_Input.actionSets[1].Deactivate();
			SteamVR_Input.actionSets[2].Deactivate();
		}


		private void Update() {
			VibrateProcess();
			moveVector = movePad[SteamVR_Input_Sources.LeftHand].axis;
		}


		private void ActionButtonChanged(SteamVR_Action_Boolean _action, SteamVR_Input_Sources _source, bool _state) {
			if (_action == turnLeft)
				onTurnLeft?.Invoke(_state);
			if (_action == turnRight)
				onTurnRight?.Invoke(_state);
		}

		public override void Vibrate(VibrationSettings settings, HandType hand) {
			if (hand == HandType.None) return;

			if (hand == HandType.Left) {
				tLeft = Time.time + settings.duration;
				vibrationSettingsLeft = settings;
			} else if (hand == HandType.Right) {
				tRight = Time.time + settings.duration;
				vibrationSettingsRight = settings;
			}
		}

		public override void VibrateProcess() {
			if (tLeft > Time.time)
				vibration.Execute(
								  vibrationSettingsLeft.delay,
								  0.01f,
								  vibrationSettingsLeft.frequency * 320,
								  vibrationSettingsLeft.amplitude,
								  (SteamVR_Input_Sources) HandType.Left);
			if (tRight > Time.time)
				vibration.Execute(
								  vibrationSettingsRight.delay,
								  0.01f,
								  vibrationSettingsRight.frequency * 320,
								  vibrationSettingsRight.amplitude,
								  (SteamVR_Input_Sources) HandType.Right);
		}

		public override Vector3 GetHandPosition(HandType hand) {
			return pose.GetLocalPosition((SteamVR_Input_Sources) hand);
		}

		public override Quaternion GetHandRotation(HandType hand) {
			return pose.GetLocalRotation((SteamVR_Input_Sources) hand);
		}

		public override Vector3 GetHeadPosition() {
			return pose.GetLocalPosition(SteamVR_Input_Sources.Head);
		}

		public override float GetTriggerSquezee(HandType _hand) {
			return trigger[(SteamVR_Input_Sources) _hand].axis;
		}

		public override float GetGripSquezee(HandType _hand) {
			return grip[(SteamVR_Input_Sources) _hand].axis;
		}

		public override Quaternion GetHeadRotation() {
			return pose.GetLocalRotation(SteamVR_Input_Sources.Head);
		}
	}

	[Serializable]
	public struct SteamToAbilityInput {
		public InputAbilityNames abilityButton;
		public SteamVR_Action_Boolean steamAction;
	}
}