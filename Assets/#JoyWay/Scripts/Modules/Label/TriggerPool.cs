using System;
using System.Collections.Generic;
using JoyWay.Systems.InputSystem;
using UnityEngine;

public delegate bool LabelCondition(Label _label);

public class TriggerPool : MonoBehaviour {
	public List<Tuple<Label, List<Collider>>> labels = new List<Tuple<Label, List<Collider>>>();
	public List<Collider> colliders = new List<Collider>();
	public LabelType filterLabel;
	private Label lastLabel, activeTarget;

	public Label CurrentTarger {
		get { return activeTarget; }
	}

	private Rigidbody rigidBody;
	private int labelIndex;

	public Action<Label> OnActiveTargetHasChanged;

	private const float minAdvantage = 0.001f;
	private LabelCondition condition;

	private void OnDisable() {
		ClearLabels();
	}

	private GrabHandPart hand;

	private void Awake() {
		hand = GetComponentInParent<GrabHandPart>();
	}

	private void ClearLabels() {
		for (int i = labels.Count - 1; i >= 0; --i)
		for (int j = labels[i].Item2.Count - 1; j >= 0; --j)
			RemoveAgent(labels[i].Item2[j]);
		activeTarget = null;
		labels.Clear();
	}

	private Label FindLabelForCollider(Collider _collider) {
		Label label = _collider.GetComponentInParent<Label>();
		if (!label) {
			rigidBody = _collider.GetComponentInParent<Rigidbody>();
			if (rigidBody)
				label = rigidBody.GetComponentInParent<Label>();
		}

		return label;
	}

	private Label FindLabelInLabels(Collider _collider) {
		for (int i = labels.Count - 1; i >= 0; --i)
		for (int j = labels[i].Item2.Count - 1; j >= 0; --j)
			if (labels[i].Item2[j] == _collider)
				return labels[i].Item1;
		return null;
	}

	private void OnTriggerEnter(Collider _collider) {
		lastLabel = FindLabelForCollider(_collider);
		if (!lastLabel || !lastLabel.Check(filterLabel)) return;

		AddToLabels(lastLabel, _collider);
		AddAgent(_collider);
		RecountActiveTarget();
	}

	private void OnTriggerExit(Collider _collider) {
		lastLabel = FindLabelForCollider(_collider);
		if (!lastLabel || !lastLabel.Check(filterLabel)) return;

		RemoveFromLabels(lastLabel, _collider);
		RemoveAgent(_collider);
		RecountActiveTarget();
	}

	public void OnColliderDisabled(Collider _collider) {
		lastLabel = FindLabelInLabels(_collider);
		if (!lastLabel) return;

		RemoveFromLabels(lastLabel, _collider);
		RecountActiveTarget();
	}

	public void OnLabelChanged(Label _label) {
		labelIndex = HasLabel(_label);
		if (labelIndex != -1) {
			if (!_label.Check(filterLabel)) {
				labels.RemoveAt(labelIndex);
				_label.ChangedLabelsAction -= OnLabelChanged;
				RecountActiveTarget();
			}
		} else {
			_label.ChangedLabelsAction -= OnLabelChanged;
		}
	}

	private int HasLabel(Label _label) {
		for (int i = labels.Count - 1; i >= 0; --i)
			if (labels[i].Item1 == _label)
				return i;
		return -1;
	}

	private void SetNewActiveLabel(Label _label) {
		Vibrate();
		if (_label == activeTarget) return;
		activeTarget = _label;
		OnActiveTargetHasChanged?.Invoke(_label);
	}

	private void Update() {
		RecountActiveTarget();
		colliders.Clear();
		for (int i = labels.Count - 1; i >= 0; --i)
		for (int j = labels[i].Item2.Count - 1; j >= 0; --j)
			colliders.Add(labels[i].Item2[j]);
	}

	private void RecountActiveTarget() {
		float minDistance = float.PositiveInfinity, checkDistance;
		Label newActiveTarget = null;
		for (int i = labels.Count - 1; i >= 0; --i) {
			if (condition != null && !condition.Invoke(labels[i].Item1)) continue;
			for (int j = labels[i].Item2.Count - 1; j >= 0; --j) {
				checkDistance = (labels[i].Item2[j].ClosestPoint(transform.position) - transform.position).magnitude;
				if (checkDistance < minDistance - minAdvantage) {
					newActiveTarget = labels[i].Item1;
					minDistance = checkDistance;
				}
			}
		}

		SetNewActiveLabel(newActiveTarget);
	}

	private void AddToLabels(Label _label, Collider _collider) {
		labelIndex = HasLabel(_label);
		if (labelIndex == -1) {
			labels.Add(new Tuple<Label, List<Collider>>(_label, new List<Collider>() {_collider}));
			_label.ChangedLabelsAction += OnLabelChanged;
		} else if (!labels[labelIndex].Item2.Contains(_collider))
			labels[labelIndex].Item2.Add(_collider);
	}

	private void RemoveFromLabels(Label _label, Collider _collider) {
		labelIndex = HasLabel(_label);
		if (labelIndex != -1) {
			if (labels[labelIndex].Item2.Contains(_collider))
				labels[labelIndex].Item2.Remove(_collider);
			if (labels[labelIndex].Item2.Count == 0) {
				labels.RemoveAt(labelIndex);
				_label.ChangedLabelsAction -= OnLabelChanged;
			}
		}
	}

	public void SetCondition(LabelCondition _condition) {
		condition = _condition;
	}

	private void AddAgent(Collider _collider) {
		TriggerAgent ta = _collider.gameObject.GetComponent<TriggerAgent>();
		if (ta)
			ta.AddWatcher(this, _collider);
		else
			_collider.gameObject.AddComponent<TriggerAgent>().AddWatcher(this, _collider);
	}

	private void RemoveAgent(Collider _collider) {
		_collider.gameObject.GetComponent<TriggerAgent>()?.RemoveWatcher(this, _collider);
	}

	private void Vibrate() {
		if (activeTarget != null && hand && hand.isFree)
			hand.Vibrate(
						 new VibrationSettings {
												   amplitude = 0.5f,
												   frequency = 0.5f,
												   duration = 0.1f,
												   delay = 0
											   });
	}
}