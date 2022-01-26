using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEditor;

public class MapGenerator : MonoBehaviour {
	public static MapGenerator instance;

	public bool autoGenerateOnLoad;
	[HideInInspector]
	public int seed = 0;
	[HideInInspector]
	public bool randomSeed;
	[HideInInspector]
	public bool autoNavmesh = true;

	public Transform mapRoot;
	public GameObject background;
	public NavMeshSurface navMeshSurface;

	public const float cellSize = 16f;
	public Vector2Int mapSize = new Vector2Int(16, 16);

	// TODO: Add revert ref to room owner
	public char[,] theMap;
	public List<GeneratedRoom> generated = new List<GeneratedRoom>();
	public List<SketchRoom> sketched = new List<SketchRoom>();
	public List<WayConnection> openPoints = new List<WayConnection>();
	private List<Vector2Int> pointsToCover = new List<Vector2Int>();

	protected Vector2Int tempP;
	protected GameObject tempObject;

	[HideInInspector]
	public MapAsset mapAsset;


	protected void Awake() {
		instance = this;
		if (autoGenerateOnLoad)
			FullGeneration();

		SaveAsset(false, '.');
	}

	private int toPlaceLeft;
	public void FullGeneration() {
		ResetAll();
		Generate();
		Place();
		toPlaceLeft = generated.Count;
	}

	public int statisticsRange = 1;
	private bool dontGenerate = false;
	public void MakeStatistics() {
		Dictionary<string, int> stats = new Dictionary<string, int>();

		string tempName;
		dontGenerate = true;

		for (int i = statisticsRange; i > 0; --i) {
			ResetAll();
			Generate();
			Place();

			for (int j = generated.Count - 1; j >= 0; --j) {
				tempName = generated[j].room.roomSceneName;
				if (stats.ContainsKey(tempName))
					++stats[tempName];
				else
					stats.Add(tempName, 1);
			}
		}

		dontGenerate = false;

		foreach (string chName in stats.Keys)
			Debug.Log($"{chName} : {stats[chName]}");
	}

	public virtual void Generate() { }

	public virtual void StartGeneration() {
		if (randomSeed)
			seed = Random.Range(int.MinValue, int.MaxValue);
		Random.InitState(seed);
		Debug.Log("Generated seed number = " + seed);
	}

	public virtual void AddPoint() { }
	public virtual void FinishGeneration() { }

	public void RoomIsReady(RoomController _controller) {
		string sceneName = _controller.gameObject.scene.path;
		int i = generated.Count - 1;
		for (; i >= 0 && (generated[i].room.roomSceneName != sceneName || generated[i].onScene != null); --i) ;
		if (i < 0) {
			Debug.LogError("OMG! Scene has come and nothing is waiting...");
			return;
		}

		GeneratedRoom gr = generated[i];
		gr.onScene = _controller;
		generated[i] = gr;

		_controller.roomData = generated[i].room;
		_controller.transform.position = GetLocalPos(generated[i].p + (generated[i].room.size - Vector2.one) * .5f);
		_controller.transform.rotation = Quaternion.AngleAxis(-90f * (int)generated[i].r, transform.up);

		if (--toPlaceLeft == 0) {
			FillTheMap();
			NavMeshIt();
		}

		EasyGameManager.instance.BeginDungeon();

		//Debug.Log(_controller.gameObject.scene.path);
	}

	public void NavMeshIt() {
		navMeshSurface.BuildNavMesh();
	}

	public void FillTheMap() {
		CountRoomsDeepth();
	}

	private int maxDeepth = 100;

	private void CountRoomsDeepth() {
		GeneratedRoom tempGR1 = generated[0], tempGR2;
		int tempGRId;
		tempGR1.depth = 0;
		generated[0] = tempGR1;

		List<GeneratedRoom> wayQueue = new List<GeneratedRoom> {
			generated[0]
		};

		for (; wayQueue.Count > 0;) {
			tempGR1 = wayQueue[0];
			wayQueue.RemoveAt(0);
			for (int i = tempGR1.room.ways.Count - 1; i >= 0; --i) {
				tempGRId = GetGeneratedRoomIndexAtPoint(tempGR1.p + tempGR1.room.ways[i].offsetTo);
				if (tempGRId < 0) continue;
				tempGR2 = generated[tempGRId];

				int j;
				for (j = tempGR2.room.ways.Count - 1; j >= 0; --j)
					if (WayConnection.AreConnected(tempGR1.room.ways[i], tempGR1.p, tempGR2.room.ways[j], tempGR2.p))
						break;
				// Если связь не взаимная, то нужно партнёра лучше отпустить
				if (j < 0) continue;

				//if (tempGR2.deepth <= tempGR1.deepth + tempGR1.room.size.x * tempGR1.room.size.y) continue;
				//tempGR2.deepth = tempGR1.deepth + tempGR1.room.size.x * tempGR1.room.size.y;

				if (tempGR2.depth <= tempGR1.depth + (tempGR2.p - tempGR1.p).sqrMagnitude) continue;
				tempGR2.depth = tempGR1.depth + (tempGR2.p - tempGR1.p).sqrMagnitude;

				wayQueue.Add(tempGR2);
				generated[tempGRId] = tempGR2;
			}
		}

		maxDeepth = 0;
		// Let the rooms know their deepth
		for (int i = generated.Count - 1; i >= 0; --i) {
			if (maxDeepth < generated[i].depth)
				maxDeepth = generated[i].depth;
			generated[i].onScene.depth = generated[i].depth;
		}
	}

	private void PrepareTheMap() {
		theMap = new char[mapSize.x, mapSize.y];
		for (int i = mapSize.x - 1; i >= 0; --i)
		for (int j = mapSize.y - 1; j >= 0; --j)
			theMap[i, j] = GetMapMark(RoomType.None);
	}
	protected void SaveAsset(bool fake = true, char symbol = '-') {
        //if (fake)
        //    return;

        //mapAsset.mapTable = new char[mapSize.x, mapSize.y];
        //for (int i = mapSize.x - 1; i >= 0; --i) {
        //    for (int j = mapSize.y - 1; j >= 0; --j)
        //        mapAsset.mapTable[i, j] = theMap[i, j];
        //}

        //mapAsset.mapTable[theMap.GetLength(0) - 1, theMap.GetLength(1) - 1] = symbol;

        //mapAsset.serrialized = new string[mapSize.y];
        //for (int i = mapSize.y - 1; i >= 0; --i)
        //    for (int j = mapSize.x - 1; j >= 0; --j)
        //        mapAsset.serrialized[i] += mapAsset.mapTable[mapSize.x - 1 - j, mapSize.y - 1 - i];

        //EditorUtility.SetDirty(mapAsset);
        //AssetDatabase.SaveAssets();
    }

	public virtual void ResetAll() {
		generated.Clear();
		sketched.Clear();
		openPoints.Clear();
		ClearScene();
		PrepareTheMap();
		Pool.Clear();
	}

	public void ClearScene() {
		if (mapRoot != null)
			DestroyImmediate(mapRoot.gameObject);
		mapRoot = new GameObject("MapRoot").transform;
		SceneManager.MoveGameObjectToScene(mapRoot.gameObject, gameObject.scene);
	}

    public GeneratedRoom RoomPlaceRandom(in RoomForGeneration _generating) {
        tempP = new Vector2Int(Random.Range(_generating.min.x, _generating.max.x + 1), Random.Range(_generating.min.y, _generating.max.y + 1));
        int tries = 1000;
        // TODO: Norm check
        for (; tries >= 0 && theMap[tempP.x, tempP.y] != GetMapMark(RoomType.None); --tries)
            tempP = new Vector2Int(Random.Range(_generating.min.x, _generating.max.x + 1), Random.Range(_generating.min.y, _generating.max.y + 1));
        if (tries < 0)
            Debug.LogError("There is no random place for room!");

        return GenerateRoom(
                            _generating.rooms.variants[0], tempP,
                            _generating.rotation == RotateMeasure.Random ? (RotateMeasure)Random.Range(0, 3) : _generating.rotation);
    }
    //  public GeneratedRoom RoomPlaceRandom(in RoomForGeneration _generating) {
    //      Vector2Int roomSize = _generating.rooms.size;
    //      RotateMeasure roomRotation = _generating.rotation == RotateMeasure.Random ? (RotateMeasure)Random.Range(0, 3) : _generating.rotation;

    //      if (roomRotation == RotateMeasure._90 || roomRotation == RotateMeasure._270) {
    //          int x = roomSize.x;
    //          int y = roomSize.y;
    //          roomSize = new Vector2Int(y, x);
    //      }
    //int tries = 1000;
    //for (; tries >= 0; --tries) {
    //	tempP = new Vector2Int(Random.Range(_generating.min.x, _generating.max.x + 1 - roomSize.x), 
    //		Random.Range(_generating.min.y, _generating.max.y + 1 - roomSize.y));
    //	if (CheckSketchPlacement(tempP, roomSize)) break;
    //}

    //if (tries < 0)
    //          Debug.LogError("There is no random place for room!");

    //      return GenerateRoom(_generating.rooms.variants[0], tempP, roomRotation);
    //  }

    public GeneratedRoom GenerateRoom(RoomData _room, Vector2Int _p, RotateMeasure _r = RotateMeasure._0) {
		AddOpenPoint(_room, _p);
		GeneratedRoom tempRoom = new GeneratedRoom(_room, _p, _r);
		RemoveOpenPoints(tempRoom);
		generated.Add(tempRoom);
		for (int i = tempRoom.p.x + tempRoom.room.size.x - 1; i >= tempRoom.p.x; --i)
			for (int j = tempRoom.p.y + tempRoom.room.size.y - 1; j >= tempRoom.p.y; --j)
				theMap[i, j] = GetMapMark(_room.roomType);
		return tempRoom;
	}

	public SketchRoom RoomSketchRandom(in RoomForGeneration _generating, int _roomId = -1) {
		SketchRoom newSketchedRoom = null;
		for (int tries = 1000; tries >= 0; --tries) {
			tempP = new Vector2Int(Random.Range(_generating.min.x, _generating.max.x + 1), Random.Range(_generating.min.y, _generating.max.y + 1));
			if (!CheckSketchPlacement(tempP, _generating.rooms.variants[0].size)) continue;

			if (_roomId == -1)
				newSketchedRoom = MakeSketchRoom(new List<RoomData>(_generating.rooms.variants), tempP);
			else
				newSketchedRoom = MakeSketchRoom(new List<RoomData>() {_generating.rooms.variants[_roomId]}, tempP);
			if (newSketchedRoom.roomVatiants.Count <= 0 || !CheckSketchWays(newSketchedRoom)) {
				UnMakeSketchRoom(newSketchedRoom);
				continue;
			}

			for (int i = newSketchedRoom.someWays.Count - 1; i >= 0; --i)
				openPoints.Add(newSketchedRoom.someWays[i] + newSketchedRoom.p);
			break;
		}

		return newSketchedRoom;
	}

	public SketchRoom MakeSketchRoom(List<RoomData> _rooms, Vector2Int _p) {
		SketchRoom tempRoom = new SketchRoom(_rooms, _p, _rooms[0].size, theMap);
		sketched.Add(tempRoom);
		for (int i = tempRoom.p.x; i < tempRoom.p.x + tempRoom.s.x; ++i)
		for (int j = tempRoom.p.y; j < tempRoom.p.y + tempRoom.s.y; ++j)
			theMap[i, j] = GetMapMark(_rooms[0].roomType);

		// Remove variants with unapropriate exits
		for (int i = _rooms.Count - 1; i >= 0; --i) {
			int j;
			for (j = _rooms[i].ways.Count - 1; j >= 0; --j) {
				Vector2Int wayTo = _p + _rooms[i].ways[j].offsetTo;
				if (theMap[wayTo.x, wayTo.y] == GetMapMark(RoomType.None)) continue;

				int neighbourId = GetGeneratedRoomIndexAtPoint(wayTo);
				if (neighbourId < 0) continue;
				GeneratedRoom neighbour = generated[neighbourId];

				int k;
				for (k = neighbour.room.ways.Count - 1; k >= 0; --k)
					if (WayConnection.AreConnected(_rooms[i].ways[j], _p, neighbour.room.ways[k], neighbour.p))
						break;

				// Нет соединения с путём. Вариант комнаты не подходит!
				if (k < 0) break;
			}

			// Путь упёрся в комнату без ответа. Вариант не подходит!
			if (j >= 0)
				_rooms.RemoveAt(i);
		}

		return tempRoom;
	}

	public void UnMakeSketchRoom(SketchRoom _room) {
		sketched.Remove(_room);
		for (int i = _room.p.x; i < _room.p.x + _room.s.x; ++i)
		for (int j = _room.p.y; j < _room.p.y + _room.s.y; ++j)
			theMap[i, j] = GetMapMark(RoomType.None);
	}

	// Checks the square is empty
	private bool CheckSketchPlacement(Vector2Int _p, Vector2Int _s) {
		if (_p.x < 0 || _p.y < 0 || _p.x + _s.x - 1 > mapSize.x - 1 || _p.y + _s.y - 1 > mapSize.y - 1) return false;
		for (int i = _s.x; i >= -1; --i)
		for (int j = _s.y; j >= -1; --j)
			try {
					int x = _p.x + i;
					int y = _p.y + j;
					if (x > theMap.GetLength(0) || x < 0)
						return false;
					if (y > theMap.GetLength(1) || y < 0)
						return false;
					if (theMap[x, y] != GetMapMark(RoomType.None))
						return false;
			}
			catch {
					Debug.LogError("Boom (Out of map check maybe)");
			}

		return true;
	}

	// Checks Sketch's and Sketch's neighbour's ways are ok
	private bool CheckSketchWays(SketchRoom _room) {
		List<GeneratedRoom> neighbours = CollectGeneratedNeighbours(_room.p, _room.p + _room.s - Vector2Int.one);
		Vector2Int wayPoint;
		// Нужно найти хотябы один вариант,..
		for (int i = _room.roomVatiants.Count - 1; i >= 0; --i) {
			// Чтобы все соседи...
			int j;
			for (j = neighbours.Count - 1; j >= 0; --j) {
				// Совпадали по всем подходящим выходам...
				int k;
				for (k = neighbours[j].room.ways.Count - 1; k >= 0; --k) {
					wayPoint = neighbours[j].p + neighbours[j].room.ways[k].offsetTo;
					if (wayPoint.x < _room.p.x || wayPoint.y < _room.p.y || wayPoint.x >= (_room.p + _room.s).x || wayPoint.y >= (_room.p + _room.s).y) continue;
					int l;
					// Хотя бы с одним нашим выходом.
					for (l = _room.roomVatiants[i].ways.Count - 1; l >= 0; --l)
						if (WayConnection.AreConnected(_room.roomVatiants[i].ways[l], _room.p, neighbours[j].room.ways[k], neighbours[j].p))
							break;
					// Ни один наш выход не подошёл? Вариант комнаты не подходит!
					if (l < 0) break;
				}
				// Встретился неподходящий выход соседа? Вариант комнаты не подходит!
				if (k >= 0) break;
			}
			// Все соседи прошли? Можно остановиться, всё ок.
			if (j < 0) return true;
		}
		// Ни один вариант не подошёл? Щито поделать.
		return false;
	}

	private void AddOpenPoint(RoomData _room, Vector2Int _p) {
		Vector2Int openingPoint;
		for (int i = _room.ways.Count - 1; i >= 0; --i) {
			openingPoint = _p + _room.ways[i].offsetTo;
			if (openingPoint.x < 0 || openingPoint.y < 0 || openingPoint.x >= mapSize.x || openingPoint.y >= mapSize.y) continue;

			if (theMap[openingPoint.x, openingPoint.y] == GetMapMark(RoomType.None))
				openPoints.Add(_room.ways[i] + _p);
		}
	}

	protected void RemoveOpenPoint(Vector2Int _p) {
		int targetOpenPoint = FindOpenPointAtPoint(_p);
		if (targetOpenPoint >= 0) {
			openPoints.RemoveAt(targetOpenPoint);
			RemoveOpenPoint(_p);
		}
	}

	protected void RemoveOpenPoints(GeneratedRoom _room) {
		for (int i = _room.p.x + _room.room.size.x - 1; i >= _room.p.x; --i)
		for (int j = _room.p.y + _room.room.size.y - 1; j >= _room.p.y; --j)
			RemoveOpenPoint(new Vector2Int(i, j));
	}

	public Vector3 GetLocalPos(Vector2 _p) {
		return new Vector3((_p.x - (mapSize.x - 1) * .5f) * cellSize, 0f, (_p.y - (mapSize.y - 1) * .5f) * cellSize);
	}

	public RoomController IsTouchingRoom(Vector3 pos) {
		for (int i = generated.Count - 1; i >= 0; --i) {
			Vector2 posChunck = new Vector2(generated[i].onScene.transform.position.x, generated[i].onScene.transform.position.z);
			float x1 = posChunck.x - generated[i].room.size.x * cellSize / 2;
			float x2 = posChunck.x + generated[i].room.size.x * cellSize / 2;
			float y1 = posChunck.y - generated[i].room.size.y * cellSize / 2;
			float y2 = posChunck.y + generated[i].room.size.y * cellSize / 2;
			if (generated[i].r == RotateMeasure._90 || generated[i].r == RotateMeasure._270) {
				x1 = x1 + x2;
				x2 = x1 - x2;
				x1 = x1 - x2;
				y1 = y1 + y2;
				y2 = y1 - y2;
				y1 = y1 - y2;
			}
			if (pos.x > x1 && pos.z > y1 && pos.x < x2 && pos.z < y2) {
				return generated[i].onScene;
			}
		}
		return null;
	}

	private void PrepareBackground() {
		tempObject = Instantiate(background, mapRoot);
		tempObject.transform.localPosition = Vector3.down * 32f;
		tempObject.transform.localScale = new Vector3((mapSize.x + 16) * cellSize, 1f, (mapSize.y + 16) * cellSize);
	}

	public static char GetMapMark(RoomType _roomType) {
		switch (_roomType) {
			case RoomType.Arena:  return 'A';
			case RoomType.Start:  return 'S';
			case RoomType.Fixer:  return 'F';
			case RoomType.Square: return 'X';
			case RoomType.Line:   return 'T';
			case RoomType.Finish: return '$';
			case RoomType.Battle: return 'B';
			case RoomType.None:   return '-';
			case RoomType.T_Path: return '+';
			case RoomType.Gainer: return 'G';
			default:              return '&';
		}
	}

	// TODO: A*
	private int[,] trackMap;
	private Queue<Vector2Int> trackQueue;

	private List<Way> roomsWays = new List<Way>();
	protected int CountDaWay(Vector2Int _pFrom, Vector2Int _pTo, int wayNumber = -1, bool _draw = true, bool usePathPoints = false) {
		//Debug.Log($"Drawing from {_pFrom} to {_pTo}");
		MarkTrackMap(_pFrom, usePathPoints);

		if (trackMap[_pTo.x, _pTo.y] == int.MaxValue) {
			Debug.LogError($"No way from {_pFrom} to {_pTo}");
			for (int i = openPoints.Count - 1; i >= 0; --i)
				Debug.Log(openPoints[i]);
			DrawTrackMap();
			return -1;
		}

		Vector2Int curPoint = _pTo;
		while (curPoint != _pFrom) {
			if (_draw)
				MarkThePoint(curPoint, wayNumber);

			if (curPoint.x > 0 && trackMap[curPoint.x - 1, curPoint.y] == trackMap[curPoint.x, curPoint.y] - 1) {
				curPoint += Vector2Int.left;
				continue;
			}
			if (curPoint.y > 0 && trackMap[curPoint.x, curPoint.y - 1] == trackMap[curPoint.x, curPoint.y] - 1) {
				curPoint += Vector2Int.down;
				continue;
			}
			if (curPoint.x < mapSize.x - 1 && trackMap[curPoint.x + 1, curPoint.y] == trackMap[curPoint.x, curPoint.y] - 1) {
				curPoint += Vector2Int.right;
				continue;
			}
			if (curPoint.y < mapSize.y - 1 && trackMap[curPoint.x, curPoint.y + 1] == trackMap[curPoint.x, curPoint.y] - 1) {
				curPoint += Vector2Int.up;
				continue;
			}

			DrawTrackMap();
			Debug.LogError("Missed the way");
			return -1;
		}

		if (_draw)
			MarkThePoint(curPoint, wayNumber);

		return trackMap[_pTo.x, _pTo.y];
	}

	protected void DrawTrackMap() {
		if (trackMap == null) {
			Debug.Log("NoTrackMap");
			return;
		}

		string[] debugingMap = new string[trackMap.GetLength(1)];
		for (int i = debugingMap.Length - 1; i >= 0; --i) {
			for (int j = 0; j < trackMap.GetLength(0); ++j)
				if (trackMap[j, i] == int.MaxValue)
					debugingMap[i] += "## ";
				else if (trackMap[j, i] < 10)
					debugingMap[i] += "0" + trackMap[j, i] + " ";
				else
					debugingMap[i] += trackMap[j, i] + " ";
			Debug.Log(debugingMap[i]);
		}
	}

	private void MarkThePoint(Vector2Int _p, int _wayNumber = -1) {
		theMap[_p.x, _p.y] = GetMapMark(RoomType.T_Path);
		pointsToCover.Add(new Vector2Int(_p.x, _p.y));
		int hasOpenPoint = FindOpenPointAtPoint(_p);
		if (hasOpenPoint != -1)
			openPoints.RemoveAt(hasOpenPoint);

		if (_wayNumber > -1) {
			if (roomsWays.Count > _wayNumber) {
				roomsWays[_wayNumber].points.Add(new Vector2Int(_p.x, _p.y));
            }
            else {
				Way way = new Way();
				way.points = new List<Vector2Int>();
				way.points.Add(new Vector2Int(_p.x, _p.y));
				roomsWays.Add(way);
            }
		}
	}

	private void MarkTrackMap(Vector2Int _from, bool _usePathPoints = false) {
		trackQueue = new Queue<Vector2Int>();
		trackQueue.Enqueue(_from);

		trackMap = new int[mapSize.x, mapSize.y];
		for (int i = trackMap.GetLength(0) - 1; i >= 0; --i)
		for (int j = trackMap.GetLength(1) - 1; j >= 0; --j)
			trackMap[i, j] = int.MaxValue;
		trackMap[_from.x, _from.y] = 1;

		Vector2Int curPoint;
		while (trackQueue.Count > 0) {
			curPoint = trackQueue.Dequeue();
			if (curPoint.x > 0)
				TryAddNewPoint(curPoint + Vector2Int.left, curPoint, _usePathPoints);
			if (curPoint.y > 0)
				TryAddNewPoint(curPoint + Vector2Int.down, curPoint, _usePathPoints);
			if (curPoint.x < mapSize.x - 1)
				TryAddNewPoint(curPoint + Vector2Int.right, curPoint, _usePathPoints);
			if (curPoint.y < mapSize.y - 1)
				TryAddNewPoint(curPoint + Vector2Int.up, curPoint, _usePathPoints);
		}
	}

	private void TryAddNewPoint(Vector2Int _new, Vector2Int _cur, bool _usePathPoints = false) {
		if ((theMap[_new.x, _new.y] == GetMapMark(RoomType.None) || 
			(_usePathPoints && theMap[_new.x, _new.y] == GetMapMark(RoomType.T_Path))) &&
			trackMap[_new.x, _new.y] > trackMap[_cur.x, _cur.y] + 1) {
			trackMap[_new.x, _new.y] = trackMap[_cur.x, _cur.y] + 1;
			trackQueue.Enqueue(_new);
		}
	}

	// Spawns generated room at scene.
	public virtual void Place() {
		if (dontGenerate) return;

		GeneratedRoom tempGR;
		PrepareBackground();

		for (int i = generated.Count - 1; i >= 0; --i) {
			tempGR = generated[i];

//#if UNITY_EDITOR
//			if (!Application.isPlaying)
//				tempObject = UnityEditor.PrefabUtility.InstantiatePrefab(tempGR.room.roomScene, mapRoot) as GameObject;
//			else
//#endif
//				tempObject = Instantiate(tempGR.room.roomModel, mapRoot); 

			SceneManager.LoadScene(tempGR.room.roomSceneName, LoadSceneMode.Additive);

			//Debug.Log("ScenesLoaded");
			//tempGR.onScene = tempObject.GetComponent<RoomController>();
			//tempGR.onScene.roomData = generated[i].room;
			//generated[i] = tempGR;

			//tempObject.transform.localPosition = GetLocalPos(generated[i].p + (generated[i].room.size - Vector2.one) * .5f);
			//tempObject.transform.localRotation = Quaternion.AngleAxis(-90f * (int) generated[i].r, transform.up);
		}
	}
	
	protected List<int> GetClosestOpenPoints(Vector2Int _p, List<WayConnection> _exeptions) {
		List<int> closestPoints = new List<int>();
		MarkTrackMap(_p);
		for (int i = openPoints.Count - 1; i >= 0; --i) {
			if (_exeptions.Contains(openPoints[i])) continue;
			if (trackMap[openPoints[i].offsetTo.x, openPoints[i].offsetTo.y] != int.MaxValue)
				closestPoints.Add(i);
		}

		closestPoints.Sort((int a, int b) => { return trackMap[openPoints[a].offsetTo.x, openPoints[a].offsetTo.y] - trackMap[openPoints[b].offsetTo.x, openPoints[b].offsetTo.y]; });
		return closestPoints;
	}

	protected Tuple<Vector2Int, Vector2Int> GetShortestRoomEscape(SketchRoom _room) {
		Tuple<Vector2Int, Vector2Int> bestPair = new Tuple<Vector2Int, Vector2Int>(-Vector2Int.one, -Vector2Int.one);
		int minDistance = int.MaxValue;
		for (int i = _room.someWays.Count - 1; i >= 0; --i) {
			MarkTrackMap(_room.someWays[i].offsetTo + _room.p);
			for (int j = openPoints.Count - 1; j >= 0; --j) {
				// Ближе ли пара?
				if (trackMap[openPoints[j].offsetTo.x, openPoints[j].offsetTo.y] >= minDistance) continue;
				// Не смотрим ли мы собственную точку?
				int k;
				for (k = _room.someWays.Count - 1; k >= 0; --k)
					if (openPoints[j] == _room.someWays[k] + _room.p)
						break;
				if (k >= 0) continue;

				minDistance = trackMap[openPoints[j].offsetTo.x, openPoints[j].offsetTo.y];
				bestPair = new Tuple<Vector2Int, Vector2Int>(_room.someWays[i].offsetTo + _room.p, openPoints[j].offsetTo);
			}
		}

		return bestPair;
	}
	protected Tuple<Vector2Int, Vector2Int> GetShortestGainerEscape(SketchRoom _room, int gainerWayNumber, out int minDistance) {
		Tuple<Vector2Int, Vector2Int> bestPair = new Tuple<Vector2Int, Vector2Int>(-Vector2Int.one, -Vector2Int.one);
		minDistance = int.MaxValue;
		for (int i = _room.someWays.Count - 1; i >= 0; --i) {
			MarkTrackMap(_room.someWays[i].offsetTo + _room.p, true);
            for (int l = roomsWays[gainerWayNumber].points.Count - 1; l >= 0; --l) {
				if (trackMap[roomsWays[gainerWayNumber].points[l].x, roomsWays[gainerWayNumber].points[l].y] >= minDistance) continue;
				int k;
				for (k = _room.someWays.Count - 1; k >= 0; --k)
					if (roomsWays[gainerWayNumber].points[l].x == _room.someWays[k].offsetTo.x + _room.p.x &&
						roomsWays[gainerWayNumber].points[l].y == _room.someWays[k].offsetTo.y + _room.p.y)
						break;
				if (k >= 0) continue;
				minDistance = trackMap[roomsWays[gainerWayNumber].points[l].x, roomsWays[gainerWayNumber].points[l].y];
				bestPair = new Tuple<Vector2Int, Vector2Int>(_room.someWays[i].offsetTo + _room.p, roomsWays[gainerWayNumber].points[l]);
			}
		}

		return bestPair;
	}

	// TODO: find all open points
	protected int FindOpenPointAtPoint(Vector2Int _p) {
		for (int i = openPoints.Count - 1; i >= 0; --i)
			if (openPoints[i].offsetTo == _p)
				return i;
		return -1;
	}

	protected int GetGeneratedRoomIndexAtPoint(Vector2Int _p) {
		for (int i = generated.Count - 1; i >= 0; --i)
			if (_p.x >= generated[i].p.x && _p.y >= generated[i].p.y && _p.x < generated[i].p.x + generated[i].room.size.x && _p.y < generated[i].p.y + generated[i].room.size.y)
				return i;
		return -1;
	}

	// Only for _p1 <= _p2
	protected List<GeneratedRoom> CollectGeneratedNeighbours(Vector2Int _p1, Vector2Int _p2) {
		List<GeneratedRoom> neighbours = new List<GeneratedRoom>();
		for (int i = generated.Count - 1; i >= 0; --i)
			if (generated[i].IsTouchingRect(_p1, _p2))
				neighbours.Add(generated[i]);
		return neighbours;
	}
}

public struct Way {
	public List<Vector2Int> points;
}
