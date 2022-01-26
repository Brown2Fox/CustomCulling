using System.Collections;
using UnityEngine;

public class AutoPockets : MonoBehaviour {
	public Player player;

	public SnapZonePocket
		leftPocket, rightPocket;

	private Snapper
		bindedLeft,
		bindedRight,
		newSnapper;

	public float autoReturnTime = 3f;

	public float forceThrow = 3f;

	private Coroutine
		leftReturnProcess, rightReturnProcess;

	private void OnEnable() {
		player.leftHand.grabber.onGrab += OnGrab;
		player.leftHand.grabber.onDrop += OnDrop;
		player.rightHand.grabber.onGrab += OnGrab;
		player.rightHand.grabber.onDrop += OnDrop;
	}

	private void OnDisable() {
		player.leftHand.grabber.onGrab -= OnGrab;
		player.leftHand.grabber.onDrop -= OnDrop;
		player.rightHand.grabber.onGrab -= OnGrab;
		player.rightHand.grabber.onDrop -= OnDrop;
	}

	private void OnGrab(GameObject _object, GrabHandPart _grabber) {
		HandType ht = _grabber.hand.type;
		newSnapper = _object.GetComponent<Snapper>();
		if (newSnapper == null) return;

		if (newSnapper == bindedLeft) {
			// if (ht == HandType.Left) {
			if (leftReturnProcess != null)
				StopCoroutine(leftReturnProcess);
			return;
			// }
		}

		if (newSnapper == bindedRight) {
			// if (ht == HandType.Right) {
			if (rightReturnProcess != null)
				StopCoroutine(rightReturnProcess);
			return;
			// }

			//if (ht == HandType.Left)
			//	bindedRight = null;
		}
	}

	private void OnDrop(GameObject _object, GrabHandPart _grabber) {
		newSnapper = _object.GetComponent<Snapper>();
		if (newSnapper == null) return;

		//HandType ht = _grabber.hand.type;

		float dot = Vector3.Dot(player.head.forward, _grabber.velocityCounter.velocity);

		if (_grabber.velocityCounter.localVelocity.magnitude > forceThrow && dot > 0) {
			//if (ht == HandType.Left)
			//	bindedLeft = null;
			//else if (ht == HandType.Right)
			//	bindedRight = null;
			if (newSnapper.GetLastSnapZone().gameObject.name.Contains("Left"))
				bindedLeft = null;
			else if (newSnapper.GetLastSnapZone().gameObject.name.Contains("Right"))
				bindedRight = null;
			return;
		}

		//if (ht == HandType.Left)
		if (newSnapper == bindedLeft)
			leftReturnProcess = StartCoroutine(ReturnLeftWithDelay(newSnapper));
		//if (ht == HandType.Right)
		if (newSnapper == bindedRight)
			rightReturnProcess = StartCoroutine(ReturnRightWithDelay(newSnapper));
	}

	private IEnumerator ReturnLeftWithDelay(Snapper _snapper) {
		//yield return new WaitForSeconds(autoReturnTime);
		float t = 0;
		float n = 0;
		while (t < autoReturnTime) {
			n = Mathf.Clamp01(t / autoReturnTime);
			_snapper.transform.position = Vector3.Lerp(_snapper.transform.position, leftPocket.transform.position, n);
			t += Time.deltaTime;
			yield return null;
		}

		if (_snapper.snapZone) yield break;
		if (bindedLeft == _snapper) {
			if (leftPocket.TrySnap(_snapper))
				_snapper.Snap(leftPocket);
		}
	}

	private IEnumerator ReturnRightWithDelay(Snapper _snapper) {
		//yield return new WaitForSeconds(autoReturnTime);
		float t = 0;
		float n = 0;
		while (t < autoReturnTime) {
			n = Mathf.Clamp01(t / autoReturnTime);
			_snapper.transform.position = Vector3.Lerp(_snapper.transform.position, rightPocket.transform.position, n);
			t += Time.deltaTime;
			yield return null;
		}

		if (_snapper.snapZone) yield break;
		if (bindedRight == _snapper) {
			if (rightPocket.TrySnap(_snapper))
				_snapper.Snap(rightPocket);
		}
	}

	public void SetBind(SnapZonePocket snapZone, Snapper snapper) {
		if (snapZone == leftPocket) {
			bindedLeft = snapper;
			if (bindedRight == snapper) {
				bindedRight = null;
				if (rightReturnProcess != null)
					StopCoroutine(rightReturnProcess);
			}
		}

		if (snapZone == rightPocket) {
			bindedRight = snapper;
			if (bindedLeft == snapper) {
				bindedLeft = null;
				if (leftReturnProcess != null)
					StopCoroutine(leftReturnProcess);
			}
		}
	}

	public void ResetBind() {
		if (leftReturnProcess != null) StopCoroutine(leftReturnProcess);
		if (rightReturnProcess != null) StopCoroutine(rightReturnProcess);
		bindedLeft = null;
		bindedRight = null;
	}
}