using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Pool {
	public static Dictionary<Object, Pool> pools = new Dictionary<Object, Pool>();
	public static GameObject recyclablePoolParent, persistentPoolParent;

	public Queue<PoolObject> objects;
	[DisableEditing]
	public Transform parent;
	[DisableEditing]
	public PoolObject prefab;

	private bool persistent = false;

	private Pool(PoolObject _poolObject, int _count) {
		prefab = _poolObject;

		if (!recyclablePoolParent)
			recyclablePoolParent = new GameObject($"RecyclablePoolsParent");

		parent = new GameObject($"{_poolObject.name}_pool").transform;
		parent.parent = recyclablePoolParent.transform;
		var beh = parent.gameObject.AddComponent<PoolBehaviour>();
		beh.pool = this;

		pools.Add(prefab, this);

		objects = new Queue<PoolObject>(_count);

		Add(_count);
	}

	public void SetPersistent() {
		if (!persistentPoolParent) {
			persistentPoolParent = new GameObject($"PersistentPoolsParent");
			Object.DontDestroyOnLoad(persistentPoolParent);
		}

		parent.parent = persistentPoolParent.transform;
		persistent = true;
	}

	public void Add(int _count) {
		for (int i = 0; i < _count; i++) {
			CreateNew(prefab, parent);
		}
	}

	public PoolObject CreateNew(PoolObject _obj, Transform _parent) {
		var po = Object.Instantiate(_obj, _parent.transform);
		po.pool = this;
		po.gameObject.SetActive(false);
		return po;
	}


	public static Pool GetOrCreate(PoolObject _poolObject) {
		Pool _pool;
			if (!pools.TryGetValue(_poolObject, out _pool)) {
			_pool = new Pool(_poolObject, 5);
		} else {
			if (_pool.parent == null) {
				pools.Remove(_poolObject);
				return new Pool(_poolObject, 5);
			}
		}


		return _pool;
	}


	public static Pool AddToPool(PoolObject _poolObject, int _count) {
		Pool _pool;
		if (pools.TryGetValue(_poolObject, out _pool)) {
			if (_pool.parent == null) {
				pools.Remove(_poolObject);
				return new Pool(_poolObject, _count);
			}

			_pool.Add(_count);
		} else {
			_pool = new Pool(_poolObject, _count);
		}

		return _pool;
	}

	public static T Pop<T>(PoolObject _poolObject) where T : Component {
		Pool _pool;
		if (!pools.TryGetValue(_poolObject, out _pool)) {
			_pool = GetOrCreate(_poolObject);
		}

		return _pool.Pop<T>();
	}


	public PoolObject Pop() {
		if (objects.Count == 0)
			CreateNew(prefab, parent);

		if (objects.Count > 0)
			return objects.Dequeue();
		else
			return null;
	}

	public T Pop<T>() where T : Component {
		if (objects.Count == 0)
			CreateNew(prefab, parent);

		if (objects.Count > 0)
			return objects.Dequeue().GetComponent<T>();
		else
			return null;
	}

	public void Return(PoolObject _po) {
		if (objects != null)
			objects.Enqueue(_po);
	}


	public void Remove(PoolObject poolObject) {
		//objects.Remove(poolObject);
		if (objects != null && objects.Count <= 0)
			Destroy();
	}

	public void Destroy(bool _forced = false) {
		if (persistent && !_forced) return;
		pools.Remove(prefab);
		// if (Application.isPlaying)
		if (parent != null)
			Object.Destroy(parent.gameObject);
		// else 
		// 	Object.DestroyImmediate(parent);
	}

	public static void Clear() {
		Dictionary<Object, Pool>.KeyCollection keys = pools.Keys;
		Object[] keysArray = new Object[keys.Count];
		int i = 0;
		foreach (Object key in keys)
			keysArray[i++] = key;
		for (i = keysArray.Length - 1; i >= 0; --i) {
			pools[keysArray[i]].Destroy();
		}
	}

	public bool IsNull() {
		return !parent && !prefab;
	}
}