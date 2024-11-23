using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(FruitTreeGrowingBranchBH))]
public class FruitTreeGrowingBranchBHPatch
{
	public static bool Prepare(MethodBase original)
	{
		return BlockFruitTreeBranchPatch.Prepare(original);
	}

	public static void CommonPrefix(FruitTreeGrowingBranchBH instance, out FruitTreeGrowingState state, BlockFruitTreeBranch branchBlock)
	{
		state = default(FruitTreeGrowingState);
		BlockEntityBehaviorValue behavior = ((BlockEntityBehavior)instance).Blockentity.GetBehavior<BlockEntityBehaviorValue>();
		if (behavior != null)
		{
			BlockEntity blockentity = ((BlockEntityBehavior)instance).Blockentity;
			BlockEntityFruitTreeBranch val = (BlockEntityFruitTreeBranch)(object)((blockentity is BlockEntityFruitTreeBranch) ? blockentity : null);
			branchBlock.TypeProps.TryGetValue(((BlockEntityFruitTreePart)val).TreeType, out var value);
			if (value != null)
			{
				state.props = value;
				state.CuttingGraftChance = value.CuttingGraftChance;
				state.CuttingRootingChance = value.CuttingRootingChance;
				FruitTreeTypeProperties obj = value;
				obj.CuttingGraftChance *= behavior.Value;
				FruitTreeTypeProperties obj2 = value;
				obj2.CuttingRootingChance *= behavior.Value;
			}
		}
	}

	public static void CommonPostfix(FruitTreeGrowingState state)
	{
		if (state.props != null)
		{
			state.props.CuttingGraftChance = state.CuttingGraftChance;
			state.props.CuttingRootingChance = state.CuttingRootingChance;
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch("GetBlockInfo")]
	public static void GetBlockInfoPrefix(FruitTreeGrowingBranchBH __instance, out FruitTreeGrowingState __state, BlockFruitTreeBranch ___branchBlock)
	{
		CommonPrefix(__instance, out __state, ___branchBlock);
	}

	[HarmonyPostfix]
	[HarmonyPatch("GetBlockInfo")]
	public static void GetBlockInfoPostfix(FruitTreeGrowingState __state)
	{
		CommonPostfix(__state);
	}

	[HarmonyPrefix]
	[HarmonyPatch("TryGrow")]
	public static void TryGrowPrefix(FruitTreeGrowingBranchBH __instance, out FruitTreeGrowingState __state, BlockFruitTreeBranch ___branchBlock)
	{
		CommonPrefix(__instance, out __state, ___branchBlock);
	}

	[HarmonyPostfix]
	[HarmonyPatch("TryGrow")]
	public static void TryGrowPostfix(FruitTreeGrowingBranchBH __instance, FruitTreeGrowingState __state)
	{
		CommonPostfix(__state);
	}
}
