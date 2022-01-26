using UnityEngine;

public class SnapZonePocket : SnapZone {
	public AutoPockets autoPockets;

	public override void OnSnap(Snapper snapper) {
		base.OnSnap(snapper);
		autoPockets.SetBind(this, snapper);
		ResizeCol(snapper);
	}

	public override void OnUnSnap(Snapper snapper) {
		base.OnUnSnap(snapper);
		RevertCol(snapper);
	}


	public override void ResetSnapZone() {
		base.ResetSnapZone();
		autoPockets.ResetBind();
	}

	private BoxCollider col;
	private PosRot colPosRot;
	private Vector3 colSize;
	private Vector3 colCenter;

	public void ResizeCol(Snapper snapper) {
		col = snapper.GetComponentInChildren<BoxCollider>();
		BoxCollider colSnapper = GetComponent<BoxCollider>();
		if (!colSnapper) return;
		colPosRot = new PosRot();

		colPosRot.pos = col.transform.localPosition;
		colPosRot.rot = col.transform.localRotation;
		colSize = col.size;
		colCenter = col.center;

		col.transform.position = colSnapper.transform.position;
		col.transform.rotation = colSnapper.transform.rotation;
		col.size = colSnapper.size;
		col.center = Vector3.zero;
	}

	public void RevertCol(Snapper snapper) {
		col.transform.localPosition = colPosRot.pos;
		col.transform.localRotation = colPosRot.rot;
		col.size = colSize;
		col.center = colCenter;
	}
}