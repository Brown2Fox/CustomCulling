using UnityEngine;

public class ProgressBarUI : MonoBehaviour {
	[ObjectFieldFilter(typeof(IProgressBarUI))]
	public Component component;
	protected IProgressBarUI target;

	public new Renderer renderer;
	protected Material material;

	public Color minColor = Color.red;
	public Color maxColor = Color.green;

	public Transform bar;

	private int colorId;

	public virtual void Awake() {
		colorId = Shader.PropertyToID("_Color");
		target = component as IProgressBarUI;
		material = renderer.material;
		renderer.material = material;
	}

	protected virtual void Start() {
	}

	public float t;

	protected virtual void LateUpdate() {
		Count();
		Resize();
		Colorize();
	}

	protected virtual void Count() {
		if (target.GetMaxValue() == 0) {
			t = 1;
			return;
		}

		t = target.GetCurrentValue() / target.GetMaxValue();
	}

	protected virtual void Resize() {
		bar.localScale = new Vector3(1, 1, t);
	}

	protected virtual void Colorize() {
		Color c = Color.Lerp(minColor, maxColor, t);
		material.SetColor(colorId, c);
	}
}