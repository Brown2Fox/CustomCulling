using System;
using System.Runtime.CompilerServices;
using UnityEngine;


[Serializable]
public struct PosRot {
	public static PosRot NULL = new PosRot {rot = Quaternion.identity};


	public Vector3 pos;

	[EulerAngles]
	public Quaternion rot;

	public Vector3 forward => rot * Vector3.forward;


	public static implicit operator PosRot(Transform t) {
		return new PosRot {
							  pos = t.localPosition,
							  rot = t.localRotation
						  };
	}

//a is goal position, b is start, returns offset
	public static PosRot operator -(PosRot a, PosRot b) {
		a.pos -= b.pos;
		a.rot = a.rot * Quaternion.Inverse(b.rot);
		return a;
	}

//a is start, b is offset, returns goal
	public static PosRot operator +(PosRot a, PosRot b) {
		a.pos += b.pos;
		a.rot = b.rot * a.rot;
		return a;
	}


	public static PosRot operator +(PosRot a, Vector3 b) {
		a.pos += b;
		return a;
	}


	//returns global posrot
	public PosRot GetGlobalOfChild(PosRot child) {
		return new PosRot {
							  rot = this.rot * child.rot,
							  pos = this.pos + this.rot * child.pos
						  };
	}


	//returns local posrot
	public PosRot GetLocalOfChild(PosRot child) {
		return new PosRot {
							  rot = Quaternion.Inverse(this.rot) * child.rot,
							  pos = Quaternion.Inverse(this.rot) * (child.pos - this.pos)
						  };
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static PosRot Lerp(PosRot a, PosRot b, float t) {
		a.pos = Vector3.Lerp(a.pos, b.pos, t);
		a.rot = Quaternion.Slerp(a.rot, b.rot, t);
		return a;
	}
}