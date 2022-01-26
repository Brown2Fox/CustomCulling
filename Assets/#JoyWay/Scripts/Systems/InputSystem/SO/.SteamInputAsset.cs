using UnityEngine;
using Valve.VR;

namespace JoyWay.Systems.InputSystem {
	[CreateAssetMenu(fileName = "SteamInputAsset", menuName = "JoyWay/Input/SteamInputAsset", order = 0)]
	public class SteamInputAsset : InputAsset {
		public SteamToAbilityInput[] abilityInputs;


		public override void SetDefault() {
			abilityInputs = new[] {
									  //gem abilities
									  new SteamToAbilityInput() {abilityButton = InputAbilityNames.Fire, steamAction = SteamVR_Actions.gemAbilities_Primary},
									  new SteamToAbilityInput() {abilityButton = InputAbilityNames.Ice, steamAction = SteamVR_Actions.gemAbilities_Primary},
									  new SteamToAbilityInput() {abilityButton = InputAbilityNames.Air, steamAction = SteamVR_Actions.gemAbilities_Secondary},
									  new SteamToAbilityInput() {abilityButton = InputAbilityNames.Electro, steamAction = SteamVR_Actions.gemAbilities_Secondary},

									  //move abilities
									  new SteamToAbilityInput() {abilityButton = InputAbilityNames.Dash, steamAction = SteamVR_Actions.locomotion_Dash},

									  //hand abilities
									  new SteamToAbilityInput() {abilityButton = InputAbilityNames.Grab, steamAction = SteamVR_Actions.default_Grab},
									  new SteamToAbilityInput() {abilityButton = InputAbilityNames.GrabApply, steamAction = SteamVR_Actions.default_Shoot},
									  new SteamToAbilityInput() {abilityButton = InputAbilityNames.Telekinesis, steamAction = SteamVR_Actions.default_Grab},
									  new SteamToAbilityInput() {abilityButton = InputAbilityNames.TelekinesisBuff, steamAction = SteamVR_Actions.default_Shoot},
									  new SteamToAbilityInput() {abilityButton = InputAbilityNames.Climb, steamAction = SteamVR_Actions.default_Grab},

									  //props abilities
									  new SteamToAbilityInput() {abilityButton = InputAbilityNames.Shoot, steamAction = SteamVR_Actions.default_Shoot},
								  };
		}
	}
}