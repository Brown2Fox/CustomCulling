using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Ability : MonoBehaviour {
	public int AbilityID;
	[DisableEditing]
	public AbilityContainer container;

	public int stacks;


	public Owner owner => container.owner;
	public int priority => settings.priority + PriorityModifiers();
	public bool predictable => (settings.predictTime > 0f);
	public float predictTime => settings.predictTime;

	//[DisableEditing]
	public BaseAbilityAsset asset;

	public AbilitySettings settings;

	public Action onSuccessCast;
	public Action onActivate;
	public Action onDeactivate;


	public AudioAsset actionSound;
	public AudioSource source;

	public List<object> blockSources = new List<object>();

	public bool blocked => blockSources.Count > 0;

	public void Init(AbilityContainer _container) {
		container = _container;
		InitBehaviour();
		++stacks;
	}


	public void PlayLoop(AudioClip _clip) {
		if (!_clip) return;
		if (source) {
			source.Stop();
			source.loop = true;
			source.clip = _clip;
			source.Play();
		}
	}

	public void PlaySound() {
		if (!actionSound) return;
		if (source)
			source.PlayOneShot(actionSound);
	}

	public void PlaySound(AudioClip _clip) {
		if (!_clip) return;
		if (source)
			source.PlayOneShot(_clip);
	}

	protected float lastUseTime;

	public float CooldownLeft => lastUseTime + settings.cooldown - Time.time;

	protected void StopSound() {
		source.loop = false;
		source.Stop();
	}


	protected abstract void InitBehaviour();
	protected abstract void DestroyBehaviour();

	public virtual void ResetAbility() {
		stacks--;
	}

	protected virtual int PriorityModifiers() {
		return 0;
	}


	private void OnDestroy() {
		DestroyBehaviour();
	}
}


public abstract class ActiveAbility : Ability, IActiveAbility {
	public CastAudio castAudio;

	public event Action<IActiveAbility> OnCast;

	public virtual bool CastStartCheck() => this.BaseCastStartCheck();

	public abstract void StartCast();

	public virtual void Cast() => OnCast?.Invoke(this);
}


public abstract class PlayerAbility : Ability {
	public Player player;
}


[Serializable]
public struct AbilitySettings {
	public int priority;
	public float predictTime;
	public float castTime;
	public float cooldown;
	public float castRange;
	public AbilityType type;
}