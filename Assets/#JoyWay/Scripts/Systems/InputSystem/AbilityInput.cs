using System;
using UnityEngine.InputSystem;

namespace JoyWay.Systems.InputSystem {
	public class AbilityInput : InputWrapper.IAbilitiesActions {
		public enum Type {
			Primary,
			Secondary,
			Dash,
			Telekinesis,
			Grab,
		}


		private static readonly CustomInputAction Primary = new CustomInputAction();
		private static readonly CustomInputAction Secondary = new CustomInputAction();
		private static readonly CustomInputAction Dash = new CustomInputAction();
		private static readonly CustomInputAction Telekinesis = new CustomInputAction();
		private static readonly CustomInputAction Grab = new CustomInputAction();

		public void OnPrimary(InputAction.CallbackContext _context) {
			OpenXRInput.PerformInputAction(Primary, _context);
		}

		public void OnSecondary(InputAction.CallbackContext _context) {
			OpenXRInput.PerformInputAction(Secondary, _context);
		}

		public void OnDash(InputAction.CallbackContext _context) {
			OpenXRInput.PerformInputAction(Dash, _context);
		}

		public void OnTelekinesis(InputAction.CallbackContext _context) {
			OpenXRInput.PerformInputAction(Telekinesis, _context);
		}

		public void OnGrab(InputAction.CallbackContext _context) {
			OpenXRInput.PerformInputAction(Grab, _context);
		}

		public static CustomInputAction GetAction(Type _input) {
			switch (_input) {
				case Type.Primary:     return Primary;
				case Type.Secondary:   return Secondary;
				case Type.Dash:        return Dash;
				case Type.Telekinesis: return Telekinesis;
				case Type.Grab:        return Grab;
			}

			return null;
		}
	}
}