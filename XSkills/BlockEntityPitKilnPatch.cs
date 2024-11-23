using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityPitKiln))]
public class BlockEntityPitKilnPatch
{
	public static bool Prepare(MethodBase original)
	{
		XSkills instance = XSkills.Instance;
		if (instance == null)
		{
			return false;
		}
		instance.Skills.TryGetValue("pottery", out var value);
		if (!(value is Pottery pottery) || !((Skill)pottery).Enabled)
		{
			return false;
		}
		if (original == null)
		{
			return true;
		}
		return ((Skill)pottery)[pottery.InspirationId].Enabled;
	}

	[HarmonyPostfix]
	[HarmonyPatch("TryIgnite")]
	public static void TryIgnitePostfix(BlockEntityPitKiln __instance, IPlayer byPlayer)
	{
		if (byPlayer == null || byPlayer.PlayerUID == null)
		{
			return;
		}
		foreach (ItemSlot item in ((BlockEntityContainer)__instance).Inventory)
		{
			if (item.Itemstack != null)
			{
				item.Itemstack.Attributes.SetString("owner", byPlayer.PlayerUID);
				break;
			}
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch("OnFired")]
	public static void OnFiredPrefix(BlockEntityPitKiln __instance, ref IPlayer __state)
	{
		__state = null;
		if (((BlockEntityContainer)__instance).Inventory == null)
		{
			return;
		}
		foreach (ItemSlot item in ((BlockEntityContainer)__instance).Inventory)
		{
			if (item.Itemstack != null)
			{
				ITreeAttribute attributes = item.Itemstack.Attributes;
				string text = ((attributes != null) ? attributes.GetString("owner", (string)null) : null);
				if (text != null)
				{
					__state = ((BlockEntity)__instance).Api.World.PlayerByUid(text);
					break;
				}
			}
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch("OnFired")]
	public static void OnFiredPostfix(BlockEntityPitKiln __instance, IPlayer __state)
	{
		if (__state == null || ((BlockEntityContainer)__instance).Inventory == null)
		{
			return;
		}
		foreach (ItemSlot item in ((BlockEntityContainer)__instance).Inventory)
		{
			if (item.Itemstack != null)
			{
				PotteryHelper.ApplyOnStack(__state, ((BlockEntity)__instance).Api.World, item);
			}
			((BlockEntity)__instance).MarkDirty(true, (IPlayer)null);
		}
	}
}
