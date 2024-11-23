using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockCrock))]
public class BlockCrockPatch
{
	[HarmonyPostfix]
	[HarmonyPatch("OnPickBlock")]
	public static void OnPickBlockPostfix(ItemStack __result, IWorldAccessor world, BlockPos pos)
	{
		QualityUtil.PickQuality(__result, world, pos);
	}

	[HarmonyPostfix]
	[HarmonyPatch("GetPlacedBlockInfo")]
	public static void GetPlacedBlockInfoPostfix(ref string __result, IWorldAccessor world, BlockPos pos)
	{
		float quality = QualityUtil.GetQuality(world, pos);
		if (!(quality <= 0f))
		{
			__result += QualityUtil.QualityString(quality);
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch("OnCreatedByCrafting")]
	public static void OnCreatedByCraftingPostfix(ItemSlot[] allInputslots, ItemSlot outputSlot)
	{
		foreach (ItemSlot val in allInputslots)
		{
			ItemStack itemstack = val.Itemstack;
			if (((itemstack != null) ? itemstack.Collectible : null) is BlockCrock)
			{
				float quality = QualityUtil.GetQuality(val);
				if (quality > 0f)
				{
					outputSlot.Itemstack.Attributes.SetFloat("quality", quality * 0.8f);
				}
				break;
			}
		}
	}
}
