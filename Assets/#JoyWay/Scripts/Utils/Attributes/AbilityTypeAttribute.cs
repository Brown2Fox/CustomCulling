using System;

public class AbilityTypeAttribute : Attribute {
	public AbilityType type;

	public AbilityTypeAttribute(AbilityType _type) {
		type = _type;
	}
}