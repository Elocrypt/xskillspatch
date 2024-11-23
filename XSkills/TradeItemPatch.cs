using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(TradeItem))]
internal class TradeItemPatch
{
	[HarmonyPostfix]
	[HarmonyPatch("Resolve")]
	public static void ResolvePostfix(ResolvedTradeItem __result, IWorldAccessor world)
	{
		ItemStack stack = __result.Stack;
		if (stack == null || !(stack.Collectible is ItemSkillBook))
		{
			return;
		}
		XLeveling val = XLeveling.Instance(world.Api);
		if (val == null)
		{
			return;
		}
		ITreeAttribute attributes = stack.Attributes;
		string text = ((attributes != null) ? attributes.GetString("skill", (string)null) : null);
		if (text != null)
		{
			Skill skill = val.GetSkill(text, false);
			if (skill != null)
			{
				float num = (float)(0.10000000149011612 + world.Api.World.Rand.NextDouble() * 1.0);
				float num2 = skill.ExpBase * num;
				__result.Price = (int)((float)__result.Price * num + 1.5f);
				stack.Attributes.SetFloat("experience", num2);
			}
		}
	}
}
