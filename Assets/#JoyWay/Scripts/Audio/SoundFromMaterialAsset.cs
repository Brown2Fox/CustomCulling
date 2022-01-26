using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundFromMaterialAsset", menuName = "JoyWay/Audio/SoundAndMaterial")]
public class SoundFromMaterialAsset : ScriptableObject
{
    public List<SoundAndMaterials> soundAndMaterials;

    [System.Serializable]
    public struct SoundAndMaterials {
        public AudioAsset soundAsset;
        public List<Material> materials;
    }


    public AudioClip GetRandomSound(Material targetMaterial) {
        foreach (SoundAndMaterials one in soundAndMaterials) {
            if (one.materials.IndexOf(targetMaterial) != -1) return one.soundAsset;
		}
        
        return null;
    }
}




