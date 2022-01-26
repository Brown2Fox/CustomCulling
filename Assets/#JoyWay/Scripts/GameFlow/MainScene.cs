using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour {
	private void Awake() {
		Invoke(nameof(SetActiveScene), 0.1f);
	}

	private void SetActiveScene() {
		SceneManager.SetActiveScene(gameObject.scene);
	}
}