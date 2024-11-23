using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;
using XLib.XLeveling;

namespace XSkills;

public class Combat : XSkill
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ExperienceEquationDelegate _003C0_003E__QuadraticEquation;
	}

	public int SwordsmanId { get; private set; }

	public int ArcherId { get; private set; }

	public int SpearmanId { get; private set; }

	public int ToolMasteryId { get; private set; }

	public int IronFistId { get; private set; }

	public int MonkId { get; private set; }

	public int LooterId { get; private set; }

	public int SniperId { get; private set; }

	public int FreshFleshId { get; private set; }

	public int ShovelKnightId { get; private set; }

	public int AdrenalineRushId { get; private set; }

	public int VampireId { get; private set; }

	public int BurningRageId { get; private set; }

	public int BloodlustId { get; private set; }

	public int MonsterExpertId { get; private set; }

	public Combat(ICoreAPI api)
		: base("combat", "xskills:skill-combat", "xskills:group-survival")
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Expected O, but got Unknown
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Expected O, but got Unknown
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Expected O, but got Unknown
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		XLeveling obj = XLeveling.Instance(api);
		if (obj != null)
		{
			obj.RegisterSkill((Skill)(object)this);
		}
		SwordsmanId = ((Skill)this).AddAbility(new Ability("swordsman", "xskills:ability-swordsman", "xskills:abilitydesc-swordsman", 1, 3, new int[9] { 10, 1, 20, 20, 2, 40, 20, 2, 60 }, false));
		ArcherId = ((Skill)this).AddAbility(new Ability("archer", "xskills:ability-archer", "xskills:abilitydesc-archer", 1, 3, new int[9] { 10, 1, 20, 20, 2, 40, 20, 2, 60 }, false));
		SpearmanId = ((Skill)this).AddAbility(new Ability("spearman", "xskills:ability-spearman", "xskills:abilitydesc-spearman", 1, 3, new int[9] { 10, 1, 20, 20, 2, 40, 20, 2, 60 }, false));
		ToolMasteryId = ((Skill)this).AddAbility(new Ability("toolmastery", "xskills:ability-toolmastery", "xskills:abilitydesc-toolmastery", 1, 3, new int[9] { 5, 2, 25, 15, 3, 40, 15, 3, 75 }, false));
		IronFistId = ((Skill)this).AddAbility(new Ability("ironfist", "xskills:ability-ironfist", "xskills:abilitydesc-ironfist", 1, 3, new int[3] { 2, 3, 4 }, false));
		MonkId = ((Skill)this).AddAbility(new Ability("monk", "xskills:ability-monk", "xskills:abilitydesc-monk", 1, 3, new int[3] { 6, 9, 12 }, false));
		LooterId = ((Skill)this).AddAbility(new Ability("looter", "xskills:ability-looter", "xskills:abilitydesc-looter", 1, 2, new int[6] { 10, 1, 20, 20, 2, 40 }, false));
		((Skill)this).SpecialisationID = ((Skill)this).AddAbility(new Ability("warrior", "xskills:ability-warrior", "xskills:abilitydesc-warrior", 5, 1, new int[1] { 40 }, false));
		SniperId = ((Skill)this).AddAbility((Ability)new StatAbility("sniper", "rangedWeaponsAcc", "xskills:ability-sniper", "xskills:abilitydesc-sniper", 5, 2, new int[2] { 15, 30 }, false));
		FreshFleshId = ((Skill)this).AddAbility(new Ability("freshflesh", "xskills:ability-freshflesh", "xskills:abilitydesc-freshflesh", 5, 3, new int[3] { 10, 20, 30 }, false));
		ShovelKnightId = ((Skill)this).AddAbility(new Ability("shovelknight", "xskills:ability-shovelknight", "xskills:abilitydesc-shovelknight", 5, 2, new int[4] { 3, 20, 5, 25 }, false));
		AdrenalineRushId = ((Skill)this).AddAbility(new Ability("adrenalinerush", "xskills:ability-adrenalinerush", "xskills:abilitydesc-adrenalinerush", 7, 2, new int[10] { 20, 20, 25, 10, 24, 20, 40, 50, 12, 20 }, false));
		VampireId = ((Skill)this).AddAbility(new Ability("vampire", "xskills:ability-vampire", "xskills:abilitydesc-vampire", 7, 3, new int[6] { 3, 80, 5, 65, 7, 50 }, false));
		BurningRageId = ((Skill)this).AddAbility(new Ability("burningrage", "xskills:ability-burningrage", "xskills:abilitydesc-burningrage", 10, 3, new int[3] { 2, 4, 6 }, false));
		BloodlustId = ((Skill)this).AddAbility(new Ability("bloodlust", "xskills:ability-bloodlust", "xskills:abilitydesc-bloodlust", 10, 1, new int[4] { 2, 3, 16, 10 }, false));
		MonsterExpertId = ((Skill)this).AddAbility(new Ability("monsterexpert", "xskills:ability-monsterexpert", "xskills:abilitydesc-monsterexpert", 10, 1, new int[0], false));
		((ICoreAPICommon)api).RegisterEntityBehaviorClass("XSkillsEntity", typeof(XSkillsEntityBehavior));
		ICoreServerAPI val = (ICoreServerAPI)(object)((api is ICoreServerAPI) ? api : null);
		if (val != null)
		{
			val.Event.PlayerJoin += new PlayerDelegate(OnPlayerJoin);
		}
		((Skill)this).Config = (CustomSkillConfig)(object)new CombatSkillConfig();
		((Skill)this).ClassExpMultipliers["commoner"] = 0.1f;
		((Skill)this).ClassExpMultipliers["blackguard"] = 0.2f;
		((Skill)this).ClassExpMultipliers["vanguard"] = 0.15f;
		((Skill)this).ClassExpMultipliers["archer"] = 0.15f;
		((Skill)this).ClassExpMultipliers["clockmaker"] = -0.2f;
		((Skill)this).ClassExpMultipliers["hunter"] = 0f;
		((Skill)this).ClassExpMultipliers["malefactor"] = -0.1f;
		((Skill)this).ClassExpMultipliers["miner"] = -0.05f;
		((Skill)this).ClassExpMultipliers["forager"] = -0.15f;
		((Skill)this).ClassExpMultipliers["gatherer"] = -0.15f;
		((Skill)this).ClassExpMultipliers["mercenary"] = 0.15f;
		object obj2 = _003C_003EO._003C0_003E__QuadraticEquation;
		if (obj2 == null)
		{
			ExperienceEquationDelegate val2 = Skill.QuadraticEquation;
			_003C_003EO._003C0_003E__QuadraticEquation = val2;
			obj2 = (object)val2;
		}
		((Skill)this).ExperienceEquation = (ExperienceEquationDelegate)obj2;
		((Skill)this).ExpBase = 100f;
		((Skill)this).ExpMult = 50f;
		((Skill)this).ExpEquationValue = 4f;
	}

	public void OnPlayerJoin(IPlayer byPlayer)
	{
		CombatSkillConfig obj = ((Skill)this).Config as CombatSkillConfig;
		if (obj != null && obj.enableAbilitiesInPvP)
		{
			XSkillsEntityBehavior xSkillsEntityBehavior = new XSkillsEntityBehavior((Entity)(object)byPlayer.Entity);
			((Entity)byPlayer.Entity).AddBehavior((EntityBehavior)(object)xSkillsEntityBehavior);
		}
	}

	public override void OnConfigReceived()
	{
		base.OnConfigReceived();
		CombatSkillConfig obj = ((Skill)this).Config as CombatSkillConfig;
		if (obj != null && obj.enableAbilitiesInPvP)
		{
			ICoreAPI api = ((Skill)this).XLeveling.Api;
			ICoreAPI obj2 = ((api is ICoreClientAPI) ? api : null);
			Entity val = (Entity)(object)((obj2 != null) ? ((IPlayer)((ICoreClientAPI)obj2).World.Player).Entity : null);
			if (val != null)
			{
				val.AddBehavior((EntityBehavior)(object)new XSkillsEntityBehavior(val));
			}
		}
	}
}
