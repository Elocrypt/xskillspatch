using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsOreBehavior : XSkillsExplodableBehavior
{
	protected static bool veinMining;

	public override PlayerAbility DropBonusAbility(PlayerSkill playerSkill)
	{
		return playerSkill[mining.OreMinerId];
	}

	public override float GetXP(IWorldAccessor world, BlockPos pos, IPlayer byPlayer)
	{
		MiningSkillConfig miningSkillConfig = ((Skill)mining).Config as MiningSkillConfig;
		return xp * (1f + mining.GetOreRarity(((RegistryObject)((BlockBehavior)this).block).FirstCodePart(2)) * miningSkillConfig.oreRarityExpMultiplier + miningSkillConfig.oreDepthExpMultiplier * (1f - (float)(Math.Min(pos.Y, world.SeaLevel) / world.SeaLevel)));
	}

	public XSkillsOreBehavior(Block block)
		: base(block)
	{
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
	{
		return base.GetDrops(world, pos, byPlayer, ref dropChanceMultiplier, ref handling);
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref EnumHandling handling)
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Invalid comparison between Unknown and I4
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		base.OnBlockBroken(world, pos, byPlayer, ref handling);
		IClientPlayer val = (IClientPlayer)(object)((byPlayer is IClientPlayer) ? byPlayer : null);
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
		PlayerSkill val2 = (PlayerSkill)obj;
		if (val2 == null)
		{
			return;
		}
		if (val != null && !veinMining)
		{
			Block block = ((BlockBehavior)this).block;
			Block obj2 = ((block is BlockOre) ? block : null);
			if (((obj2 != null) ? ((BlockOre)obj2).OreName : null) == "quartz")
			{
				PlayerAbility val3 = val2[mining.GeologistId];
				if (val3 != null && val3.Tier >= 1)
				{
					mining.CheckBlock(world, val, pos.X, pos.Y, pos.Z, pos.dimension, val3.Value(0, 0));
				}
			}
		}
		if (veinMining || (int)world.Api.Side != 1)
		{
			return;
		}
		ItemSlot activeHotbarSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		PlayerAbility val4 = val2[mining.VeinMinerId];
		object obj3;
		if (activeHotbarSlot == null)
		{
			obj3 = null;
		}
		else
		{
			ItemStack itemstack = activeHotbarSlot.Itemstack;
			obj3 = ((itemstack != null) ? itemstack.Item : null);
		}
		Item val5 = (Item)obj3;
		if (val5 == null || ((CollectibleObject)val5).Tool != (EnumTool?)1 || val5 is ItemProspectingPick)
		{
			return;
		}
		int num;
		if (activeHotbarSlot.Itemstack.Attributes.GetInt("toolMode", 0) != 2)
		{
			if (activeHotbarSlot.Itemstack.Attributes.GetInt("toolMode", 0) == 1)
			{
				PlayerAbility obj4 = val2[mining.TunnelDiggerId];
				num = ((obj4 != null && obj4.Tier == 0) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		if ((val4 != null && val4.Tier <= 0) || !flag)
		{
			return;
		}
		int val6 = Math.Min(val4.Value(0, 0) + val2.Level / val4.Value(1, 0), val4.Value(2, 0));
		val6 = Math.Min(val6, ((CollectibleObject)val5).GetRemainingDurability(activeHotbarSlot.Itemstack));
		EntityBehaviorHunger behavior2 = ((Entity)byPlayer.Entity).GetBehavior<EntityBehaviorHunger>();
		if (((behavior2 != null) ? new float?(behavior2.Saturation) : null) < (float)(val4.Value(4, 0) * val6))
		{
			return;
		}
		veinMining = true;
		List<BlockPos> list = new List<BlockPos>();
		List<BlockPos> list2 = new List<BlockPos>();
		list.Add(pos);
		list2.Add(pos);
		while (list.Count > 0 && list2.Count < val6)
		{
			BlockPos val7 = HashsetExtensions.PopOne<BlockPos>((ICollection<BlockPos>)list);
			ShouldVeinMine(((BlockBehavior)this).block, val7.NorthCopy(1), world, list, list2, val6);
			ShouldVeinMine(((BlockBehavior)this).block, val7.SouthCopy(1), world, list, list2, val6);
			ShouldVeinMine(((BlockBehavior)this).block, val7.EastCopy(1), world, list, list2, val6);
			ShouldVeinMine(((BlockBehavior)this).block, val7.WestCopy(1), world, list, list2, val6);
			ShouldVeinMine(((BlockBehavior)this).block, val7.UpCopy(1), world, list, list2, val6);
			ShouldVeinMine(((BlockBehavior)this).block, val7.DownCopy(1), world, list, list2, val6);
		}
		int num2 = 0;
		PlayerAbility val8 = val2[mining.DurabilityId];
		foreach (BlockPos item in list2)
		{
			if (item == pos)
			{
				handling = (EnumHandling)3;
			}
			world.BlockAccessor.BreakBlock(item, byPlayer, 1f);
			if (!((double?)((val8 != null) ? new float?(val8.SkillDependentFValue(0)) : null) > world.Rand.NextDouble()))
			{
				num2++;
			}
		}
		veinMining = false;
		num2 = (int)((float)num2 * (1f + val4.FValue(3, 0f)));
		num2 = Math.Min(num2, ((CollectibleObject)activeHotbarSlot.Itemstack.Item).GetRemainingDurability(activeHotbarSlot.Itemstack) - 1);
		((CollectibleObject)activeHotbarSlot.Itemstack.Item).DamageItem(world, (Entity)(object)byPlayer.Entity, activeHotbarSlot, num2);
		if (behavior2 != null)
		{
			behavior2.ConsumeSaturation((float)(list2.Count * val4.Value(4, 0)));
		}
	}

	protected void ShouldVeinMine(Block block, BlockPos blockPos, IWorldAccessor world, List<BlockPos> toCheck, List<BlockPos> toMine, int max)
	{
		if (toMine.Count < max && ((CollectibleObject)world.BlockAccessor.GetBlock(blockPos)).Id == ((CollectibleObject)block).Id && !toMine.Contains(blockPos))
		{
			toMine.Add(blockPos);
			toCheck.Add(blockPos);
		}
	}

	public override List<ItemStack> GetDropsList(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropChanceMultiplier, ref EnumHandling handling)
	{
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
		PlayerAbility val2 = DropBonusAbility(val);
		float num = ((val2 != null) ? (0.01f * (float)val2.SkillDependentValue(0)) : 0f);
		float num2 = 0f;
		if (temporalAdaptation != null)
		{
			PlayerSkillSet behavior2 = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			PlayerSkill val3 = ((behavior2 != null) ? behavior2.PlayerSkills[((Skill)temporalAdaptation).Id] : null);
			SystemTemporalStability modSystem = ((Skill)mining).XLeveling.Api.ModLoader.GetModSystem<SystemTemporalStability>(true);
			if (val3 != null && modSystem != null)
			{
				val2 = val3[temporalAdaptation.StableMinerId];
				PlayerAbility val4 = val3[temporalAdaptation.TemporalUnstableId];
				float num3 = Math.Clamp(modSystem.GetTemporalStability(pos), 0f, 1f);
				if (val2 != null && val4 != null)
				{
					num2 = ((val4.Tier <= 0) ? (0.01f * (float)val2.Value(0, 0) * num3) : (0.0001f * (float)val2.Value(0, 0) * (float)(100 + val4.Value(0, 0)) * (1f - num3)));
				}
			}
		}
		for (int i = 0; i < ((BlockBehavior)this).block.Drops.Length; i++)
		{
			float num4 = 1f;
			ItemStack resolvedItemstack = ((BlockBehavior)this).block.Drops[i].ResolvedItemstack;
			object obj2;
			if (resolvedItemstack == null)
			{
				obj2 = null;
			}
			else
			{
				Item item = resolvedItemstack.Item;
				obj2 = ((item != null) ? ((RegistryObject)item).FirstCodePart(0) : null);
			}
			if ((string?)obj2 == "crystalizedore")
			{
				val2 = val.PlayerAbilities[mining.CrystalSeekerId];
				num4 = ((val2 != null) ? (1f + (float)val2.Value(0, 0) * 0.01f) : 1f);
			}
			ItemStack nextItemStack = ((BlockBehavior)this).block.Drops[i].GetNextItemStack(dropChanceMultiplier + num * num4 + num2);
			if (nextItemStack != null)
			{
				list.Add(nextItemStack);
			}
		}
		return list;
	}
}
