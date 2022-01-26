using Unity.Mathematics;
using UnityEngine;

public class GeometryUtils {
	// Calculate the closest point of approach for line-segment vs line-segment.
	public static bool SegmentSegmentCPA(out float3 c0, out float3 c1, float3 p0, float3 p1, float3 q0, float3 q1) {
		float3 u = p1 - p0;
		float3 v = q1 - q0;
		float3 w0 = p0 - q0;

		float a = math.dot(u, u);
		float b = math.dot(u, v);
		float c = math.dot(v, v);
		float d = math.dot(u, w0);
		float e = math.dot(v, w0);

		float den = (a * c - b * b);
		float sc, tc;

		if (den == 0) {
			sc = 0;
			tc = d / b;

			// todo: handle b = 0 (=> a and/or c is 0)
		} else {
			sc = (b * e - c * d) / (a * c - b * b);
			tc = (a * e - b * d) / (a * c - b * b);
		}

		c0 = math.lerp(p0, p1, sc);
		c1 = math.lerp(q0, q1, tc);

		return den != 0;
	}

	// Calculate the closest point of approach for line-segment vs line-segment.
	public static float PointSegmentCPA_SQRDist(out float _factor, Vector3 p, Vector3 s0, Vector3 s1) {
		Vector3 line = s1 - s0;
		Vector3 c0;
		float len = line.sqrMagnitude;

		_factor = ((p.x - s0.x) * line.x +
				   (p.y - s0.y) * line.y +
				   (p.z - s0.z) * line.z) /
				  len;

		if (_factor < 0) c0 = s0;
		else if (_factor > 1) c0 = s1;
		else c0 = s0 + line * _factor;


		return (p - c0).sqrMagnitude;
	}

//result < 0 means angle of C < 90
//result = 0 means angle of C = 90
//result > 0 means angle of C > 90
	public static float TriangleAngleProportions(Vector3 _A, Vector3 _B, Vector3 _C) {
		Vector3 ab = (_A - _B);
		Vector3 ac = (_A - _C);
		Vector3 bc = (_B - _C);

		float a = bc.sqrMagnitude;
		float b = ac.sqrMagnitude;
		float c = ab.sqrMagnitude;

		return c - (a + b);
	}
}