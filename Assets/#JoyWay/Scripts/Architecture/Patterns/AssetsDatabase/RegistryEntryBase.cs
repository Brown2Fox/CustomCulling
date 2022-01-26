using UnityEngine;

public class RegistryEntryBase : ScriptableObject {
	[DisableEditing]
	public int entryID;
}


public class Entry : RegistryEntryBase { }