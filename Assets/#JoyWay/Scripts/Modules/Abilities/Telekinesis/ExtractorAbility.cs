public class ExtractorAbility : HandSplitAbility<ExtractObject> {
	public readonly float extractDistance = 20f;

	private readonly float blockRange = .28f;
	public float sqrBlockRange;


	public Pool stonePool;
	public Pool effectPool;

	protected override void InitBehaviour() {
		base.InitBehaviour();
		sqrBlockRange = blockRange * blockRange;
		TelekinesisAbility tk = container.GetEntryOfType<TelekinesisAbility>();
		left.telekinesisHand = tk.left;
		right.telekinesisHand = tk.right;
	}

	protected override void DestroyBehaviour() {
		base.DestroyBehaviour();
	}

	public override void ResetAbility() {
		//	container.Remove(this);
	}
}