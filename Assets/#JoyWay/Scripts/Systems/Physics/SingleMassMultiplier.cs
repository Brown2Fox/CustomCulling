using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SingleMassMultiplier : MonoBehaviour {
	public float
		multiplier = 100f,
		resetTime = 0.3f;
	private Rigidbody rigidBody;
	private int multipliers;
	private float initMass;
	private Coroutine resetProcess;

	private void Awake() {
		rigidBody = GetComponent<Rigidbody>();
		initMass = rigidBody.mass;
	}

	public void Multiply() {
		if (++multipliers == 1) {
			rigidBody.mass = multiplier * initMass;
			if (resetProcess != null) {
				StopCoroutine(resetProcess);
				resetProcess = null;
			}
		}
	}

	public void ResetMass() {
		if (--multipliers == 0)
			resetProcess = StartCoroutine(ResetProcess());
	}

	private IEnumerator ResetProcess() {
		float startTime = Time.time;
		while (startTime + resetTime > Time.time) {
			rigidBody.mass = Mathf.Lerp(initMass * multiplier, initMass, (Time.time - startTime) / resetTime);
			yield return null;
		}
		rigidBody.mass = initMass;
		resetProcess = null;
	}
}
