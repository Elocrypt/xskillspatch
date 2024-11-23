using System;
using System.Collections.Generic;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
using Vintagestory.ServerMods;
using XLib.XLeveling;

namespace XSkills;

public class Mining : CollectingSkill
{
	protected Dictionary<BlockPos, IPlayer> BombPosToPlayerMap;

	protected Dictionary<string, float> OreRarities;

	public int StoneBreakerId { get; private set; }

	public int StoneCutterId { get; private set; }

	public int OreMinerId { get; private set; }

	public int GemstoneMinerId { get; private set; }

	public int CrystalSeekerId { get; private set; }

	public int BombermanId { get; private set; }

	public int GeologistId { get; private set; }

	public int VeinMinerId { get; private set; }

	public int TunnelDiggerId { get; private set; }

	public int BlasterId { get; private set; }

	public Mining(ICoreAPI api)
		: base("mining")
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Expected O, but got Unknown
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Expected O, but got Unknown
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		XLeveling.Instance(api).RegisterSkill((Skill)(object)this);
		base.Tool = (EnumTool)1;
		BombPosToPlayerMap = new Dictionary<BlockPos, IPlayer>();
		((Skill)this).Config = (CustomSkillConfig)(object)new MiningSkillConfig();
		if (!(((Skill)this).Config as MiningSkillConfig).geologistBlacklist.Contains("quartz"))
		{
			(((Skill)this).Config as MiningSkillConfig).geologistBlacklist.Add("quartz");
		}
		StoneBreakerId = ((Skill)this).AddAbility(new Ability("stonebreaker", "xskills:ability-stonebreaker", "xskills:abilitydesc-stonebreaker", 1, 3, new int[9] { 10, 2, 30, 20, 4, 60, 20, 4, 100 }, false));
		StoneCutterId = ((Skill)this).AddAbility(new Ability("stonecutter", "xskills:ability-stonecutter", "xskills:abilitydesc-stonecutter", 1, 3, new int[9] { 2, 1, 12, 4, 2, 24, 4, 2, 44 }, false));
		OreMinerId = ((Skill)this).AddAbility(new Ability("oreminer", "xskills:ability-oreminer", "xskills:abilitydesc-oreminer", 1, 3, new int[9] { 5, 1, 15, 10, 2, 30, 10, 2, 50 }, false));
		GemstoneMinerId = ((Skill)this).AddAbility(new Ability("gemstoneminer", "xskills:ability-gemstoneminer", "xskills:abilitydesc-gemstoneminer", 5, 3, new int[9] { 5, 1, 15, 10, 2, 30, 10, 2, 50 }, false));
		base.MiningSpeedId = ((Skill)this).AddAbility(new Ability("pickaxeexpert", "xskills:ability-pickaxeexpert", "xskills:abilitydesc-pickaxeexpert", 1, 3, new int[15]
		{
			1, 1, 2, 10, 4, 2, 2, 4, 10, 6,
			2, 2, 6, 10, 8
		}, false));
		base.DurabilityId = ((Skill)this).AddAbility(new Ability("carefulminer", "xskills:ability-carefulminer", "xskills:abilitydesc-carefulminer", 1, 3, new int[9] { 5, 1, 15, 5, 2, 25, 5, 2, 45 }, false));
		((Skill)this).SpecialisationID = ((Skill)this).AddAbility(new Ability("miner", "xskills:ability-miner", "xskills:abilitydesc-miner", 5, 1, new int[1] { 40 }, false));
		CrystalSeekerId = ((Skill)this).AddAbility(new Ability("crystalseeker", "xskills:ability-crystalseeker", "xskills:abilitydesc-crystalseeker", 5, 1, new int[1] { 100 }, false));
		BombermanId = ((Skill)this).AddAbility(new Ability("bomberman", "xskills:ability-bomberman", "xskills:abilitydesc-bomberman", 7, 3, new int[3] { 33, 66, 100 }, false));
		GeologistId = ((Skill)this).AddAbility(new Ability("geologist", "xskills:ability-geologist", "xskills:abilitydesc-geologist", 10, 3, new int[3] { 1, 2, 3 }, false));
		VeinMinerId = ((Skill)this).AddAbility(new Ability("veinminer", "xskills:ability-veinminer", "xskills:abilitydesc-veinminer", 12, 1, new int[5] { 2, 3, 10, 100, 10 }, false));
		TunnelDiggerId = ((Skill)this).AddAbility(new Ability("tunneldigger", "xskills:ability-tunneldigger", "xskills:abilitydesc-tunneldigger", 12, 1, new int[2] { 50, 20 }, false));
		BlasterId = ((Skill)this).AddAbility(new Ability("blaster", "xskills:ability-blaster", "xskills:abilitydesc-blaster", 12, 1, 0, false));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsStone", typeof(XSkillsStoneBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsOres", typeof(XSkillsOreBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsGems", typeof(XSkillsGemBehavior));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsBomb", typeof(XSkillsBombBehavior));
		((ICoreAPICommon)api).RegisterItemClass("XSkillsItemPickaxe", typeof(XSkillsItemPickaxe));
		((Skill)this).ClassExpMultipliers["commoner"] = 0.1f;
		((Skill)this).ClassExpMultipliers["miner"] = 0.2f;
		((Skill)this).ClassExpMultipliers["hunter"] = -0.2f;
		((Skill)this).ClassExpMultipliers["malefactor"] = -0.1f;
		((Skill)this).ClassExpMultipliers["clockmaker"] = 0.15f;
		((Skill)this).ClassExpMultipliers["blackguard"] = 0.15f;
		((Skill)this).ClassExpMultipliers["forager"] = -0.15f;
		((Skill)this).ClassExpMultipliers["archer"] = -0.1f;
		((Skill)this).ClassExpMultipliers["vanguard"] = 0.05f;
		((Skill)this).ClassExpMultipliers["gatherer"] = -0.15f;
		((Skill)this).ClassExpMultipliers["mercenary"] = 0.05f;
	}

	internal void RegisterExplosion(BlockPos pos, IPlayer player)
	{
		if (!(pos == (BlockPos)null) && player != null)
		{
			BombPosToPlayerMap.Add(pos, player);
			Action<float> action = delegate
			{
				UnregisterExplosion(pos);
			};
			((Skill)this).XLeveling.Api.World.RegisterCallback(action, 1000);
		}
	}

	internal void UnregisterExplosion(BlockPos pos)
	{
		if (!(pos == (BlockPos)null))
		{
			BombPosToPlayerMap.Remove(pos);
		}
	}

	internal IPlayer GetPlayerCausingExplosion(BlockPos pos)
	{
		if (pos == (BlockPos)null)
		{
			return null;
		}
		BombPosToPlayerMap.TryGetValue(pos, out var value);
		return value;
	}

	public float GetOreRarity(string ore)
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Invalid comparison between Unknown and I4
		if (ore == null)
		{
			return 0f;
		}
		if (OreRarities == null)
		{
			IWorldAccessor world = ((Skill)this).XLeveling.Api.World;
			OreRarities = new Dictionary<string, float>();
			float num = 0f;
			float num2 = 1f;
			DepositVariant[] array = ((Skill)this).XLeveling.Api.ModLoader.GetModSystem<GenDeposits>(true)?.Deposits;
			if (array == null)
			{
				return 0f;
			}
			DepositVariant[] array2 = array;
			foreach (DepositVariant val in array2)
			{
				DepositGeneratorBase generatorInst = val.GeneratorInst;
				DiscDepositGenerator val2 = (DiscDepositGenerator)(object)((generatorInst is DiscDepositGenerator) ? generatorInst : null);
				if (val2 == null)
				{
					continue;
				}
				AssetLocation val3 = val2.PlaceBlock.Code.Clone();
				val3.Path = val3.Path.Replace("{rock}", "*");
				Block[] array3 = world.SearchBlocks(val3);
				if (array3.Length == 0 || (int)array3[0].BlockMaterial != 7)
				{
					continue;
				}
				if (val2.SurfaceBlockChance <= 0.1f)
				{
					float absAvgQuantity = val2.GetAbsAvgQuantity();
					if (!OreRarities.TryGetValue(ore, out var value))
					{
						value = 1f;
					}
					absAvgQuantity /= (float)(world.BlockAccessor.MapSizeY * world.BlockAccessor.ChunkSize * world.BlockAccessor.ChunkSize);
					float num3 = value - absAvgQuantity;
					num = Math.Max(num3, num);
					num2 = Math.Min(num3, num2);
					OreRarities[val.Code] = num3;
				}
				else
				{
					OreRarities[val.Code] = 0f;
				}
			}
			List<string> list = new List<string>(OreRarities.Keys);
			num -= num2;
			foreach (string item in list)
			{
				OreRarities[item] = Math.Max((OreRarities[item] - num2) / num, 0f);
			}
		}
		if (OreRarities.TryGetValue(ore, out var value2))
		{
			return value2;
		}
		return 0f;
	}

	public void CheckBlock(IWorldAccessor world, IClientPlayer byPlayer, int x, int y, int z, int dimension, int range)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		ICoreAPI api = world.Api;
		ICoreClientAPI val = (ICoreClientAPI)(object)((api is ICoreClientAPI) ? api : null);
		if (val == null)
		{
			return;
		}
		BlockOre val2 = null;
		int num = (range + 1) * 3;
		for (int i = x - range; i <= x + range; i++)
		{
			for (int j = y - range; j <= y + range; j++)
			{
				for (int k = z - range; k <= z + range; k++)
				{
					Block block = world.BlockAccessor.GetBlock(new BlockPos(i, j, k, dimension));
					BlockOre val3 = (BlockOre)(object)((block is BlockOre) ? block : null);
					if (val3 != null)
					{
						int num2 = Math.Abs(i - x) + Math.Abs(j - y) + Math.Abs(k - z);
						if (num2 < num && !(((Skill)this).Config as MiningSkillConfig).geologistBlacklist.Contains(val3.OreName))
						{
							num = num2;
							val2 = val3;
						}
					}
				}
			}
		}
		if (val2 == null || num <= 1)
		{
			return;
		}
		IClientWorldAccessor world2 = val.World;
		ClientMain val4 = (ClientMain)(object)((world2 is ClientMain) ? world2 : null);
		object? obj = ((object)val4)?.GetType().GetField("eventManager", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(val4);
		ClientEventManager val5 = (ClientEventManager)((obj is ClientEventManager) ? obj : null);
		if (val5 != null)
		{
			object obj2;
			if (byPlayer == null)
			{
				obj2 = null;
			}
			else
			{
				EntityPlayer entity = ((IPlayer)byPlayer).Entity;
				obj2 = ((entity != null) ? ((Entity)entity).Pos.XYZInt : null);
			}
			Vec3i val6 = (Vec3i)obj2;
			if (!(val6 == (Vec3i)null))
			{
				val6 = new Vec3i(x - val6.X, y - val6.Y, z - val6.Z);
				string text = "[" + val6.X + ", " + val6.Y + ", " + val6.Z + "]";
				text = text + Lang.Get("game:ore-" + ((RegistryObject)val2).Variant?["type"], Array.Empty<object>()) + Lang.GetUnformatted("xskills:isnearby");
				val5.TriggerNewServerChatLine(GlobalConstants.InfoLogChatGroup, text, (EnumChatType)4, (string)null);
			}
		}
	}
}
