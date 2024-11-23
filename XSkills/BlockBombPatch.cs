using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockBomb))]
public class BlockBombPatch
{
	[HarmonyPatch("OnBlockExploded")]
	public static void Postfix(BlockBomb __instance, IWorldAccessor world, BlockPos pos, BlockPos explosionCenter, EnumBlastType blastType)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		EnumHandling val = (EnumHandling)0;
		BlockBehavior[] blockBehaviors = ((Block)__instance).BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			blockBehaviors[i].OnBlockExploded(world, pos, explosionCenter, blastType, ref val);
			if ((int)val == 3)
			{
				break;
			}
		}
	}
}
