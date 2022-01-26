using UnityEngine;

public class HandleGrabbable : LongHoldGrabbable {
	public Transform handle;

	private void OnEnable() {
		transform.CopyGlobalPosRot(handle);
	}

	public override void Drop() {
		if (rb) {
			rb.isKinematic = true;
		}

		var _grabber = curGrabber;
		curGrabber = null;
		if (grabType == GrabType.JointHandle) {
			rb.isKinematic = true;
			_grabber.hand.GetComponent<Rigidbody>().isKinematic = false;
			Destroy(_grabber.hand.GetComponentInParent<LateCopyPosRot>());
		}

		onDropped.Invoke(_grabber);
		onDrop?.Invoke(this);
		transform.CopyGlobalPosRot(handle);
	}
}