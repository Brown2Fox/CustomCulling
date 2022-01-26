using UnityEngine;

public class HandPopUpUI : MonoBehaviour {
    public Player player;
    public bool leftHand = true;
    private Transform hand;
    private Vector3 closeVector;
    public Transform holder;
    private bool showCondition;

    protected virtual void Awake() {
        if (leftHand) {
            hand = player.leftHand.transform;
            closeVector = Vector3.up;
        }
        else {
            hand = player.rightHand.transform;
            closeVector = Vector3.down;
        }
    }

    public void SetShowCondition(bool showCondition) {
        this.showCondition = showCondition;
    }
    protected virtual void Update() {
        if (showCondition)
            CheckHandPos();
    }

    private void CheckHandPos() {
        ActivateHolder(Vector3.Angle(hand.right, closeVector) < 60);
    }

    public void ActivateHolder(bool state) {
        holder.gameObject.SetActive(state);
    }
}