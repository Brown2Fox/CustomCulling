using UnityEngine;

public static class VectorUtils {
	public static Vector2 xy(this Vector3 aVector) {
		return new Vector2(aVector.x, aVector.y);
	}

	public static Vector2 xz(this Vector3 aVector) {
		return new Vector2(aVector.x, aVector.z);
	}

	public static Vector2 yz(this Vector3 aVector) {
		return new Vector2(aVector.y, aVector.z);
	}

	public static Vector2 yx(this Vector3 aVector) {
		return new Vector2(aVector.y, aVector.x);
	}

	public static Vector2 zx(this Vector3 aVector) {
		return new Vector2(aVector.z, aVector.x);
	}

	public static Vector2 zy(this Vector3 aVector) {
		return new Vector2(aVector.z, aVector.y);
	}

	public static Vector2 xx(this Vector3 aVector) {
		return new Vector2(aVector.x, aVector.x);
	}

	public static Vector2 yy(this Vector3 aVector) {
		return new Vector2(aVector.y, aVector.y);
	}

	public static Vector2 zz(this Vector3 aVector) {
		return new Vector2(aVector.z, aVector.z);
	}


	public static Vector3 RotateY(this Vector3 v, float degrees) {
		float angle = degrees * Mathf.Deg2Rad;
		float sin = Mathf.Sin(angle);
		float cos = Mathf.Cos(angle);

		float tx = v.x;
		float tz = v.z;
		v.x = (cos * tx) - (sin * tz);
		v.z = (sin * tx) + (cos * tz);
		return v;
	}

	public static Vector2 Rotate(this Vector2 v, float degrees) {
		float angle = degrees * Mathf.Deg2Rad;
		float sin = Mathf.Sin(angle);
		float cos = Mathf.Cos(angle);

		float tx = v.x;
		float ty = v.y;
		v.x = (cos * tx) - (sin * ty);
		v.y = (sin * tx) + (cos * ty);
		return v;
	}
}