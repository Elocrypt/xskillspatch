using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityFirepit))]
public class BlockEntityFirepitPatch
{
	public static bool ContainsFood(BlockEntityFirepit firepit)
	{
		ItemSlot inputSlot = firepit.inputSlot;
		object obj;
		if (inputSlot == null)
		{
			obj = null;
		}
		else
		{
			ItemStack itemstack = inputSlot.Itemstack;
			obj = ((itemstack != null) ? itemstack.Collectible : null);
		}
		CollectibleObject val = (CollectibleObject)obj;
		if (val == null)
		{
			return false;
		}
		bool flag = val is BlockCookingContainer || val is BlockBucket;
		if (!flag)
		{
			CombustibleProperties combustibleProps = val.CombustibleProps;
			object obj2;
			if (combustibleProps == null)
			{
				obj2 = null;
			}
			else
			{
				JsonItemStack smeltedStack = combustibleProps.SmeltedStack;
				if (smeltedStack == null)
				{
					obj2 = null;
				}
				else
				{
					ItemStack resolvedItemstack = smeltedStack.ResolvedItemstack;
					obj2 = ((resolvedItemstack == null) ? null : resolvedItemstack.Collectible?.NutritionProps);
				}
			}
			flag = obj2 != null;
		}
		return flag;
	}

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
		if (!((Skill)cooking)[cooking.FastFoodId].Enabled)
		{
			return ((Skill)cooking)[cooking.WellDoneId].Enabled;
		}
		return true;
	}

	[HarmonyPostfix]
	[HarmonyPatch("maxCookingTime")]
	public static void maxCookingTimePostfix(BlockEntityFirepit __instance, ref float __result)
	{
		if (ContainsFood(__instance))
		{
			__result *= CookingUtil.GetCookingTimeMultiplier((BlockEntity)(object)__instance);
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch("SetDialogValues")]
	public static void SetDialogValuesPostfix(BlockEntityFirepit __instance, ITreeAttribute dialogTree)
	{
		if (ContainsFood(__instance))
		{
			float? num = dialogTree.TryGetFloat("maxOreCookingTime");
			if (num.HasValue)
			{
				dialogTree.SetFloat("maxOreCookingTime", num.Value * CookingUtil.GetCookingTimeMultiplier((BlockEntity)(object)__instance));
			}
		}
	}
}
