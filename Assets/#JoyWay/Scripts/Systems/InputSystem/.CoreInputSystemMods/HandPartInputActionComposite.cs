using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad] // Automatically register in editor.
#endif

// Determine how GetBindingDisplayString() formats the composite by applying
// the  DisplayStringFormat attribute.
[DisplayStringFormat("{firstPart}+{secondPart}")]
public class HandPartInputActionComposite : InputBindingComposite<bool> {
	public override bool ReadValue(ref InputBindingCompositeContext context) {
		return true;
	}


	static HandPartInputActionComposite() {
		// Can give custom name or use default (type name with "Composite" clipped off).
		// Same composite can be registered multiple times with different names to introduce
		// aliases.
		//
		// NOTE: Registering from the static constructor using InitializeOnLoad and
		//       RuntimeInitializeOnLoadMethod is only one way. You can register the
		//       composite from wherever it works best for you. Note, however, that
		//       the registration has to take place before the composite is first used
		//       in a binding. Also, for the composite to show in the editor, it has
		//       to be registered from code that runs in edit mode.
		InputSystem.RegisterBindingComposite<HandPartInputActionComposite>("HandPartAbilityAction");
	}
}