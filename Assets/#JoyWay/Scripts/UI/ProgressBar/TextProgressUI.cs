using TMPro;
using UnityEngine;

public class TextProgressUI : MonoBehaviour {
	public TextMeshPro text;
	public Component component;
	private IProgressBarUI target;
	private int lastvalue;


	private void Start() {
		target = component as IProgressBarUI;
	}

	private void Update() {
		var curVal = (int) target.GetCurrentValue();
		if (lastvalue != curVal) {
			lastvalue = curVal;
			text.text = curVal.ToString();
		}
	}
}