using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using XLib.XLeveling;

namespace XSkills;

public class Pottery : XSkill
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ExperienceEquationDelegate _003C0_003E__QuadraticEquation;
	}

	public Dictionary<string, List<CollectibleObject>> InspirationCollectibles { get; protected set; }

	public int ThriftId { get; private set; }

	public int LayerLayerId { get; private set; }

	public int PerfectFitId { get; private set; }

	public int PerfectionistId { get; private set; }

	public int FastPotterId { get; private set; }

	public int JackPotId { get; private set; }

	public int InfallibleId { get; private set; }

	public int InspirationId { get; private set; }

	public Pottery(ICoreAPI api)
		: base("pottery", "xskills:skill-pottery", "xskills:group-processing")
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		XLeveling obj = XLeveling.Instance(api);
		if (obj != null)
		{
			obj.RegisterSkill((Skill)(object)this);
		}
		ThriftId = ((Skill)this).AddAbility(new Ability("thrift", "xskills:ability-thrift", "xskills:abilitydesc-thrift", 1, 3, new int[3] { 3, 6, 10 }, false));
		LayerLayerId = ((Skill)this).AddAbility(new Ability("layerlayer", "xskills:ability-layerlayer", "xskills:abilitydesc-layerlayer", 1, 3, new int[3] { 1, 2, 4 }, false));
		PerfectFitId = ((Skill)this).AddAbility(new Ability("perfectfit", "xskills:ability-perfectfit", "xskills:abilitydesc-perfectfit", 3, 1, 0, false));
		PerfectionistId = ((Skill)this).AddAbility(new Ability("perfectionist", "xskills:ability-perfectionist", "xskills:abilitydesc-perfectionist", 3, 1, 0, false));
		((Skill)this).SpecialisationID = ((Skill)this).AddAbility(new Ability("potter", "xskills:ability-potter", "xskills:abilitydesc-potter", 5, 1, new int[1] { 40 }, false));
		FastPotterId = ((Skill)this).AddAbility(new Ability("fastpotter", "xskills:ability-fastpotter", "xskills:abilitydesc-fastpotter", 5, 3, new int[9] { 1, 1, 2, 2, 2, 4, 2, 2, 6 }, false));
		JackPotId = ((Skill)this).AddAbility(new Ability("jackpot", "xskills:ability-jackpot", "xskills:abilitydesc-jackpot", 5, 3, new int[9] { 5, 0, 5, 5, 1, 15, 5, 1, 25 }, false));
		InfallibleId = ((Skill)this).AddAbility(new Ability("infallible", "xskills:ability-infallible", "xskills:abilitydesc-infallible", 5, 2, new int[2] { 1, 2 }, false));
		InspirationId = ((Skill)this).AddAbility(new Ability("inspiration", "xskills:ability-inspiration", "xskills:abilitydesc-inspiration", 7, 2, new int[2] { 10, 20 }, false));
		((Skill)this).ClassExpMultipliers["commoner"] = 0.1f;
		((Skill)this).ClassExpMultipliers["blackguard"] = -0.2f;
		((Skill)this).ClassExpMultipliers["hunter"] = 0.15f;
		((Skill)this).ClassExpMultipliers["malefactor"] = 0.05f;
		((Skill)this).ClassExpMultipliers["clockmaker"] = 0.2f;
		((Skill)this).ClassExpMultipliers["miner"] = -0.15f;
		((Skill)this).ClassExpMultipliers["forager"] = -0.05f;
		((Skill)this).ClassExpMultipliers["archer"] = 0.1f;
		((Skill)this).ClassExpMultipliers["vanguard"] = -0.1f;
		((Skill)this).ClassExpMultipliers["gatherer"] = -0.05f;
		((Skill)this).ClassExpMultipliers["mercenary"] = -0.1f;
		InspirationCollectibles = new Dictionary<string, List<CollectibleObject>>();
		InspirationCollectibles.Add("clayplanter-burnt", null);
		InspirationCollectibles.Add("flowerpot-burnt", null);
		InspirationCollectibles.Add("storagevessel-burned", null);
		object obj2 = _003C_003EO._003C0_003E__QuadraticEquation;
		if (obj2 == null)
		{
			ExperienceEquationDelegate val = Skill.QuadraticEquation;
			_003C_003EO._003C0_003E__QuadraticEquation = val;
			obj2 = (object)val;
		}
		((Skill)this).ExperienceEquation = (ExperienceEquationDelegate)obj2;
		((Skill)this).ExpBase = 40f;
		((Skill)this).ExpMult = 10f;
		((Skill)this).ExpEquationValue = 0.8f;
	}
}
