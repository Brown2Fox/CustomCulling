using UnityEngine;

// TODO: Должен сажать агента, который при уничтожении коллайдера в зоне будет сигналить об этом.
public class CustomCharacterGrounder : MonoBehaviour {
	public CharacterControllerExtention characterControllerExtention;
	private int groundsCount;
	private Label lastLabel;

	private void OnTriggerEnter(Collider _collider) {
		lastLabel = _collider.GetComponent<Label>();
		if (lastLabel && lastLabel.Check(LabelType.Walkable)) {
			++groundsCount;
			if (groundsCount == 1)
				characterControllerExtention.SetGrounded(true);
		}
	}

	private void OnTriggerExit(Collider _collider) {
		lastLabel = _collider.GetComponent<Label>();
		if (lastLabel && lastLabel.Check(LabelType.Walkable)) {
			--groundsCount;
			if (groundsCount == 0)
				characterControllerExtention.SetGrounded(false);
		}
	}

	public void ForceReset() {
		groundsCount = 0;
	}
}
