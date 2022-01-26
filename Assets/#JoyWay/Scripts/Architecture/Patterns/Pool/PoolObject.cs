using UnityEngine;

public class PoolObject : MonoBehaviour {
	public Pool pool;

	private void OnDisable() {
		if (pool == null || pool.IsNull()) {
			Destroy(gameObject);
			return;
		}
		pool.Return(this);
		Invoke(nameof(ReParent), 0f);
	}


	public void ReParent() {
		if (gameObject.activeInHierarchy)
			return;
		gameObject.SetActive(false);
		transform.parent = pool.parent;
	}

	private void OnDestroy() {
		if (pool != null)
			pool.Remove(this);
	}
}