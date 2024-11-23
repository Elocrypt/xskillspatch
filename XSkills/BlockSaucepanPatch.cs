using System;
using System.Collections.Generic;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

internal class BlockSaucepanPatch : ManualPatch
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
			Type typeFromHandle = typeof(BlockSaucepanPatch);
			if (((Skill)cooking)[cooking.CanteenCookId].Enabled || ((Skill)cooking)[cooking.FastFoodId].Enabled || ((Skill)cooking)[cooking.WellDoneId].Enabled || ((Skill)cooking)[cooking.DilutionId].Enabled || ((Skill)cooking)[cooking.GourmetId].Enabled || ((Skill)cooking)[cooking.HappyMealId].Enabled)
			{
				ManualPatch.PatchMethod(harmony, type, typeFromHandle, "DoSmelt");
			}
		}
	}

	public static void DoSmeltPrefix(out DoSmeltState __state, ISlotProvider cookingSlotsProvider, ItemSlot outputSlot)
	{
		InventoryBase val = (InventoryBase)(object)((cookingSlotsProvider is InventoryBase) ? cookingSlotsProvider : null);
		List<ItemStack> list = new List<ItemStack>();
		__state = new DoSmeltState();
		if (val == null)
		{
			return;
		}
		DoSmeltState obj = __state;
		ItemStack itemstack = val[2].Itemstack;
		obj.quality = ((itemstack != null) ? itemstack.Attributes.GetFloat("quality", 0f) : 0f);
		for (int i = 3; i <= 6; i++)
		{
			if (!val[i].Empty)
			{
				list.Add(val[i].Itemstack);
			}
		}
		if (list.Count > 0)
		{
			__state.stacks = list.ToArray();
		}
		__state.stackSize = outputSlot.StackSize;
	}

	public static void DoSmeltPostfix(DoSmeltState __state, IWorldAccessor world, ISlotProvider cookingSlotsProvider, ItemSlot outputSlot)
	{
		InventoryBase val = (InventoryBase)(object)((cookingSlotsProvider is InventoryBase) ? cookingSlotsProvider : null);
		if (val != null)
		{
			BlockEntity obj = ((world != null) ? world.BlockAccessor.GetBlockEntity(val.Pos) : null);
			BlockEntityBehaviorOwnable blockEntityBehaviorOwnable = ((obj != null) ? obj.GetBehavior<BlockEntityBehaviorOwnable>() : null);
			float expMult = 1f;
			object obj2;
			if (outputSlot == null)
			{
				obj2 = null;
			}
			else
			{
				ItemStack itemstack = outputSlot.Itemstack;
				obj2 = ((itemstack != null) ? itemstack.Collectible : null);
			}
			if (!(obj2 is BlockLiquidContainerBase))
			{
				expMult = 0.25f;
			}
			object obj3;
			if (obj == null)
			{
				obj3 = null;
			}
			else
			{
				XLeveling modSystem = obj.Api.ModLoader.GetModSystem<XLeveling>(true);
				obj3 = ((modSystem != null) ? modSystem.GetSkill("cooking", false) : null);
			}
			if (obj3 is Cooking cooking)
			{
				cooking.ApplyAbilities(outputSlot, blockEntityBehaviorOwnable.Owner, __state.quality, outputSlot.StackSize - __state.stackSize, __state.stacks, expMult);
			}
		}
	}
}
