using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityTrough))]
public class BlockEntityTroughPatch
{
	[HarmonyPatch("OnInteract")]
	public static void Prefix(BlockEntityTrough __instance, out int __state)
	{
		ItemStack itemstack = ((BlockEntityContainer)__instance).Inventory[0].Itemstack;
		__state = ((itemstack != null) ? itemstack.StackSize : 0);
	}

	[HarmonyPatch("OnInteract")]
	public static void Postfix(BlockEntityTrough __instance, int __state, IPlayer byPlayer)
	{
		ItemStack itemstack = ((BlockEntityContainer)__instance).Inventory[0].Itemstack;
		if (__state < ((itemstack != null) ? itemstack.StackSize : 0))
		{
			((BlockEntityContainer)__instance).Inventory[0].Itemstack.Attributes.SetString("owner", byPlayer.PlayerUID);
		}
	}

	[HarmonyPatch("ConsumeOnePortion")]
	public static bool Prefix(BlockEntityTrough __instance, out float __result)
	{
		__result = 0f;
		IPlayer owner = __instance.GetOwner();
		if (owner == null)
		{
			return true;
		}
		XLeveling obj = XLeveling.Instance(((BlockEntity)__instance).Api);
		if (!(((obj != null) ? obj.GetSkill("husbandry", false) : null) is Husbandry husbandry))
		{
			return true;
		}
		EntityPlayer entity = owner.Entity;
		object obj2;
		if (entity == null)
		{
			obj2 = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
			obj2 = ((behavior != null) ? behavior[((Skill)husbandry).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj2;
		if (val == null)
		{
			return true;
		}
		PlayerAbility val2 = val[husbandry.FeederId];
		if (val2 == null)
		{
			return true;
		}
		if (((BlockEntity)__instance).Api.World.Rand.NextDouble() < (double)val2.SkillDependentFValue(0))
		{
			__result = 1f;
			return false;
		}
		return true;
	}
}
