using UnityEngine;
using UnityEngine.Events;

public class TimeEvent : MonoBehaviour {
	public float timeDelay;
	public UnityEvent onTime;

	public void RunDelay() {
		Invoke(nameof(DoOnTime), timeDelay);
	}

	public void RunDelay(float _delay) {
		Invoke(nameof(DoOnTime), _delay);
	}

	public void CancelRun() {
		CancelInvoke(nameof(DoOnTime));
	}

	private void DoOnTime() {
		onTime.Invoke();
	}

	public void StopTimeEvent() {
		CancelInvoke(nameof(DoOnTime));
	}
}
