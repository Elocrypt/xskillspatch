using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockLayeredSlowDig))]
public class BlockLayeredSlowDigPatch
{
	[HarmonyPostfix]
	[HarmonyPatch("OnLoaded")]
	public static void OnLoadedPostfix(BlockLayeredSlowDig __instance, ICoreAPI api)
	{
		CollectibleBehavior[] collectibleBehaviors = ((CollectibleObject)__instance).CollectibleBehaviors;
		for (int i = 0; i < collectibleBehaviors.Length; i++)
		{
			collectibleBehaviors[i].OnLoaded(api);
		}
	}
}
