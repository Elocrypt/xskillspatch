using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using XLib.XLeveling;

namespace XSkills;

public class CollectingSkill : XSkill
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ExperienceEquationDelegate _003C0_003E__QuadraticEquation;
	}

	public int MiningSpeedId { get; protected set; }

	public int DurabilityId { get; protected set; }

	protected internal EnumTool Tool { get; set; }

	public CollectingSkill(string name)
		: base(name, "xskills:skill-" + name, "xskills:group-collecting")
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		object obj = _003C_003EO._003C0_003E__QuadraticEquation;
		if (obj == null)
		{
			ExperienceEquationDelegate val = Skill.QuadraticEquation;
			_003C_003EO._003C0_003E__QuadraticEquation = val;
			obj = (object)val;
		}
		((Skill)this).ExperienceEquation = (ExperienceEquationDelegate)obj;
		((Skill)this).ExpBase = 200f;
		((Skill)this).ExpMult = 100f;
		((Skill)this).ExpEquationValue = 8f;
	}
}
