using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsStoneBehavior : XSkillsExplodableBehavior
{
	public override PlayerAbility DropBonusAbility(PlayerSkill playerSkill)
	{
		return playerSkill[mining.StoneBreakerId];
	}

	public XSkillsStoneBehavior(Block block)
		: base(block)
	{
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref EnumHandling handling)
	{
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected I4, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Expected O, but got Unknown
		base.OnBlockBroken(world, pos, byPlayer, ref handling);
		if (mining == null)
		{
			return;
		}
		object obj;
		if (byPlayer == null)
		{
			obj = null;
		}
		else
		{
			EntityPlayer entity = byPlayer.Entity;
			if (entity == null)
			{
				obj = null;
			}
			else
			{
				PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
				obj = ((behavior != null) ? behavior[((Skill)mining).Id] : null);
			}
		}
		PlayerSkill val = (PlayerSkill)obj;
		if (val == null)
		{
			return;
		}
		IClientPlayer val2 = (IClientPlayer)(object)((byPlayer is IClientPlayer) ? byPlayer : null);
		PlayerAbility val3;
		if (val2 != null)
		{
			val3 = val[mining.GeologistId];
			if (val3 != null && val3.Tier >= 1)
			{
				mining.CheckBlock(world, val2, pos.X, pos.Y, pos.Z, pos.dimension, val3.Value(0, 0));
			}
		}
		ItemSlot activeHotbarSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		val3 = val[mining.TunnelDiggerId];
		object obj2;
		if (activeHotbarSlot == null)
		{
			obj2 = null;
		}
		else
		{
			ItemStack itemstack = activeHotbarSlot.Itemstack;
			obj2 = ((itemstack != null) ? itemstack.Item : null);
		}
		if (obj2 == null || val3 == null || byPlayer.CurrentBlockSelection == null || ((CollectibleObject)activeHotbarSlot.Itemstack.Item).Tool != (EnumTool?)1 || val3.Tier <= 0 || activeHotbarSlot.Itemstack.Attributes.GetInt("toolMode", 0) != 1 || activeHotbarSlot.Itemstack.Item is ItemProspectingPick || byPlayer.CurrentBlockSelection.Position != pos)
		{
			return;
		}
		PlayerAbility val4 = val[mining.DurabilityId];
		int num = 0;
		int num2 = 0;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i == 0 && j == 0)
				{
					continue;
				}
				EnumAxis axis = byPlayer.CurrentBlockSelection.Face.Axis;
				BlockPos val5;
				switch ((int)axis)
				{
				case 0:
					val5 = new BlockPos(pos.X, pos.Y + i, pos.Z + j, pos.dimension);
					break;
				case 1:
					val5 = new BlockPos(pos.X + i, pos.Y, pos.Z + j, pos.dimension);
					break;
				case 2:
					val5 = new BlockPos(pos.X + i, pos.Y + j, pos.Z, pos.dimension);
					break;
				default:
					continue;
				}
				if (((CollectibleObject)world.BlockAccessor.GetBlock(val5)).Id == ((CollectibleObject)((BlockBehavior)this).block).Id)
				{
					world.BlockAccessor.BreakBlock(val5, byPlayer, 1f);
					num2++;
					if (!((double?)((val4 != null) ? new float?(val4.SkillDependentFValue(0)) : null) > world.Rand.NextDouble()))
					{
						num++;
					}
				}
			}
		}
		num = (int)((float)num * (1f + val3.FValue(0, 0f)));
		num = Math.Min(num, ((CollectibleObject)activeHotbarSlot.Itemstack.Item).GetRemainingDurability(activeHotbarSlot.Itemstack) - 1);
		((CollectibleObject)activeHotbarSlot.Itemstack.Item).DamageItem(world, (Entity)(object)byPlayer.Entity, activeHotbarSlot, num);
		EntityBehaviorHunger behavior2 = ((Entity)byPlayer.Entity).GetBehavior<EntityBehaviorHunger>();
		if (behavior2 != null)
		{
			behavior2.ConsumeSaturation((float)(num2 * val3.Value(1, 0)));
		}
	}

	public override List<ItemStack> GetDropsList(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropChanceMultiplier, ref EnumHandling handling)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		List<ItemStack> list = new List<ItemStack>();
		if (Skill == null)
		{
			return list;
		}
		object obj;
		if (byPlayer == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj = ((behavior != null) ? behavior[((Skill)Skill).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj;
		if (val == null)
		{
			return list;
		}
		if (((BlockBehavior)this).block.Drops.Length == 0)
		{
			return list;
		}
		handling = (EnumHandling)2;
		PlayerAbility val2 = val.PlayerAbilities[mining.StoneCutterId];
		float num = ((val2 != null) ? val2.SkillDependentFValue(0) : 0f);
		if (world.Rand.NextDouble() <= (double)num)
		{
			list.Add(new ItemStack(((BlockBehavior)this).block, 1));
			return list;
		}
		val2 = DropBonusAbility(val);
		num = ((val2 != null) ? (dropChanceMultiplier + 0.01f * (float)val2.SkillDependentValue(0)) : dropChanceMultiplier);
		for (int i = 0; i < ((BlockBehavior)this).block.Drops.Length; i++)
		{
			ItemStack nextItemStack = ((BlockBehavior)this).block.Drops[i].GetNextItemStack(num);
			if (nextItemStack != null)
			{
				list.Add(nextItemStack);
			}
		}
		return list;
	}
}
