using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(InventoryOven))]
public class InventoryOvenPatch
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
		if (!((Skill)cooking)[cooking.CanteenCookId].Enabled && !((Skill)cooking)[cooking.FastFoodId].Enabled && !((Skill)cooking)[cooking.WellDoneId].Enabled && !((Skill)cooking)[cooking.DilutionId].Enabled && !((Skill)cooking)[cooking.DesalinateId].Enabled && !((Skill)cooking)[cooking.GourmetId].Enabled)
		{
			return ((Skill)cooking)[cooking.HappyMealId].Enabled;
		}
		return true;
	}

	[HarmonyPostfix]
	[HarmonyPatch("GetSuitability")]
	public static void GetSuitabilityPostfix(InventorySmelting __instance, float __result, ItemSlot sourceSlot)
	{
		if (__result == 0f)
		{
			return;
		}
		InventoryBase inventory = sourceSlot.Inventory;
		InventoryBase obj = ((inventory is InventoryBasePlayer) ? inventory : null);
		IPlayer val = ((obj != null) ? ((InventoryBasePlayer)obj).Player : null);
		if (val != null)
		{
			BlockEntity blockEntity = ((InventoryBase)__instance).Api.World.BlockAccessor.GetBlockEntity(((InventoryBase)__instance).Pos);
			BlockEntityBehaviorOwnable blockEntityBehaviorOwnable = ((blockEntity != null) ? blockEntity.GetBehavior<BlockEntityBehaviorOwnable>() : null);
			if (blockEntityBehaviorOwnable != null)
			{
				blockEntityBehaviorOwnable.Owner = val;
			}
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch("NewSlot")]
	public static bool NewSlotPrefix(InventoryOven __instance, out ItemSlot __result)
	{
		__result = (ItemSlot)(object)new ItemSlotOven((InventoryBase)(object)__instance);
		return true;
	}
}
