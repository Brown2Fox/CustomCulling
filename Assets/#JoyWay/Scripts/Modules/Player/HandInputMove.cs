using JoyWay.Systems.InputSystem;
using UnityEngine;

public class HandInputMove : MonoBehaviour {
    public HandType type;
    void Update() {
        transform.localPosition = InputSystem.instance.GetHandPosition(type);
        transform.localRotation = InputSystem.instance.GetHandRotation(type);
    }
}
