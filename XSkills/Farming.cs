using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class Farming : XSkill
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ExperienceEquationDelegate _003C0_003E__QuadraticEquation;
	}

	private List<ItemPlantableSeed> seeds;

	public int GreenThumbId { get; private set; }

	public int DemetersBlessId { get; private set; }

	public int GathererId { get; private set; }

	public int OrchardistId { get; private set; }

	public int RepottingId { get; private set; }

	public int RecyclerId { get; private set; }

	public int CarefulHandsId { get; private set; }

	public int BrightHarvestsId { get; private set; }

	public int CultivatedSeedsId { get; private set; }

	public int BeekeeperId { get; private set; }

	public int ExtensiveFarmingId { get; private set; }

	public int CompostingId { get; private set; }

	public int CrossBreedingId { get; private set; }

	public int BeemasterId { get; private set; }

	public static float MultiBreakMultiplier { get; set; }

	public List<ItemPlantableSeed> Seeds
	{
		get
		{
			if (seeds == null)
			{
				seeds = new List<ItemPlantableSeed>();
				foreach (Item item in ((Skill)this).XLeveling.Api.World.Items)
				{
					ItemPlantableSeed val = (ItemPlantableSeed)(object)((item is ItemPlantableSeed) ? item : null);
					if (val != null)
					{
						seeds.Add(val);
					}
				}
			}
			return seeds;
		}
	}

	public Farming(ICoreAPI api)
		: base("farming", "xskills:skill-farming", "xskills:group-collecting")
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Expected O, but got Unknown
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		XLeveling.Instance(api).RegisterSkill((Skill)(object)this);
		((Skill)this).Config = (CustomSkillConfig)(object)new FarmingConfig();
		MultiBreakMultiplier = 1f;
		seeds = null;
		GreenThumbId = ((Skill)this).AddAbility(new Ability("greenthumb", "xskills:ability-greenthumb", "xskills:abilitydesc-greenthumb", 1, 3, new int[9] { 10, 2, 30, 20, 4, 60, 20, 4, 100 }, false));
		DemetersBlessId = ((Skill)this).AddAbility(new Ability("demetersbless", "xskills:ability-demetersbless", "xskills:abilitydesc-demetersbless", 1, 3, new int[9] { 5, 1, 15, 10, 2, 30, 10, 2, 50 }, false));
		GathererId = ((Skill)this).AddAbility(new Ability("gatherer", "xskills:ability-gatherer", "xskills:abilitydesc-gatherer", 1, 2, new int[6] { 10, 2, 30, 20, 4, 60 }, false));
		OrchardistId = ((Skill)this).AddAbility(new Ability("orchardist", "xskills:ability-orchardist", "xskills:abilitydesc-orchardist", 3, 2, new int[6] { 10, 2, 30, 20, 4, 60 }, false));
		RepottingId = ((Skill)this).AddAbility(new Ability("repotting", "xskills:ability-repotting", "xskills:abilitydesc-repotting", 3, 1, 0, false));
		if (!api.ModLoader.IsModEnabled("farmlanddropssoil") && !api.ModLoader.IsModEnabled("xfarmlanddrops"))
		{
			RecyclerId = ((Skill)this).AddAbility(new Ability("recycler", "xskills:ability-recycler", "xskills:abilitydesc-recycler", 3, 2, new int[2] { 0, 65 }, false));
		}
		CarefulHandsId = ((Skill)this).AddAbility(new Ability("carefulhands", "xskills:ability-carefulhands", "xskills:abilitydesc-carefulhands", 3, 1, 0, false));
		((Skill)this).SpecialisationID = ((Skill)this).AddAbility(new Ability("farmer", "xskills:ability-farmer", "xskills:abilitydesc-farmer", 5, 1, new int[1] { 40 }, false));
		BrightHarvestsId = ((Skill)this).AddAbility(new Ability("brightharvest", "xskills:ability-brightharvest", "xskills:abilitydesc-brightharvest", 5, 2, new int[2] { 40, 80 }, false));
		CultivatedSeedsId = ((Skill)this).AddAbility(new Ability("cultivatedseeds", "xskills:ability-cultivatedseeds", "xskills:abilitydesc-cultivatedseeds", 5, 2, new int[6] { 10, 1, 30, 10, 2, 50 }, false));
		BeekeeperId = ((Skill)this).AddAbility(new Ability("beekeeper", "xskills:ability-beekeeper", "xskills:abilitydesc-beekeeper", 5, 3, new int[3] { 1, 2, 3 }, false));
		ExtensiveFarmingId = ((Skill)this).AddAbility(new Ability("extensivefarming", "xskills:ability-extensivefarming", "xskills:abilitydesc-extensivefarming", 6, 2, new int[2] { 2, 3 }, false));
		CompostingId = ((Skill)this).AddAbility(new Ability("composting", "xskills:ability-composting", "xskills:abilitydesc-composting", 7, 2, new int[4] { 10, 4, 20, 8 }, false));
		CrossBreedingId = ((Skill)this).AddAbility(new Ability("crossbreeding", "xskills:ability-crossbreeding", "xskills:abilitydesc-crossbreeding", 8, 1, new int[1] { 1 }, false));
		BeemasterId = ((Skill)this).AddAbility(new Ability("beemaster", "xskills:ability-beemaster", "xskills:abilitydesc-beemaster", 10, 1, 0, false));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsGrass", typeof(XSkillsGrassBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsCrop", typeof(XSkillsCropBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsFarmland", typeof(XSkillsFarmlandBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsMushroom", typeof(XSkillsMushroomBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsSkep", typeof(XSkillsSkepBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsBerryBush", typeof(XSkillsBerryBushBehavior));
		((Skill)this).ClassExpMultipliers["commoner"] = 0.1f;
		((Skill)this).ClassExpMultipliers["forager"] = 0.2f;
		((Skill)this).ClassExpMultipliers["blackguard"] = -0.15f;
		((Skill)this).ClassExpMultipliers["miner"] = -0.2f;
		((Skill)this).ClassExpMultipliers["hunter"] = -0.15f;
		((Skill)this).ClassExpMultipliers["malefactor"] = 0.1f;
		((Skill)this).ClassExpMultipliers["clockmaker"] = 0.15f;
		((Skill)this).ClassExpMultipliers["archer"] = -0.05f;
		((Skill)this).ClassExpMultipliers["vanguard"] = 0.1f;
		((Skill)this).ClassExpMultipliers["gatherer"] = 0.1f;
		((Skill)this).ClassExpMultipliers["mercenary"] = 0f;
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
