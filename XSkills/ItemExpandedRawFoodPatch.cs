using System;
using System.Text;
using HarmonyLib;
using Vintagestory.API.Common;
using XLib.XLeveling;

namespace XSkills;

internal class ItemExpandedRawFoodPatch : ManualPatch
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
			Type typeFromHandle = typeof(ItemExpandedRawFoodPatch);
			ManualPatch.PatchMethod(harmony, type, typeFromHandle, "GetHeldItemInfo");
			if (((Skill)cooking)[cooking.CanteenCookId].Enabled || ((Skill)cooking)[cooking.FastFoodId].Enabled || ((Skill)cooking)[cooking.WellDoneId].Enabled || ((Skill)cooking)[cooking.DilutionId].Enabled || ((Skill)cooking)[cooking.GourmetId].Enabled || ((Skill)cooking)[cooking.HappyMealId].Enabled)
			{
				ManualPatch.PatchMethod(harmony, type, typeFromHandle, "DoSmelt");
			}
		}
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

	public static void DoSmeltPrefix(out DoSmeltState __state, ItemSlot outputSlot)
	{
		__state = new DoSmeltState();
		DoSmeltState obj = __state;
		ItemStack itemstack = outputSlot.Itemstack;
		obj.stackSize = ((itemstack != null) ? itemstack.StackSize : 0);
		DoSmeltState obj2 = __state;
		ItemStack itemstack2 = outputSlot.Itemstack;
		obj2.quality = ((itemstack2 != null) ? itemstack2.Attributes.GetFloat("quality", 0f) : 0f);
	}

	public static void DoSmeltPostfix(DoSmeltState __state, IWorldAccessor world, ISlotProvider cookingSlotsProvider, ItemSlot outputSlot)
	{
		InventoryBase val = (InventoryBase)(object)((cookingSlotsProvider is InventoryBase) ? cookingSlotsProvider : null);
		if (val != null)
		{
			BlockEntity obj = ((world != null) ? world.BlockAccessor.GetBlockEntity(val.Pos) : null);
			BlockEntityBehaviorOwnable blockEntityBehaviorOwnable = ((obj != null) ? obj.GetBehavior<BlockEntityBehaviorOwnable>() : null);
			ItemStack itemstack = outputSlot.Itemstack;
			int num = ((itemstack != null) ? itemstack.StackSize : 0) - __state.stackSize;
			if (blockEntityBehaviorOwnable?.Owner != null && num > 0)
			{
				CollectibleObjectPatch.DoSmeltCooking(blockEntityBehaviorOwnable?.Owner, outputSlot, num, __state.quality);
			}
		}
	}
}
