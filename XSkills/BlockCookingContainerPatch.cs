using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockCookingContainer))]
public class BlockCookingContainerPatch
{
	public static bool Prepare(MethodBase original)
	{
		XSkills instance = XSkills.Instance;
		if (instance == null)
		{
			return false;
		}
		instance.Skills.TryGetValue("cooking", out var value);
		if (!(value is Cooking cooking) || !((Skill)cooking).Enabled)
		{
			return false;
		}
		if (original == null)
		{
			return true;
		}
		if (original.Name == "CanSmelt")
		{
			if (!((Skill)cooking)[cooking.DesalinateId].Enabled)
			{
				return ((Skill)cooking)[cooking.CanteenCookId].Enabled;
			}
			return true;
		}
		return ((Skill)cooking)[cooking.CanteenCookId].Enabled;
	}

	[HarmonyPrefix]
	[HarmonyPatch("CanSmelt")]
	public static bool CanSmeltPrefix(BlockCookingContainer __instance, out bool __result, IWorldAccessor world, ISlotProvider cookingSlotsProvider)
	{
		__result = false;
		BlockEntityBehaviorOwnable ownableFromInventory = CookingUtil.GetOwnableFromInventory((InventoryBase)(object)((cookingSlotsProvider is InventoryBase) ? cookingSlotsProvider : null));
		if (ownableFromInventory == null)
		{
			return true;
		}
		IPlayer owner = ownableFromInventory.Owner;
		if (owner == null)
		{
			return true;
		}
		XLeveling modSystem = world.Api.ModLoader.GetModSystem<XLeveling>(true);
		if (!(((modSystem != null) ? modSystem.GetSkill("cooking", false) : null) is Cooking cooking))
		{
			return true;
		}
		PlayerSkill val;
		try
		{
			PlayerSkillSet behavior = ((Entity)owner.Entity).GetBehavior<PlayerSkillSet>();
			val = ((behavior != null) ? behavior[((Skill)cooking).Id] : null);
		}
		catch (NullReferenceException)
		{
			ownableFromInventory.OwnerString = null;
			ownableFromInventory.Owner = null;
			return true;
		}
		if (val == null)
		{
			return true;
		}
		int maxServingSize = __instance.MaxServingSize;
		PlayerAbility val2 = val[cooking.CanteenCookId];
		if (val2 != null)
		{
			__instance.MaxServingSize = (int)((float)maxServingSize * (1f + val2.FValue(0, 0f)));
		}
		ItemStack[] cookingStacks = __instance.GetCookingStacks(cookingSlotsProvider, false);
		CookingRecipe matchingCookingRecipe = __instance.GetMatchingCookingRecipe(world, cookingStacks);
		__instance.MaxServingSize = maxServingSize;
		if (matchingCookingRecipe != null)
		{
			if (matchingCookingRecipe.Code == "salt")
			{
				PlayerAbility val3 = val[cooking.DesalinateId];
				if (val3 == null || (float)cookingStacks[0].StackSize * 0.01f < (float)val3.Value(0, 10000))
				{
					return false;
				}
			}
			__result = true;
		}
		return false;
	}

	[HarmonyPrefix]
	[HarmonyPatch("DoSmelt")]
	public static void DoSmeltPrefix(BlockCookingContainer __instance, out int __state, ISlotProvider cookingSlotsProvider)
	{
		__state = CookingUtil.SetMaxServingSize(__instance, cookingSlotsProvider);
	}

	[HarmonyPostfix]
	[HarmonyPatch("DoSmelt")]
	public static void DoSmeltPostfix(BlockCookingContainer __instance, int __state, ISlotProvider cookingSlotsProvider, ItemSlot outputSlot)
	{
		__instance.MaxServingSize = __state;
		IPlayer ownerFromInventory = CookingUtil.GetOwnerFromInventory((InventoryBase)(object)((cookingSlotsProvider is InventoryBase) ? cookingSlotsProvider : null));
		if (((ownerFromInventory != null) ? ownerFromInventory.Entity : null) != null)
		{
			XLeveling modSystem = ((Entity)ownerFromInventory.Entity).Api.ModLoader.GetModSystem<XLeveling>(true);
			if (((modSystem != null) ? modSystem.GetSkill("cooking", false) : null) is Cooking cooking)
			{
				cooking.ApplyAbilities(outputSlot, ownerFromInventory, 0f);
			}
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch("GetOutputText")]
	public static void GetOutputTextPrefix(BlockCookingContainer __instance, out int __state, ISlotProvider cookingSlotsProvider)
	{
		__state = CookingUtil.SetMaxServingSize(__instance, cookingSlotsProvider);
	}

	[HarmonyPostfix]
	[HarmonyPatch("GetOutputText")]
	public static void GetOutputTextPostfix(BlockCookingContainer __instance, int __state)
	{
		__instance.MaxServingSize = __state;
	}
}
