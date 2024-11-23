using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public abstract class DropBonusBehavior : BlockBehavior
{
	protected float xp;

	public abstract CollectingSkill Skill { get; }

	public abstract EnumTool? Tool { get; }

	public abstract PlayerAbility DropBonusAbility(PlayerSkill playerSkill);

	public virtual float GetXP(IWorldAccessor world, BlockPos pos, IPlayer byPlayer)
	{
		return xp;
	}

	public DropBonusBehavior(Block block)
		: base(block)
	{
	}

	public override void Initialize(JsonObject properties)
	{
		((CollectibleBehavior)this).Initialize(properties);
		xp = properties["xp"].AsFloat(0f);
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref EnumHandling handling)
	{
		if (Skill == null || byPlayer == null)
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val = ((behavior != null) ? behavior.PlayerSkills[((Skill)Skill).Id] : null);
		if (val != null)
		{
			BlockReinforcement reinforcment = world.Api.ModLoader.GetModSystem<ModSystemBlockReinforcement>(true).GetReinforcment(pos);
			if (reinforcment == null || reinforcment.Strength <= 0)
			{
				val.AddExperience(GetXP(world, pos, byPlayer), true);
			}
		}
	}

	public virtual List<ItemStack> GetDropsList(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropChanceMultiplier, ref EnumHandling handling)
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
		if (base.block.Drops.Length == 0)
		{
			return list;
		}
		float num = dropChanceMultiplier;
		PlayerAbility val2 = DropBonusAbility(val);
		if (val2 == null)
		{
			return list;
		}
		handling = (EnumHandling)2;
		num += ((val2.Ability.ValuesPerTier >= 3) ? (0.01f * (float)val2.SkillDependentValue(0)) : (0.01f * (float)val2.Value(0, 0)));
		for (int i = 0; i < base.block.Drops.Length; i++)
		{
			ItemStack nextItemStack = base.block.Drops[i].GetNextItemStack(num);
			if (nextItemStack != null)
			{
				list.Add(nextItemStack);
			}
		}
		return list;
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
	{
		return GetDropsList(world, pos, byPlayer, dropChanceMultiplier, ref handling).ToArray();
	}
}
