using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsCarcassBehavior : BlockBehavior
{
	private Husbandry husbandry;

	public XSkillsCarcassBehavior(Block block)
		: base(block)
	{
	}

	public override void Initialize(JsonObject properties)
	{
		((CollectibleBehavior)this).Initialize(properties);
	}

	public override void OnLoaded(ICoreAPI api)
	{
		XLeveling obj = XLeveling.Instance(api);
		husbandry = ((obj != null) ? obj.GetSkill("husbandry", false) : null) as Husbandry;
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
	{
		if (husbandry == null)
		{
			return null;
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
				if (behavior == null)
				{
					obj = null;
				}
				else
				{
					PlayerSkill obj2 = behavior[((Skill)husbandry).Id];
					obj = ((obj2 != null) ? obj2[husbandry.BoneBrakerId] : null);
				}
			}
		}
		PlayerAbility val = (PlayerAbility)obj;
		if (val == null || val.Tier <= 0)
		{
			return null;
		}
		dropChanceMultiplier += val.SkillDependentFValue(0);
		return null;
	}
}
