using System;
using UnityEngine;

public class GhostControllersController : MonoBehaviour {
	private static readonly int moveID = Animator.StringToHash("move");
	private static readonly int rotateID = Animator.StringToHash("rotate");
	private static readonly int climbID = Animator.StringToHash("climb");
	private static readonly int dashID = Animator.StringToHash("dash");
	private static readonly int grabID = Animator.StringToHash("grab");
	private static readonly int shootID = Animator.StringToHash("shoot");
	private static readonly int airID = Animator.StringToHash("air");
	private static readonly int fireID = Animator.StringToHash("fire");
	private static readonly int idleAnimID = Animator.StringToHash("idle");


	public void ShowMoveControllers() {
		GhostControllers.instance.Activate(HandType.Left);
		GhostControllers.instance.SetButtonAnimation(moveID, HandType.Left);
		GhostControllers.instance.SetControllersFollow();
	}

	public void ShowRotateControllers() {
		GhostControllers.instance.Activate(HandType.Right);
		GhostControllers.instance.SetButtonAnimation(rotateID, HandType.Right);
		GhostControllers.instance.SetControllersFollow();
	}

	public void ShowClimbControllers() {
		GhostControllers.instance.Activate();
		GhostControllers.instance.SetButtonAnimation(climbID);
		GhostControllers.instance.SetControllersFollow();
	}

	public void ShowGrabControllers() {
		GhostControllers.instance.Activate();
		GhostControllers.instance.SetButtonAnimation(grabID);
		GhostControllers.instance.SetControllersFollow();
	}

	public void ShowShootControllers() {
		GhostControllers.instance.Activate();
		GhostControllers.instance.SetButtonAnimation(shootID);
		GhostControllers.instance.SetControllersFollow();
	}

	public void ShowTelekinesisControllers() {
		GhostControllers.instance.Activate();
		GhostControllers.instance.SetButtonAnimation(grabID);
		GhostControllers.instance.SetControllersFollow();
	}

	public void ShowDashControllers() {
		GhostControllers.instance.Activate(HandType.Right);
		GhostControllers.instance.SetControllersAnimated(dashID);
		GhostControllers.instance.controllers.rightAnimScript.onClipEnd += GhostControllers.instance.SetUnderRorationRoot;
		GhostControllers.instance.controllers.rightAnimScript.onClipStart += GhostControllers.instance.SetUnderMoveRoot;
	}

	public void ShowAirControllers() {
		GhostControllers.instance.Activate(HandType.Right);
		GhostControllers.instance.SetControllersAnimated(airID);
		GhostControllers.instance.controllers.rightAnimScript.onClipEnd += GhostControllers.instance.SetUnderRorationRoot;
		GhostControllers.instance.controllers.rightAnimScript.onClipStart += GhostControllers.instance.SetUnderMoveRoot;
	}

	public void ShowFireControllers() {
		GhostControllers.instance.Activate(HandType.Right);
		GhostControllers.instance.SetButtonAnimation(fireID);
		GhostControllers.instance.SetControllersFollow();
	}

	public void ShowReloadControllers() { }

	public void OnDisable() {
		GhostControllers.instance.SetButtonAnimation(idleAnimID);
		GhostControllers.instance.Disable();
	}
}