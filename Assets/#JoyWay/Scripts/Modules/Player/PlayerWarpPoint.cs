using UnityEngine;

public class PlayerWarpPoint : MonoBehaviour {
	public void Warp() {
		Player.instance.movable.position = transform.position;
		Player.instance.movable.rotation = transform.rotation;
		Player.instance.movable.GetComponent<Rigidbody>().velocity = Vector3.zero;
		// Player.instance.owner.aContainer.GetEntryOfType<PlayerMoveAbility>().Warp(transform.position);

	}

	private void OnTriggerEnter(Collider _collider) {
		if (_collider.GetComponentInParent<Player>() != null)
			Warp();
	}
}
