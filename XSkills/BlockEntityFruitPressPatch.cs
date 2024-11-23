using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityFruitPress))]
public class BlockEntityFruitPressPatch
{
	public static bool Prepare(MethodBase original)
	{
		XSkills instance = XSkills.Instance;
		if (instance == null)
		{
			return false;
		}
		instance.Skills.TryGetValue("cooking", out var value);
		if (!(value is Cooking cooking) || !((Skill)cooking).Enabled)
		{
			return false;
		}
		return ((Skill)cooking)[cooking.JuicerId].Enabled;
	}

	[HarmonyPostfix]
	[HarmonyPatch("OnBlockInteractStart")]
	public static void OnBlockInteractStartPostfix(BlockEntityFruitPress __instance, bool __result, IPlayer byPlayer)
	{
		if (__result)
		{
			BlockEntityBehaviorOwnable blockEntityBehaviorOwnable = ((__instance != null) ? ((BlockEntity)__instance).GetBehavior<BlockEntityBehaviorOwnable>() : null);
			if (blockEntityBehaviorOwnable != null)
			{
				blockEntityBehaviorOwnable.Owner = byPlayer;
			}
		}
	}

	protected static float CurrentLitres(BlockEntityFruitPress press)
	{
		ItemSlot bucketSlot = press.BucketSlot;
		object obj;
		if (bucketSlot == null)
		{
			obj = null;
		}
		else
		{
			ItemStack itemstack = bucketSlot.Itemstack;
			obj = ((itemstack != null) ? itemstack.Collectible : null);
		}
		BlockLiquidContainerBase val = (BlockLiquidContainerBase)((obj is BlockLiquidContainerBase) ? obj : null);
		if (val == null)
		{
			return 0f;
		}
		return val.GetCurrentLitres(press.BucketSlot.Itemstack);
	}

	[HarmonyPrefix]
	[HarmonyPatch("onTick100msServer")]
	public static void onTick100msServerPrefix(BlockEntityFruitPress __instance, ref float __state)
	{
		__state = CurrentLitres(__instance);
	}

	[HarmonyPostfix]
	[HarmonyPatch("onTick100msServer")]
	public static void onTick100msServerPostfix(BlockEntityFruitPress __instance, float __state)
	{
		float num = CurrentLitres(__instance);
		if (num <= __state)
		{
			return;
		}
		IPlayer val = ((__instance != null) ? ((BlockEntity)__instance).GetBehavior<BlockEntityBehaviorOwnable>() : null)?.Owner;
		if (val == null)
		{
			return;
		}
		XLeveling obj = XLeveling.Instance(((BlockEntity)__instance).Api);
		if (((obj != null) ? obj.GetSkill("cooking", false) : null) is Cooking cooking)
		{
			PlayerSkillSet behavior = ((Entity)val.Entity).GetBehavior<PlayerSkillSet>();
			PlayerSkill val2 = ((behavior != null) ? behavior[((Skill)cooking).Id] : null);
			if (val2 != null)
			{
				float fruitPressExpPerLitre = (((Skill)cooking).Config as CookingSkillConfig).fruitPressExpPerLitre;
				float num2 = num - __state;
				val2.AddExperience(num2 * fruitPressExpPerLitre, true);
			}
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch("getJuiceableProps")]
	public static void getJuiceablePropsPostfix(BlockEntityFruitPress __instance, JuiceableProperties __result)
	{
		if (__result == null)
		{
			return;
		}
		IPlayer val = ((__instance != null) ? ((BlockEntity)__instance).GetBehavior<BlockEntityBehaviorOwnable>() : null)?.Owner;
		if (val == null)
		{
			return;
		}
		XLeveling obj = XLeveling.Instance(((BlockEntity)__instance).Api);
		if (!(((obj != null) ? obj.GetSkill("cooking", false) : null) is Cooking cooking))
		{
			return;
		}
		EntityPlayer entity = val.Entity;
		object obj2;
		if (entity == null)
		{
			obj2 = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
			if (behavior == null)
			{
				obj2 = null;
			}
			else
			{
				PlayerSkill obj3 = behavior[((Skill)cooking).Id];
				obj2 = ((obj3 != null) ? obj3[cooking.JuicerId] : null);
			}
		}
		PlayerAbility val2 = (PlayerAbility)obj2;
		if (val2 != null)
		{
			__result.LitresPerItem *= 1f + val2.FValue(0, 0f);
		}
	}
}
