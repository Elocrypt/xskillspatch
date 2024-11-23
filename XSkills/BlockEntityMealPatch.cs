using System.Text;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityMeal))]
public class BlockEntityMealPatch
{
	[HarmonyPatch(/*Could not decode attribute arguments.*/)]
	public static void Postfix(ref InventoryGeneric ___inventory)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		int num = ((___inventory != null) ? (((InventoryBase)___inventory).Count + 1) : 7);
		___inventory = new InventoryGeneric(num, (string)null, (ICoreAPI)null, (NewSlotDelegate)null);
	}

	[HarmonyPostfix]
	[HarmonyPatch("GetBlockInfo")]
	public static void GetBlockInfoPostfix(BlockEntityMeal __instance, StringBuilder dsc)
	{
		QualityUtil.AddQualityString(((BlockEntityContainer)__instance).Inventory[0], dsc);
	}

	[HarmonyPostfix]
	[HarmonyPatch("OnBlockPlaced")]
	public static void OnBlockPlacedPostfix(BlockEntityMeal __instance, ItemStack byItemStack)
	{
		if (!(((byItemStack != null) ? byItemStack.Block : null) is BlockCookedContainer))
		{
			return;
		}
		float @float = byItemStack.Attributes.GetFloat("quality", 0f);
		if (@float <= 0f)
		{
			return;
		}
		ItemSlot obj = ((BlockEntityContainer)__instance).Inventory[0];
		if (obj != null)
		{
			ItemStack itemstack = obj.Itemstack;
			if (itemstack != null)
			{
				itemstack.Attributes.SetFloat("quality", @float);
			}
		}
	}
}
