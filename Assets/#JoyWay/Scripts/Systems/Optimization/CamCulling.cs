using UnityEngine;
using JoyWay.Systems.InputSystem;
using InputAction = UnityEngine.InputSystem.InputAction;

public class CamCulling : MonoBehaviour {
	public int samples = 10;

	public float timeOffsetVelocity = 1f;
	private float currentTimeOffset;

	public float
		spinPerCast = 133.5958f,
		maxTilt = 45f;
	private float tiltPerCast;

	private Quaternion currentRot;

	private Vector3[] finishPoints = new Vector3[0];

	private bool working = true;

	private void Start() {
		OpenXRInput.wrapper.DebugKeys.Opty.performed += OnOpty;
	}

	private void Update() {
		if (!working) return;

		currentTimeOffset = (currentTimeOffset + timeOffsetVelocity * Time.deltaTime) % 360f;

		currentRot = Quaternion.AngleAxis(currentTimeOffset, transform.forward);

		Quaternion spinShift = Quaternion.AngleAxis(spinPerCast, transform.forward);
		tiltPerCast = maxTilt / samples;
		//Quaternion tiltShift = Quaternion.AngleAxis(tiltPerCast, transform.right);

		RaycastHit hit;
		finishPoints = new Vector3[samples];

		for (int i = samples - 1; i >= 0; --i) {
			if (Physics.Raycast(origin: transform.position, direction: currentRot * transform.forward, maxDistance: 1000f, layerMask: (int) FlaggedLayer.Vision, hitInfo: out hit)) {
				OptyProxy op = hit.collider.GetComponent<OptyProxy>();
				if (op) op.roomController.SetVisible(true);

				finishPoints[i] = hit.point;
			} else
				finishPoints[i] = transform.position + currentRot * transform.forward * 1000f;

			currentRot = spinShift * currentRot;
			currentRot = Quaternion.AngleAxis(tiltPerCast, currentRot * transform.right) * currentRot;
		}
	}

	private void OnDrawGizmos() {
		for (int i = finishPoints.Length - 1; i >= 0; --i)
			Gizmos.DrawLine(transform.position, finishPoints[i]);

		//currentTimeOffset = (currentTimeOffset + timeOffsetVelocity * Time.deltaTime) % 360f;

		//currentRot = Quaternion.AngleAxis(currentTimeOffset, transform.forward);

		//Quaternion spinShift = Quaternion.AngleAxis(spinVelocity, transform.forward);
		//tilt = maxTilt / samples;
		//Quaternion tiltShift = Quaternion.AngleAxis(tilt, transform.right);

		//for (int i = samples; i > 0; --i) {
		//	Gizmos.DrawLine(transform.position, transform.position + currentRot * Vector3.forward);
		//	currentRot = spinShift * currentRot;
		//	currentRot *= tiltShift;
		//}
	}

	public void OnOpty(InputAction.CallbackContext context) {
		if (context.performed)
			working = !working;
	}
}