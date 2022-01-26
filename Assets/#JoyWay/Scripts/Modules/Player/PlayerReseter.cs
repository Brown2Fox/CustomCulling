using UnityEngine;

public class PlayerReseter : MonoBehaviour, IActivatable {
	public void Activate() {
		Player.instance.TotalReset();
	}
}