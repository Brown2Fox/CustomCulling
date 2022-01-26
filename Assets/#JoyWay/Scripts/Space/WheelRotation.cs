using UnityEngine;
using UnityEngine.Events;

public class WheelRotation : MonoBehaviour
{
    public Transform currentTarget;
    private float takeAngle;
    private float newAngle;
    public float curAngle { private set; get; }

    public UnityEvent
        hasTaken, hasReleased;

    public void SetTarget(Transform _target) {
        currentTarget = _target;
        takeAngle = Vector3.SignedAngle(Vector3.ProjectOnPlane(currentTarget.position - transform.position, transform.right), transform.forward, transform.right);
    }

    public void SetRealesed(Transform _released) {
        currentTarget = null;
    }

    private void Update() {
        if (currentTarget == null) return;
        newAngle = Vector3.SignedAngle(Vector3.ProjectOnPlane(currentTarget.position - transform.position, transform.right), transform.forward, transform.right);
        curAngle += takeAngle - newAngle;
        transform.localRotation = Quaternion.Euler(curAngle, 0f, 0f);
    }
}
