using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.Newtonsoft.Json.Utilities;

public class Owner : MonoBehaviour {
	public static Owner Wolrd;

#if UNITY_EDITOR
	public bool update;
#endif

	public Team team;

	/// <summary>
	///  Главный овнер, можно только читать!
	/// </summary>
	[DisableEditing]
	public Owner owner;

	public Owner grandOwner => owner ? owner.grandOwner : this;

	public List<Owner> owners = new List<Owner>();


	//[HideInInspector]
	public List<Owner> childies = new List<Owner>();


	[DisableEditing]
	public Health health;

	[DisableEditing]
	public AbilityContainer aContainer;

	[DisableEditing]
	public AudioSource defaultASource;

	[DisableEditing]
	public Transform movable;
	public Transform mainPoint;

	[DisableEditing]
	public Rigidbody rigidBody;

	public Action<Owner> OnChildAdded;
	public Action<Owner> OnChildRemoved;

	public List<IShowingStat> showingStats = new List<IShowingStat>();

	private void Awake() {
		Init();
	}


	public bool inited { get; private set; }
	public float removeDelay;
	private WaitForSeconds removeDelayWFS;
	private Coroutine removeCoroutine;

	public void Init() {
		if (inited) return;
		if (health) health.Reset();
		if (aContainer) aContainer.Init();
		foreach (var childy in childies) {
			childy.Init();
		}

		removeDelayWFS = new WaitForSeconds(removeDelay);

		inited = true;
	}

	public void ResetOwner() {
		if (health) health.Reset();
		if (aContainer) aContainer.ResetAbilities();
	}


	public void SetOwner(Owner _owner) {
		TryStopRemoveOwner(_owner);
		owner = _owner;
		owners.Add(_owner);
		owner.childies.AddDistinct(this);
		owner.OnChildAdded?.Invoke(this);
	}

	public void StartRemoveOwner() {
		TryStopRemoveOwner(owner);
		if (gameObject.activeInHierarchy)
			removeCoroutine = StartCoroutine(RemoveProcess());
		else {
			removeCoroutine = null;
			RemoveOwner();
		}
	}

	private void TryStopRemoveOwner(Owner _owner) {
		// Debug.Log("interrupt remove Owner", this);
		if (removeCoroutine != null) StopCoroutine(removeCoroutine);
		removeCoroutine = null;
	}

	public IEnumerator RemoveProcess() {
		yield return removeDelayWFS;

		removeCoroutine = null;
		RemoveOwner();
	}

	public void RemoveOwner() {
		if (owner == null) return;
		owner.childies.Remove(this);
		owner.OnChildRemoved?.Invoke(this);
		owners.Remove(owner);
		owner = (owners.Count > 0)? owners[0] : null; 
	}

	public void ClearOwners() {

		if (owners.Count < 1) return;

		for (int i =  0; i < owners.Count; i ++) {
			RemoveOwner();
		}
		
	}

	public void Warp(PosRot point) {
		movable.CopyGlobalPosRot(point);
		var rb = movable.GetComponent<Rigidbody>();
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		onWarp?.Invoke();
	}

	public Action onWarp;


	private void OnDisable() {
		if (owner) ClearOwners();
		inited = false;


		ResetOwner();
	}


#if UNITY_EDITOR
	private void OnValidate() {
		update = false;


		if (!health)
			health = GetComponent<Health>();
		if (!aContainer)
			aContainer = GetComponent<AbilityContainer>();
		if (!rigidBody)
			rigidBody = GetComponent<Rigidbody>();
		if (!mainPoint)
			mainPoint = transform;
		if (defaultASource == null) defaultASource = GetComponentInChildren<AudioSource>();

		childies.Clear();

		Queue<Transform> q = new Queue<Transform>();
		q.Enqueue(transform);
		Transform t;
		while (q.Count > 0) {
			t = q.Dequeue();
			foreach (Transform child in t) {
				q.Enqueue(child);
			}


			Owner ch = t.GetComponent<Owner>();
			if (ch && ch != this) ch.SetOwner(this);
		}


		if (aContainer) aContainer.owner = this;
		if (health) health.owner = this;
	}

#endif
}

[Flags]
public enum Team {
	Default = 0,
	Player = 1 << 0,
	_2 = 1 << 1,
	_3 = 1 << 2,
	_4 = 1 << 3,
	_5 = 1 << 4,
	_6 = 1 << 5,
	World = 1 << 6,
	_8 = 1 << 7,
	_9 = 1 << 8,
	_10 = 1 << 9,
	_11 = 1 << 10,
	_12 = 1 << 11,
	NeutralObjects = 1 << 12,
	Neutrals = 1 << 13,
	EnemyBot = 1 << 14,


	CountAsWall = NeutralObjects | World,
	CountAsBots = Player | Neutrals | EnemyBot,
}

[Serializable]
public struct CommonStats {
	public static CommonStats BaseStats() {
		CommonStats stats;
		stats.armor = 0;
		stats.evasion = 0;
		stats.critChance = 0;
		stats.critDamage = 0;
		stats.damageInMultiplier = 1;
		stats.damageOutMultiplier = 1;
		stats.statusResistance = 1;
		return stats;
	}


	public float armor;
	public float evasion;
	public float critChance;
	public float critDamage;
	public float damageInMultiplier;
	public float damageOutMultiplier;
	public float statusResistance;
}