using System.Collections;
using UnityEngine;

public class DissolveEffectController : MonoBehaviour, IStatable {
	public Renderer[] renderers;
	private int alphaPropId;

	public GameObject explosionEffect;

	public ParticleSystem[] particles;
	public AnimationCurve emmisionRate;
	public float curveMultiplier = 10f;
	public float effectTime = 3f;


	private void Awake() {
		alphaPropId = Shader.PropertyToID("_dissolve");
	}

	public void ExplosionEffect() {
		if (explosionEffect)
			explosionEffect.SetActive(true);
	}

	public void ResetEffect() {	
		SetState(0f);
		if (explosionEffect)
			explosionEffect.SetActive(false);
	}

	public void StartEffect() {
		StartCoroutine(EffectProcess());
	}

	private IEnumerator EffectProcess() {
		float
			timeLeft = 0f,
			divAsMult = 1f / effectTime;

		while (timeLeft < effectTime) {
			SetState(timeLeft * divAsMult);
			timeLeft += Time.deltaTime;
			yield return null;
		}
		SetState(1f);
	}

	public void SetState(float _state) {
		for (int i = renderers.Length - 1; i >= 0; --i)
			renderers[i].material.SetFloat(alphaPropId, _state);

		foreach (ParticleSystem ps in particles) {
			ParticleSystem.EmissionModule emission = ps.emission;
			emission.rateOverTimeMultiplier = emmisionRate.Evaluate(_state) * curveMultiplier;

			if (emission.rateOverTimeMultiplier > float.Epsilon) {
				if (!ps.isPlaying) ps.Play();
			} else {
				if (ps.isPlaying) ps.Stop();
			}
		}
	}
}
