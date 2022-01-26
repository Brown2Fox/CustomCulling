using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class Container : MonoBehaviour {
	public GameObject container;
	public Owner owner;
}

public class Container<T> : Container {
	public Dictionary<int, T> entries = new Dictionary<int, T>();
	public Action<T> onEntryAdd;


	public void Init() {
		entries = new Dictionary<int, T>();
		T[] newEntries = container.GetComponents<T>();
		foreach (T t in newEntries) {
			PreInitEntry(t);
			AddEntry(t, null);
		}
	}

	public virtual void PreInitEntry(T _entry) { }

	public virtual int GetID(T _entry) {
		return 0;
	}


	public virtual void AddEntry(T _entry, Entry _asset) {
		T current;
		int id = GetID(_entry);
		if (entries.TryGetValue(id, out current)) {
			//Update
		} else entries.Add(id, _entry);
	}

	public virtual void SetUpEntry(T _entry) { }


	public List<S> GetEntriesOfType<S>() {
		List<S> list = new List<S>();
		foreach (KeyValuePair<int, T> entry in entries) {
			if (entry.Value is S s) {
				list.Add(s);
			}
		}

		return list;
	}

	public S GetEntryOfType<S>() where S : T {
		foreach (KeyValuePair<int, T> entry in entries) {
			if (entry.Value is S s) {
				return s;
			}
		}

		return default;
	}

	public bool TryGetEntry<S>(out S result) where S : T {
		foreach (KeyValuePair<int, T> entry in entries) {
			if (entry.Value is S s) {
				result = s;
				return true;
			}
		}

		result = default;
		return false;
	}
}