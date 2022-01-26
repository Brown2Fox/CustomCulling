using System;
using System.Collections.Generic;
using UnityEngine;

public class Label : MonoBehaviour {
	public List<LabelType> labels = new List<LabelType>();

	public Action<Label> ChangedLabelsAction;

	public Transform CheckPoint {
		get {
			if (checkPoint != null)
				return checkPoint;
			else
				return transform;
		}
	}

	[SerializeField]
	private Transform checkPoint;

	public bool Check(LabelType[] _labels) {
		for (int i = labels.Count - 1; i >= 0; --i)
			for (int j = _labels.Length - 1; j >= 0; --j)
				if (labels[i] == _labels[j])
					return true;
		return false;
	}

	public bool Check(LabelType _labelType) {
		for (int i = labels.Count - 1; i >= 0; --i)
			if (labels[i] == _labelType)
				return true;
		return false;
	}

	public void AddLabel(LabelType _newLabel) {
		if (!labels.Contains(_newLabel)) {
			labels.Add(_newLabel);
			ChangedLabelsAction?.Invoke(this);
		}
	}

	public void RemoveLabel(LabelType _label) {
		if (labels.Contains(_label)) {
			labels.Remove(_label);
			ChangedLabelsAction?.Invoke(this);
		}
	}
}
