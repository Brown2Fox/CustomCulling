using UnityEngine;

[System.Serializable]
#pragma warning disable 660,661
public struct WayConnection {
	public Vector2Int offsetFrom, offsetTo;

	public WayConnection(Vector2Int _offsetFrom, Vector2Int _offsetTo) {
		offsetFrom = _offsetFrom;
		offsetTo = _offsetTo;
	}

	public static bool AreConnected(WayConnection _w1, Vector2Int _p1, WayConnection _w2, Vector2Int _p2) => _p1 + _w1.offsetTo == _p2 + _w2.offsetFrom && _p1 + _w1.offsetFrom == _p2 + _w2.offsetTo;

	public static WayConnection operator +(WayConnection _c, Vector2Int _p) {
		return new WayConnection {offsetFrom = _c.offsetFrom + _p, offsetTo = _c.offsetTo + _p};
	}

	public static bool operator ==(WayConnection _a, WayConnection _b) {
		return _a.offsetFrom == _b.offsetFrom && _a.offsetTo == _b.offsetTo;
	}

	public static bool operator !=(WayConnection _a, WayConnection _b) {
		return _a.offsetFrom != _b.offsetFrom || _a.offsetTo != _b.offsetTo;
	}
}
#pragma warning restore 660,661