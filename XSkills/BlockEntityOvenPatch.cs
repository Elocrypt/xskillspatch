using System;
using System.Text;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class BlockEntityOvenPatch : ManualPatch
{
	public static void Apply(Harmony harmony, Type ovenType, XSkills xSkills)
	{
		if (xSkills == null)
		{
			return;
		}
		xSkills.Skills.TryGetValue("cooking", out var value);
		if (value is Cooking cooking && ((Skill)cooking).Enabled)
		{
			Type typeFromHandle = typeof(BlockEntityOvenPatch);
			ManualPatch.PatchMethod(harmony, ovenType, typeFromHandle, "GetBlockInfo");
			if (((Skill)cooking)[cooking.CanteenCookId].Enabled || ((Skill)cooking)[cooking.FastFoodId].Enabled || ((Skill)cooking)[cooking.WellDoneId].Enabled || ((Skill)cooking)[cooking.DilutionId].Enabled || ((Skill)cooking)[cooking.GourmetId].Enabled || ((Skill)cooking)[cooking.HappyMealId].Enabled)
			{
				ManualPatch.PatchMethod(harmony, ovenType, typeFromHandle, "OnInteract");
				ManualPatch.PatchMethod(harmony, ovenType, typeFromHandle, "IncrementallyBake");
			}
		}
	}

	public static void OnInteractPostfix(BlockEntity __instance, IPlayer byPlayer)
	{
		BlockEntityBehaviorOwnable behavior = __instance.GetBehavior<BlockEntityBehaviorOwnable>();
		if (behavior != null)
		{
			behavior.Owner = byPlayer;
		}
	}

	public static void IncrementallyBakePrefix(InventoryOven ___ovenInv, ref CookingState __state, int slotIndex)
	{
		__state = new CookingState();
		__state.quality = 0f;
		__state.stacks = (ItemStack[])(object)new ItemStack[1] { ((InventoryBase)___ovenInv)[slotIndex].Itemstack };
	}

	public static void IncrementallyBakePostfix(BlockEntity __instance, ref CookingState __state, InventoryOven ___ovenInv, int slotIndex)
	{
		IPlayer val = __instance.GetBehavior<BlockEntityBehaviorOwnable>()?.Owner;
		if (val != null && __state.stacks[0] != ((InventoryBase)___ovenInv)[slotIndex].Itemstack)
		{
			if (((InventoryBase)___ovenInv)[slotIndex].Itemstack.StackSize < __state.stacks[0].StackSize)
			{
				((InventoryBase)___ovenInv)[slotIndex].Itemstack.StackSize = __state.stacks[0].StackSize;
			}
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
				cooking.ApplyAbilities(((InventoryBase)___ovenInv)[slotIndex], val, __state.quality, 1f, __state.stacks);
			}
		}
	}

	public static void GetBlockInfoPostfix(BlockEntity __instance, InventoryOven ___ovenInv, OvenItemData[] ___bakingData, float ___fuelBurnTime, IPlayer forPlayer, StringBuilder sb)
	{
		XLeveling obj = XLeveling.Instance(__instance.Api);
		if (!(((obj != null) ? obj.GetSkill("cooking", false) : null) is Cooking cooking))
		{
			return;
		}
		object obj2;
		if (forPlayer == null)
		{
			obj2 = null;
		}
		else
		{
			EntityPlayer entity = forPlayer.Entity;
			if (entity == null)
			{
				obj2 = null;
			}
			else
			{
				PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
				obj2 = ((behavior != null) ? behavior[((Skill)cooking).Id][((Skill)cooking).SpecialisationID] : null);
			}
		}
		PlayerAbility val = (PlayerAbility)obj2;
		if (val == null || val.Tier < 1)
		{
			return;
		}
		if (___fuelBurnTime > 0f)
		{
			sb.AppendLine($"Burning: {___fuelBurnTime:N2} sec");
		}
		for (int i = 0; i < ___bakingData.Length; i++)
		{
			ItemSlot obj3 = ((InventoryBase)___ovenInv)[i];
			if (((obj3 != null) ? obj3.Itemstack : null) == null)
			{
				continue;
			}
			OvenItemData val2 = ___bakingData[i];
			BakingProperties val3 = BakingProperties.ReadFrom(((InventoryBase)___ovenInv)[i].Itemstack);
			if (val3 != null && val2 != null)
			{
				float num = Math.Min((val2.BakedLevel - val3.LevelFrom) / (val3.LevelTo - val3.LevelFrom), 1f);
				ItemSlot obj4 = ((InventoryBase)___ovenInv)[i];
				if (((obj4 != null) ? obj4.Itemstack : null) != null)
				{
					sb.AppendLine(Lang.Get("xskills:progress", new object[1] { num }));
				}
			}
		}
	}
}
