using System.Collections.Generic;
using UnityEngine;

public class SketchRoom {
	public Vector2Int p, s;
	public List<WayConnection> someWays, trueWays;
	public List<RoomData> roomVatiants;

	public SketchRoom(List<RoomData> _roomVatiants, Vector2Int _p, Vector2Int _s, char[,] _theMap) {
		roomVatiants = _roomVatiants;
		p = _p;
		s = _s;
		someWays = new List<WayConnection>();
		trueWays = new List<WayConnection>();

		// TODO: exclude unapropriate variants by ways out
		BulkWays(_theMap);

		//// TODO: Remove hardcode
		//// Right
		//if (p.x + s.x < _theMap.GetLength(0))
		//	for (int i = 0; i < s.y; i += 2)
		//		if (_theMap[p.x + s.x, p.y + i] == MapGenerator.GetMapMark(RoomType.None))
		//			someWays.Add(new WayConnection(ConnectionType.Way, new Vector2Int(s.x - 1, i), new Vector2Int(s.x, i)));
		//		else if (_theMap[p.x + s.x, p.y + i] != MapGenerator.GetMapMark(RoomType.Start))
		//			trueWays.Add(new WayConnection(ConnectionType.Way, new Vector2Int(s.x - 1, i), new Vector2Int(s.x, i)));
		//// Top
		//if (p.y + s.y < _theMap.GetLength(1))
		//	for (int i = s.x - 1; i >= 0; i -= 2)
		//		if (_theMap[p.x + i, p.y + s.y] == MapGenerator.GetMapMark(RoomType.None))
		//			someWays.Add(new WayConnection(ConnectionType.Way, new Vector2Int(i, s.y - 1), new Vector2Int(i, s.y)));
		//		else if (_theMap[p.x + i, p.y + s.y] != MapGenerator.GetMapMark(RoomType.Start))
		//			trueWays.Add(new WayConnection(ConnectionType.Way, new Vector2Int(i, s.y - 1), new Vector2Int(i, s.y)));
		//// Left
		//if (p.x > 0)
		//	for (int i = s.y - 1; i >= 0; i -= 2)
		//		if (_theMap[p.x - 1, p.y + i] == MapGenerator.GetMapMark(RoomType.None))
		//			someWays.Add(new WayConnection(ConnectionType.Way, new Vector2Int(0, i), new Vector2Int(-1, i)));
		//		else if (_theMap[p.x - 1, p.y + i] != MapGenerator.GetMapMark(RoomType.Start))
		//			trueWays.Add(new WayConnection(ConnectionType.Way, new Vector2Int(0, i), new Vector2Int(-1, i)));
		//// Bot
		//if (p.y > 0)
		//	for (int i = 0; i < s.x; i += 2)
		//		if (_theMap[p.x + i, p.y - 1] == MapGenerator.GetMapMark(RoomType.None))
		//			someWays.Add(new WayConnection(ConnectionType.Way, new Vector2Int(i, 0), new Vector2Int(i, -1)));
		//		else
		//			trueWays.Add(new WayConnection(ConnectionType.Way, new Vector2Int(i, 0), new Vector2Int(i, -1)));
	}

	private void BulkWays(char[,] _theMap) {
		Vector2Int wayPoint;

		for (int i = roomVatiants.Count - 1; i >= 0; --i)
			for (int j = roomVatiants[i].ways.Count - 1; j >= 0; --j) {
				if (someWays.Contains(roomVatiants[i].ways[j])) continue;
				wayPoint = roomVatiants[i].ways[j].offsetTo + p;
				if (_theMap[wayPoint.x, wayPoint.y] == MapGenerator.GetMapMark(RoomType.None))
					someWays.Add(roomVatiants[i].ways[j]);
				else if (_theMap[wayPoint.x, wayPoint.y] != MapGenerator.GetMapMark(RoomType.Start))
					trueWays.Add(roomVatiants[i].ways[j]);
			}
	}

	public int GetClosestPossibleExit(Vector2Int _aim) {
		float closestSqrDist = float.PositiveInfinity, tempSqrDist;
		int closestWayId = -1;
		for (int i = someWays.Count - 1; i >= 0; --i) {
			tempSqrDist = (p + someWays[i].offsetTo - _aim).sqrMagnitude;
			if (tempSqrDist < closestSqrDist) {
				closestSqrDist = tempSqrDist;
				closestWayId = i;
			}
		}
		return closestWayId;
	}

	public void ApproveWay(int _wayId) {
		trueWays.Add(someWays[_wayId]);
		someWays.RemoveAt(_wayId);
	}

	public void ApproveWay(Vector2Int _wayPoint) {
		int i;
		for (i = someWays.Count - 1; i >= 0; --i)
			if (p + someWays[i].offsetTo == _wayPoint)
				break;
		if (i < 0) return;

		trueWays.Add(someWays[i]);
		someWays.RemoveAt(i);

		// TODO: exclude anapropriate rooms
	}
}
