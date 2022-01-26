using System;
using UnityEngine;

public class HandOffset : MonoBehaviour {
	public PosRot vive;
	public PosRot oculus;
	[HideInInspector]
	public PosRot currentOffset;

	private void Start() {
		if (true) {
			// if (Platforms.isOculus) {
			SetOffsets(oculus);
			currentOffset = oculus;
		} else {
			SetOffsets(vive);
			currentOffset = vive;
		}
	}

	public void SetOffsets(PosRot offset) {
		transform.CopyLocalPosRot(offset);
	}
}