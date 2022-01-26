﻿using System.Collections.Generic;
using UnityEngine;

public class OculusDashboardSets : MonoBehaviour {
	public static OculusDashboardSets instance;

	private List<Renderer> ghostHandsRends = new List<Renderer>();
	private List<Material> ghostHandsmats = new List<Material>();
	public Material invisibleMat;

	private Renderer bodyRend;
	private Material bodyMat;

	private void Awake() {
		instance = this;
	}
	private void Start() {
//		ghostHandsRends.Add(Ghost.instance.visuals[0].GetComponent<Renderer>());
//		ghostHandsRends.Add(Ghost.instance.visuals[1].GetComponent<Renderer>());
		foreach (Renderer r in ghostHandsRends)
			ghostHandsmats.Add(r.material);
	}

	public void HideRends() {
//		for (int i = 0; i < ghostHandsRends.Count; i++)
//			ghostHandsRends[i].material = invisibleMat;
//
//		if (Ghost.instance.currentBody == null)
//			return;
//		bodyRend = Ghost.instance.currentBody.GetController<BodyController>().bodyRend;
//		if (bodyRend == null)
//			return;
//		bodyMat = bodyRend.material;
//		bodyRend.material = invisibleMat;
	}

	public void ShowRends() {
		for (int i = 0; i < ghostHandsRends.Count; i++)
			ghostHandsRends[i].material = ghostHandsmats[i];

		if (bodyRend == null)
			return;
		bodyRend.material = bodyMat;
		bodyRend = null;
	}
}
