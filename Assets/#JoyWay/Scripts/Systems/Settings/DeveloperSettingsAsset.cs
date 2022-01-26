using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DeveloperSettingsAsset", menuName = "JoyWay/DeveloperSettings")]
public class DeveloperSettingsAsset : ScriptableObject {
	public bool _showAllStats;
	public bool showAllStats {
		get {
			if (deactivateDeveloperMode)
				return false;
			return _showAllStats;
		}
	}

	public bool _noPause;
	public bool noPause {
		get {
			if (deactivateDeveloperMode)
				return false;
			return _noPause;
		}
	}

	public bool _useHotKeys;
	public bool useHotKeys {
		get {
			if (deactivateDeveloperMode)
				return false;
			return _useHotKeys;
		}
	}
	[Space]
	public bool deactivateDeveloperMode;
}