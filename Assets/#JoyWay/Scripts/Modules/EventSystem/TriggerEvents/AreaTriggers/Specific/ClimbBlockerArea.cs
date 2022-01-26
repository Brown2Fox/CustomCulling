public class ClimbBlockerArea : AreaInOutTriggerBase {
	protected override bool Check(Owner _owner) {
		if (!base.Check(_owner)) return false;
		if (!_owner.aContainer) return false;
		return true;
	}


	public override void TriggerEnter(Owner _owner) {
		var climbAbility = _owner.aContainer.GetAbility<ClimbAbility>();
		climbAbility.blockSources.Add(this);
	}

	public override void TriggerExit(Owner _owner) {
		var climbAbility = _owner.aContainer.GetAbility<ClimbAbility>();
		climbAbility.blockSources.Remove(this);
	}
}