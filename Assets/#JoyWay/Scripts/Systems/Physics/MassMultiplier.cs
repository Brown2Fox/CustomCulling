using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MassMultiplier : MonoBehaviour {
	public float defaultMultiplier;
	private Rigidbody rigidBody;
	private float currentMultiplier = 10f;

	private void Awake() {
		rigidBody = GetComponent<Rigidbody>();
	}

	public void Multiply() {
		rigidBody.mass *= defaultMultiplier;
		currentMultiplier *= defaultMultiplier;
	}

	public void ResetMass() {
		rigidBody.mass /= currentMultiplier;
		currentMultiplier = 1f;
	}
}
