using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioAsset", menuName = "JoyWay/Audio/AudioAsset")]
public class AudioAsset : ScriptableObject {
	public List<AudioClip> clips;

	public AudioClip GetRandom() {
		if (clips == null || clips.Count == 0) return null;
		int r = Random.Range(0, clips.Count);
		return clips[r];
	}

	public static implicit operator AudioClip(AudioAsset asset) => asset.GetRandom();
}