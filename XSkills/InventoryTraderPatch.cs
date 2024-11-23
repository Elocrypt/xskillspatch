using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(InventoryTrader))]
public class InventoryTraderPatch
{
	[HarmonyPrefix]
	[HarmonyPatch("GetPlayerAssets")]
	public static void GetPlayerAssetsPrefix(EntityAgent eagent)
	{
		EntityAgent obj = ((eagent is EntityPlayer) ? eagent : null);
		object obj2;
		if (obj == null)
		{
			obj2 = null;
		}
		else
		{
			IPlayer player = ((EntityPlayer)obj).Player;
			if (player == null)
			{
				obj2 = null;
			}
			else
			{
				IPlayerInventoryManager inventoryManager = player.InventoryManager;
				obj2 = ((inventoryManager != null) ? inventoryManager.GetOwnInventory("xskillshotbar") : null);
			}
		}
		(obj2 as XSkillsPlayerInventory).Linked = false;
	}

	[HarmonyPostfix]
	[HarmonyPatch("GetPlayerAssets")]
	public static void GetPlayerAssetsPostfix(EntityAgent eagent)
	{
		EntityAgent obj = ((eagent is EntityPlayer) ? eagent : null);
		object obj2;
		if (obj == null)
		{
			obj2 = null;
		}
		else
		{
			IPlayer player = ((EntityPlayer)obj).Player;
			if (player == null)
			{
				obj2 = null;
			}
			else
			{
				IPlayerInventoryManager inventoryManager = player.InventoryManager;
				obj2 = ((inventoryManager != null) ? inventoryManager.GetOwnInventory("xskillshotbar") : null);
			}
		}
		(obj2 as XSkillsPlayerInventory).Linked = true;
	}

	[HarmonyPrefix]
	[HarmonyPatch("DeductFromEntity")]
	public static void DeductFromEntityPrefix(EntityAgent eagent)
	{
		EntityAgent obj = ((eagent is EntityPlayer) ? eagent : null);
		object obj2;
		if (obj == null)
		{
			obj2 = null;
		}
		else
		{
			IPlayer player = ((EntityPlayer)obj).Player;
			if (player == null)
			{
				obj2 = null;
			}
			else
			{
				IPlayerInventoryManager inventoryManager = player.InventoryManager;
				obj2 = ((inventoryManager != null) ? inventoryManager.GetOwnInventory("xskillshotbar") : null);
			}
		}
		(obj2 as XSkillsPlayerInventory).Linked = false;
	}

	[HarmonyPostfix]
	[HarmonyPatch("DeductFromEntity")]
	public static void DeductFromEntityPostfix(EntityAgent eagent)
	{
		EntityAgent obj = ((eagent is EntityPlayer) ? eagent : null);
		object obj2;
		if (obj == null)
		{
			obj2 = null;
		}
		else
		{
			IPlayer player = ((EntityPlayer)obj).Player;
			if (player == null)
			{
				obj2 = null;
			}
			else
			{
				IPlayerInventoryManager inventoryManager = player.InventoryManager;
				obj2 = ((inventoryManager != null) ? inventoryManager.GetOwnInventory("xskillshotbar") : null);
			}
		}
		(obj2 as XSkillsPlayerInventory).Linked = true;
	}
}
