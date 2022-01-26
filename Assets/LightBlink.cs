using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlink : MonoBehaviour
{
    //Light light;
    public AnimationCurve curve;
    public float anim_time = 0.5f, target_intensity = 2f, z_move_speed = 0f;
    float current_time;
    void OnEnable()
    {
        GetComponent<Light>().enabled = true;
        current_time = 0;
        transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (current_time <= anim_time)
        {
            GetComponent<Light>().intensity = curve.Evaluate(current_time / anim_time) * target_intensity;
            transform.position += new Vector3(0, 0, z_move_speed * Time.deltaTime);
            current_time += Time.deltaTime;
        }

        else
        {
            GetComponent<Light>().enabled = false;
        }

        
        
    }
}
