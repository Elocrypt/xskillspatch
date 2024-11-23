using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(ItemShears))]
public class ItemShearsPatch
{
	[HarmonyPatch(/*Could not decode attribute arguments.*/)]
	public static void Postfix(ref int __result)
	{
		__result = (int)((float)__result * Farming.MultiBreakMultiplier);
	}

	[HarmonyPatch("OnBlockBrokenWith")]
	public static void Prefix(Entity byEntity)
	{
		XLeveling obj = XLeveling.Instance(byEntity?.Api);
		if (((obj != null) ? obj.GetSkill("farming", false) : null) is Farming farming)
		{
			PlayerSkillSet behavior = byEntity.GetBehavior<PlayerSkillSet>();
			object obj2;
			if (behavior == null)
			{
				obj2 = null;
			}
			else
			{
				PlayerSkill obj3 = behavior[((Skill)farming).Id];
				obj2 = ((obj3 != null) ? obj3[farming.BrightHarvestsId] : null);
			}
			PlayerAbility val = (PlayerAbility)obj2;
			if (val != null)
			{
				Farming.MultiBreakMultiplier = 1f + val.FValue(0, 0f);
			}
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch("GetNearblyMultibreakables")]
	public static bool Prefix(ItemShears __instance, out OrderedDictionary<BlockPos, float> __result, IWorldAccessor world, BlockPos pos, Vec3d hitPos)
	{
		__result = new OrderedDictionary<BlockPos, float>();
		int num = ((__instance.MultiBreakQuantity <= 9) ? 1 : 2);
		for (int i = -num; i <= num; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				for (int k = -num; k <= num; k++)
				{
					if (i != 0 || j != 0 || k != 0)
					{
						BlockPos val = pos.AddCopy(i, j, k);
						if (__instance.CanMultiBreak(world.BlockAccessor.GetBlock(val)))
						{
							__result.Add(val, hitPos.SquareDistanceTo((double)val.X + 0.5, (double)val.Y + 0.5, (double)val.Z + 0.5));
						}
					}
				}
			}
		}
		return false;
	}
}
