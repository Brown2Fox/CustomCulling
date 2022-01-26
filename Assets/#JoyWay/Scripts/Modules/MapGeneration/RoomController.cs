using System;
using UnityEngine;
using UnityEngine.UI;
using JoyWay.Systems.InputSystem;
using System.Collections.Generic;
using System.Linq;
using InputAction = UnityEngine.InputSystem.InputAction;
#if UNITY_EDITOR
using UnityEditor;

#endif

public class RoomController : MonoBehaviour {
	public GameObject[] pedestals;
	[DisableEditing]
	public int giftTierId = -1;
	[DisableEditing]
	public float giftTierValue = -1;

	public float populationMultiplier = 1f;

	//[HideInInspector]
	public RoomData roomData;
	[DisableEditing]
	public int depth;
	[DisableEditing]
	public float totalCost;

	private Vector2 wayPoint;
	public static float fontSizeMultiplier = 0;

	public Action onRoomActivation, onRoomFinish;
	private bool isActivated;

	public bool arena;
	private bool activateAfterTutor;

	public GameObject proxy;
	public GameObject fullGeom;
	public GameObject trash;
	public GameObject cliffRoot;
	private List<Renderer> cliffs;


	private float
		timeLeft = float.NegativeInfinity,
		timeTillHide = 1f;

	public Sprite spriteForMinimap;

	private void Awake() {
		Show();

		cliffs = cliffRoot.GetComponentsInChildren<Renderer>(true).ToList();

		if (MapGenerator.instance != null)
			MapGenerator.instance.RoomIsReady(this);
	}

	private Vector3 myPosition;

	public void Start() {
		myPosition = transform.position;
	}


	private bool visible;

	public bool IsVisible() {
		return visible;
	}


	public void SetVisible(bool _state) {
		visible = _state;
		if (_state) timeLeft = timeTillHide;
	}


	public void Show() {
		timeLeft = timeTillHide;
	}

	public void SetState(RoomState _state) {
		bool proxyB, props;
		switch (_state) {
			case RoomState.Hidden:
				proxyB = false;
				props = false;
				break;
			case RoomState.VisibleFar:
				proxyB = true;
				props = false;
				break;
			case RoomState.VisibleNear:
				proxyB = false;
				props = true;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(_state), _state, null);
		}


		proxy.SetActive(proxyB);
		fullGeom.SetActive(props);

		trash.SetActive(props);

		foreach (Renderer cliff in cliffs) {
			cliff.enabled = props || proxyB;
		}
	}


	[Tooltip("squared distance to player")]
	public float farDistance = 50 * 50;

	public bool CheckDistance() {
		return (myPosition - Player.instance.movable.position).sqrMagnitude < farDistance;
	}


	private void Update() {
		RoomState newState;
		if (IsVisible()) {
			if (CheckDistance()) {
				newState = RoomState.VisibleNear;
			} else {
				newState = RoomState.VisibleFar;
			}
		} else

			newState = RoomState.Hidden;

		SetState(newState);

		timeLeft -= Time.deltaTime;
		if (timeLeft < 0)
			SetVisible(false);
	}

	private void RoomActivateDelayer() {
		activateAfterTutor = true;
	}

	


	public void ActivateRoom() {
		if (!isActivated) {
			onRoomActivation?.Invoke();
			isActivated = true;
			Invoke(nameof(FinishRoom), 60f);
		}
	}

	public void FinishRoom() {
		if (isActivated) {
			onRoomFinish?.Invoke();
			isActivated = false;
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos() {
		if (roomData == null) return;
		Gizmos.color = Color.HSVToRGB(1f / Enum.GetValues(typeof(RoomType)).Length * (int) roomData.roomType, 1, 1);
		for (int i = roomData.ways.Count - 1; i >= 0; --i) {
			wayPoint = roomData.ways[i].offsetTo - (roomData.size - Vector2.one) * .5f;
			if (roomData.ways[i].offsetTo.x == -1) wayPoint += Vector2.right * .5f;
			else if (roomData.ways[i].offsetTo.x == roomData.size.x) wayPoint -= Vector2.right * .5f;
			else if (roomData.ways[i].offsetTo.y == -1) wayPoint += Vector2.up * .5f;
			else if (roomData.ways[i].offsetTo.y == roomData.size.y) wayPoint -= Vector2.up * .5f;

			Gizmos.DrawLine(transform.position + Vector3.up * 10f, transform.position + MapGenerator.cellSize * new Vector3(wayPoint.x, 0f, wayPoint.y) + Vector3.up * 10f);
		}

		Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 10f);

		Gizmos.color = new Color(.2f, 1f, .8f);
		for (int i = pedestals.Length - 1; i >= 0; --i)
			if (pedestals[i].activeInHierarchy)
				Gizmos.DrawSphere(pedestals[i].transform.position + Vector3.up * 15f, 3f);

		int fs = (int) (fontSizeMultiplier / (SceneView.currentDrawingSceneView.camera.transform.position - transform.position).magnitude);
		if (fs == 0) return;

		GUIStyle style = new GUIStyle();
		style.fontSize = fs;
		style.normal.textColor = Color.cyan;
		Handles.Label(transform.position + Vector3.up * 16f, depth.ToString(), style);

		if (giftTierValue >= 0f) {
			Vector3 textColor = new Vector3(.2f, 1f, .8f);
			textColor *= giftTierValue;
			style.normal.textColor = new Color(textColor.x, textColor.y, textColor.z);
			Handles.Label(transform.position + Vector3.up * 8f - Vector3.forward * 8f, giftTierValue.ToString("0.0"), style);
		}
	}
#endif
}


public enum RoomState {
	Hidden,
	VisibleFar,
	VisibleNear,
}

#if UNITY_EDITOR
[CustomEditor(typeof(RoomController))]
public class RoomControllerEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		RoomController the = (RoomController) target;

		// Возможно я перепутал Vector3.right и Vector3.forward
		if (GUILayout.Button("TPPlayer"))
			Player.instance.movable.position = the.transform.position + Vector3.up * 16f + the.roomData.size.x * Vector3.right + the.roomData.size.y * Vector3.forward;
	}
}
#endif