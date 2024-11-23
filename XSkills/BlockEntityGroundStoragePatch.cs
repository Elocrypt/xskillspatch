using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityGroundStorage))]
internal class BlockEntityGroundStoragePatch
{
	[HarmonyPrefix]
	[HarmonyPatch("TryPutItem")]
	internal static void TryPutItemPrefix(BlockEntityGroundStorage __instance, out BlockEntityGroundStorageState __state, IPlayer player)
	{
		__state = new BlockEntityGroundStorageState();
		__state.stack = ((player != null) ? player.InventoryManager.ActiveHotbarSlot.Itemstack : null);
		InventoryBase inventory = ((BlockEntityContainer)__instance).Inventory;
		if (inventory == null || inventory.Count >= 1)
		{
			BlockEntityGroundStorageState obj = __state;
			ItemStack itemstack = ((BlockEntityContainer)__instance).Inventory[0].Itemstack;
			obj.stackSize = ((itemstack != null) ? itemstack.StackSize : 0);
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch("TryPutItem")]
	internal static void TryPutItemPostfix(BlockEntityGroundStorage __instance, bool __result, BlockEntityGroundStorageState __state)
	{
		if (__state?.stack == null || !__result || __state.stackSize == 0)
		{
			return;
		}
		int stackSize = ((BlockEntityContainer)__instance).Inventory[0].Itemstack.StackSize;
		int num = stackSize - __state.stackSize;
		if (num > 0)
		{
			float valueOrDefault = ((BlockEntityContainer)__instance).Inventory[0].Itemstack.Attributes.TryGetFloat("quality").GetValueOrDefault();
			float valueOrDefault2 = __state.stack.Attributes.TryGetFloat("quality").GetValueOrDefault();
			if (!(valueOrDefault2 <= 0f) || !(valueOrDefault <= 0f))
			{
				float num2 = ((float)__state.stackSize * valueOrDefault + (float)num * valueOrDefault2) / (float)stackSize;
				((BlockEntityContainer)__instance).Inventory[0].Itemstack.Attributes.SetFloat("quality", num2);
			}
		}
	}
}
