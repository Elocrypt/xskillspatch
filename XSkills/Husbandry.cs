using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using XLib.XLeveling;

namespace XSkills;

public class Husbandry : XSkill
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ExperienceEquationDelegate _003C0_003E__QuadraticEquation;
	}

	public int HunterId { get; private set; }

	public int ButcherId { get; private set; }

	public int FurrierId { get; private set; }

	public int BoneBrakerId { get; private set; }

	public int RancherId { get; private set; }

	public int FeederId { get; private set; }

	public int LightFootedId { get; private set; }

	public int PreserverId { get; private set; }

	public int TannerId { get; private set; }

	public int CheesyCheeseId { get; private set; }

	public int CatcherId { get; private set; }

	public int BreederId { get; private set; }

	public int MassHusbandryId { get; private set; }

	public Husbandry(ICoreAPI api)
		: base("husbandry", "xskills:skill-husbandry", "xskills:group-collecting")
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Expected O, but got Unknown
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Expected O, but got Unknown
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Expected O, but got Unknown
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Expected O, but got Unknown
		XLeveling obj = XLeveling.Instance(api);
		if (obj != null)
		{
			obj.RegisterSkill((Skill)(object)this);
		}
		HunterId = ((Skill)this).AddAbility((Ability)new TraitAbility("hunter", "bowyer", "xskills:ability-hunter", "xskills:abilitydesc-hunter", 1, 3, new int[9] { 10, 0, 10, 10, 1, 20, 10, 1, 30 }, false));
		ButcherId = ((Skill)this).AddAbility(new Ability("butcher", "xskills:ability-butcher", "xskills:abilitydesc-butcher", 1, 3, new int[15]
		{
			5, 1, 15, 1, 10, 5, 2, 25, 1, 15,
			5, 2, 45, 1, 20
		}, false));
		FurrierId = ((Skill)this).AddAbility(new Ability("furrier", "xskills:ability-furrier", "xskills:abilitydesc-furrier", 1, 3, new int[15]
		{
			5, 1, 15, 1, 10, 5, 2, 25, 1, 15,
			5, 2, 45, 1, 20
		}, false));
		BoneBrakerId = ((Skill)this).AddAbility(new Ability("bonebreaker", "xskills:ability-bonebreaker", "xskills:abilitydesc-bonebreaker", 1, 3, new int[9] { 10, 1, 20, 15, 2, 40, 20, 2, 60 }, false));
		RancherId = ((Skill)this).AddAbility(new Ability("rancher", "xskills:ability-rancher", "xskills:abilitydesc-rancher", 3, 2, new int[6] { 33, 2, 5, 50, 3, 10 }, false));
		FeederId = ((Skill)this).AddAbility(new Ability("feeder", "xskills:ability-feeder", "xskills:abilitydesc-feeder", 3, 2, new int[10] { 10, 1, 20, 1, 4, 20, 1, 40, 1, 8 }, false));
		LightFootedId = ((Skill)this).AddAbility((Ability)new StatAbility("lightfooted", "animalSeekingRange", "xskills:ability-lightfooted", "xskills:abilitydesc-lightfooted", 3, 2, new int[2] { -20, -40 }, false));
		((Skill)this).SpecialisationID = ((Skill)this).AddAbility(new Ability("shepherd", "xskills:ability-shepherd", "xskills:abilitydesc-shepherd", 5, 1, new int[1] { 40 }, false));
		PreserverId = ((Skill)this).AddAbility(new Ability("preserver", "xskills:ability-preserver", "xskills:abilitydesc-preserver", 5, 1, new int[3] { 10, 1, 30 }, false));
		TannerId = ((Skill)this).AddAbility(new Ability("tanner", "xskills:ability-tanner", "xskills:abilitydesc-tanner", 5, 3, new int[9] { 10, 1, 20, 10, 2, 30, 10, 2, 50 }, false));
		CheesyCheeseId = ((Skill)this).AddAbility(new Ability("cheesycheese", "xskills:ability-cheesycheese", "xskills:abilitydesc-cheesycheese", 5, 2, new int[4] { 5, 1, 5, 2 }, false));
		CatcherId = ((Skill)this).AddAbility((Ability)new TraitAbility("catcher", "ability-catcher", "xskills:ability-catcher", "xskills:abilitydesc-catcher", 6, 1, 0, false));
		BreederId = ((Skill)this).AddAbility(new Ability("breeder", "xskills:ability-breeder", "xskills:abilitydesc-breeder", 8, 1, new int[4] { 10, 2, 1, 60 }, false));
		MassHusbandryId = ((Skill)this).AddAbility(new Ability("masshusbandry", "xskills:ability-masshusbandry", "xskills:abilitydesc-masshusbandry", 10, 1, new int[4] { 0, 1, 1, 30 }, false));
		((Skill)this).ClassExpMultipliers["commoner"] = 0.1f;
		((Skill)this).ClassExpMultipliers["hunter"] = 0.2f;
		((Skill)this).ClassExpMultipliers["clockmaker"] = -0.2f;
		((Skill)this).ClassExpMultipliers["malefactor"] = 0.15f;
		((Skill)this).ClassExpMultipliers["blackguard"] = -0.1f;
		((Skill)this).ClassExpMultipliers["miner"] = -0.15f;
		((Skill)this).ClassExpMultipliers["forager"] = 0.1f;
		((Skill)this).ClassExpMultipliers["archer"] = 0.1f;
		((Skill)this).ClassExpMultipliers["vanguard"] = -0.1f;
		((Skill)this).ClassExpMultipliers["gatherer"] = 0.1f;
		((Skill)this).ClassExpMultipliers["mercenary"] = -0.1f;
		((ICoreAPICommon)api).RegisterEntityBehaviorClass("XSkillsAnimal", typeof(XSkillsAnimalBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsCarcass", typeof(XSkillsCarcassBehavior));
		((ICoreAPICommon)api).RegisterBlockClass("XSkillsCage", typeof(BlockCage));
		((ICoreAPICommon)api).RegisterBlockEntityClass("XSkillsBECage", typeof(BlockEntityCage));
		object obj2 = _003C_003EO._003C0_003E__QuadraticEquation;
		if (obj2 == null)
		{
			ExperienceEquationDelegate val = Skill.QuadraticEquation;
			_003C_003EO._003C0_003E__QuadraticEquation = val;
			obj2 = (object)val;
		}
		((Skill)this).ExperienceEquation = (ExperienceEquationDelegate)obj2;
		((Skill)this).ExpBase = 100f;
		((Skill)this).ExpMult = 50f;
		((Skill)this).ExpEquationValue = 4f;
	}
}
