using System;
using UnityEngine;

public class BackgroundAudioController : Singleton<BackgroundAudioController> {
	public enum States {
		Common,
		Battle
	}

	public AudioAsset commonMusic;
	public AudioAsset battleMusic;

	public AudioSource source;


	private void Start() {
		SwitchTo(States.Common);
		Play();
	}


	public void SwitchTo(States _state) {
		switch (_state) {
			case States.Common:
				source.clip = commonMusic;
				break;
			case States.Battle:

				source.clip = battleMusic;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(_state), _state, null);
		}
	}

	public void Play() {
		source.Play();
	}

	public void Stop() {
		source.Stop();
	}
}