using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsWoodBehavior : CollectingBehavior
{
	protected Forestry forestry;

	public override CollectingSkill Skill => forestry;

	public override EnumTool? Tool => (EnumTool)2;

	public override PlayerAbility DropBonusAbility(PlayerSkill skill)
	{
		return skill[forestry.LumberjackId];
	}

	public XSkillsWoodBehavior(Block block)
		: base(block)
	{
	}

	public override void OnLoaded(ICoreAPI api)
	{
		XLeveling obj = XLeveling.Instance(api);
		forestry = ((obj != null) ? obj.GetSkill("forestry", false) : null) as Forestry;
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref EnumHandling handling)
	{
		if (forestry == null)
		{
			XLeveling obj = XLeveling.Instance(world.Api);
			forestry = ((obj != null) ? obj.GetSkill("forestry", false) : null) as Forestry;
		}
		base.OnBlockBroken(world, pos, byPlayer, ref handling);
	}

	public override List<ItemStack> GetDropsList(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropChanceMultiplier, ref EnumHandling handling)
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		List<ItemStack> dropsList = base.GetDropsList(world, pos, byPlayer, dropChanceMultiplier, ref handling);
		if (dropsList.Count == 0)
		{
			return dropsList;
		}
		PlayerSkill obj = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>()[((Skill)Skill).Id];
		PlayerAbility val = obj[forestry.ResinFarmerId];
		PlayerAbility val2 = obj[forestry.ResinExtractor];
		if (val == null || val2 == null)
		{
			return dropsList;
		}
		if ((((RegistryObject)((BlockBehavior)this).block).FirstCodePart(2) == "pine" && (double)val.Value(0, 0) * 0.01 > world.Rand.NextDouble()) || (double)val.Value(0, 0) * 0.01 * (double)val2.Value(0, 0) * 0.01 > world.Rand.NextDouble())
		{
			ItemStack val3 = new ItemStack(world.GetItem(new AssetLocation("game", "resin")), 1);
			if (val3 != null)
			{
				dropsList.Add(val3);
			}
		}
		return dropsList;
	}
}
