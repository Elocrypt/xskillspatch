using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace XSkills;

public class XSkillsBlockEntitySapling : BlockEntitySapling
{
	public float GrowthTimeMultiplier { get; set; }

	public XSkillsBlockEntitySapling()
	{
		GrowthTimeMultiplier = 1f;
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		((BlockEntitySapling)this).ToTreeAttributes(tree);
		tree.SetFloat("growthTimeMultiplier", GrowthTimeMultiplier);
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		((BlockEntitySapling)this).FromTreeAttributes(tree, worldForResolving);
		GrowthTimeMultiplier = tree.GetFloat("growthTimeMultiplier", 1f);
	}
}
