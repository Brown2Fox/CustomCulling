using System;
using UnityEngine;

public class PoolBehaviour : MonoBehaviour {
	public Pool pool;


	private void OnDestroy() {
		pool.Destroy(true);
	}
}