using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsClayBehavior : XSkillsSoilBehavior
{
	public override PlayerAbility DropBonusAbility(PlayerSkill skill)
	{
		if (((RegistryObject)((BlockBehavior)this).block).FirstCodePart(0) == "rawclay")
		{
			return skill[digging.ClayDiggerId];
		}
		return skill[digging.PeatCutterId];
	}

	public XSkillsClayBehavior(Block block)
		: base(block)
	{
	}

	public override List<ItemStack> GetDropsList(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropChanceMultiplier, ref EnumHandling handling)
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		List<ItemStack> dropsList = base.GetDropsList(world, pos, byPlayer, dropChanceMultiplier, ref handling);
		if (dropsList.Count == 0)
		{
			return dropsList;
		}
		if (((RegistryObject)((BlockBehavior)this).block).FirstCodePart(0) == "rawclay" && (double)((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>()[((Skill)digging).Id].PlayerAbilities[digging.MixedClayId].FValue(0, 0f) >= world.Rand.NextDouble())
		{
			if (((RegistryObject)((BlockBehavior)this).block).FirstCodePart(1) == "blue")
			{
				dropsList.Add(new ItemStack(world.GetItem(new AssetLocation("game", "clay-fire")), 1));
			}
			else if (((RegistryObject)((BlockBehavior)this).block).FirstCodePart(1) == "fire")
			{
				dropsList.Add(new ItemStack(world.GetItem(new AssetLocation("game", "clay-blue")), 1));
			}
		}
		return dropsList;
	}
}
