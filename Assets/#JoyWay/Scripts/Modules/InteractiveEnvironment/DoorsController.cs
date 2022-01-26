using UnityEngine;

public class DoorsController : MonoBehaviour {
	public Animator[] doors;
	public bool openOnRoomFinish = true;

	private RoomController room;

	private void Start() {
		if (openOnRoomFinish) {
			room = GetComponentInParent<RoomController>();
			if (room)
				room.onRoomFinish += OpenDoors;
		}
	}

	public void OpenDoors() {
		for (int i = doors.Length - 1; i >= 0; --i) {
			doors[i].SetBool("Open", true);
		}
	}
	public void CloseDoors() {
		for (int i = doors.Length - 1; i >= 0; --i) {
			doors[i].SetBool("Open", false);
		}
	}

	private void OnDestroy() {
		if (room)
			room.onRoomFinish -= OpenDoors;
	}
}
