using System;
using JoyWay.Systems.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateFromInput : MonoBehaviour {
	public Transform target;
	public Transform point;
	public Transform trackingSpace;

	public float turnDegrees = 30f;

	private Quaternion rot = Quaternion.identity;
	private Quaternion rotinv = Quaternion.identity;


	private bool continiousRotation;
	private bool snapping;


	private void OnEnable() {
		OpenXRInput.wrapper.PlayerControls.Rotate.performed += RotateCheck;
	}

	private void RotateCheck(InputAction.CallbackContext _context) {
		var value = _context.ReadValue<float>();
		switch (GameMain.settings.serializable.input.turnType) {
			case TurnType.SnapTurn:
				if (value > -.7 && value < .7) {
					snapping = false;
					return;
				}

				if (snapping) return;

				snapping = true;

				if (value > 0) {
					value = 1;
				} else {
					value = -1;
				}

				break;
			case TurnType.SmoothTurn:
				if (value == 0) {
					continiousRotation = false;
					return;
				}

				break;
		}

		Turn(value);
	}


	public float GetSnapRotation() {
		switch (GameMain.settings.serializable.input.snapTurnDegrees) {
			case SnapTurnDegrees._15:
				turnDegrees = 15;
				break;
			case SnapTurnDegrees._30:
				turnDegrees = 30;
				break;
			case SnapTurnDegrees._45:
				turnDegrees = 45;
				break;
			case SnapTurnDegrees._90:
				turnDegrees = 90;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		return turnDegrees;
	}


	public float GetSmoothRotation() {
		turnDegrees = 1 + (int) GameMain.settings.serializable.input.smoothTurnSpeed;
		return turnDegrees;
	}


	private void Turn(float value) {
		switch (GameMain.settings.serializable.input.turnType) {
			case TurnType.SnapTurn:
				GetSnapRotation();
				rot = Quaternion.Euler(0, turnDegrees * value, 0);
				Rotate(rot);
				break;
			case TurnType.SmoothTurn:
				GetSmoothRotation();
				turnDegrees *= value;
				continiousRotation = true;
				break;
		}
	}

	private void FixedUpdate() {
		if (continiousRotation) {
			rot = Quaternion.Euler(0, turnDegrees * 90f * Time.fixedDeltaTime, 0);
			Rotate(rot);
		}
	}


	private void Rotate(Quaternion _rot) {
		PosRot cam = point.Global();
		cam.rot = Quaternion.identity;
		PosRot space = target.Global();
		PosRot offset = cam.GetLocalOfChild(space);
		cam.rot = _rot;
		space = cam.GetGlobalOfChild(offset);
		target.CopyGlobalPosRot(space);
		trackingSpace.CopyGlobalPosRot(space);
	}


	// private void TurnLeft(bool _state) {
	// 	if (_state) {
	// 		switch (GameMain.settings.serializable.input.turnType) {
	// 			case TurnType.SnapTurn:
	// 				GetSnapRotation();
	// 				rot = Quaternion.Euler(0, -turnDegrees, 0);
	// 				Rotate(rot);
	// 				break;
	// 			case TurnType.SmoothTurn:
	// 				GetSmoothRotation();
	// 				turnDegrees *= -1;
	// 				continiousRotation = true;
	// 				break;
	// 		}
	// 	} else {
	// 		continiousRotation = false;
	// 	}
	// }
	//
	//
	// private void TurnRight(bool _state) {
	// 	if (_state) {
	// 		switch (GameMain.settings.serializable.input.turnType) {
	// 			case TurnType.SnapTurn:
	// 				GetSnapRotation();
	// 				rot = Quaternion.Euler(0, turnDegrees, 0);
	// 				Rotate(rot);
	// 				break;
	// 			case TurnType.SmoothTurn:
	// 				GetSmoothRotation();
	// 				continiousRotation = true;
	// 				break;
	// 		}
	// 	} else {
	// 		continiousRotation = false;
	// 	}
	// }
}