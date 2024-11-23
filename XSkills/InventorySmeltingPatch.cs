using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(InventorySmelting))]
public class InventorySmeltingPatch
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
	[HarmonyPatch("GetBestSuitedSlot")]
	public static void GetBestSuitedSlotPostfix(InventorySmelting __instance, WeightedSlot __result, ItemSlot sourceSlot)
	{
		if (__result == null)
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
	public static bool NewSlotPrefix(InventorySmelting __instance, out ItemSlot __result, int i)
	{
		__result = null;
		if (i >= 3)
		{
			__result = (ItemSlot)(object)new ItemSlotCooking((InventoryBase)(object)__instance);
			return false;
		}
		if (i == 1)
		{
			__result = (ItemSlot)(object)new InputSlot((InventoryBase)(object)__instance);
			return false;
		}
		return true;
	}
}
