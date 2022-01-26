using UnityEngine;

namespace JoyWay.Registries {
	public class AssetsDatabase : ScriptableObject {
		public static AssetsDatabase current;
		public AbilityAssetRegistry abilities;


		public static BaseAbilityAsset GetAbilityByID(int _id) {
			if (_id < current.abilities.entries.Count)
				return current.abilities.entries[_id];
			else return null;
		}

		public void Init() {
			current = this;
		}
	}
}