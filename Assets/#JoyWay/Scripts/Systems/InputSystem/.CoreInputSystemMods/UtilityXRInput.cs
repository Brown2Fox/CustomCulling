using System.Linq;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class UtilityXRInput {
	static UtilityXRInput() {
#if UNITY_EDITOR
		// In the editor we need to make sure the OpenXR layouts get registered even if the user doesn't
		// navigate to the project settings.  The following code will register the base layouts as well
		// as any enabled interaction features.
		RegisterLayouts();

		// Find all enabled interaction features and force them to register their device layouts
		// var packageSettings = OpenXRSettings.GetPackageSettings();
		// if (null == packageSettings)
		// 	return;
		//
		// foreach (var feature in packageSettings.GetFeatures<OpenXRInteractionFeature>().Where(f => f.feature.enabled).Select(f => f.feature))
		// 	feature.OnEnabledChange();
#endif
	}

	private static void RegisterLayouts() {
		InputSystem.RegisterLayout<BoolDeviceControl>("BoolDevice");
	}
}