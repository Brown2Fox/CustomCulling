using UnityEngine;

namespace JoyWay.Systems.InputSystem {
	[CreateAssetMenu(fileName = "OculusInputAsset", menuName = "JoyWay/Input/OculusInputAsset", order = 0)]
	public class OculusInputAsset : InputAsset {
		public OculusToAbilityInput[] abilityInputs;


		public override void SetDefault() {
			abilityInputs = new[] {
									  //gem abilities 
									  new OculusToAbilityInput {abilityButton = InputAbilityNames.Fire, oculusButton = OVRInput.Button.PrimaryIndexTrigger},
									  new OculusToAbilityInput {abilityButton = InputAbilityNames.Ice, oculusButton = OVRInput.Button.PrimaryIndexTrigger},
									  new OculusToAbilityInput {abilityButton = InputAbilityNames.Air, oculusButton = OVRInput.Button.PrimaryHandTrigger},
									  new OculusToAbilityInput {abilityButton = InputAbilityNames.Electro, oculusButton = OVRInput.Button.PrimaryHandTrigger},

									  //move abilities
									  new OculusToAbilityInput {abilityButton = InputAbilityNames.Dash, oculusButton = OVRInput.Button.One},

									  //hand abilities
									  new OculusToAbilityInput {abilityButton = InputAbilityNames.Grab, oculusButton = OVRInput.Button.PrimaryHandTrigger},
									  new OculusToAbilityInput {abilityButton = InputAbilityNames.GrabApply, oculusButton = OVRInput.Button.PrimaryIndexTrigger},
									  new OculusToAbilityInput {abilityButton = InputAbilityNames.Telekinesis, oculusButton = OVRInput.Button.PrimaryHandTrigger},
									  new OculusToAbilityInput {abilityButton = InputAbilityNames.TelekinesisBuff, oculusButton = OVRInput.Button.PrimaryIndexTrigger},
									  new OculusToAbilityInput {abilityButton = InputAbilityNames.Climb, oculusButton = OVRInput.Button.PrimaryHandTrigger},

									  //props abilities
									  new OculusToAbilityInput {abilityButton = InputAbilityNames.Shoot, oculusButton = OVRInput.Button.PrimaryIndexTrigger},
								  };
		}
	}
}