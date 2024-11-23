using System.Text;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockPie))]
public class BlockPiePatch
{
	[HarmonyPostfix]
	[HarmonyPatch("GetHeldItemInfo")]
	public static void GetHeldItemInfoPostfix(ItemSlot inSlot, StringBuilder dsc)
	{
		float? obj;
		if (inSlot == null)
		{
			obj = null;
		}
		else
		{
			ItemStack itemstack = inSlot.Itemstack;
			obj = ((itemstack != null) ? itemstack.Attributes.TryGetFloat("quality") : null);
		}
		float? num = obj;
		QualityUtil.AddQualityString(num.GetValueOrDefault(), dsc);
	}

	[HarmonyPostfix]
	[HarmonyPatch("GetPlacedBlockInfo")]
	public static void Postfix(ref string __result, IWorldAccessor world, BlockPos pos)
	{
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
		BlockEntity obj = ((blockEntity is BlockEntityPie) ? blockEntity : null);
		float? obj2;
		if (obj == null)
		{
			obj2 = null;
		}
		else
		{
			ItemSlot obj3 = ((BlockEntityContainer)obj).Inventory[0];
			if (obj3 == null)
			{
				obj2 = null;
			}
			else
			{
				ItemStack itemstack = obj3.Itemstack;
				obj2 = ((itemstack != null) ? new float?(itemstack.Attributes.GetFloat("quality", 0f)) : null);
			}
		}
		float? num = obj2;
		float valueOrDefault = num.GetValueOrDefault();
		if (!(valueOrDefault <= 0f))
		{
			__result += QualityUtil.QualityString(valueOrDefault);
		}
	}
}
