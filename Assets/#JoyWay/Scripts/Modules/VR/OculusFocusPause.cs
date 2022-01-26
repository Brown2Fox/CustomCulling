﻿using UnityEngine;

public class OculusFocusPause : MonoBehaviour {
    private bool inputFocus = true, vrFocus = false, hmd = false;
    public static bool active;

    void Start() {
        Subscribe();
    }

    private void Subscribe() {
        OVRManager.InputFocusAcquired += () => {
            inputFocus = true;
            Debug.Log($"InputFocusAcquired - {active} - {inputFocus} - {vrFocus} - {hmd}");
            UpdateStatus();
        };
        OVRManager.InputFocusLost += () => {
            inputFocus = false;
            Debug.Log($"InputFocusLost - {active} - {inputFocus} - {vrFocus} - {hmd}");
            UpdateStatus();
        };
        OVRManager.VrFocusAcquired += () => {
            vrFocus = true;
            Debug.Log($"VrFocusAcquired - {active} - {inputFocus} - {vrFocus} - {hmd}");
            UpdateStatus();
        };
        OVRManager.VrFocusLost += () => {
            vrFocus = false;
            Debug.Log($"VrFocusLost - {active} - {inputFocus} - {vrFocus} - {hmd}");
            UpdateStatus();
        };
        OVRManager.HMDMounted += () => {
            hmd = true;
            Debug.Log($"HMDMounted - {active} - {inputFocus} - {vrFocus} - {hmd}");
            UpdateStatus();
        };
        OVRManager.HMDUnmounted += () => {
            hmd = false;
            Debug.Log($"HMDUnmounted - {active} - {inputFocus} - {vrFocus} - {hmd}");
            UpdateStatus();
        };
    }

    private void UpdateStatus() {
//        if (inputFocus && vrFocus && hmd && active) {
//            if (LevelManager.currentLevel == LevelManager.instance.mainRegistry.mainMenu)
//                TimeScalePause.UnPause();
//            active = false;
//
//            OculusDashboardSets.instance.ShowRends();
//        }
//
//        if ((!inputFocus || !vrFocus || !hmd) && !active) {
//            MenuController.instance.Pause();
//
//            if (LevelManager.currentLevel == LevelManager.instance.mainRegistry.mainMenu)
//                TimeScalePause.Pause();
//
//            active = true;
//
//            OculusDashboardSets.instance.HideRends();
//        }
    }
}