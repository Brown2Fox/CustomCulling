using UnityEngine;

public class PlayerFlyAbility : Ability {
	private Rigidbody rigidBody;

	public const float g = 9.8f;

	[DisableEditing]
	public float currentMultiplier = g;

	protected override void InitBehaviour() {
		var player = container.GetComponent<Player>();
		rigidBody = player.movable.GetComponent<Rigidbody>();
		player.ccExtention.onSetFlight += SetState;
	}

	protected override void DestroyBehaviour() {
		var player = container.GetComponent<Player>();
		player.ccExtention.onSetFlight -= SetState;
	}

	public void SetState(bool _state) {
		enabled = _state;
	}

	public void OnEnable() {
		if (!rigidBody) return;
		rigidBody.useGravity = false;
		PlaySound();
	}

	public void FixedUpdate() {
		rigidBody.velocity += currentMultiplier * Time.fixedDeltaTime * Vector3.down;
	}

	public void OnDisable() {
		if (!rigidBody) return;
		rigidBody.useGravity = true;
	}
}