using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.Scripting;

public struct BoolDevice {
	public bool state;
	public HandType hand;
}


public class BoolDeviceControl : ButtonControl {
	[Preserve]
	[InputControl(offset = 0)]
	public ButtonControl state { get; private set; }


	[Preserve]
	[InputControl(offset = 4)]
	public IntegerControl hand { get; private set; }

	public BoolDeviceControl() { }


	protected override void FinishSetup() {
		state = GetChildControl<ButtonControl>("value");
		hand = GetChildControl<IntegerControl>("hand");

		base.FinishSetup();
	}
}