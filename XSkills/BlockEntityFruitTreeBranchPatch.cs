using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityFruitTreeBranch))]
public class BlockEntityFruitTreeBranchPatch
{
	public static bool Prepare(MethodBase original)
	{
		return BlockFruitTreeBranchPatch.Prepare(original);
	}

	[HarmonyPostfix]
	[HarmonyPatch("FromTreeAttributes")]
	public static void FromTreeAttributesPostfix(BlockEntityFruitTreeBranch __instance, ITreeAttribute tree)
	{
		if (((BlockEntity)__instance).GetBehavior<FruitTreeGrowingBranchBH>() == null)
		{
			return;
		}
		float @float = tree.GetFloat("value", -1f);
		if (!(@float <= 0f))
		{
			BlockEntityBehaviorValue blockEntityBehaviorValue = ((BlockEntity)__instance).Behaviors.Find((BlockEntityBehavior x) => x is BlockEntityBehaviorValue) as BlockEntityBehaviorValue;
			if (blockEntityBehaviorValue == null)
			{
				blockEntityBehaviorValue = new BlockEntityBehaviorValue((BlockEntity)(object)__instance);
				((BlockEntity)__instance).Behaviors.Add((BlockEntityBehavior)(object)blockEntityBehaviorValue);
			}
			blockEntityBehaviorValue.Value = @float;
		}
	}
}
