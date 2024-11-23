using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using XLib.XLeveling;

namespace XSkills;

public class TemporalAdaptation : XSkill
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ExperienceEquationDelegate _003C0_003E__QuadraticEquation;
	}

	public int TemporalStableId { get; private set; }

	public int CavemanId { get; private set; }

	public int TemporalAdaptedId { get; private set; }

	public int TemporalRecoveryId { get; private set; }

	public int ShifterId { get; private set; }

	public int FastForwardId { get; private set; }

	public int StableMinerId { get; private set; }

	public int StableWarriorId { get; private set; }

	public int TemporalUnstableId { get; private set; }

	public int TimelessId { get; private set; }

	public TemporalAdaptation(ICoreAPI api)
		: base("temporaladaptation", "xskills:skill-temporaladaptation", "xskills:group-survival")
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		XLeveling obj = XLeveling.Instance(api);
		if (obj != null)
		{
			obj.RegisterSkill((Skill)(object)this);
		}
		TemporalStableId = ((Skill)this).AddAbility(new Ability("temporalstable", "xskills:ability-temporalstable", "xskills:abilitydesc-temporalstable", 1, 3, new int[9] { 5, 1, 15, 10, 2, 30, 10, 2, 50 }, false));
		CavemanId = ((Skill)this).AddAbility(new Ability("caveman", "xskills:ability-caveman", "xskills:abilitydesc-caveman", 1, 3, new int[9] { 10, 2, 30, 20, 3, 50, 30, 3, 90 }, false));
		TemporalAdaptedId = ((Skill)this).AddAbility(new Ability("temporaladapted", "xskills:ability-temporaladapted", "xskills:abilitydesc-temporaladapted", 1, 3, new int[9] { 10, 2, 30, 20, 3, 50, 30, 3, 90 }, false));
		TemporalRecoveryId = ((Skill)this).AddAbility(new Ability("temporalrecovery", "xskills:ability-temporalrecovery", "xskills:abilitydesc-temporalrecovery", 1, 2, new int[2] { 50, 100 }, false));
		ShifterId = ((Skill)this).AddAbility(new Ability("shifter", "xskills:ability-shifter", "xskills:abilitydesc-shifter", 3, 3, new int[3] { 11, 22, 33 }, false));
		FastForwardId = ((Skill)this).AddAbility(new Ability("fastforward", "xskills:ability-fastforward", "xskills:abilitydesc-fastforward", 3, 2, new int[2] { 10, 20 }, false));
		((Skill)this).SpecialisationID = ((Skill)this).AddAbility(new Ability("timelord", "xskills:ability-timelord", "xskills:abilitydesc-timelord", 5, 1, new int[1] { 40 }, false));
		StableMinerId = ((Skill)this).AddAbility(new Ability("stableminer", "xskills:ability-stableminer", "xskills:abilitydesc-stableminer", 5, 2, new int[2] { 10, 20 }, false));
		StableWarriorId = ((Skill)this).AddAbility(new Ability("stablewarrior", "xskills:ability-stablewarrior", "xskills:abilitydesc-stablewarrior", 5, 2, new int[2] { 10, 20 }, false));
		TemporalUnstableId = ((Skill)this).AddAbility(new Ability("temporalunstable", "xskills:ability-temporalunstable", "xskills:abilitydesc-temporalunstable", 10, 3, new int[3] { 33, 66, 100 }, false));
		TimelessId = ((Skill)this).AddAbility(new Ability("timeless", "xskills:ability-timeless", "xskills:abilitydesc-timeless", 10, 1, new int[0], false));
		((Skill)this).ClassExpMultipliers["commoner"] = 0.1f;
		((Skill)this).ClassExpMultipliers["clockmaker"] = 0.2f;
		((Skill)this).ClassExpMultipliers["vanguard"] = 0.15f;
		((Skill)this).ClassExpMultipliers["miner"] = 0.1f;
		((Skill)this).ClassExpMultipliers["archer"] = -0.2f;
		((Skill)this).ClassExpMultipliers["hunter"] = -0.15f;
		((Skill)this).ClassExpMultipliers["malefactor"] = -0.05f;
		((Skill)this).ClassExpMultipliers["blackguard"] = 0.1f;
		((Skill)this).ClassExpMultipliers["forager"] = -0.15f;
		object obj2 = _003C_003EO._003C0_003E__QuadraticEquation;
		if (obj2 == null)
		{
			ExperienceEquationDelegate val = Skill.QuadraticEquation;
			_003C_003EO._003C0_003E__QuadraticEquation = val;
			obj2 = (object)val;
		}
		((Skill)this).ExperienceEquation = (ExperienceEquationDelegate)obj2;
		((Skill)this).ExpBase = 200f;
		((Skill)this).ExpMult = 100f;
		((Skill)this).ExpEquationValue = 8f;
	}
}
