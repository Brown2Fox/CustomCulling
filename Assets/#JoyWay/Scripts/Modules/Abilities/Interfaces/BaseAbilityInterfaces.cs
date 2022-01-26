using System;
using UnityEngine;


public interface IActiveAbility {
	bool CastStartCheck();
	void StartCast();
	event Action<IActiveAbility> OnCast;
}

public interface ILongAbility : IActiveAbility {
	void EndCast();
}


public interface IChannelingAbility : ILongAbility, IInterruptible {
	bool EndCastCheck();
	void SuccessCast();
	void FailedCast();

	event Action<IActiveAbility> OnSuccessCast;
	event Action<IActiveAbility> OnFailedCast;
}

public interface IInterruptible {
	void Interrupt();
}

public interface IAbilityWithCastEffect {
	void StartEffect();
	void EndEffect();
}

public interface IZoneHintable {
	void ShowHint(Vector3 _point);
	void HideHint();
}

public interface IChargeable {
	void StartCharge();
	bool EndCharge();
}


public interface IAttackAbility { }