using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsSalpeterBehavior : XSkillsSoilBehavior
{
	public override PlayerAbility DropBonusAbility(PlayerSkill skill)
	{
		return skill[digging.SaltpeterDiggerId];
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
	{
		ItemStack[] result = (ItemStack[])(object)new ItemStack[0];
		if (Skill == null)
		{
			return result;
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
			return result;
		}
		PlayerAbility val2 = DropBonusAbility(val);
		if (val2 == null)
		{
			return result;
		}
		dropChanceMultiplier += ((val2.Ability.ValuesPerTier >= 3) ? (0.01f * (float)val2.SkillDependentValue(0)) : (0.01f * (float)val2.Value(0, 0)));
		return result;
	}

	public XSkillsSalpeterBehavior(Block block)
		: base(block)
	{
	}
}
