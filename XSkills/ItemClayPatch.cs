using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(ItemClay))]
public class ItemClayPatch
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
		return ((Skill)pottery)[pottery.ThriftId].Enabled;
	}

	[HarmonyPrefix]
	[HarmonyPatch("OnHeldInteractStop")]
	public static void OnHeldInteractStopPrefix(ItemClay __instance, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel)
	{
		if (blockSel != null)
		{
			BlockEntity blockEntity = ((Entity)byEntity).World.BlockAccessor.GetBlockEntity(blockSel.Position);
			PotteryHelper.AddClay((BlockEntityClayForm)(object)((blockEntity is BlockEntityClayForm) ? blockEntity : null), slot, byEntity);
		}
	}
}
