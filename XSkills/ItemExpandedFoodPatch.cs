using System;
using System.Text;
using HarmonyLib;
using Vintagestory.API.Common;

namespace XSkills;

internal class ItemExpandedFoodPatch : ManualPatch
{
	public static void Apply(Harmony harmony, Type type)
	{
		Type typeFromHandle = typeof(ItemExpandedFoodPatch);
		ManualPatch.PatchMethod(harmony, type, typeFromHandle, "GetHeldItemInfo");
	}

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
}
