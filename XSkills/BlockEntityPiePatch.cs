using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityPie))]
public class BlockEntityPiePatch
{
	[HarmonyPostfix]
	[HarmonyPatch("TakeSlice")]
	public static void TakeSlicePostfix(BlockEntityPie __instance, ItemStack __result)
	{
		ItemSlot obj = ((BlockEntityContainer)__instance).Inventory[0];
		float? obj2;
		if (obj == null)
		{
			obj2 = null;
		}
		else
		{
			ItemStack itemstack = obj.Itemstack;
			if (itemstack == null)
			{
				obj2 = null;
			}
			else
			{
				ITreeAttribute attributes = itemstack.Attributes;
				obj2 = ((attributes != null) ? new float?(attributes.GetFloat("quality", 0f)) : null);
			}
		}
		float? num = obj2;
		float valueOrDefault = num.GetValueOrDefault();
		if (!(valueOrDefault <= 0f) && __result != null)
		{
			ITreeAttribute attributes2 = __result.Attributes;
			if (attributes2 != null)
			{
				attributes2.SetFloat("quality", valueOrDefault);
			}
		}
	}
}
