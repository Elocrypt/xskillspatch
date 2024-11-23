using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsCharcoalBehavior : DropBonusBehavior
{
	public Forestry Forestry { get; set; }

	public override CollectingSkill Skill => Forestry;

	public override EnumTool? Tool => (EnumTool)4;

	public override PlayerAbility DropBonusAbility(PlayerSkill skill)
	{
		return skill[Forestry.CharcoalBurnerId];
	}

	public XSkillsCharcoalBehavior(Block block)
		: base(block)
	{
	}

	public override void OnLoaded(ICoreAPI api)
	{
		((CollectibleBehavior)this).OnLoaded(api);
		XLeveling obj = XLeveling.Instance(api);
		Forestry = ((obj != null) ? obj.GetSkill("forestry", false) : null) as Forestry;
	}

	public override float GetXP(IWorldAccessor world, BlockPos pos, IPlayer byPlayer)
	{
		return 0f;
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
	{
		if (Skill == null || byPlayer == null)
		{
			return base.GetDrops(world, pos, byPlayer, ref dropChanceMultiplier, ref handling);
		}
		EntityPlayer entity = byPlayer.Entity;
		object obj;
		if (entity == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
			obj = ((behavior == null) ? null : behavior.PlayerSkills?[((Skill)Skill).Id]);
		}
		if (obj != null)
		{
			((PlayerSkill)obj).AddExperience(xp, true);
		}
		return base.GetDrops(world, pos, byPlayer, ref dropChanceMultiplier, ref handling);
	}
}
