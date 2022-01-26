using UnityEngine;

public class DeadTime: MonoBehaviour
{
  public float deadTime = 1.5f;
  public bool destroyRoot;
  void Awake()
  {
    Destroy(!destroyRoot ? gameObject : transform.root.gameObject, deadTime);
  }
}
