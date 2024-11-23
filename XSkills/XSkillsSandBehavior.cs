using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsSandBehavior : XSkillsSoilBehavior
{
	public XSkillsSandBehavior(Block block)
		: base(block)
	{
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
		PlayerAbility val2 = val[digging.ScrapDetectorId];
		if (val2 == null)
		{
			return list;
		}
		if ((double)((float)val2.Value(0, 0) * 0.01f) > world.Rand.NextDouble())
		{
			if (!digging.PanningDrops.TryGetValue("@(sand|gravel)-.*", out var value))
			{
				return list;
			}
			handling = (EnumHandling)2;
			float num = dropChanceMultiplier * 8f;
			val2 = val.PlayerAbilities[digging.ScrapSpecialistId];
			for (int i = 0; i < value.Length; i++)
			{
				ItemStack val3 = ((!(value[i].Quantity.avg <= (float)val2.Value(0, 0) * 0.005f)) ? value[i].GetNextItemStack(num) : value[i].GetNextItemStack(num * (1f + (float)val2.Value(1, 0) * 0.01f)));
				if (val3 != null)
				{
					list.Add(val3);
				}
			}
		}
		return list;
	}
}
