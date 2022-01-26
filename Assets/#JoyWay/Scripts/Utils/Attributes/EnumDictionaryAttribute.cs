using System;
using UnityEngine;

/// <summary>
/// Mark a method with an integer argument with this to display the argument as an enum popup in the UnityEvent
/// drawer. Use: [EnumAction(typeof(SomeEnumType))]
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class EnumDictionaryAttribute : PropertyAttribute {
	public Type enumType;
	public string[] names;
	public int[] values;

	public EnumDictionaryAttribute(Type enumType) {
		this.enumType = enumType;
		names = Enum.GetNames(enumType);
		values = new int[names.Length];
		int i = 0;
		foreach (var value in Enum.GetValues(enumType)) {
			values[i] = (int) value;
			i++;
		}
	}
}