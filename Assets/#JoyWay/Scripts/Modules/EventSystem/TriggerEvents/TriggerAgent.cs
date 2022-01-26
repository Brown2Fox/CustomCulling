using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAgent : MonoBehaviour {
	private List<Tuple<TriggerPool, Collider>> watchers = new List<Tuple<TriggerPool, Collider>>();
	public int d_count = 0;

	public void AddWatcher(TriggerPool _watcher, Collider _collider) {
		++d_count;
		watchers.Add(new Tuple<TriggerPool, Collider> (_watcher, _collider));
	}

	public void RemoveWatcher(TriggerPool _watcher, Collider _collider) {
		--d_count;
		watchers.Remove(new Tuple<TriggerPool, Collider>(_watcher, _collider));
	}

	private void OnDisable() {
		for (int i = watchers.Count - 1; i >= 0; --i)
			watchers[i].Item1.OnColliderDisabled(watchers[i].Item2);
		watchers.Clear();
	}
}
