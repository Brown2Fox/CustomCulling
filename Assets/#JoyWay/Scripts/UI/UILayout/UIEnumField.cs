using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIEnumField : MonoBehaviour {
	public List<string> options;
	public UIButton left, right;
	public int currentValue;

	public TextMeshPro text;

	public bool disableOnEdgeValues;
	public Action<int> onNewValue;

	public void SetUp(Type _t, int _current, Action<int> _action = null) {
		options = new List<string>();
		string[] values = Enum.GetNames(_t);
		foreach (string value in values) {
			options.Add(value.TrimStart('_'));
		}

		onNewValue = _action;

		SetValue(_current);
	}


	public void Next() {
		SetValue(currentValue + 1);
	}


	public void Prev() {
		SetValue(currentValue - 1);
	}

	public void SetValue(int k) {
		k %= options.Count;
		if (k < 0) k += options.Count;
		currentValue = k;
		text.text = options[k];

		if (disableOnEdgeValues) {
			left.SetDisabled(currentValue == 0);
			right.SetDisabled(currentValue == options.Count - 1);
		}

		onNewValue?.Invoke(currentValue);
	}

	public void SetDisabled() {
		options = new List<string>();
		options.Add(" ");
		SetValue(0);
	}
}