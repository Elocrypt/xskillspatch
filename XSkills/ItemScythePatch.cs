using HarmonyLib;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(ItemScythe))]
public class ItemScythePatch
{
	[HarmonyPatch(/*Could not decode attribute arguments.*/)]
	public static void Postfix(ref int __result)
	{
		__result = (int)((float)__result * Farming.MultiBreakMultiplier);
	}
}
