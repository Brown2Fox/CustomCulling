public class TelekinesisBlockerArea : AreaInOutTriggerBase {
	protected override bool Check(Owner _owner) {
		if (!base.Check(_owner)) return false;
		if (!_owner.aContainer) return false;
		return true;
	}


	public override void TriggerEnter(Owner _owner) {
		TelekinesisAbility telekinesisAbility = _owner.aContainer.GetAbility<TelekinesisAbility>();
		telekinesisAbility.blockSources.Add(this);
	}

	public override void TriggerExit(Owner _owner) {
		TelekinesisAbility telekinesisAbility = _owner.aContainer.GetAbility<TelekinesisAbility>();
		telekinesisAbility.blockSources.Remove(this);
	}
}
