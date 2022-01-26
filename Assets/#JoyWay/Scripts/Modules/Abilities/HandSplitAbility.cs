using JoyWay.Systems.InputSystem;
using UnityEngine;

public abstract class HandSplitAbility : ActiveAbility {
	public HandSplitAbilityVibrationSettings vibrations;
	public Pool chargingPool;
	[DisableEditing]
	public AbilityInput.Type input;

	[DisableEditing]
	public AbilityHandPart left, right;

	public AbilityHandPart this[HandType type] {
		get => type == HandType.Left ? left : right;
		set {
			if (type == HandType.Left)
				left = value;
			else
				right = value;
		}
	}

	protected abstract void SetDetection(bool _state, HandType _type);


	public override bool CastStartCheck() => !blocked;
	public override void StartCast() { }
}

public abstract class HandSplitAbility<AHP> : HandSplitAbility where AHP : AbilityHandPart {
	[HideInInspector]
	public AHP handPart;


#pragma warning disable 0108
	public AHP left => (AHP) base.left;
	public AHP right => (AHP) base.right;
#pragma warning restore 0108

	protected override void InitBehaviour() {
		if (stacks > 0) return;
		source = container.owner.defaultASource;
		var player = container.owner.GetComponent<Player>();

		if (!left)
			base.left = player.leftHand.gameObject.AddComponent<AHP>();
		left.Init(this);

		if (!right)
			base.right = player.rightHand.gameObject.AddComponent<AHP>();
		right.Init(this);


		AbilityInput.GetAction(input).AddListener(SetDetection);
	}


	protected override void SetDetection(bool _state, HandType _type) {
		handPart = this[_type];

		if (_state) {
			if (!CastStartCheck()) return;
			handPart.TryCast();
		} else {
			handPart.StopPredict();
			if (handPart.casting)
				handPart.EndCast();
		}
	}

	protected override void DestroyBehaviour() {
		AbilityInput.GetAction(input).RemoveListener(SetDetection);
		AHP handL = this[HandType.Left];
		AHP handR = this[HandType.Right];
		handL.Destroy();
		handR.Destroy();
	}

	public AHP this[HandType type] => base[type] as AHP;
}