using System;
using UnityEngine;

[Serializable]
public struct GeneratedRoom {
	public RoomData room;
	public Vector2Int p;
	public RotateMeasure r;
	public int depth;
	public RoomController onScene;
	public bool primed;

	public GeneratedRoom(RoomData _room, Vector2Int _p, RotateMeasure _r, int _depth = int.MaxValue, RoomController _onScene = null) {
		room = _room;
		p = _p;
		r = _r;
		depth = _depth;
		onScene = _onScene;
		// Если не арена, то считаем, что прайм уже закрыли
		primed = !(_room.roomType == RoomType.Arena || _room.roomType == RoomType.Battle || _room.roomType == RoomType.Finish);
	}

	public bool IsTouchingRect(Vector2Int _p1, Vector2Int _p2) {
		if (p.x + room.size.x == _p1.x && p.y + room.size.y > _p1.y && p.y <= _p2.y) return true;
		if (p.x - 1 == _p2.x && p.y + room.size.y > _p1.y && p.y <= _p2.y) return true;
		if (p.y + room.size.y == _p1.y && p.x + room.size.x > _p1.x && p.x <= _p2.x) return true;
		if (p.y - 1 == _p2.y && p.x + room.size.x > _p1.x && p.x <= _p2.x) return true;
		return false;
	}

	public bool IsPointWithin(Vector2Int _p) => _p.x >= p.x && _p.y >= p.y && _p.x < p.x + room.size.x && _p.y < p.y + room.size.y;
}
