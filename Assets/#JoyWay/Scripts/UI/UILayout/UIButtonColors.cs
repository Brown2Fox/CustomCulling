using System;
using UnityEngine;

[Serializable]
public struct UIButtonColorScheme {
	public UIButtonColors disabled;
	public UIButtonColors idle;
	public UIButtonColors hover;
	public UIButtonColors click;
}

[Serializable]
public struct UIButtonColors {
	[ColorUsage(true, true)]
	public Color button;
	[ColorUsage(true, true)]
	public Color text;
}