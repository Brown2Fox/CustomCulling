using UnityEngine;
using UnityEngine.Events;

public class TelekinesisGrabEvent : MonoBehaviour, ITelekinesisable {
	public UnityEvent
		onTelekinesisGrab,
		onTelekinesisDrop;

	public void Grabbed(TelekinesisHandPart tk) {
		onTelekinesisGrab.Invoke();
	}

	public void Dropped() {
		onTelekinesisDrop.Invoke();
	}
}
