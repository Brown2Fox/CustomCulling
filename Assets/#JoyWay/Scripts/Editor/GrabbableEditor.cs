using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SimpleGrabbable), true)]
public class GrabbableEditor : Editor {
	protected static GrabAbility ability;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		SimpleGrabbable grabbable = (target as SimpleGrabbable);

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("LeftGrab")) {
			if (!ability)
				ability = Player.instance.owner.aContainer.GetEntryOfType<GrabAbility>();
			GrabHandPart grabber = ability[HandType.Left];
			grabber.ForceGrab(grabbable);
		}

		if (GUILayout.Button("Drop"))
			grabbable.GetGrabber().TryDrop();

		if (GUILayout.Button("RightGrab")) {
			if (!ability)
				ability = Player.instance.owner.aContainer.GetEntryOfType<GrabAbility>();
			GrabHandPart grabber = ability[HandType.Right];
			grabber.ForceGrab(grabbable);
		}

		GUILayout.EndHorizontal();

		if (GUILayout.Button("Update Offset"))
			grabbable.UpdateOffset();
	}
}