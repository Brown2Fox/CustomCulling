using UnityEngine;
using TMPro;

public class AnimatorUI : MonoBehaviour {
	public Component component;
	private IProgressBarUI target;
	public TextMeshProUGUI textNumbers;

	public Animator animator;

	private void Start() {
		target = component as IProgressBarUI;
	}

	protected void LateUpdate() {
		animator.SetFloat("State", target.GetCurrentValue() / target.GetMaxValue());
		if (textNumbers)
			textNumbers.text = $"{target.GetCurrentValue():000}/{target.GetMaxValue():000}";
	}
}
