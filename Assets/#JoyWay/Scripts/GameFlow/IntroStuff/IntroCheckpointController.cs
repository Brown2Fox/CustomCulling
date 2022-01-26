using UnityEngine;

namespace Scripts.GameFlow.IntroStuff {
	public class IntroCheckpointController : MonoBehaviour {
		public LevelLoader loader;


		private void Awake() {
			switch (GameMain.settings.serializable.introCHK) {
				case 1:
					loader.sceneToLoad = ScenesList.FakeHub2;
					loader.Activate();
					break;
				case 2:
					// no checkpoint yet
					break;
			}
		}
	}
}