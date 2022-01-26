using System;
using UnityEngine;

public class FakeDungeonDeathChanger : MonoBehaviour {
	private void Start() {
		Player.instance.onDeath = () => {
			LevelManager.Load(ScenesList.FakeHub2);
			ReturnToNormalDeath();
		};
	}


	public void ReturnToNormalDeath() {
		Player.instance.onDeath = LevelManager.LoadDeathScene;
	}
}