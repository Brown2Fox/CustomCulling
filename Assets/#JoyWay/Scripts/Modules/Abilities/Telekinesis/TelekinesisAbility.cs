using System;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisAbility : HandSplitAbility<TelekinesisHandPart>, IShowingStat {
	private readonly float blockRange = .28f;

	public float sqrBlockRange;

	public TelekinesisSettings currentSettings;
	public TelekinesisSettings defaultSettings;

	[Header("Show stats")]
	public bool show_Dist_byDefault = true;
	public bool show_Power_byDefault = true;

	protected override void InitBehaviour() {
		currentSettings = defaultSettings;
		sqrBlockRange = blockRange * blockRange;
		base.InitBehaviour();

		if (owner) owner.showingStats.Add(this);
	}

	public override void ResetAbility() {
	}

	public void SetupSize(float _value) {
		currentSettings.maxDist = _value;
		var scale = new Vector3(_value, _value, _value);
		left.Scale(scale);
		right.Scale(scale);
	}

	public List<ShowingStatsUI.NameDiscr> GetStatsInfo() {

		List<ShowingStatsUI.NameDiscr> nameDiscrs = new List<ShowingStatsUI.NameDiscr>();
		ShowingStatsUI.NameDiscr telekinesis = new ShowingStatsUI.NameDiscr();

		telekinesis.Fill("Telekinesis", "Distance", currentSettings.maxDist.ToString(), 
			(GameMain.instance.developerSettingsAsset.showAllStats) || show_Dist_byDefault);
		nameDiscrs.Add(telekinesis);

		telekinesis.Fill("Telekinesis", "Power", currentSettings.throwPower.ToString(), 
			(GameMain.instance.developerSettingsAsset.showAllStats) || show_Power_byDefault);
		nameDiscrs.Add(telekinesis);

		return nameDiscrs;
	}
}

[Serializable]
public struct TelekinesisSettings {
	public float pullScale;
	public float minDist;
	public float maxDist;
	public float maximumVelocityInPoint;
	public float rangeMaxVelocityMultiplier;
	public float adjustVelocityMultiplier;
	public float constantPull;
	public float throwPower;
	public float targetThrowPower;
	public float verticalCorrection;
	public float grabDistance;
}