using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIController : MonoBehaviour {
	public SettingsUIPage gameplay, audio, video;
	private SettingsUIPage current;


	private void OnEnable() {
		current = gameplay;
		SwitchToGameplay();
		//audio.SetDisabled();
		//video.SetDisabled();
	}


	public void SwitchToGameplay() {
		current.Hide();
		current = gameplay;
		current.Show();
	}

	public void SwitchToAudio() {
		current.Hide();
		current = audio;
		current.Show();
	}

	public void SwitchToVideo() {
		current.Hide();
		current = video;
		current.Show();
	}


	public void Toggle() {
		if (gameObject.activeSelf) {
			gameObject.SetActive(false);
		} else {
			gameObject.SetActive(true);
		}
	}

	private void OnDisable() {
		current.Hide();
		current = null;
	}


	public void ResetSave() {
		GameMain.settings.SetDefaultSave();
		GameMain.settings.SaveProgress();
	}
}


public abstract class SettingsUIPage : MonoBehaviour {
	public UIButton button;
	public abstract void Show();
	public abstract void Hide();


	public void SetDisabled() {
		button.SetDisabled(true);
		gameObject.SetActive(false);
	}
}