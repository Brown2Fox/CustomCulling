using UnityEngine;

public class PseudoRandom : Singleton<PseudoRandom> {
	public AnimationCurve cCurve;

	public static float GetChance(float _baseChance, int _iteration) {
		var c = instance.cCurve.Evaluate(_baseChance);
		return c * _iteration;
	}
}