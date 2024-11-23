using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsLeavesBehavior : BlockBehavior
{
	protected Forestry forestry;

	protected float xp;

	public XSkillsLeavesBehavior(Block block)
		: base(block)
	{
	}

	public override void Initialize(JsonObject properties)
	{
		((CollectibleBehavior)this).Initialize(properties);
		xp = properties["xp"].AsFloat(0f);
	}

	public override void OnLoaded(ICoreAPI api)
	{
		XLeveling obj = XLeveling.Instance(api);
		forestry = ((obj != null) ? obj.GetSkill("forestry", false) : null) as Forestry;
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref EnumHandling handling)
	{
		if (forestry != null && byPlayer != null)
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			PlayerSkill val = ((behavior != null) ? behavior.PlayerSkills[((Skill)forestry).Id] : null);
			if (val != null)
			{
				val.AddExperience(xp, true);
			}
		}
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
	{
		List<ItemStack> list = new List<ItemStack>();
		if (forestry == null)
		{
			return list.ToArray();
		}
		object obj;
		if (byPlayer == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj = ((behavior != null) ? behavior[((Skill)forestry).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj;
		if (val == null)
		{
			return list.ToArray();
		}
		handling = (EnumHandling)2;
		if (base.block.Drops.Length == 0)
		{
			return list.ToArray();
		}
		PlayerAbility val2 = val[forestry.AfforestationId];
		if (val2 == null)
		{
			return list.ToArray();
		}
		float num = dropChanceMultiplier + (float)val2.Value(0, 0) * 0.01f;
		ItemStack nextItemStack = base.block.Drops[0].GetNextItemStack(num);
		if (nextItemStack != null)
		{
			list.Add(nextItemStack);
		}
		if (base.block.Drops.Length > 1)
		{
			val2 = val.PlayerAbilities[forestry.MoreLaddersId];
			num = dropChanceMultiplier + 0.01f * (float)val2.SkillDependentValue(0);
			nextItemStack = base.block.Drops[1].GetNextItemStack(num);
			if (nextItemStack != null)
			{
				list.Add(nextItemStack);
			}
		}
		for (int i = 2; i < base.block.Drops.Length; i++)
		{
			nextItemStack = base.block.Drops[i].GetNextItemStack(dropChanceMultiplier);
			if (nextItemStack != null)
			{
				list.Add(nextItemStack);
			}
		}
		return list.ToArray();
	}
}
