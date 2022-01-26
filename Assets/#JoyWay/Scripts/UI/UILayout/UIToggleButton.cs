using System;
using UnityEngine.Events;

public class UIToggleButton : UIButton {
	public UIButtonColorsAsset normalColorsAsset, activeColorsAsset;
	public UIButtonColorScheme normalColors, activeColors;
	public UnityEvent onActivate, onDeactivate;

	public bool state;
	public Action<bool> onStateChange;

	protected override void InitButtonColors() {
		if (normalColorsAsset != null)
			normalColors = normalColorsAsset.scheme;
		if (activeColorsAsset != null)
			activeColors = activeColorsAsset.scheme;

		colorScheme = normalColors;
		base.InitButtonColors();
	}

	public void SetUp(bool _state, Action<bool> _action) {
		onStateChange = _action;
		SetValue(_state);
	}

	private void SetValue(bool _new) {
		state = _new;

		if (state)
			Activate();
		else
			Deactivate();

		if (disabled)
			ChangeColor(colorScheme.disabled);
		else
			ChangeColor(colorScheme.idle);

		onStateChange?.Invoke(_new);
	}

	public override void Trigger() {
		if (!inited)
			InitButtonColors();
		base.Trigger();
		SetValue(!state);
	}

	public override void OnHover(bool _state) {
		if (!disabled)
			base.OnHover(_state);
		else
			hovering = _state;
	}

	public void Activate() {
		colorScheme = activeColors;
		onActivate.Invoke();
	}
	public void Deactivate() {
		colorScheme = normalColors;
		onDeactivate.Invoke();
	}

	public void SetColorSchemes(UIButtonColorScheme _norm, UIButtonColorScheme _active) {
		normalColors = _norm;
		activeColors = _active;
		colorScheme = state ? activeColors : normalColors;
		UpdateCurrentColor();
	}
}