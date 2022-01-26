public class TelekinesisAudio : AudioController {
	public AudioAsset init;
	public AudioAsset pull;
	public AudioAsset inTelekinesis;
	public AudioAsset push;

	public virtual void Start() {
		PlayOnce(init, 1f);
	}

	public void PullSound() {
		PlayOnce(pull, volume);
	}

	public void PushSound() {
		PlayOnce(push, volume);
	}

	public void InTelekinesisSound() {
		PlayLoop(inTelekinesis, volume);
	}
}
