using UnityEngine;

public class UIArea : MonoBehaviour {
	public AnimationCurve alphaCurve;

	private void OnEnable() {
		if (UICursorController.instance != null)
			UICursorController.instance.AddArea(this);
	}


	private void OnDisable() {
		UICursorController.instance.RemoveArea(this);
	}
}