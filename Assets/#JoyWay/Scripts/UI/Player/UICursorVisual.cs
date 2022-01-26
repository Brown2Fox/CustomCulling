using System;
using UnityEngine;

public class UICursorVisual : MonoBehaviour {
	public LineRenderer renderer;

	public Transform pointer;


	private void Awake() {
		start = renderer.startColor;
		end = renderer.endColor;
	}

	private bool active;

	public void SetActive(bool _state) {
		active = _state;
		gameObject.SetActive(_state);
	}


	public void SetActivePointer(bool _state) {
		pointer.gameObject.SetActive(_state);
	}


	public void UpdateLaserPoints(Vector3 _startPoint, Vector3 _endPoint) {
		renderer.SetPosition(0, _startPoint);
		renderer.SetPosition(1, _endPoint);
		pointer.position = _endPoint;
	}


	private Color start, end;

	public void SetAlpha(float _alpha) {
		start.a = _alpha;
		renderer.startColor = start;

		end.a = _alpha;
		renderer.endColor = end;
	}
}