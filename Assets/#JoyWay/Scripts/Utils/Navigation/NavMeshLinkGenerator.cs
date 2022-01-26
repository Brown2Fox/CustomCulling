using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshLinkGenerator : MonoBehaviour {
	public Transform a;
	public Transform b;
	public GameObject go;
	public LayerMask layerMask;
	private float radius = 10f;
	private float maxLengthLink = 8f;
	private float delay = 1f;

	public NavMeshLinkGenerator linkGenerator;

	public bool isGenerate;

	void StartGenerate() {
		FindGenerator();
		if (!linkGenerator) return;

		if (Length().magnitude < linkGenerator.Length().magnitude)
			Generate(linkGenerator);
		else
			linkGenerator.Generate(this);
	}

	private void Update() {
		if (delay <= 0) {
			StartGenerate();
			enabled = false;
		}
		delay -= Time.deltaTime;
	}

	public Vector3 Length() {
		return b.position - a.position;
	}

	public Vector3 MidPoint() {
		return (b.position + a.position) * 0.5f;
	}

	public void Generate(NavMeshLinkGenerator _linkGenerator) {
		if (isGenerate) return;
		isGenerate = true;
		linkGenerator = _linkGenerator;
		go = new GameObject("NewMeshLink", typeof(MeshRenderer), typeof(NavMeshLink));
		NavMeshLink link = go.GetComponent<NavMeshLink>();
		link.area = 2;

		go.transform.parent = a.transform.parent;
		go.transform.position = MidPoint();

		Vector3 newForw = Vector3.Cross(Length(), Vector3.up);
		if (Vector3.Angle(newForw, _linkGenerator.MidPoint() - go.transform.position) > 90f)
			newForw *= -1;
		go.transform.rotation = Quaternion.LookRotation(newForw, Vector3.Cross(newForw, Length()));

		link.width = Length().magnitude;
		link.startPoint = Vector3.zero;
		Vector3 e = go.transform.InverseTransformPoint(_linkGenerator.MidPoint());
		e.x = 0;
		link.endPoint = e;
	}

	public NavMeshLinkGenerator FindGenerator() {
		NavMeshLinkGenerator linkScript = null;
		float minDistance = float.MaxValue;
		float dist = 0;
		Collider[] cols = Physics.OverlapSphere(transform.position, radius, layerMask, QueryTriggerInteraction.Collide);
		foreach (Collider col in cols) {
			linkScript = col.GetComponent<NavMeshLinkGenerator>();
			if (linkScript && linkScript != this) {
				dist = Vector3.Distance(col.transform.position, transform.position);
				if (minDistance > dist && dist < maxLengthLink && Mathf.Abs(transform.position.y - col.transform.position.y) > 0.2f) {
					minDistance = dist;
					linkGenerator = linkScript;
				}
			}
		}
		return linkScript;
	}
}
