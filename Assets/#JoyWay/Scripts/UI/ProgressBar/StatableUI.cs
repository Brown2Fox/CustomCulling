using UnityEngine;

public class StatableUI : MonoBehaviour {
	public Component component, statableComponent;
	private IProgressBarUI target;
	private IStatable statable;

	private void Start() {
		target = component as IProgressBarUI;
		statable = statableComponent as IStatable;
	}

	//TODO логически странный код, но видимо была причина так написать..
	
	protected void LateUpdate() {
		statable.SetState(target.GetCurrentValue() / target.GetMaxValue());
	}
}
