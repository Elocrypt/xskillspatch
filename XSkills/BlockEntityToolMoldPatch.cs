using System.Reflection;
using System.Text;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityToolMold))]
public class BlockEntityToolMoldPatch
{
	public static bool Prepare(MethodBase original)
	{
		XSkills instance = XSkills.Instance;
		if (instance == null)
		{
			return false;
		}
		instance.Skills.TryGetValue("metalworking", out var value);
		if (!(value is Metalworking metalworking) || !((Skill)metalworking).Enabled)
		{
			return false;
		}
		if (original == null)
		{
			return true;
		}
		if (original.Name == "GetBlockInfo")
		{
			return ((Skill)metalworking)[metalworking.SmelterId].Enabled;
		}
		return true;
	}

	[HarmonyPostfix]
	[HarmonyPatch("GetBlockInfo")]
	public static void GetBlockInfoPostfix(BlockEntityToolMold __instance, IPlayer forPlayer, StringBuilder dsc)
	{
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		PlayerSkillSet behavior = ((Entity)forPlayer.Entity).GetBehavior<PlayerSkillSet>();
		if (behavior == null)
		{
			return;
		}
		XLeveling xLeveling = behavior.XLeveling;
		if (!(((xLeveling != null) ? xLeveling.GetSkill("metalworking", false) : null) is Metalworking metalworking))
		{
			return;
		}
		PlayerSkill obj = behavior[((Skill)metalworking).Id];
		PlayerAbility val = ((obj != null) ? obj[metalworking.SmelterId] : null);
		if (val == null || val.Tier <= 0)
		{
			return;
		}
		if (__instance.metalContent != null)
		{
			ItemStack[] moldedStacks = __instance.GetMoldedStacks(__instance.metalContent);
			for (int i = 0; i < moldedStacks.Length; i++)
			{
				if (moldedStacks[i].Collectible.CombustibleProps != null)
				{
					return;
				}
			}
		}
		else
		{
			ItemStack val2 = new ItemStack(((BlockEntity)__instance).Api.World.GetItem(new AssetLocation("game", "ingot-copper")), 1);
			ItemStack[] moldedStacks = __instance.GetMoldedStacks(val2);
			for (int i = 0; i < moldedStacks.Length; i++)
			{
				if (moldedStacks[i].Collectible.CombustibleProps != null)
				{
					return;
				}
			}
		}
		dsc.Append(Lang.Get("xskills:resourcereduction", new object[1] { val.FValue(0, 0f) }));
	}
}
