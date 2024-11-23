using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace XSkills;

internal class CookingRecipePatch : ManualPatch
{
	public static void Apply(Harmony harmony, Type cookingRecipeType)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		MethodInfo method = cookingRecipeType.GetMethod("Matches", new Type[2]
		{
			typeof(ItemStack[]),
			typeof(int).MakeByRefType()
		});
		MethodInfo method2 = typeof(CookingRecipePatch).GetMethod("MatchesPrefix");
		HarmonyMethod val = ((!(method2 != null)) ? ((HarmonyMethod)null) : new HarmonyMethod(method2));
		harmony.Patch((MethodBase)method, val, (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
	}

	public static bool MatchesPrefix(CookingRecipe __instance, ref bool __result, ItemStack[] inputStacks, ref int quantityServings)
	{
		List<ItemStack> list = new List<ItemStack>(inputStacks);
		List<CookingRecipeIngredient> list2 = new List<CookingRecipeIngredient>(__instance.Ingredients);
		__result = false;
		int num = 99999;
		int[] array = new int[list2.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 0;
		}
		while (list.Count > 0)
		{
			ItemStack val = list[0];
			list.RemoveAt(0);
			if (val == null)
			{
				continue;
			}
			bool flag = false;
			for (int j = 0; j < list2.Count; j++)
			{
				CookingRecipeIngredient val2 = list2[j];
				if (val2.Matches(val) && array[j] < val2.MaxQuantity)
				{
					int val3 = val.StackSize;
					if (val.Collectible.Attributes["waterTightContainerProps"].Exists)
					{
						WaterTightContainableProps containableProps = BlockLiquidContainerBase.GetContainableProps(val);
						val3 = (int)((float)val.StackSize / containableProps.ItemsPerLitre / __instance.GetIngrendientFor(val, Array.Empty<CookingRecipeIngredient>()).PortionSizeLitres);
					}
					num = Math.Min(num, val3);
					array[j]++;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		for (int k = 0; k < list2.Count; k++)
		{
			if (array[k] < list2[k].MinQuantity)
			{
				return false;
			}
		}
		quantityServings = num;
		if (quantityServings <= 0)
		{
			__result = false;
			return false;
		}
		foreach (ItemStack val4 in inputStacks)
		{
			if (val4 != null)
			{
				int num2 = val4.StackSize;
				if (val4.Collectible.Attributes["waterTightContainerProps"].Exists)
				{
					WaterTightContainableProps containableProps2 = BlockLiquidContainerBase.GetContainableProps(val4);
					num2 = (int)((float)val4.StackSize / containableProps2.ItemsPerLitre / __instance.GetIngrendientFor(val4, Array.Empty<CookingRecipeIngredient>()).PortionSizeLitres);
				}
				if (num2 != quantityServings)
				{
					return false;
				}
			}
		}
		__result = true;
		return false;
	}
}
