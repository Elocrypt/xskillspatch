using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BarrelRecipe))]
public class BarrelRecipePatch
{
	[HarmonyPatch("TryCraftNow")]
	public static void Prefix(ItemSlot[] inputslots, out ItemStack __state)
	{
		object obj;
		if (inputslots == null)
		{
			obj = null;
		}
		else
		{
			ItemSlot obj2 = inputslots[1];
			if (obj2 == null)
			{
				obj = null;
			}
			else
			{
				ItemStack itemstack = obj2.Itemstack;
				obj = ((itemstack != null) ? itemstack.Clone() : null);
			}
		}
		__state = (ItemStack)obj;
	}

	[HarmonyPatch("TryCraftNow")]
	public static void Postfix(ItemSlot[] inputslots, ItemStack __state, bool __result)
	{
		if (!__result || inputslots == null || inputslots.Length < 2 || __state == null)
		{
			return;
		}
		int num = 0;
		if (inputslots[1].Itemstack == null)
		{
			num = 0;
			inputslots[1].Itemstack = __state;
		}
		else
		{
			if (inputslots[1].Itemstack.Collectible != __state.Collectible)
			{
				return;
			}
			num = inputslots[1].Itemstack.StackSize;
		}
		num += (int)((float)(__state.StackSize - num) * (1f - __state.Attributes.GetFloat("usage", 1f)));
		if (num > 0 && num != inputslots[1].Itemstack.StackSize)
		{
			int num2 = num % 100;
			num -= num2;
			num2 = ((num2 >= 50) ? 100 : 0);
			inputslots[1].Itemstack.StackSize = num + num2;
		}
		if (num <= 0)
		{
			inputslots[1].Itemstack = null;
		}
		inputslots[1].MarkDirty();
	}
}
