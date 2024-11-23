using System;
using System.Collections.Generic;
using ACulinaryArtillery;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class BlockEntityMixingBowlPatch : ManualPatch
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
			if (((Skill)cooking)[cooking.CanteenCookId].Enabled)
			{
				ManualPatch.PatchMethod(harmony, type, typeFromHandle, "GetMatchingMixingRecipe");
			}
			if (((Skill)cooking)[cooking.CanteenCookId].Enabled || ((Skill)cooking)[cooking.FastFoodId].Enabled || ((Skill)cooking)[cooking.WellDoneId].Enabled || ((Skill)cooking)[cooking.DilutionId].Enabled || ((Skill)cooking)[cooking.GourmetId].Enabled || ((Skill)cooking)[cooking.HappyMealId].Enabled)
			{
				ManualPatch.PatchMethod(harmony, type, typeFromHandle, "mixInput");
			}
			InventoryMixingBowlPatch.Apply(harmony, typeof(InventoryMixingBowl), xSkills);
			ItemSlotMixingBowlPatch.Apply(harmony, typeof(ItemSlotMixingBowl), xSkills);
		}
	}

	public static void GetMatchingMixingRecipePrefix(BlockEntityMixingBowl __instance, out int __state)
	{
		__state = __instance.Pot?.MaxServingSize ?? 0;
		IPlayer val = ((BlockEntity)__instance).GetBehavior<BlockEntityBehaviorOwnable>()?.Owner;
		if (((val != null) ? val.Entity : null) == null || __state == 0)
		{
			return;
		}
		XLeveling modSystem = ((BlockEntity)__instance).Api.ModLoader.GetModSystem<XLeveling>(true);
		if (((modSystem != null) ? modSystem.GetSkill("cooking", false) : null) is Cooking cooking)
		{
			PlayerSkillSet behavior = ((Entity)val.Entity).GetBehavior<PlayerSkillSet>();
			PlayerSkill obj = ((behavior != null) ? behavior[((Skill)cooking).Id] : null);
			PlayerAbility val2 = ((obj != null) ? obj[cooking.CanteenCookId] : null);
			if (val2 != null)
			{
				BlockCookingContainer pot = __instance.Pot;
				pot.MaxServingSize += (int)((float)__instance.Pot.MaxServingSize * val2.FValue(0, 0f));
			}
		}
	}

	public static void GetMatchingMixingRecipePostfix(BlockEntityMixingBowl __instance, int __state)
	{
		if (__instance.Pot != null)
		{
			__instance.Pot.MaxServingSize = __state;
		}
	}

	public static void mixInputPrefix(out CookingState __state, BlockEntityMixingBowl __instance)
	{
		InventoryBase inventory = ((BlockEntityContainer)__instance).Inventory;
		List<ItemStack> list = new List<ItemStack>();
		__state = new CookingState();
		if (inventory == null)
		{
			return;
		}
		CookingState obj = __state;
		ItemStack itemstack = inventory[1].Itemstack;
		obj.quality = ((itemstack != null) ? itemstack.Attributes.GetFloat("quality", 0f) : 0f);
		for (int i = 2; i <= 7; i++)
		{
			if (!inventory[i].Empty)
			{
				list.Add(inventory[i].Itemstack);
			}
		}
		if (list.Count > 0)
		{
			__state.stacks = list.ToArray();
		}
	}

	public static void mixInputPostfix(CookingState __state, BlockEntityMixingBowl __instance)
	{
		IPlayer val = ((BlockEntity)__instance).GetBehavior<BlockEntityBehaviorOwnable>()?.Owner;
		if (val != null)
		{
			EntityPlayer entity = val.Entity;
			object obj;
			if (entity == null)
			{
				obj = null;
			}
			else
			{
				XLeveling modSystem = ((Entity)entity).Api.ModLoader.GetModSystem<XLeveling>(true);
				obj = ((modSystem != null) ? modSystem.GetSkill("cooking", false) : null);
			}
			if (obj is Cooking cooking)
			{
				cooking.ApplyAbilities(__instance.OutputSlot, val, __state.quality, 1f, __state.stacks);
			}
		}
	}
}
