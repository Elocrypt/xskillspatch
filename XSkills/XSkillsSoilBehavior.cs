using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsSoilBehavior : CollectingBehavior
{
	protected Digging digging;

	public override CollectingSkill Skill => digging;

	public override EnumTool? Tool => (EnumTool)4;

	public override PlayerAbility DropBonusAbility(PlayerSkill playerSkill)
	{
		return null;
	}

	public XSkillsSoilBehavior(Block block)
		: base(block)
	{
	}

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
	}

	public override void OnLoaded(ICoreAPI api)
	{
		XLeveling obj = XLeveling.Instance(api);
		digging = ((obj != null) ? obj.GetSkill("digging", false) : null) as Digging;
	}
}
