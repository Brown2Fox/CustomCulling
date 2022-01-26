using UnityEngine;

public class TelekinesisZoneColorAnimator : MonoBehaviour {
	public Color color;
	public Vector2 bounds;
	public float velocity;

	private Material material;
	private float curState, curValue;

	private void Awake() {
		material = GetComponent<Renderer>().material;
	}

	private void Update() {
		curState = Mathf.Clamp01(curState + Time.deltaTime * velocity);
		if (curState >= 1 || curState <= 0)
			velocity *= -1f;

		curValue = Mathf.Lerp(bounds.x, bounds.y, curState);

		material.SetColor("_BaseColor", new Color(color.r, color.g, color.b, curValue));
	}
}
