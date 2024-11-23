using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockSkep))]
public class BlockSkepPatch
{
	[HarmonyPatch("OnBlockInteractStart")]
	public static bool Prefix(BlockSkep __instance, ref bool __result, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		EnumHandling val = (EnumHandling)0;
		bool flag = false;
		__result = false;
		BlockBehavior[] blockBehaviors = ((Block)__instance).BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (blockBehaviors[i].OnBlockInteractStart(world, byPlayer, blockSel, ref val))
			{
				__result = true;
			}
			if ((int)val != 0)
			{
				flag = true;
			}
			if ((int)val == 3)
			{
				break;
			}
		}
		return !flag;
	}
}
