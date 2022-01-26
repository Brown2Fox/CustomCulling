public class MoveBlockerArea : AreaInOutTriggerBase {
	protected override bool Check(Owner _owner) {
		if (!base.Check(_owner)) return false;
		if (!_owner.aContainer) return false;
		return true;
	}


	public override void TriggerEnter(Owner _owner) {
		MoveAbilityBase moveAbility = _owner.aContainer.GetAbility<MoveAbilityBase>();
		moveAbility.blockSources.Add(this);
	}

	public override void TriggerExit(Owner _owner) {
		MoveAbilityBase moveAbility = _owner.aContainer.GetAbility<MoveAbilityBase>();
		moveAbility.blockSources.Remove(this);
	}
}