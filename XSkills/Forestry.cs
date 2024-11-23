using Vintagestory.API.Common;
using XLib.XLeveling;

namespace XSkills;

public class Forestry : CollectingSkill
{
	public int LumberjackId { get; private set; }

	public int AfforestationId { get; private set; }

	public int MoreLaddersId { get; private set; }

	public int ResinFarmerId { get; private set; }

	public int TreeNurseryId { get; private set; }

	public int CharcoalBurnerId { get; private set; }

	public int GrafterId { get; private set; }

	public int ResinExtractor { get; private set; }

	public Forestry(ICoreAPI api)
		: base("forestry")
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Expected O, but got Unknown
		XLeveling obj = XLeveling.Instance(api);
		if (obj != null)
		{
			obj.RegisterSkill((Skill)(object)this);
		}
		base.Tool = (EnumTool)2;
		LumberjackId = ((Skill)this).AddAbility(new Ability("lumberjack", "xskills:ability-lumberjack", "xskills:abilitydesc-lumberjack", 1, 3, new int[9] { 10, 1, 20, 20, 2, 40, 20, 2, 60 }, false));
		AfforestationId = ((Skill)this).AddAbility(new Ability("afforestation", "xskills:ability-afforestation", "xskills:abilitydesc-afforestation", 1, 2, new int[2] { 10, 20 }, false));
		MoreLaddersId = ((Skill)this).AddAbility(new Ability("moreladders", "xskills:ability-moreladders", "xskills:abilitydesc-moreladders", 1, 2, new int[6] { 10, 2, 30, 20, 4, 60 }, false));
		ResinFarmerId = ((Skill)this).AddAbility(new Ability("resinfarmer", "xskills:ability-resinfarmer", "xskills:abilitydesc-resinfarmer", 1, 1, new int[1] { 2 }, false));
		TreeNurseryId = ((Skill)this).AddAbility(new Ability("treenursery", "xskills:ability-treenursery", "xskills:abilitydesc-treenursery", 3, 3, new int[3] { 87, 74, 60 }, false));
		base.MiningSpeedId = ((Skill)this).AddAbility(new Ability("axeexpert", "xskills:ability-axeexpert", "xskills:abilitydesc-axeexpert", 1, 3, new int[15]
		{
			1, 1, 2, 10, 30, 2, 2, 4, 10, 40,
			2, 2, 6, 10, 45
		}, false));
		base.DurabilityId = ((Skill)this).AddAbility(new Ability("carefullumberjack", "xskills:ability-carefullumberjack", "xskills:abilitydesc-carefullumberjack", 1, 3, new int[9] { 5, 1, 15, 5, 2, 25, 5, 2, 45 }, false));
		((Skill)this).SpecialisationID = ((Skill)this).AddAbility(new Ability("forester", "xskills:ability-forester", "xskills:abilitydesc-forester", 5, 1, new int[1] { 40 }, false));
		CharcoalBurnerId = ((Skill)this).AddAbility(new Ability("charcoalburner", "xskills:ability-charcoalburner", "xskills:abilitydesc-charcoalburner", 5, 3, new int[3] { 13, 26, 40 }, false));
		GrafterId = ((Skill)this).AddAbility(new Ability("grafter", "xskills:ability-grafter", "xskills:abilitydesc-grafter", 5, 2, new int[2] { 50, 100 }, false));
		ResinExtractor = ((Skill)this).AddAbility(new Ability("resinextractor", "xskills:ability-resinextractor", "xskills:abilitydesc-resinextractor", 7, 1, new int[1] { 50 }, false));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsSapling", typeof(XSkillsSaplingBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsWood", typeof(XSkillsWoodBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsLeaves", typeof(XSkillsLeavesBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsCharcoal", typeof(XSkillsCharcoalBehavior));
		((Skill)this).ClassExpMultipliers["commoner"] = 0.1f;
		((Skill)this).ClassExpMultipliers["forager"] = 0.2f;
		((Skill)this).ClassExpMultipliers["hunter"] = 0.1f;
		((Skill)this).ClassExpMultipliers["malefactor"] = -0.2f;
		((Skill)this).ClassExpMultipliers["clockmaker"] = -0.15f;
		((Skill)this).ClassExpMultipliers["blackguard"] = -0.05f;
		((Skill)this).ClassExpMultipliers["miner"] = 0.1f;
		((Skill)this).ClassExpMultipliers["archer"] = 0.15f;
		((Skill)this).ClassExpMultipliers["vanguard"] = -0.15f;
		((Skill)this).ClassExpMultipliers["gatherer"] = 0.2f;
		((Skill)this).ClassExpMultipliers["mercenary"] = -0.15f;
	}
}
