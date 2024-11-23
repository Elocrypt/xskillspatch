using System;
using System.Collections.Generic;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockFullCoating))]
public class BlockFullCoatingPatch
{
	[HarmonyPatch("OnLoaded")]
	public static void Postfix(Block __instance, ICoreAPI api)
	{
		BlockBehavior[] blockBehaviors = __instance.BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			((CollectibleBehavior)blockBehaviors[i]).OnLoaded(api);
		}
	}

	[HarmonyPatch("GetDrops")]
	public static bool Prefix(Block __instance, ref ItemStack[] __result, BlockFacing[] ___ownFacings, IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		bool flag = false;
		List<ItemStack> list = new List<ItemStack>();
		BlockBehavior[] blockBehaviors = __instance.BlockBehaviors;
		foreach (BlockBehavior obj in blockBehaviors)
		{
			EnumHandling val = (EnumHandling)0;
			ItemStack[] drops = obj.GetDrops(world, pos, byPlayer, ref dropQuantityMultiplier, ref val);
			if (drops != null)
			{
				list.AddRange(drops);
			}
			if ((int)val == 3)
			{
				__result = drops;
				flag = true;
			}
			if ((int)val == 2)
			{
				flag = true;
			}
		}
		if (flag)
		{
			__result = list.ToArray();
			return false;
		}
		for (int j = 0; j < __instance.Drops.Length; j++)
		{
			int num = 0;
			if (___ownFacings.Length == 1)
			{
				float num2 = dropQuantityMultiplier;
				num += (int)num2 + (((double)(num2 - (float)(int)num2) > world.Rand.NextDouble()) ? 1 : 0);
			}
			else
			{
				for (int k = 0; k < ___ownFacings.Length; k++)
				{
					float num3 = __instance.Drops[j].Quantity.nextFloat() * dropQuantityMultiplier;
					num += (int)num3 + (((double)(num3 - (float)(int)num3) > world.Rand.NextDouble()) ? 1 : 0);
				}
			}
			ItemStack val2 = __instance.Drops[j].ResolvedItemstack.Clone();
			val2.StackSize = Math.Max(1, num);
			list.Add(val2);
		}
		__result = list.ToArray();
		return false;
	}
}
