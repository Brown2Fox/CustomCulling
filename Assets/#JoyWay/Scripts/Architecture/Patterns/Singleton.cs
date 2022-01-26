using UnityEngine;

public class Singleton : MonoBehaviour { }

public class Singleton<T> : Singleton where T : Singleton {
	public static T instance;

	protected virtual void Awake() {
		instance = this as T;

		if (transform.parent == null)
			DontDestroyOnLoad(this);
	}
}
