using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorDirect : MapGenerator {
	[Header("Components")]
	[Header("Direct map generator")]
	public RoomForGeneration start;
	public RoomForGeneration finish;
	private RoomForGeneration roomForGeneration;

	public int
		interestPoints = 10,
		windowWidth = 4;

	public RoomsPoolAsset battleRoom, arenaRoom;

	public int gainerTargetWay = 1;
	public List<RoomsPoolAsset> gainerAssets = new List<RoomsPoolAsset>();
	[Serializable]
	public struct Box {
		public Vector2Int min, max;
	}
	public List<Box> gainerBoxes = new List<Box>();

	public Vector2Int pointsSpawnZoneSize;
	public List<RoomsPoolAsset> fillersAssets;

	private List<GeneratedRoom> roomsPrepared = new List<GeneratedRoom>();

	private GeneratedRoom finishGen;
	private SketchRoom tempRoom;

	public GameObject d_OpenPoint;

	private bool arened, finished;

	private int curIteratePoint = 0;

	public override void Generate() {
		CheckGainerTargetWayNumber();
		base.Generate();
		StartGeneration();
		for (int i = interestPoints - 1; i >= 0; --i) {
			AddPoint();
			SaveAsset(false, (char)(curIteratePoint + 48));
		}

		FinishGeneration();
		SaveAsset(false, 'f');
		AddGainer();
		SaveAsset(false, 'g');
		CoverFillers();
		CoverSpecialRooms();
	}

	private void CheckGainerTargetWayNumber() {
		if (gainerTargetWay > interestPoints)
			gainerTargetWay = interestPoints;
	}

	public override void ResetAll() {
		base.ResetAll();
		roomsPrepared.Clear();
		finished = false;
	}

	public override void StartGeneration() {
		base.StartGeneration();

		curIteratePoint = 0;
		arened = false;

		PrepareFillers();

		RoomPlaceRandom(start);
	}

	private void PrepareFillers() {
		for (int i = fillersAssets.Count - 1; i >= 0; --i) {
			for (int j = fillersAssets[i].variants.Count - 1; j >= 0; --j) {
				GeneratedRoom generatedRoom = new GeneratedRoom(new RoomData(fillersAssets[i].variants[j]), Vector2Int.zero, RotateMeasure._0);
				for (int k = 0; k < 4; ++k) {
					generatedRoom.room = new RoomData(generatedRoom.room);
					generatedRoom.room.modelRotation = generatedRoom.r = (RotateMeasure)k;
					generatedRoom.room.RotateWays();
					generatedRoom.room.size = new Vector2Int(generatedRoom.room.size.y, generatedRoom.room.size.x);
					generatedRoom.r = (RotateMeasure)((k + 1) % 4);
					roomsPrepared.Add(generatedRoom);
				}
			}
		}
	}

	public override void AddPoint() {
		base.AddPoint();

		if ((curIteratePoint + 1f) / interestPoints > .6 && !arened) {
			arened = true;

			//Debug.Log($"Arened at {(curIteratePoint + 1f) / interestPoints}");

			roomForGeneration = new RoomForGeneration {
				rooms = arenaRoom,
				min = new Vector2Int(mapSize.x - 1, 3)
			};
		} else {
			roomForGeneration = new RoomForGeneration {
				rooms = battleRoom,
				min = new Vector2Int(mapSize.x - 1, 3)
			};
		}

		// Takes 1-st room's size
		float t = 1f * curIteratePoint * (pointsSpawnZoneSize.y - windowWidth - roomForGeneration.rooms.variants[0].size.y + 1) / (interestPoints - 1);
		roomForGeneration.min = new Vector2Int((mapSize.x - pointsSpawnZoneSize.x) / 2, (int)t + (mapSize.y - pointsSpawnZoneSize.y) / 2);
		roomForGeneration.max = new Vector2Int(mapSize.x - (mapSize.x - roomForGeneration.min.x - pointsSpawnZoneSize.x) - roomForGeneration.rooms.variants[0].size.x, (int)t + windowWidth - 1 + (mapSize.y - pointsSpawnZoneSize.y) / 2);

		if (roomForGeneration.rooms.roomType == RoomType.Arena)
			// TODO: Fix generation from multiple variants inside
			tempRoom = RoomSketchRandom(roomForGeneration, UnityEngine.Random.Range(0, roomForGeneration.rooms.variants.Count));
		else
			tempRoom = RoomSketchRandom(roomForGeneration, curIteratePoint % roomForGeneration.rooms.variants.Count);

		if (tempRoom != null)
		{
			if (tempRoom.trueWays.Count <= 0) {
				Tuple<Vector2Int, Vector2Int> bestpair = GetShortestRoomEscape(tempRoom);
				if (bestpair.Item1 == -Vector2Int.one) {
					Debug.LogError($"No way from {tempRoom.p}");
					return;
				}
				CountDaWay(bestpair.Item1, bestpair.Item2, curIteratePoint);
				tempRoom.ApproveWay(bestpair.Item1);
				RemoveOpenPoint(bestpair.Item2);
			} else
				for (int i = tempRoom.trueWays.Count - 1; i >= 0; --i)
					RemoveOpenPoint(tempRoom.trueWays[i].offsetTo + tempRoom.p);	
		}

		++curIteratePoint;
	}

	private struct SketchGainer {
		public SketchRoom room;
		public int minDistance;
		public Tuple<Vector2Int, Vector2Int> bestPair;
	}
	protected void AddGainer() {
		//test
		if (gainerAssets.Count <= 0)
			return;

		RoomsPoolAsset gainer = gainerAssets[UnityEngine.Random.Range(0, gainerAssets.Count)];

		for (int i = 0; i < gainerBoxes.Count; i++) {
			gainerBoxes[i] = new Box {
				min = new Vector2Int(gainerBoxes[i].min.x + 1, gainerBoxes[i].min.y + 1),
				max = new Vector2Int(gainerBoxes[i].max.x - gainer.size.x, gainerBoxes[i].max.y - gainer.size.y),
			};
		}
		SketchGainer sketchGainer = new SketchGainer();
		sketchGainer.minDistance = int.MaxValue;
        for (int i = 0; i < gainerBoxes.Count; i++) {
			int tries = 20;
			while (tries > 0) {
				RoomForGeneration rfg = new RoomForGeneration();
				rfg.rooms = gainer;
				rfg.min = gainerBoxes[i].min;
				rfg.max = gainerBoxes[i].max;
				rfg.rotation = RotateMeasure.Random;
				tempRoom = RoomSketchRandom(rfg, -1);
				if (tempRoom != null && tempRoom.trueWays.Count <= 0) {
					int minDistance = int.MaxValue;
					Tuple<Vector2Int, Vector2Int> bestpair = GetShortestGainerEscape(tempRoom, gainerTargetWay, out minDistance);
					if (bestpair.Item1 != -Vector2Int.one && minDistance < sketchGainer.minDistance) {
						if (sketchGainer.minDistance != int.MaxValue) {
							RemoveGainer(sketchGainer.room);
						}
						sketchGainer.room = tempRoom;
						sketchGainer.minDistance = minDistance;
						sketchGainer.bestPair = bestpair;
						break;
					}
				}
				if (tempRoom != null) {
					RemoveGainer(tempRoom);
				}
				tries--;
			}
		}
		if (sketchGainer.minDistance == int.MaxValue)
			return;
		CountDaWay(sketchGainer.bestPair.Item1, sketchGainer.bestPair.Item2, -1, true, true);
		sketchGainer.room.ApproveWay(sketchGainer.bestPair.Item1);
		RemoveOpenPoint(sketchGainer.bestPair.Item2);
    }

	private void RemoveGainer(SketchRoom sketchRoom) {
		for (int j = sketchRoom.someWays.Count - 1; j >= 0; --j)
			openPoints.Remove(sketchRoom.someWays[j] + sketchRoom.p);
		UnMakeSketchRoom(sketchRoom);
	}

	public override void FinishGeneration() {
		finishGen = RoomPlaceRandom(finish);
		// TODO: Process multiway room finish.
		if (theMap[(finishGen.p + finishGen.room.ways[0].offsetTo).x, (finishGen.p + finishGen.room.ways[0].offsetTo).y] == GetMapMark(RoomType.None)) {
			List<WayConnection> finishWays = new List<WayConnection>(finishGen.room.ways);
			for (int i = finishWays.Count - 1; i >= 0; --i)
				finishWays[i] += finishGen.p;
			List<int> openPointsToConnect = GetClosestOpenPoints(finishGen.p + Vector2Int.down, finishWays);
			if (openPointsToConnect.Count == 0)
				Debug.LogError($"No points to connect to {tempRoom.p}");
			else
				CountDaWay(finishGen.p + finishGen.room.ways[0].offsetTo, openPoints[openPointsToConnect[0]].offsetTo);
		}
		finished = true;

		base.FinishGeneration();

		// Инициализируем миникарту
		if (Minimap.instance) {
			Minimap.instance.mapGenerator = this;
			Minimap.instance.Invoke("Proccess", 0.1f);		 // Делаем с небольшой задержской так как нужно чтобы Эвейки всех сцен сработали
		}
	}

	public override void Place() {
		if (finished)
			CoverOpenPoints();
		//GenerateFakeRooms();
		base.Place();

		ShowOpenPoints();
	}

	private void CoverOpenPoints() {
		for (int i = openPoints.Count - 1; i >= 0; i = openPoints.Count - 1) {
			CoverPoint(openPoints[i].offsetTo);
			if (i == openPoints.Count - 1) {
				Debug.LogError("Can not cover any open point");
				return;
			}
		}
	}

	// TODO: Choose right room
	private void CoverSpecialRooms() {
		for (int i = sketched.Count - 1; i >= 0; --i)
			GenerateRoom(sketched[i].roomVatiants[0], sketched[i].p);
		sketched.Clear();
	}

	private void ShowOpenPoints() {
		for (int i = openPoints.Count - 1; i >= 0; --i) {
			GameObject go = Instantiate(d_OpenPoint, mapRoot);
			go.transform.localPosition = GetLocalPos(openPoints[i].offsetTo);
		}
	}

	private void CoverFillers() {
		for (int i = mapSize.x - 1; i >= 0; --i)
			for (int j = mapSize.y - 1; j >= 0; --j) {
				if (theMap[i, j] != GetMapMark(RoomType.T_Path)) continue;
				CoverPoint(new Vector2Int(i, j));
			}
	}

	private const float scoreSimilarity = 0.001f;
	private void CoverPoint(Vector2Int _p) {
		float lastScore, bestScore;
		List<int> bestRooms = new List<int>();
		List<Vector2Int> bestPoints = new List<Vector2Int>();

		bestScore = float.NegativeInfinity;

		for (int k = roomsPrepared.Count - 1; k >= 0; --k)
			for (int i = _p.x - roomsPrepared[k].room.size.x + 1; i <= _p.x; ++i)
				for (int j = _p.y - roomsPrepared[k].room.size.y + 1; j <= _p.y; ++j) {
					lastScore = CheckCoverPlacement(roomsPrepared[k].room, new Vector2Int(i, j));
					if (Mathf.Abs(bestScore - lastScore) < scoreSimilarity) {
						bestRooms.Add(k);
						bestPoints.Add(new Vector2Int(i, j));
					} else if (bestScore < lastScore) {
						bestScore = lastScore;
						bestRooms.Clear();
						bestRooms.Add(k);
						bestPoints.Clear();
						bestPoints.Add(new Vector2Int(i, j));
					}
				}

		if (bestRooms.Count <= 0)
			Debug.LogError($"No good room for {_p}!");
		else {
			int bestRoomId = UnityEngine.Random.Range(0, bestRooms.Count);
			//Debug.Log($"Best room score = {bestScore}, rooms count = {bestRooms.Count}");
			GenerateRoom(roomsPrepared[bestRooms[bestRoomId]].room, bestPoints[bestRoomId], roomsPrepared[bestRooms[bestRoomId]].room.modelRotation);
		}
	}

	private const float scoreUp = 1f, scoreDown = .3f, scoreAjust = .01f;
	private float CheckCoverPlacement(RoomData _room, Vector2Int _p, bool canHaveOpenPoints = true) {
		// Check room in map
		if (_p.x < 0 || _p.y < 0 || _p.x + _room.size.x > mapSize.x || _p.y + _room.size.y > mapSize.y) return float.NegativeInfinity;

		// Count score
		float placeScore = 0f;
		for (int i = _room.size.x - 1; i >= 0; --i)
			for (int j = _room.size.y - 1; j >= 0; --j)
				if (theMap[_p.x + i, _p.y + j] == GetMapMark(RoomType.None))
					placeScore -= scoreDown;
				else if (theMap[_p.x + i, _p.y + j] == GetMapMark(RoomType.T_Path))
					placeScore += scoreUp;
				else
					return float.NegativeInfinity;

		// Get way's id's
		List<int> waysToConnect = new List<int>();
		for (int i = _room.ways.Count - 1; i >= 0; --i)
			waysToConnect.Add(i);

		List<GeneratedRoom> neighbours = CollectGeneratedNeighbours(_p, _p + _room.size - Vector2Int.one);

		// Check neighbour's ways
		for (int i = neighbours.Count - 1; i >= 0; --i) {
			for (int j = neighbours[i].room.ways.Count - 1; j >= 0; --j) {
				int k = _room.ways.Count - 1;
				for (; k >= 0; --k)
					if (WayConnection.AreConnected(_room.ways[k], _p, neighbours[i].room.ways[j], neighbours[i].p)) {
						waysToConnect.Remove(k);
						break;
					}
				if (k < 0) {
					Vector2Int wayFromNeighbour = neighbours[i].room.ways[j].offsetTo + neighbours[i].p;
					if (wayFromNeighbour.x < _p.x + _room.size.x && wayFromNeighbour.x >= _p.x && wayFromNeighbour.y < _p.y + _room.size.y && wayFromNeighbour.y >= _p.y)
						return float.NegativeInfinity;
				}
			}
		}

		// Check marked ways
		List<Vector2Int> marks = CollectMarksAround(_p, _p + _room.size - Vector2Int.one);
		for (int i = marks.Count - 1; i >= 0; --i) {
			int j = _room.ways.Count - 1;
			for (; j >= 0 && _p + _room.ways[j].offsetTo != marks[i]; --j) ;
			if (j < 0)
				return float.NegativeInfinity;
			else
				waysToConnect.Remove(j);
		}
		if (waysToConnect.Count > 0 && !canHaveOpenPoints)
			return float.NegativeInfinity;

		// Check room's ways
		for (int i = waysToConnect.Count - 1; i >= 0; --i) {
			Vector2Int wayToPoint = _p + _room.ways[waysToConnect[i]].offsetTo;
			if (wayToPoint.x < 0 || wayToPoint.y < 0 || wayToPoint.x >= mapSize.x || wayToPoint.y >= mapSize.y)
				return float.NegativeInfinity;
			if (theMap[wayToPoint.x, wayToPoint.y] != GetMapMark(RoomType.None))
				return float.NegativeInfinity;
		}
		placeScore -= waysToConnect.Count * scoreAjust;

		return placeScore;
	}

	private List<Vector2Int> CollectMarksAround(Vector2Int _p1, Vector2Int _p2) {
		List<Vector2Int> marks = new List<Vector2Int>();

		if (_p1.x > 0)
			for (int i = _p1.y; i <= _p2.y; ++i)
				if (CheckMapMark(theMap[_p1.x - 1, i], RoomType.T_Path, RoomType.Battle, RoomType.Arena, RoomType.Gainer))
					marks.Add(new Vector2Int(_p1.x - 1, i));
		if (_p1.y > 0)
			for (int i = _p1.x; i <= _p2.x; ++i)
				if (CheckMapMark(theMap[i, _p1.y - 1], RoomType.T_Path, RoomType.Battle, RoomType.Arena, RoomType.Gainer))
					marks.Add(new Vector2Int(i, _p1.y - 1));
		if (_p2.x < mapSize.x - 1)
			for (int i = _p1.y; i <= _p2.y; ++i)
				if (CheckMapMark(theMap[_p2.x + 1, i], RoomType.T_Path, RoomType.Battle, RoomType.Arena, RoomType.Gainer))
					marks.Add(new Vector2Int(_p2.x + 1, i));
		if (_p2.y < mapSize.y - 1)
			for (int i = _p1.x; i <= _p2.x; ++i)
				if (CheckMapMark(theMap[i, _p2.y + 1], RoomType.T_Path, RoomType.Battle, RoomType.Arena, RoomType.Gainer))
					marks.Add(new Vector2Int(i, _p2.y + 1));

		return marks;
	}

	private bool CheckMapMark(char mapChar, params RoomType[] roomTypes) {
        for (int i = 0; i < roomTypes.Length; i++) 
			if (mapChar == GetMapMark(roomTypes[i]))
				return true;
		return false;
    }
}
