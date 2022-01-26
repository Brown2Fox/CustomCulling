using System;
using UnityEngine;

[Serializable]
public struct ClampingRescale {
	public Vector2
		input, output;

	public float Get(float _in, bool _lowerToZero = false) {
		if (Mathf.Abs(input.y - input.x) < 1e-5) return output.x;
		if (_lowerToZero && _in < input.x) return 0f;

		_in = Mathf.Clamp(_in, input.x, input.y) - input.x;
		return _in / (input.y - input.x) * (output.y - output.x) + output.x;
	}
}
