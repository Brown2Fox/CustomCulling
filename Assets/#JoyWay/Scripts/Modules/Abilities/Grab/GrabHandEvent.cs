using UnityEngine;
using UnityEngine.Events;

public class GrabHandEvent : MonoBehaviour {
	private SimpleGrabbable grabbable;
	public HandType handType;

	public UnityEvent onLeftGrab;
	public UnityEvent onRightGrab;
	public UnityEvent onBothGrab;

	public UnityEvent onLeftUnGrab;
	public UnityEvent onRightUnGrab;
	public UnityEvent onBothUnGrab;

	private void Awake() {
		grabbable = GetComponent<SimpleGrabbable>();
		if (grabbable == null) return;
		grabbable.onGrabbed.AddListener(Grab);
		grabbable.onDropped.AddListener(UnGrab);
	}

	private void Grab(GrabHandPart grabHand) {
		if (grabHand.hand.type == HandType.Left) onLeftGrab?.Invoke();
		if (grabHand.hand.type == HandType.Right) onRightGrab?.Invoke();
		if (grabHand.hand.type == HandType.Any) onBothGrab?.Invoke();
	}

	public void UnGrab(GrabHandPart grabHand) {
		if (grabHand.hand.type == HandType.Left) onLeftUnGrab?.Invoke();
		if (grabHand.hand.type == HandType.Right) onRightUnGrab?.Invoke();
		if (grabHand.hand.type == HandType.Any) onBothUnGrab?.Invoke();
	}
}