using System.Text;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockCookedContainer))]
public class BlockCookedContainerPatch
{
	[HarmonyPostfix]
	[HarmonyPatch("GetHeldItemInfo")]
	public static void GetHeldItemInfoPostfix(ItemSlot inSlot, StringBuilder dsc)
	{
		QualityUtil.AddQualityString(inSlot, dsc);
	}

	[HarmonyPostfix]
	[HarmonyPatch("OnPickBlock")]
	public static void OnPickBlockPostfix(ItemStack __result, IWorldAccessor world, BlockPos pos)
	{
		QualityUtil.PickQuality(__result, world, pos);
	}
}
