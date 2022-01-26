using System.Runtime.CompilerServices;
using UnityEngine;

public static class AbilityCastCheckUtils {
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool CooldownCheck(this Ability _ability) {
		return _ability.CooldownLeft < 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool BaseCastStartCheck(this Ability _ability) {
		if (_ability.blocked) return false;
		if (!_ability.CooldownCheck()) return false;
		return true;
	}
}