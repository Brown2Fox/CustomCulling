using System;
using System.Collections.Generic;

public class DictionaryTypeEqulityComparer : IEqualityComparer<Type> {
	public bool Equals(Type x, Type y) {
		if (x == y) return true;
		return y.IsSubclassOf(x);
	}

	public int GetHashCode(Type obj) {
		return obj.GetHashCode();
	}
}