using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace XSkills;

public class BlockEntityBehaviorValue : BlockEntityBehavior
{
	public float Value { get; set; }

	public BlockEntityBehaviorValue(BlockEntity blockentity)
		: base(blockentity)
	{
		Value = 0f;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		((BlockEntityBehavior)this).FromTreeAttributes(tree, worldAccessForResolve);
		Value = tree.GetFloat("value", 0f);
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		((BlockEntityBehavior)this).ToTreeAttributes(tree);
		tree.SetFloat("value", Value);
	}
}
