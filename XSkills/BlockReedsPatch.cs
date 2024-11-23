using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockReeds))]
public class BlockReedsPatch
{
	[HarmonyPatch("OnBlockBroken")]
	public static bool Prefix(BlockReeds __instance, IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Invalid comparison between Unknown and I4
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Invalid comparison between Unknown and I4
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		bool flag = false;
		BlockBehavior[] blockBehaviors = ((Block)__instance).BlockBehaviors;
		foreach (BlockBehavior obj in blockBehaviors)
		{
			EnumHandling val = (EnumHandling)0;
			obj.OnBlockBroken(world, pos, byPlayer, ref val);
			if ((int)val == 2)
			{
				flag = true;
			}
			if ((int)val == 3)
			{
				return false;
			}
		}
		if (flag)
		{
			return false;
		}
		object obj2;
		if (byPlayer == null)
		{
			obj2 = null;
		}
		else
		{
			XLeveling obj3 = XLeveling.Instance(((Entity)byPlayer.Entity).Api);
			obj2 = ((obj3 != null) ? obj3.GetSkill("farming", false) : null) as Farming;
		}
		Farming farming = (Farming)obj2;
		object obj4;
		if (farming == null)
		{
			obj4 = null;
		}
		else if (byPlayer == null)
		{
			obj4 = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj4 = ((behavior != null) ? behavior[((Skill)farming).Id] : null);
		}
		PlayerSkill val2 = (PlayerSkill)obj4;
		if ((int)world.Side == 1 && (byPlayer == null || (int)byPlayer.WorldData.CurrentGameMode != 2))
		{
			BlockDropItemStack[] drops = ((Block)__instance).Drops;
			foreach (BlockDropItemStack obj5 in drops)
			{
				float num = 1f;
				ItemStack resolvedItemstack = obj5.ResolvedItemstack;
				if (!(((resolvedItemstack != null) ? resolvedItemstack.Item : null) is ItemCattailRoot) && val2 != null)
				{
					PlayerAbility val3 = val2[farming.GathererId];
					num += ((val3 != null) ? val3.SkillDependentFValue(0) : 0f);
				}
				ItemStack nextItemStack = obj5.GetNextItemStack(num);
				if (nextItemStack != null)
				{
					world.SpawnItemEntity(nextItemStack, new Vec3d((double)pos.X + 0.5, (double)pos.Y + 0.5, (double)pos.Z + 0.5), (Vec3d)null);
				}
			}
			world.PlaySoundAt(((Block)__instance).Sounds.GetBreakSound(byPlayer), (double)pos.X, (double)pos.Y, (double)pos.Z, byPlayer, true, 32f, 1f);
		}
		bool flag2 = false;
		if (byPlayer != null)
		{
			PlayerAbility val3 = ((val2 != null) ? val2[farming.CarefulHandsId] : null);
			flag2 = byPlayer.InventoryManager.ActiveTool == (EnumTool?)0 || byPlayer.InventoryManager.ActiveTool == (EnumTool?)9 || byPlayer.InventoryManager.ActiveTool == (EnumTool?)13 || (val3 != null && val3.Tier > 0);
		}
		if (((RegistryObject)__instance).Variant["state"] == "normal" && flag2)
		{
			world.BlockAccessor.SetBlock(world.GetBlock(((RegistryObject)__instance).CodeWithVariants(new string[2] { "habitat", "state" }, new string[2] { "land", "harvested" })).BlockId, pos);
			return false;
		}
		((Block)__instance).SpawnBlockBrokenParticles(pos);
		world.BlockAccessor.SetBlock(0, pos);
		return false;
	}
}
