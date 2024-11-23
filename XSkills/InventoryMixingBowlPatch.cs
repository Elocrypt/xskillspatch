using System;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class InventoryMixingBowlPatch : ManualPatch
{
	public static void Apply(Harmony harmony, Type type, XSkills xSkills)
	{
		if (xSkills == null)
		{
			return;
		}
		xSkills.Skills.TryGetValue("cooking", out var value);
		if (value is Cooking cooking && ((Skill)cooking).Enabled)
		{
			Type typeFromHandle = typeof(BlockEntityMixingBowlPatch);
			if (((Skill)cooking)[cooking.CanteenCookId].Enabled || ((Skill)cooking)[cooking.FastFoodId].Enabled || ((Skill)cooking)[cooking.WellDoneId].Enabled || ((Skill)cooking)[cooking.DilutionId].Enabled || ((Skill)cooking)[cooking.DesalinateId].Enabled || ((Skill)cooking)[cooking.GourmetId].Enabled || ((Skill)cooking)[cooking.HappyMealId].Enabled)
			{
				ManualPatch.PatchMethod(harmony, type, typeFromHandle, "NewSlot");
			}
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch("NewSlot")]
	public static bool NewSlotPrefix(InventorySmelting __instance, out ItemSlot __result, int i)
	{
		__result = null;
		if (i == 1)
		{
			__result = (ItemSlot)(object)new ItemSlotCooking((InventoryBase)(object)__instance);
			return false;
		}
		return true;
	}
}
