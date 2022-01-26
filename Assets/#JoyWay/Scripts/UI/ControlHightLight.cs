using UnityEngine;
using HighlightPlus;

[RequireComponent(typeof(HighlightEffect))]
public class ControlHightLight : MonoBehaviour {

	public Color aimingTelekinesColor;
	public Color telekinesColor;
	public Color nearGrabberColor;
	public Color baseColor;
	public EffectCreator curCreator;
	public EffectStates curState;
	public HandType curHand;
	private HighlightEffect highlightEffect;

	private void Awake() {
		baseColor = Color.white;
		aimingTelekinesColor = Color.green;
		telekinesColor = Color.red;
		nearGrabberColor = Color.blue;
		curState = EffectStates.Base;
		curCreator = EffectCreator.None;
		highlightEffect = GetComponent<HighlightEffect>();
	}

	public void AimingTelekines(HandType hand) {
		CheckInState(EffectStates.Aiming, hand , EffectCreator.Telekines);
	}

	public void Telekines(HandType hand) {
		CheckInState(EffectStates.Telekines, hand, EffectCreator.Telekines);
	}

	public void NearGrabber(HandType hand) {
		CheckInState(EffectStates.NearGrabber, hand, EffectCreator.Grab);
	}

	public void Grabber(HandType hand) {
		CheckInState(EffectStates.Grabber, hand, EffectCreator.Grab);
	}

	public void ReturnToBase(HandType hand) {
		if(curHand == hand)
			Clear();
	}

	public void Clear() {
		ChangeColor(baseColor);
		HighLighed(false);
		curState = EffectStates.Base;
		curCreator = EffectCreator.None;
	}

	private void ChangeColor(Color newColor) {
		highlightEffect.glowHQColor = newColor;
		HighLighed(true);
	}

	private void HighLighed(bool _state) {
		highlightEffect.highlighted = _state;
	}

	public void StateChanged() {
		switch(curState) {
			case EffectStates.Aiming:
				ChangeColor(aimingTelekinesColor);
				break;
			case EffectStates.Telekines:
				ChangeColor(telekinesColor);
				break;
			case EffectStates.NearGrabber:
				ChangeColor(nearGrabberColor);
				break;
			case EffectStates.Grabber:
				HighLighed(false);
				break;
			default:
				Clear();
				break;
		}
	}

	public void CheckInState(EffectStates _to, HandType hand, EffectCreator creator) {
		if ((int)curState < (int)_to || (curHand == hand && (int)curCreator < (int)creator)) {
			curHand = hand;
			curState = _to;
			curCreator = creator;
			StateChanged();
		}
	}
}

public enum EffectCreator {
	None,
	Telekines,
	Grab
}

public enum EffectStates {
	Base = 0,
	Aiming = 1,
	Telekines = 2,
	NearGrabber = 3,
	Grabber = 4
}
