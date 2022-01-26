using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent_Scale : MonoBehaviour
{
	public	Transform	rootMove;
	public	Transform	rootScale;
	public	Transform	TargetRootMove;

	public	float		speedScalingRoot	= 5f;
	public	float		speedMovingRoot		= 5f;
	public	float		speedMovingStaff	= 5f;
	public	float		speedRotationStaff	= 400f;
	
	public	Vector3		targetScale;
	private Vector3		localTargetScale;
	private Vector3		originalScale;

	private Vector3		targetRootMove;
	private Vector3		originalRootMove;
	

	private bool		proccessing;
	private bool		scalingForward;
	
	public Transform	staffClone;
	public Transform	staffOrignal;



	private void OnEnable() {
		originalScale = rootScale.localScale = Vector3.one;
		originalRootMove = rootScale.localPosition;
	}

	public void Scaling() {
		
		localTargetScale	= targetScale;
		targetRootMove		= TargetRootMove.position;
		proccessing			= true;

		scalingForward = true;
	}

	public void ScalingBack() {

		rootScale.localScale = localTargetScale;
		localTargetScale = originalScale;
		proccessing = true;

		scalingForward = false;
	}


	void EndScalingBack() {
		staffOrignal.gameObject.SetActive(true);
		staffClone.gameObject.SetActive(false);
	}

	private void LateUpdate() {

		if (!proccessing) return;
		


		// Если мы разскейливаемся обратно, то позицию фейкового посоха нужно двигать к оригиналу
		if (!scalingForward) {
			staffClone.position = Vector3.MoveTowards (staffClone.position, staffOrignal.position, speedMovingStaff * Time.deltaTime);
			
			// Поворачиваем копию посоха в положение оригинала
			staffClone.rotation = Quaternion.RotateTowards(staffClone.rotation, staffOrignal.rotation, speedRotationStaff * Time.deltaTime);
		}
		
		// Меняем скейл
		rootScale.localScale = Vector3.MoveTowards(rootScale.localScale, localTargetScale, speedScalingRoot * Time.deltaTime);

		// Двигаем рут за посохом
		float yOffset = TargetRootMove.position.y - rootMove.position.y;
		if (scalingForward) rootMove.position = Vector3.MoveTowards(rootMove.position, TargetRootMove.position - new Vector3 (0, yOffset, 0), speedMovingRoot * Time.deltaTime);

		

		if (rootScale.localScale == localTargetScale) {
			proccessing = false; 
			if (!scalingForward) EndScalingBack();
		}
		
	}

	public void Interrupt() {
		proccessing = false;
		EndScalingBack();
		rootScale.localScale = originalScale;
	}
}
