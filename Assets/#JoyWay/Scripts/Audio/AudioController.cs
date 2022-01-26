using UnityEngine;

public class AudioController : MonoBehaviour {
	public AudioSource audioSource;
	public float volume = 1f;
	public virtual void PlayOnce(AudioAsset audioAsset, float volume = 1f) {
		if (audioSource && audioAsset) {
			audioSource.PlayOneShot(audioAsset);
		}
	}

	public virtual void PlayLoop(AudioAsset audioAsset, float volume = 1f) {
		if (audioSource && audioAsset) {
			audioSource.clip = audioAsset;
			audioSource.loop = true;
			audioSource.Play();
		}
	}
	public virtual void StopPlay() {
		if (audioSource && audioSource.isPlaying)
			audioSource.Stop();
	}
	public virtual void Reset() {
		if (audioSource)
			audioSource.loop = false;
	}
}
