using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIRadioGroup : MonoBehaviour {
	public List<UIToggleButton> group = new List<UIToggleButton>();
	public GameObject[] enablable;
	public UnityEvent[] onActivation, onDeactivation;

	public int defaultValue = 0;
	private int
		currentValue = -1;

	public bool autoinit = true;

	public Action<int, int> onChange;

	private void Awake() {
		if (autoinit)
			Init();
	}

	public void Init() {
		for (int i = group.Count - 1; i >= 0; --i) {
			int j = i;
			// Вот тут вообще капец как осторожно! Мне было лень отписывать замыкание в меню, так как набор данных пока статичный.
			group[j].onStateChange += (_state) => ProcessButtonClick(_state, j);
		}

		for (int i = enablable.Length - 1; i >= 0; --i)
			enablable[i].SetActive(false);

		if (defaultValue >= 0 && currentValue != defaultValue)
			group[defaultValue].Trigger();
	}

	private void OnEnable() {
		if (defaultValue >= 0 && currentValue != defaultValue)
			group[defaultValue].Trigger();
	}
	private void OnDisable() {
		SelectRadio(-1);
	}

	private void ProcessButtonClick(bool _state, int _id) {
		if (!_state || currentValue == _id)
			return;

		SelectRadio(_id);
	}

	private void SelectRadio(int _id) {
		if (_id == currentValue)
			return;

		if (enablable.Length > 0) {
			if (currentValue >= 0)
				enablable[currentValue].SetActive(false);
			if (_id >= 0)
				enablable[_id].SetActive(true);
		}

		if (currentValue >= 0) {
			group[currentValue].SetDisabled(false);
			group[currentValue].Trigger();
		}
		if (_id >= 0)
			group[_id].SetDisabled(true);

		onChange?.Invoke(currentValue, _id);
		currentValue = _id;
	}
}
