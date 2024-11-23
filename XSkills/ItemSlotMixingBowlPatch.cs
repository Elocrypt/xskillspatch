using System;
using System.Collections.Generic;
using ACulinaryArtillery;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using XLib.XLeveling;

namespace XSkills;

public class ItemSlotMixingBowlPatch : ManualPatch
{
	public static int ContainerMaxSlotStackSize = 6;

	public static void Apply(Harmony harmony, Type type, XSkills xSkills)
	{
		if (xSkills == null)
		{
			return;
		}
		xSkills.Skills.TryGetValue("cooking", out var value);
		if (value is Cooking cooking && ((Skill)cooking).Enabled)
		{
			Type typeFromHandle = typeof(ItemSlotMixingBowlPatch);
			if (((Skill)cooking)[cooking.CanteenCookId].Enabled || ((Skill)cooking)[cooking.FastFoodId].Enabled || ((Skill)cooking)[cooking.WellDoneId].Enabled || ((Skill)cooking)[cooking.DilutionId].Enabled || ((Skill)cooking)[cooking.DesalinateId].Enabled || ((Skill)cooking)[cooking.GourmetId].Enabled || ((Skill)cooking)[cooking.HappyMealId].Enabled)
			{
				ManualPatch.PatchMethod(harmony, type, typeFromHandle, "CanTakeFrom");
				ManualPatch.PatchMethod(harmony, type, typeFromHandle, "ActivateSlotLeftClick", "ActivateSlotPrefix", null);
				ManualPatch.PatchMethod(harmony, type, typeFromHandle, "ActivateSlotRightClick", "ActivateSlotPrefix", null);
			}
		}
	}

	public static void ActivateSlotPrefix(ItemSlotMixingBowl __instance, ref ItemStackMoveOperation op)
	{
		InventoryBase inventory = ((ItemSlot)__instance).Inventory;
		BlockEntity obj = ((inventory != null) ? inventory.Api.World.BlockAccessor.GetBlockEntity(((ItemSlot)__instance).Inventory.Pos) : null);
		BlockEntityBehaviorOwnable blockEntityBehaviorOwnable = ((obj != null) ? obj.GetBehavior<BlockEntityBehaviorOwnable>() : null);
		if (op.ActingPlayer != null && blockEntityBehaviorOwnable != null)
		{
			blockEntityBehaviorOwnable.Owner = op.ActingPlayer;
		}
		XLeveling modSystem = ((Entity)op.ActingPlayer.Entity).Api.ModLoader.GetModSystem<XLeveling>(true);
		if (!(((modSystem != null) ? modSystem.GetSkill("cooking", false) : null) is Cooking cooking))
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)op.ActingPlayer.Entity).GetBehavior<PlayerSkillSet>();
		object obj2;
		if (behavior == null)
		{
			obj2 = null;
		}
		else
		{
			List<PlayerSkill> playerSkills = behavior.PlayerSkills;
			if (playerSkills == null)
			{
				obj2 = null;
			}
			else
			{
				PlayerSkill obj3 = playerSkills[((Skill)cooking).Id];
				obj2 = ((obj3 != null) ? obj3[cooking.CanteenCookId] : null);
			}
		}
		PlayerAbility val = (PlayerAbility)obj2;
		if (val != null)
		{
			int maxSlotStackSize = ContainerMaxSlotStackSize + (int)((float)ContainerMaxSlotStackSize * val.FValue(0, 0f));
			((ItemSlot)__instance).MaxSlotStackSize = maxSlotStackSize;
		}
	}

	public static void CanTakeFromPostfix(ItemSlotMixingBowl __instance, bool __result, ItemSlot sourceSlot)
	{
		if (!__result)
		{
			return;
		}
		InventoryBase inventory = sourceSlot.Inventory;
		InventoryBase obj = ((inventory is InventoryBasePlayer) ? inventory : null);
		IPlayer val = ((obj != null) ? ((InventoryBasePlayer)obj).Player : null);
		if (val == null)
		{
			return;
		}
		BlockEntity blockEntity = ((ItemSlot)__instance).Inventory.Api.World.BlockAccessor.GetBlockEntity(((ItemSlot)__instance).Inventory.Pos);
		BlockEntityBehaviorOwnable blockEntityBehaviorOwnable = ((blockEntity != null) ? blockEntity.GetBehavior<BlockEntityBehaviorOwnable>() : null);
		if (blockEntityBehaviorOwnable == null)
		{
			return;
		}
		blockEntityBehaviorOwnable.Owner = val;
		XLeveling modSystem = ((Entity)val.Entity).Api.ModLoader.GetModSystem<XLeveling>(true);
		if (!(((modSystem != null) ? modSystem.GetSkill("cooking", false) : null) is Cooking cooking))
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)val.Entity).GetBehavior<PlayerSkillSet>();
		object obj2;
		if (behavior == null)
		{
			obj2 = null;
		}
		else
		{
			List<PlayerSkill> playerSkills = behavior.PlayerSkills;
			if (playerSkills == null)
			{
				obj2 = null;
			}
			else
			{
				PlayerSkill obj3 = playerSkills[((Skill)cooking).Id];
				obj2 = ((obj3 != null) ? obj3[cooking.CanteenCookId] : null);
			}
		}
		PlayerAbility val2 = (PlayerAbility)obj2;
		if (val2 != null)
		{
			int maxSlotStackSize = ContainerMaxSlotStackSize + (int)((float)ContainerMaxSlotStackSize * val2.FValue(0, 0f));
			((ItemSlot)__instance).MaxSlotStackSize = maxSlotStackSize;
		}
	}
}
