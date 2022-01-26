public abstract class PowerupAsset : Entry, IDescription {
	public abstract void InitAsset(Owner _to, Owner _from);
	public abstract string GetDescription();
}