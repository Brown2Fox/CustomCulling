using System;


public enum AbilityType {
	Passive = 0,
	Fire = 1 << 1,
	Ice = 1 << 2,
	// UndefinedTrigger = 1 << 3,

	Air = 1 << 5,
	Electro = 1 << 6,
	// UndefinedGrab = 1 << 7,

	Move = 1 << 10,
	Dash = 1 << 11,


	Grab = 1 << 20,
	Telekinesis = 1 << 21,
	Climb = 1 << 22,


	Shoot = 1 << 30,
}

public enum GemAbilityType {
	Fire = AbilityType.Fire,
	Ice = AbilityType.Ice,
	// UndefinedTrigger = 1 << 3,

	Air = AbilityType.Air,
	Electro = AbilityType.Electro,
	// UndefinedGrab = 1 << 3,
}


[Flags]
public enum AbilityTypeFlagged {
	Passive = AbilityType.Passive,


	Fire = AbilityType.Fire,
	Ice = AbilityType.Ice,
	// UndefinedTrigger = 1 << 3,

	Air = AbilityType.Air,
	Electro = AbilityType.Electro,
	// UndefinedGrab = 1 << 3,

	Grab = AbilityType.Grab,
	Telekinesis = AbilityType.Telekinesis,
	Climb = AbilityType.Climb,


	Gem_Ability = Fire | Ice | Air | Electro,
	Ability_On_Trigger = Fire | Ice,
	Ability_On_Grab = Air | Electro | Grab | Telekinesis | Climb,
}


public enum InputAbilityNames {
	Passive = AbilityType.Passive,


	Fire = AbilityType.Fire,
	Ice = AbilityType.Ice,

	Air = AbilityType.Air,
	Electro = AbilityType.Electro,

	//Move = AbilityType.Move,
	Dash = AbilityType.Dash,

	Grab = AbilityType.Grab,
	GrabApply = AbilityType.Grab + 1,
	Telekinesis = AbilityType.Telekinesis,
	TelekinesisBuff = AbilityType.Telekinesis + 1,
	Climb = AbilityType.Climb,

	Shoot = AbilityType.Shoot,
}