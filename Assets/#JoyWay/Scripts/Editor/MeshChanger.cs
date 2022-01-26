using UnityEngine;
                               //TODO для чего?
public class MeshChanger : MonoBehaviour
{
    public SkinnedMeshRenderer mainMesh;

    private void Awake()
    {
        mainMesh.enabled = false;
        SkinnedMeshRenderer rend = GetComponent<SkinnedMeshRenderer>();
        rend.rootBone = mainMesh.rootBone;
        rend.bones = mainMesh.bones;
    }
}
