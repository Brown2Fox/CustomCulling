using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[Serializable]
public class RoomData {
	public RoomType roomType;
#if UNITY_EDITOR
	public SceneAsset roomScene;
#endif
	public string roomSceneName;
	public Vector2Int size = Vector2Int.one;
	public List<WayConnection> ways;
	public RotateMeasure modelRotation;

	public RoomData(RoomData _roomData) {
		roomType = _roomData.roomType;
#if UNITY_EDITOR
		roomScene = _roomData.roomScene;
#endif
		roomSceneName = _roomData.roomSceneName;
		size = _roomData.size;
		ways = new List<WayConnection>(_roomData.ways);
	}

#if UNITY_EDITOR
	public void UpdateSceneName() {
		if (roomScene != null)
			roomSceneName = roomScene ? AssetDatabase.GetAssetPath(roomScene) : "";
	}

	public RoomData(Vector2Int _size, RoomType _roomType = RoomType.Fixer, SceneAsset _roomModel = null, RotateMeasure _modelRotation = RotateMeasure._0) {
		size = _size;
		roomType = _roomType;
		roomScene = _roomModel;
		UpdateSceneName();
		modelRotation = _modelRotation;
		ways = new List<WayConnection>();
	}
#endif

	public void RotateWays() {
		for (int i = ways.Count - 1; i >= 0; --i)
			ways[i] = new WayConnection(new Vector2Int(size.y - ways[i].offsetFrom.y - 1, ways[i].offsetFrom.x), new Vector2Int(size.y - ways[i].offsetTo.y - 1, ways[i].offsetTo.x));
		modelRotation = (RotateMeasure)(((int)modelRotation + 1) % 4);
	}
}
