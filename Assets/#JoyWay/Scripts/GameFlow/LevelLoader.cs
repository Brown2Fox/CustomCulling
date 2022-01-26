using UnityEngine;

public class LevelLoader : MonoBehaviour, IActivatable {
	public ScenesList sceneToLoad;

	public void Activate() {
		LevelManager.Load(sceneToLoad);
	}
}