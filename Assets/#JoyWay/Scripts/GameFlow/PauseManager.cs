using System;
using JoyWay.Systems.InputSystem;
using UnityEngine;

public class PauseManager : Singleton<PauseManager> {
	public bool state;
	public PosRot offset;

	private Transform camRotate;

	public Action<bool> onPause;

	private void Start() {
		camRotate = Player.instance.camRotate;
		gameObject.SetActive(false);
	}


	public void TogglePause(bool _b) {
		if (!_b) return;
		if (!state) Pause();
		else UnPause();
	}

	public void SetUp() {
		transform.CopyGlobalPosRot(camRotate.Global().GetGlobalOfChild(offset));
	}


	public void Pause() {
		if (DeveloperSettings_NoPause())
			return;
		SetUp();
		state = true;
		Time.timeScale = 0;
		gameObject.SetActive(true);
		onPause?.Invoke(true);
	}


	public void UnPause() {
		if (DeveloperSettings_NoPause())
			return;
		state = false;
		Time.timeScale = 1;
		gameObject.SetActive(false);
		onPause?.Invoke(false);
	}

	private bool DeveloperSettings_NoPause() {
		return GameMain.instance.developerSettingsAsset.noPause;
	}
}