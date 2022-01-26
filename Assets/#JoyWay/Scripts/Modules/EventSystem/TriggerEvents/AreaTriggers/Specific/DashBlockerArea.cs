public class DashBlockerArea : AreaInOutTriggerBase {
	protected override bool Check(Owner _owner) {
		if (!base.Check(_owner)) return false;
		if (!_owner.aContainer) return false;
		return true;
	}


	public override void TriggerEnter(Owner _owner) {
		DashAbility dashAbility = _owner.aContainer.GetAbility<DashAbility>();
		dashAbility.blockSources.Add(this);
	}

	public override void TriggerExit(Owner _owner) {
		DashAbility dashAbility = _owner.aContainer.GetAbility<DashAbility>();
		dashAbility.blockSources.Remove(this);
	}
}