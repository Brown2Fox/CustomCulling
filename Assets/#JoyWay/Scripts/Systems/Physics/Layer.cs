using System;

public enum Layer {
	Default = 0,
	TransparentFX = 1,
	IgnoreRaycast = 2,

	Water = 4,
	UI = 5,


	Entity = 8,
	CharacterLayer = 9,
	GrabbedObject = 10,
	PostProcess = 11,
	VisionOnly = 12,
	NoneLayer = 13,
	UIArea = 14,

	Vision = 30,
	Navigation = 31
}


[Flags]
public enum FlaggedLayer {
	None = 0,
	Default = 1 << Layer.Default,
	TransparentFX = 1 << Layer.TransparentFX,
	IgnoreRaycast = 1 << Layer.IgnoreRaycast,

	Water = 1 << Layer.Water,
	UI = 1 << Layer.UI,


	Entity = 1 << Layer.Entity,
	CharacterLayer = 1 << Layer.CharacterLayer,
	GrabbedObject = 1 << Layer.GrabbedObject,
	PostProcess = 1 << Layer.PostProcess,


	Vision = 1 << Layer.Vision,
	DefaultVision = Default | Vision,
	Navigation = 1 << Layer.Navigation,
	VisionNavigation = Vision | Navigation,
}