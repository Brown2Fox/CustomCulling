//private Vector2Int dif, step;
//private bool invercedLineParam;
//private float tlit, shift;
//private int x, y;

//private bool DrawMapLineWithPoints(Vector2Int _p1, Vector2Int _p2, bool _draw = true) {
//	bool result = true;

//	PrepareParams(_p1, _p2);
//	while (x != _p2.x || y != _p2.y) {
//		if (theMap[x, y] != GetMapMark(RoomType.None))
//			result = false;
//		if (_draw) {
//			theMap[x, y] = '+';

//			int hasOpenPoint = FindOpenPointAtPoint(new Vector2Int(x, y));
//			if (hasOpenPoint != -1)
//				openPoints.RemoveAt(hasOpenPoint);
//		}
//		MakeStep();
//	}
//	if (theMap[x, y] != GetMapMark(RoomType.None))
//		result = false;
//	if (_draw) {
//		theMap[x, y] = '+';

//		int hasOpenPoint = FindOpenPointAtPoint(new Vector2Int(x, y));
//		if (hasOpenPoint != -1)
//			openPoints.RemoveAt(hasOpenPoint);
//	}

//	return result;
//}
//private bool DrawMapLineWithoutPoints(Vector2Int _p1, Vector2Int _p2, bool _draw = true) {
//	if (_p1 == _p2) return true;
//	bool result = true;

//	PrepareParams(_p1, _p2);
//	MakeStep();
//	while (x != _p2.x || y != _p2.y) {
//		if (theMap[x, y] != GetMapMark(RoomType.None))
//			result = false;
//		if (_draw) {
//			theMap[x, y] = '+';

//			int hasOpenPoint = FindOpenPointAtPoint(new Vector2Int(x, y));
//			if (hasOpenPoint != -1)
//				openPoints.RemoveAt(hasOpenPoint);
//		}
//		MakeStep();
//	}
//	return result;
//}

//private void PrepareParams(Vector2Int _p1, Vector2Int _p2) {
//	dif = new Vector2Int(Mathf.Abs(_p2.x - _p1.x), Mathf.Abs(_p2.y - _p1.y));
//	step = new Vector2Int(_p2.x - _p1.x < 0 ? -1 : 1, _p2.y - _p1.y < 0 ? -1 : 1);
//	invercedLineParam = dif.y > dif.x;

//	if (invercedLineParam) {
//		dif.x += dif.y;
//		dif.y = dif.x - dif.y;
//		dif.x -= dif.y;
//	}

//	tlit = 1f * dif.y / dif.x;
//	shift = tlit - .5f;
//	x = _p1.x;
//	y = _p1.y;
//}

//private void MakeStep() {
//	if (shift >= 0) {
//		if (invercedLineParam)
//			x += step.x;
//		else
//			y += step.y;
//		shift -= 1;
//	} else {
//		if (invercedLineParam)
//			y += step.y;
//		else
//			x += step.x;
//		shift += tlit;
//	}
//}