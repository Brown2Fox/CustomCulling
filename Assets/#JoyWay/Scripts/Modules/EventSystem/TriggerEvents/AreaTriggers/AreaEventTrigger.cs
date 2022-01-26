using UnityEngine.Events;

public class AreaEventTrigger : BaseAreaTrigger {
	public UnityEvent onTrigger;


	public override void Trigger() {
		onTrigger?.Invoke();
	}
}