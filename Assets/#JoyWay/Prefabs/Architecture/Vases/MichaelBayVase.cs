using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MichaelBayVase : MonoBehaviour
{

    void OnEnable() 
    {
        transform.rotation = Quaternion.Euler(0,Random.Range(0,360f),0);
    }

}
