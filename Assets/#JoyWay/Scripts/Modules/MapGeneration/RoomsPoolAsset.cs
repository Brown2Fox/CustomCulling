using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomsPoolAsset", menuName = "JoyWay/MapGeneration/Room pool asset")]
public class RoomsPoolAsset : ScriptableObject {
	public RoomType roomType;
	public Vector2Int size;
	public List<RoomData> variants;
}
