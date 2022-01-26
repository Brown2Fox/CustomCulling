using UnityEngine;

[System.Serializable]
public struct RoomForGeneration {
	public RoomsPoolAsset rooms;
	public Vector2Int min, max;
	public RotateMeasure rotation;
}
