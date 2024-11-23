using System;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace XSkills;

public class XSkillsCookingRecipeNaming : ICookingRecipeNamingHelper
{
	public string GetNameForIngredients(IWorldAccessor world, string recipeCode, ItemStack[] stacks)
	{
		string text = "";
		bool flag = true;
		foreach (ItemStack val in stacks)
		{
			if (!flag)
			{
				text += ", ";
			}
			if (((val != null) ? ((RegistryObject)val.Collectible).Code.Path : null) == "waterportion")
			{
				return Lang.Get("game:item-salt", Array.Empty<object>());
			}
			if (val != null)
			{
				text += val.GetName();
			}
			flag = false;
		}
		return text;
	}
}
