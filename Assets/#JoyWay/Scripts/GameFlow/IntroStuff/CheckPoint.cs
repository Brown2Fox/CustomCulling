using UnityEngine;

namespace Scripts.GameFlow.IntroStuff {
	public class CheckPoint : MonoBehaviour, IActivatable {
		public int num;

		public void Activate() {
			GameMain.settings.serializable.introCHK = num;
		}
	}
}