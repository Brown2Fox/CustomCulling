using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UnityEventFloat : UnityEvent<float> { }

[Serializable]
public class UnityEventCollider : UnityEvent<Collider> { }

[Serializable]
public class UnityEventGrabber : UnityEvent<GrabHandPart> { }


[Serializable]
public class UnityEventOwner : UnityEvent<Owner> { }