using System.Collections;
using System.Collections.Generic;
using JoyWay.Systems.InputSystem;
using UnityEngine;

public class UICursorController : MonoBehaviour {
	public UICursorVisual cursor;
	public Transform[] sources;
	private int currentSourceId;
	private Transform TheSource => sources[currentSourceId];

	public static UICursorController instance;
	public List<UIArea> areas;

	private const float
		checkPeriod = 1f,
		castDist = 30f;
	private WaitForSeconds wfs;
	private RaycastHit hit;
	private Collider[] overlapedAreas;
	private float missedLaserLength;

	private IClickableUI currentClickableUi;

	private void Awake() {
		instance = this;
		currentSourceId = 1;
		wfs = new WaitForSeconds(checkPeriod);
		UIInput.Click.AddListener(InputClicked);
	}

	private void OnDestroy() {
		UIInput.Click.RemoveListener(InputClicked);
	}

	private void InputClicked(bool _state, HandType _hand) {
		if (areas.Count <= 0) return;
		if ((int) _hand - 1 != currentSourceId) {
			currentSourceId = (int) _hand - 1;
			return;
		}

		if (currentClickableUi != null)
			currentClickableUi.OnClick(_state);
	}

	public void AddArea(UIArea _new) {
		areas.Add(_new);
	}

	public void RemoveArea(UIArea _old) {
		areas.Remove(_old);
		if (areas.Count == 0) {
			cursor.SetActive(false);
		}
	}


	private void LateUpdate() {
		if (areas.Count <= 0) return;

		var startPoint = TheSource.position;

		overlapedAreas = Physics.OverlapSphere(startPoint, .01f, 1 << (int) Layer.UIArea);
		if (overlapedAreas.Length <= 0) {
			if (!Physics.Raycast(startPoint, TheSource.forward, out hit, castDist, 1 << (int) Layer.UIArea | 1 << (int) Layer.Vision, QueryTriggerInteraction.Collide)) {
				cursor.SetActive(false);

				UpdateClickable(null);
				return;
			}

			UIArea hittedArea = hit.collider.GetComponent<UIArea>();
			if (hittedArea == null) {
				cursor.SetActive(false);

				UpdateClickable(null);
				return;
			}

			Vector3 flatHit = hittedArea.transform.InverseTransformPoint(hit.point);
			flatHit.z = 0f;
			cursor.SetAlpha(hittedArea.alphaCurve.Evaluate(flatHit.magnitude));
			missedLaserLength = (startPoint - hittedArea.transform.position).magnitude + hittedArea.transform.lossyScale.x * .5f;
		} else {
			cursor.SetAlpha(1f);
			missedLaserLength = overlapedAreas[0].transform.lossyScale.x * 2;
		}

		cursor.SetActive(true);

		if (!Physics.Raycast(startPoint, TheSource.forward, out hit, missedLaserLength, 1 << (int) Layer.UI, QueryTriggerInteraction.Collide)) {
			cursor.SetActivePointer(false);
			cursor.UpdateLaserPoints(startPoint, startPoint + missedLaserLength * TheSource.forward);
			UpdateClickable(null);
			return;
		}

		UpdateClickable(hit.collider.GetComponent<IClickableUI>());
		cursor.SetActivePointer(true);
		cursor.UpdateLaserPoints(startPoint, hit.point);
	}

	private void UpdateClickable(IClickableUI _newClickableUi) {
		if (_newClickableUi != currentClickableUi) {
			if (currentClickableUi != null && !currentClickableUi.Equals(null))
				currentClickableUi.OnHover(false);
			if (_newClickableUi != null)
				_newClickableUi.OnHover(true);
		}

		currentClickableUi = _newClickableUi;
	}
}