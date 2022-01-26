using System;
using UnityEngine;

namespace JoyWay.Systems.InputSystem {
	[Obsolete]
	public class OculusInput : InputSystem {
		public OVRInput.Button shoot;
		public OVRInput.Button gripButton;
		public OVRInput.Button teleport;
		public OVRInput.Button dash;
		public OVRInput.Button turnLeft;
		public OVRInput.Button turnRight;
		public OVRInput.Button pause;
		public OVRInput.Controller pauseHand;
		public OculusInputAsset abilityInputAsset;

		public override void Vibrate(VibrationSettings settings, HandType hand) {
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
				OVRInput.SetControllerVibration(
												vibrationSettingsLeft.frequency,
												vibrationSettingsLeft.amplitude,
												(OVRInput.Controller) HandType.Left);
			else
				OVRInput.SetControllerVibration(0, 0, (OVRInput.Controller) HandType.Left);
			if (tRight > Time.time)
				OVRInput.SetControllerVibration(
												vibrationSettingsRight.frequency,
												vibrationSettingsRight.amplitude,
												(OVRInput.Controller) HandType.Right);
			else
				OVRInput.SetControllerVibration(0, 0, (OVRInput.Controller) HandType.Right);
		}

		private void Update() {
			VibrateProcess();
			if (OVRInput.GetDown(shoot, OVRInput.Controller.LTouch))
				Shoot.Trigger(true, HandType.Left);
			if (OVRInput.GetDown(shoot, OVRInput.Controller.RTouch))
				Shoot.Trigger(true, HandType.Right);
			if (OVRInput.GetUp(shoot, OVRInput.Controller.LTouch))
				Shoot.Trigger(false, HandType.Left);
			if (OVRInput.GetUp(shoot, OVRInput.Controller.RTouch))
				Shoot.Trigger(false, HandType.Right);

			if (OVRInput.GetDown(shoot, OVRInput.Controller.LTouch))
				UIClick.Trigger(true, HandType.Left);
			if (OVRInput.GetDown(shoot, OVRInput.Controller.RTouch))
				UIClick.Trigger(true, HandType.Right);
			if (OVRInput.GetUp(shoot, OVRInput.Controller.LTouch))
				UIClick.Trigger(false, HandType.Left);
			if (OVRInput.GetUp(shoot, OVRInput.Controller.RTouch))
				UIClick.Trigger(false, HandType.Right);

			if (OVRInput.GetDown(gripButton, OVRInput.Controller.LTouch))
				Grab.Trigger(true, HandType.Left);
			if (OVRInput.GetDown(gripButton, OVRInput.Controller.RTouch))
				Grab.Trigger(true, HandType.Right);
			// if (OVRInput.GetDown(dash, OVRInput.Controller.LTouch))
			// 	Dash.Trigger(true, HandType.Left);
			// if (OVRInput.GetDown(dash, OVRInput.Controller.RTouch))
			// 	Dash.Trigger(true, HandType.Right);
			if (OVRInput.GetDown(turnLeft, OVRInput.Controller.RTouch))
				onTurnLeft?.Invoke(true);
			if (OVRInput.GetUp(turnLeft, OVRInput.Controller.RTouch))
				onTurnLeft?.Invoke(false);
			if (OVRInput.GetDown(turnRight, OVRInput.Controller.RTouch))
				onTurnRight?.Invoke(true);
			if (OVRInput.GetUp(turnRight, OVRInput.Controller.RTouch))
				onTurnRight?.Invoke(false);
			if (OVRInput.GetDown(pause, pauseHand))
				Pause.Trigger(true, HandType.OsulusBoth);

			foreach (OculusToAbilityInput sta in abilityInputAsset.abilityInputs) {
				if (OVRInput.GetDown(sta.oculusButton, OVRInput.Controller.LTouch))
					abilityActions[sta.abilityButton].Trigger(true, HandType.Left);
				if (OVRInput.GetUp(sta.oculusButton, OVRInput.Controller.LTouch))
					abilityActions[sta.abilityButton].Trigger(false, HandType.Left);
				if (OVRInput.GetDown(sta.oculusButton, OVRInput.Controller.RTouch))
					abilityActions[sta.abilityButton].Trigger(true, HandType.Right);
				if (OVRInput.GetUp(sta.oculusButton, OVRInput.Controller.RTouch))
					abilityActions[sta.abilityButton].Trigger(false, HandType.Right);
			}

			moveVector = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
		}

		public override Vector3 GetHandPosition(HandType hand) {
			return OVRInput.GetLocalControllerPosition((OVRInput.Controller) ((int) hand * 32));
		}

		public override Quaternion GetHandRotation(HandType hand) {
			return OVRInput.GetLocalControllerRotation((OVRInput.Controller) ((int) hand * 32));
		}

		public override Vector3 GetHeadPosition() {
			Vector3 centerEyePosition = Vector3.zero;
			OVRNodeStateProperties.GetNodeStatePropertyVector3(UnityEngine.XR.XRNode.CenterEye, NodeStatePropertyType.Position, OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render, out centerEyePosition);
			return centerEyePosition;
		}

		public override float GetTriggerSquezee(HandType _hand) {
			return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, (OVRInput.Controller) _hand);
		}

		public override float GetGripSquezee(HandType _hand) {
			return OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, (OVRInput.Controller) _hand);
		}

		public override Quaternion GetHeadRotation() {
			Quaternion centerEyeRotation = Quaternion.identity;
			OVRNodeStateProperties.GetNodeStatePropertyQuaternion(UnityEngine.XR.XRNode.CenterEye, NodeStatePropertyType.Orientation, OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render,
																  out centerEyeRotation);
			return centerEyeRotation;
		}
	}

	[Serializable]
	public struct OculusToAbilityInput {
		public InputAbilityNames abilityButton;
		public OVRInput.Button oculusButton;
	}
}