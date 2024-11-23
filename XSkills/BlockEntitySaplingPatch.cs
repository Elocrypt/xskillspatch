using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntitySapling))]
public class BlockEntitySaplingPatch
{
	[HarmonyPatch("CheckGrow")]
	public static void Prefix(BlockEntitySapling __instance, out EnumTreeGrowthStage __state, EnumTreeGrowthStage ___stage)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Expected I4, but got Unknown
		__state = (EnumTreeGrowthStage)(int)___stage;
	}

	[HarmonyPatch("CheckGrow")]
	public static void Postfix(BlockEntitySapling __instance, EnumTreeGrowthStage __state, EnumTreeGrowthStage ___stage, ref double ___totalHoursTillGrowth)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		if (__instance is XSkillsBlockEntitySapling xSkillsBlockEntitySapling && __state != ___stage)
		{
			double totalHours = ((BlockEntity)xSkillsBlockEntitySapling).Api.World.Calendar.TotalHours;
			double num = (___totalHoursTillGrowth - totalHours) * (double)xSkillsBlockEntitySapling.GrowthTimeMultiplier;
			___totalHoursTillGrowth = totalHours + num;
		}
	}
}
