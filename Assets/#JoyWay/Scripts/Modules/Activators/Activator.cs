using UnityEngine;

public class Activator : MonoBehaviour, IActivatable {
	[ObjectFieldFilter(typeof(IActivatable))]
	public Component target;

	public void Activate() {
		(target as IActivatable).Activate();
	}
}