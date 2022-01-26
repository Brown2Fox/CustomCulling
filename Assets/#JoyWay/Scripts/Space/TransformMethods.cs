using UnityEngine;

public static class TransformMethods {
    public static void CopyLocalPosRot(this Transform to, Transform from) {
        to.localPosition = from.localPosition;
        to.localRotation = from.localRotation;
    }

    public static void CopyLocalPosRot(this Transform to, PosRot from) {
        to.localPosition = from.pos;
        to.localRotation = from.rot;
    }

    public static void CopyGlobalPosRot(this Transform to, Transform from) {
        to.position = from.position;
        to.rotation = from.rotation;
    }

    public static void CopyGlobalPosRot(this Transform to, PosRot from) {
        to.position = from.pos;
        to.rotation = from.rot;
    }

    public static PosRot Global(this Transform source) {
        PosRot pr = new PosRot() {
            pos = source.position,
            rot = source.rotation
        };
        return pr;
    }

    public static void GlobalReset(this Transform to) {
        to.position = Vector3.zero;
        to.rotation = Quaternion.identity;
    }
    public static void LocalReset(this Transform to) {
        to.localPosition = Vector3.zero;
        to.localRotation = Quaternion.identity;
    }


    public static bool AlmostEquals(this Transform source, Transform target, float epsilonPos = 10e-5f, float epsilonRot = 10e-2f) {
        var s = source.position - target.position;

        var eps2 = epsilonPos * epsilonPos;

        if (s.sqrMagnitude > eps2) return false;

        if (Quaternion.Angle(source.rotation, target.rotation) > epsilonRot) return false;

        return true;
    }
    
    
    
}