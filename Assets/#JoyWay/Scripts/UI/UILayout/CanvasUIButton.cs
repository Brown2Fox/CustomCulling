using UnityEngine;
using UnityEngine.UI;

public class CanvasUIButton : UIButton {
	private Graphic graphic;


	public void OnEnable() {
		graphic = GetComponent<Graphic>();
	}


	public override void ChangeColor(UIButtonColors _color) {
		graphic.material.color = _color.button;
	}
}