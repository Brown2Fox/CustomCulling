using UnityEngine;


public class Portal : AreaEventTrigger {
	[SerializeField]
	private Transform endPointT;

	[SerializeField]
	private PosRot endPointPR;

	public bool resetPlayer;

	public override void Trigger() {
		if (resetPlayer) Player.instance.TotalReset();
		base.Trigger();
		Owner owner = lastTriggerredObject.GetComponentInParent<Owner>();
		if (!owner) return;
		if (endPointT) {
			owner.Warp(endPointT.Global());
		} else {
			owner.Warp(endPointPR);
		}
	}
}
