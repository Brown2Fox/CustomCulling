// using System.Runtime.CompilerServices;
//
// public static class AbilityInterfaceStaticMethods {
// 	[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 	public static bool DefaultCastCheck<ActiveAbility>(this ActiveAbility _ability) where ActiveAbility : BotAbility, IActiveAbility {
// 		return _ability.CooldownCheck() && _ability.RangeCheck();
// 	}
//
// 	[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 	public static bool CooldownCheck<ActiveAbility>(this ActiveAbility _ability) where ActiveAbility : BotAbility, IActiveAbility {
// 		return _ability.CooldownLeft < 0;
// 	}
//
// 	[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 	public static bool RangeCheck(this BotAbility _ability) {
// 		return _ability.settings.castRange * _ability.settings.castRange > (_ability.owner.mainPoint.position - _ability.bot.target.mainPoint.position).sqrMagnitude;
// 	}
// }