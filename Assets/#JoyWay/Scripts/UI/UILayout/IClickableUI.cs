public interface IClickableUI : IHoverableUI {
	void OnClick(bool _state);
}

public interface IHoverableUI : IUIElement {
	void OnHover(bool _state);
}


public interface IUIElement {
	void SetDisabled(bool _b);
}