using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(ItemWorkItem))]
internal class ItemWorkItemPatch
{
	[HarmonyPatch("GetHelveWorkableMode")]
	public static void Postfix(ItemWorkItem __instance, ref EnumHelveWorkableMode __result, BlockEntityAnvil beAnvil)
	{
		if (__result || ((beAnvil != null) ? beAnvil.SelectedRecipe : null) == null)
		{
			return;
		}
		XLeveling modSystem = ((BlockEntity)beAnvil).Api.ModLoader.GetModSystem<XLeveling>(true);
		if (!(((modSystem != null) ? modSystem.GetSkill("metalworking", false) : null) is Metalworking metalworking))
		{
			return;
		}
		IPlayer usedByPlayer = beAnvil.GetUsedByPlayer();
		object obj;
		if (usedByPlayer == null)
		{
			obj = null;
		}
		else
		{
			EntityPlayer entity = usedByPlayer.Entity;
			if (entity == null)
			{
				obj = null;
			}
			else
			{
				PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
				if (behavior == null)
				{
					obj = null;
				}
				else
				{
					PlayerSkill obj2 = behavior[((Skill)metalworking).Id];
					obj = ((obj2 != null) ? obj2[metalworking.AutomatedSmithingId] : null);
				}
			}
		}
		PlayerAbility val = (PlayerAbility)obj;
		if (val == null)
		{
			return;
		}
		int num = val.Value(0, 0);
		if (num <= 0)
		{
			return;
		}
		bool[,,] voxels = ((LayeredVoxelRecipe<SmithingRecipe>)(object)beAnvil.SelectedRecipe).Voxels;
		for (int i = 0; i < ((LayeredVoxelRecipe<SmithingRecipe>)(object)beAnvil.SelectedRecipe).QuantityLayers; i++)
		{
			bool flag = true;
			for (int j = 0; j < voxels.GetLength(0) && flag; j++)
			{
				for (int k = 0; k < voxels.GetLength(2) && flag; k++)
				{
					if (voxels[j, i, k])
					{
						flag = false;
						if (i > num)
						{
							return;
						}
					}
				}
			}
		}
		__result = (EnumHelveWorkableMode)2;
	}
}
