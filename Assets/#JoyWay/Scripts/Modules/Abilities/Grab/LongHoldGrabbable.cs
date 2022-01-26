public class LongHoldGrabbable : SimpleGrabbable {
	protected bool hasGrabbed;

	// public override bool TryDrop(GrabHandPart _grabber) {
	// 	if (hasGrabbed && GameMain.settings.serializable.input.longHolds) {
	// 		hasGrabbed = false;
	// 		return false;
	// 	}
	//
	// 	return base.TryDrop(_grabber);
	// }

	public override void Grab(GrabHandPart _grabber) {
		hasGrabbed = true;
		base.Grab(_grabber);
	}
}