using System;
using System.Text;
using UnityEngine;
using UnityEngine.XR;
#if !UNITY_ANDROID
using Valve.VR;

#endif

public enum HMDType {
	OculusRift,
	OculusRiftS,
	Vive,
	ViveCosmos,
	ValveIndex,
	WMR
};


public enum ControllerType {
	None,
	ViveController,
	ViveCosmosController,
	OculusController,
	Knuckles, //index
	WMRController
}


public class HMDVarietyHandler : Singleton<HMDVarietyHandler> {
	public static HMDType hmd = HMDType.OculusRift;
	public static ControllerType right = ControllerType.None;
	public static ControllerType left = ControllerType.None;

	[DisableEditing]
	public string detectedHMD = "";
	[DisableEditing]
	public ControllerType leftC;
	[DisableEditing]
	public ControllerType rightC;

	private bool tryFindControllers;

	protected override void Awake() {
		base.Awake();
		XRDevice.deviceLoaded += SetDevice;
	}

	private void SetDevice(string model) {
		detectedHMD = model.ToLower();


		Debug.Log(detectedHMD);

		if (detectedHMD.Contains("vive")) {
			if (detectedHMD.Contains("cosmos"))
				hmd = HMDType.ViveCosmos;
			else
				hmd = HMDType.Vive;
		} else if (detectedHMD.Contains("oculus")) {
			if (XRDevice.refreshRate <= 80)
				hmd = HMDType.OculusRiftS;
			else
				hmd = HMDType.OculusRift;
		} else if (detectedHMD.Contains("valve") || detectedHMD.Contains("index")) {
			hmd = HMDType.ValveIndex;
		} else {
			hmd = HMDType.WMR;
		}

		SetControllers();
	}

	public void SetControllers() {
		right = ControllerType.OculusController;
		left = ControllerType.OculusController;
		OnRightControllerDefined?.Invoke();
		OnLeftControllerDefined?.Invoke();

#if !UNITY_ANDROID
		tryFindControllers = true;
#endif
	}

	private bool rightControllerDefined;
	private bool leftControllerDefined;
	public event Action OnRightControllerDefined;
	public event Action OnLeftControllerDefined;
#if !UNITY_ANDROID

	private void Update() {
		if (!tryFindControllers) return;
		if (!leftControllerDefined)
			leftControllerDefined = TryGetControllerType(HandType.Left);
		if (!rightControllerDefined)
			rightControllerDefined = TryGetControllerType(HandType.Right);
	}

	public bool TryGetControllerType(HandType hand) {
		ControllerType ct = GetControllerType(hand);
		if (ct == ControllerType.None)
			return false;
		else {
			Debug.Log($"{hand} controller is {ct}");
			if (hand == HandType.Left) {
				left = ct;
				OnLeftControllerDefined?.Invoke();
				leftC = left;
			} else {
				right = ct;
				OnRightControllerDefined?.Invoke();
				rightC = right;
			}

			return true;
		}
	}

	public ControllerType GetControllerType(HandType hand) {
		var system = OpenVR.System;
		var id = system.GetTrackedDeviceIndexForControllerRole((ETrackedControllerRole) hand);

		ETrackedPropertyError error = ETrackedPropertyError.TrackedProp_Success;
		StringBuilder sbuilder = new StringBuilder(64);

		system.GetStringTrackedDeviceProperty(id, ETrackedDeviceProperty.Prop_RenderModelName_String, sbuilder, 64u, ref error);
		if (sbuilder.ToString() == "")
			system.GetStringTrackedDeviceProperty(id, ETrackedDeviceProperty.Prop_ManufacturerName_String, sbuilder, 64u, ref error);
		string result = sbuilder.ToString();


		//Debug.Log(result);


		if (result.Contains("HTC"))
			return ControllerType.ViveCosmosController;
		if (result.Contains("vive"))
			return ControllerType.ViveController;
		if (result.Contains("oculus"))
			return ControllerType.OculusController;
		if (result.Contains("valve"))
			return ControllerType.Knuckles;
		if (result.Contains("wmr") || result.Contains("Windows"))
			return ControllerType.WMRController;


		return ControllerType.None;
	}

#endif
}