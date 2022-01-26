using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivatable {
	void Activate();
}

interface ILongActivatable : IActivatable, IInterruptible {
	void StartActivation();
}