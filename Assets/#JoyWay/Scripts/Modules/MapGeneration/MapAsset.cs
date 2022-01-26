using UnityEngine;

[CreateAssetMenu(fileName = "Map asset", menuName = "JoyWay/MapGeneration/Map Asset")]
public class MapAsset : ScriptableObject {
	public char[, ] mapTable;
	public string[] serrialized;
}
