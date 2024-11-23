using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsGrassBehavior : BlockBehavior
{
	private Farming farming;

	private float xp;

	public XSkillsGrassBehavior(Block block)
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
		farming = ((obj != null) ? obj.GetSkill("farming", false) : null) as Farming;
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
	{
		ItemStack[] result = (ItemStack[])(object)new ItemStack[0];
		object obj;
		if (byPlayer == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj = ((behavior != null) ? behavior[((Skill)farming).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj;
		if (val == null)
		{
			return result;
		}
		val.AddExperience(xp, true);
		PlayerAbility val2 = val[farming.GathererId];
		if (val2 != null)
		{
			dropChanceMultiplier += val2.SkillDependentFValue(0);
		}
		return result;
	}
}
