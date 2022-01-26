using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[Serializable]
public struct SerializableSettings {
	public string version;
	public InputSettings input;
	public HeightSettings calibration;
	public GamePlayConfigs configs;
	// 0 - firstPlay,
	// 1-n - checkpoints,
	// -1 - intro completed
	public int introCHK;


	//public MyAudioSettings audio;
	//public Save save;
}


[Serializable]
public struct GamePlayConfigs {
	public bool friendlyFire;
	public bool damageNumbers;
	public bool impactPoints;
}

[Serializable]
public struct HeightSettings {
	public bool auto;
	public int height;
}

[Serializable]
public struct InputSettings {
	public TurnType turnType;
	public SnapTurnDegrees snapTurnDegrees;
	public SmoothTurnSpeed smoothTurnSpeed;
	public HandType mainHand;
	public bool longHolds;
}

public enum TurnType {
	Disabled = 0,
	SnapTurn,
	SmoothTurn
}

public enum SmoothTurnSpeed {
	_1X,
	_2X,
	_3X,
	_4X,
}

public enum SnapTurnDegrees {
	_15,
	_30,
	_45,
	_90,
}

