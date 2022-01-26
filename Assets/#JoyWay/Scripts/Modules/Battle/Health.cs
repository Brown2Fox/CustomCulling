using System;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Health))]
public class HealthEditor : Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		if (Application.isPlaying) {
			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Kill"))
				(target as Health).Death();
			if (GUILayout.Button("Reset"))
				(target as Health).Reset();

			GUILayout.EndHorizontal();
		}
	}
}

#endif


public class Health : MonoBehaviour, IProgressBarUI {
	public Owner owner;

	public float
		curHealth = 100f,
		baseMaxHealth = 100f,
		additionalHealth;

	public UnityEvent onDeath;
	public Action onHeal, onHealthChange, onDamage;

	private DamagableStatus damagableStatus;
	public AudioSource audioSource;
	public AudioAsset deathSound;

	public void Death() {
		damagableStatus = DamagableStatus.Dead;
		audioSource.Stop();
		audioSource.PlayOneShot(deathSound);
		//Debug.Log($"#Death sound", gameObject);
		onDeath.Invoke();
	}

	public void Heal(float amount) {
		curHealth = Mathf.Clamp(curHealth + amount, curHealth, GetMaxValue());
		onHeal?.Invoke();
		onHealthChange?.Invoke();
	}

	public void Reset() {
		curHealth = GetMaxValue();
		damagableStatus = DamagableStatus.Ok;
		onHeal?.Invoke();
		onHealthChange?.Invoke();
	}

	public float GetMaxValue() => baseMaxHealth + additionalHealth;

	public float GetCurrentValue() {
		return curHealth;
	}

}

public enum DamagableStatus {
	Ok,
	Damaged,
	Dead,
}
