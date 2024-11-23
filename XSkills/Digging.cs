using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class Digging : CollectingSkill
{
	private Dictionary<string, BlockDropItemStack[]> panningDrops;

	public int ClayDiggerId { get; private set; }

	public int PeatCutterId { get; private set; }

	public int SaltpeterDiggerId { get; private set; }

	public int MixedClayId { get; private set; }

	public int ScrapDetectorId { get; private set; }

	public int ScrapSpecialistId { get; private set; }

	internal List<PanningDrop> StoneDrops { get; private set; }

	public Dictionary<string, BlockDropItemStack[]> PanningDrops
	{
		get
		{
			if (panningDrops == null)
			{
				LoadPanningDrops();
			}
			return panningDrops;
		}
		private set
		{
			panningDrops = value;
		}
	}

	public Digging(ICoreAPI api)
		: base("digging")
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		XLeveling obj = XLeveling.Instance(api);
		if (obj != null)
		{
			obj.RegisterSkill((Skill)(object)this);
		}
		base.Tool = (EnumTool)4;
		ClayDiggerId = ((Skill)this).AddAbility(new Ability("claydigger", "xskills:ability-claydigger", "xskills:abilitydesc-claydigger", 1, 3, new int[9] { 10, 2, 30, 20, 4, 60, 20, 4, 100 }, false));
		PeatCutterId = ((Skill)this).AddAbility(new Ability("peatcutter", "xskills:ability-peatcutter", "xskills:abilitydesc-peatcutter", 1, 3, new int[9] { 10, 2, 30, 20, 4, 60, 20, 4, 100 }, false));
		SaltpeterDiggerId = ((Skill)this).AddAbility(new Ability("saltpeterdigger", "xskills:ability-saltpeterdigger", "xskills:abilitydesc-saltpeterdigger", 1, 3, new int[9] { 10, 2, 30, 20, 4, 60, 20, 4, 100 }, false));
		base.MiningSpeedId = ((Skill)this).AddAbility(new Ability("shovelexpert", "xskills:ability-shovelexpert", "xskills:abilitydesc-shovelexpert", 1, 3, new int[15]
		{
			1, 1, 2, 10, 4, 2, 2, 4, 10, 6,
			2, 2, 6, 10, 8
		}, false));
		base.DurabilityId = ((Skill)this).AddAbility(new Ability("carefuldigger", "xskills:ability-carefuldigger", "xskills:abilitydesc-carefuldigger", 1, 3, new int[9] { 5, 1, 15, 5, 2, 25, 5, 2, 45 }, false));
		MixedClayId = ((Skill)this).AddAbility(new Ability("mixedclay", "xskills:ability-mixedclay", "xskills:abilitydesc-mixedclay", 3, 2, new int[2] { 50, 100 }, false));
		((Skill)this).SpecialisationID = ((Skill)this).AddAbility(new Ability("digger", "xskills:ability-digger", "xskills:abilitydesc-digger", 5, 1, new int[1] { 40 }, false));
		ScrapDetectorId = ((Skill)this).AddAbility(new Ability("scrapdetector", "xskills:ability-scrapdetector", "xskills:abilitydesc-scrapdetector", 5, 2, new int[2] { 2, 4 }, false));
		ScrapSpecialistId = ((Skill)this).AddAbility(new Ability("scrapspecialist", "xskills:ability-scrapspecialist", "xskills:abilitydesc-scrapspecialist", 5, 2, new int[4] { 1, 50, 2, 100 }, false));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsSoil", typeof(XSkillsSoilBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsClay", typeof(XSkillsClayBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsSalpeter", typeof(XSkillsSalpeterBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsSand", typeof(XSkillsSandBehavior));
		((Skill)this).ClassExpMultipliers["commoner"] = 0.1f;
		((Skill)this).ClassExpMultipliers["miner"] = 0.2f;
		((Skill)this).ClassExpMultipliers["archer"] = -0.2f;
		((Skill)this).ClassExpMultipliers["hunter"] = -0.15f;
		((Skill)this).ClassExpMultipliers["malefactor"] = 0.1f;
		((Skill)this).ClassExpMultipliers["clockmaker"] = -0.15f;
		((Skill)this).ClassExpMultipliers["blackguard"] = 0.1f;
		((Skill)this).ClassExpMultipliers["forager"] = 0.15f;
		((Skill)this).ClassExpMultipliers["vanguard"] = -0.05f;
		((Skill)this).ClassExpMultipliers["gatherer"] = 0.15f;
		((Skill)this).ClassExpMultipliers["mercenary"] = -0.05f;
	}

	protected void LoadPanningDrops()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		PanningDrops = new Dictionary<string, BlockDropItemStack[]>();
		StoneDrops = new List<PanningDrop>();
		XLeveling xLeveling = ((Skill)this).XLeveling;
		ICoreAPI val = ((xLeveling != null) ? xLeveling.Api : null);
		Block obj = ((val != null) ? val.World.GetBlock(new AssetLocation("game", "pan-wooden")) : null);
		BlockPan val2 = (BlockPan)(object)((obj is BlockPan) ? obj : null);
		if (val2 == null)
		{
			return;
		}
		Dictionary<string, PanningDrop[]> dictionary = ((CollectibleObject)val2).Attributes["panningDrops"].AsObject<Dictionary<string, PanningDrop[]>>((Dictionary<string, PanningDrop[]>)null);
		if (dictionary == null)
		{
			return;
		}
		foreach (string key in dictionary.Keys)
		{
			List<BlockDropItemStack> list = new List<BlockDropItemStack>();
			PanningDrop[] array = dictionary[key];
			foreach (PanningDrop val3 in array)
			{
				if (((JsonItemStack)val3).Code.GetName().Contains("{rocktype}"))
				{
					StoneDrops.Add(val3);
					continue;
				}
				CollectibleObject val4 = (CollectibleObject)(((int)((JsonItemStack)val3).Type != 0) ? ((object)val.World.GetItem(new AssetLocation(((JsonItemStack)val3).Code.Path))) : ((object)val.World.GetBlock(new AssetLocation(((JsonItemStack)val3).Code.Path))));
				if (val4 != null)
				{
					BlockDropItemStack val5 = new BlockDropItemStack(new ItemStack(val4, 1), 1f);
					val5.Quantity = val3.Chance;
					list.Add(val5);
				}
			}
			if (list.Count == 0)
			{
				((ModSystem)((Skill)this).XLeveling).Mod.Logger.Warning("Failed to resolve any panning drops for key: " + key);
			}
			PanningDrops[key] = list.ToArray();
		}
	}
}
