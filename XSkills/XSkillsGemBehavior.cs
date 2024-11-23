using Vintagestory.API.Common;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsGemBehavior : XSkillsOreBehavior
{
	public override PlayerAbility DropBonusAbility(PlayerSkill playerSkill)
	{
		return playerSkill[mining.GemstoneMinerId];
	}

	public XSkillsGemBehavior(Block block)
		: base(block)
	{
	}
}
