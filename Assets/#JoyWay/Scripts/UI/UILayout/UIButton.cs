using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Events;

public class UIButton : MonoBehaviour, IClickableUI {
	private static readonly int ColorTint = Shader.PropertyToID("_colorTint");
	protected bool disabled;
	[SerializeField]
	private Collider collider;
	public UIButtonColorsAsset colorSchemeAsset;
	public UIButtonColorScheme colorScheme;
	public UnityEvent onHover, onUnhover, onClick;
	public TextMeshPro text;

	[SerializeField]
	private Renderer renderer;
	[SerializeField]
	private AudioSource source;
	public UIButtonAudioAsset clips;

	private void Start() {
		if (inited) return;
		InitButtonColors();
	}

	protected bool inited = false;

	protected virtual void InitButtonColors() {
		inited = true;
		if (colorSchemeAsset != null)
			colorScheme = colorSchemeAsset.scheme;
		UpdateCurrentColor();
	}

	public void SetColorScheme(UIButtonColorScheme _scheme) {
		colorScheme = _scheme;
		UpdateCurrentColor();
	}
	protected void UpdateCurrentColor() {
		if (disabled)
			ChangeColor(colorScheme.disabled);
		else if (clicking)
			ChangeColor(colorScheme.click);
		else if (hovering)
			ChangeColor(colorScheme.hover);
		else
			ChangeColor(colorScheme.idle);
	}

	public virtual void ChangeColor(UIButtonColors _color) {
		if (renderer)
			foreach (Material material in renderer.materials)
				material.SetColor(ColorTint, _color.button);

		if (text)
			text.color = _color.text;

		//Debug.Log($"Border color to {_color.button}", gameObject);
	}

	protected bool hovering;

	public virtual void OnHover(bool _state) {
		hovering = _state;
		if (_state) {
			source.PlayOneShot(clips.hoverSound);
			ChangeColor(colorScheme.hover);
			onHover.Invoke();
		} else {
			ChangeColor(colorScheme.idle);
			onUnhover.Invoke();
		}
	}

	protected bool clicking;
	public virtual void OnClick(bool _state) {
		clicking = _state;
		if (_state) {
			source.PlayOneShot(clips.clickSound);
			ChangeColor(colorScheme.click);
		} else {
			if (hovering)
				Trigger();
		}
	}


	public virtual void Trigger() {
		ChangeColor(colorScheme.hover);
		onClick.Invoke();
	}

	public void SetDisabled(bool _b) {
		if (disabled == _b) return;
		disabled = _b;
		if (disabled) {
			ChangeColor(colorScheme.disabled);
			collider.enabled = false;
		} else {
			ChangeColor(colorScheme.idle);
			collider.enabled = true;
		}
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(UIButton), true)]
public class UIButtonEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		if (GUILayout.Button("Press"))
			((UIButton) target).Trigger();
	}
}
#endif