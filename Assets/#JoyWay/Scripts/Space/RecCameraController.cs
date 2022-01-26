using JoyWay.Systems.InputSystem;
using UnityEngine;
using InputAction = UnityEngine.InputSystem.InputAction;

public class RecCameraController : MonoBehaviour, InputWrapper.ICaptureCameraActions {
	public Transform cameraTarget;
	private Camera theCamera;

	public float
		positionLerper = 1f,
		rotationLerper = 1f;

	private void Start() {
		theCamera = GetComponent<Camera>();
		theCamera.cullingMask = cameraTarget.GetComponentInParent<Camera>().cullingMask;
		OpenXRInput.wrapper.CaptureCamera.SetCallbacks(this);
	}


	private void LateUpdate() {
		transform.position = Vector3.Lerp(transform.position, cameraTarget.position, positionLerper * Time.deltaTime);
		transform.rotation = Quaternion.Lerp(transform.rotation, cameraTarget.rotation, rotationLerper * Time.deltaTime);
	}

	public void OnBeautify(InputAction.CallbackContext context) {
		if (context.performed) {
			theCamera.enabled = true;
		}
	}

	public void OnUnbeautify(InputAction.CallbackContext context) {
		if (context.performed) {
			theCamera.enabled = false;
		}
	}
}