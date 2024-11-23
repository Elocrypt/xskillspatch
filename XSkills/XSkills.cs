using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;
using Vintagestory.GameContent;
using Vintagestory.Server;
using XLib.XLeveling;

namespace XSkills;

public class XSkills : ModSystem
{
	private static Harmony harmony;

	public static XSkills Instance { get; private set; }

	public Dictionary<string, Skill> Skills { get; set; }

	public ICoreAPI Api { get; private set; }

	public XLeveling XLeveling { get; private set; }

	internal static void DoHarmonyPatch(ICoreAPI api)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		if (harmony == null)
		{
			XSkills modSystem = api.ModLoader.GetModSystem<XSkills>(true);
			harmony = new Harmony("XSkillsPatch");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
			BlockEntityAnvilPatch.Apply(harmony, api.ClassRegistry.GetBlockEntity("Anvil"));
			BlockEntityOvenPatch.Apply(harmony, api.ClassRegistry.GetBlockEntity("Oven"), modSystem);
			CookingRecipePatch.Apply(harmony, typeof(CookingRecipe));
			Type blockEntity = api.ClassRegistry.GetBlockEntity("ExpandedOven");
			if (blockEntity != null)
			{
				BlockEntityOvenPatch.Apply(harmony, blockEntity, modSystem);
			}
			blockEntity = api.ClassRegistry.GetBlockEntity("OvenBakingTop");
			if (blockEntity != null)
			{
				BlockEntityOvenPatch.Apply(harmony, blockEntity, modSystem);
			}
			blockEntity = api.ClassRegistry.GetBlockEntity("MixingBowl");
			if (blockEntity != null)
			{
				BlockEntityMixingBowlPatch.Apply(harmony, blockEntity, modSystem);
			}
			blockEntity = api.ClassRegistry.GetItemClass("ExpandedRawFood");
			if (blockEntity != null)
			{
				ItemExpandedRawFoodPatch.Apply(harmony, blockEntity, modSystem);
			}
			blockEntity = api.ClassRegistry.GetBlockClass("BlockSaucepan");
			if (blockEntity != null)
			{
				BlockSaucepanPatch.Apply(harmony, blockEntity, modSystem);
			}
			blockEntity = blockEntity?.Assembly.GetType("ACulinaryArtillery.InventoryMixingBowl");
			if (blockEntity != null)
			{
				InventoryMixingBowlPatch.Apply(harmony, blockEntity, modSystem);
			}
		}
	}

	public override double ExecuteOrder()
	{
		return 0.25;
	}

	public XSkills()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public override void Dispose()
	{
		((ModSystem)this).Dispose();
		Harmony obj = harmony;
		if (obj != null)
		{
			obj.UnpatchAll("XSkillsPatch");
		}
		harmony = null;
	}

	public override void StartPre(ICoreAPI api)
	{
		((ModSystem)this).StartPre(api);
		Api = api;
		XLeveling = XLeveling.Instance(Api);
		Skills = new Dictionary<string, Skill>();
		Survival survival = new Survival(api);
		Skills.Add(((Skill)survival).Name, (Skill)(object)survival);
		Farming farming = new Farming(api);
		Skills.Add(((Skill)farming).Name, (Skill)(object)farming);
		Digging digging = new Digging(api);
		Skills.Add(((Skill)digging).Name, (Skill)(object)digging);
		Forestry forestry = new Forestry(api);
		Skills.Add(((Skill)forestry).Name, (Skill)(object)forestry);
		Mining mining = new Mining(api);
		Skills.Add(((Skill)mining).Name, (Skill)(object)mining);
		Husbandry husbandry = new Husbandry(api);
		Skills.Add(((Skill)husbandry).Name, (Skill)(object)husbandry);
		Combat combat = new Combat(api);
		Skills.Add(((Skill)combat).Name, (Skill)(object)combat);
		Metalworking metalworking = new Metalworking(api);
		Skills.Add(((Skill)metalworking).Name, (Skill)(object)metalworking);
		Pottery pottery = new Pottery(api);
		Skills.Add(((Skill)pottery).Name, (Skill)(object)pottery);
		Cooking cooking = new Cooking(api);
		Skills.Add(((Skill)cooking).Name, (Skill)(object)cooking);
		if (api.World.Config.GetBool("temporalStability", false))
		{
			TemporalAdaptation temporalAdaptation = new TemporalAdaptation(api);
			Skills.Add(((Skill)temporalAdaptation).Name, (Skill)(object)temporalAdaptation);
		}
		((ICoreAPICommon)api).RegisterEntityBehaviorClass("XSkillsPlayer", typeof(XSkillsPlayerBehavior));
	}

	public override void Start(ICoreAPI api)
	{
		((ModSystem)this).Start(api);
		(Skills["metalworking"] as Metalworking).RegisterAnvil();
		string[] array = new string[GlobalConstants.IgnoredStackAttributes.Length + 2];
		int i;
		for (i = 0; i < GlobalConstants.IgnoredStackAttributes.Length; i++)
		{
			array[i] = GlobalConstants.IgnoredStackAttributes[i];
		}
		array[i] = "quality";
		i++;
		array[i] = "owner";
		GlobalConstants.IgnoredStackAttributes = array;
		((ICoreAPICommon)api).RegisterBlockEntityBehaviorClass("XskillsOwnable", typeof(BlockEntityBehaviorOwnable));
		ICoreAPI obj = ((api is ServerCoreAPI) ? api : null);
		object obj2 = ((obj != null) ? ((APIBase)obj).ClassRegistryNative : null);
		if (obj2 == null)
		{
			ICoreAPI obj3 = ((api is ClientCoreAPI) ? api : null);
			obj2 = ((obj3 != null) ? ((APIBase)obj3).ClassRegistryNative : null);
		}
		ClassRegistry val = (ClassRegistry)obj2;
		if (val != null)
		{
			val.blockEntityClassnameToTypeMapping["Sapling"] = typeof(XSkillsBlockEntitySapling);
			val.blockEntityTypeToClassnameMapping[typeof(XSkillsBlockEntitySapling)] = "Sapling";
			if (Api.ModLoader.IsModEnabled("primitivesurvival"))
			{
				HoeUtil.RegisterItemHoePrimitive(val);
			}
			HoeUtil.RegisterItemHoe(val);
			val.ItemClassToTypeMapping["ItemPlantableSeed"] = typeof(XSkillsItemPlantableSeed);
		}
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		((ModSystem)this).StartClientSide(api);
		XLeveling.CreateDescriptionFile();
		api.Input.RegisterHotKey("xskillshotbarswitch", "Xskills hotbar switch", (GlKeys)100, (HotkeyType)4, false, false, false);
		api.Input.SetHotKeyHandler("xskillshotbarswitch", (ActionConsumable<KeyCombination>)OnHotbarSwitch);
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		((ModSystem)this).StartServerSide(api);
		DoHarmonyPatch((ICoreAPI)(object)api);
	}

	public override void AssetsLoaded(ICoreAPI api)
	{
		((ModSystem)this).AssetsLoaded(api);
		Survival survival = Skills["survival"] as Survival;
		LimitationRequirement val = XLeveling.Limitations["specialisations"];
		if (val != null && survival != null)
		{
			val.ModifierAbility = ((Skill)survival)[survival.AllRounderId];
		}
		foreach (Skill value in Skills.Values)
		{
			value.DisplayName = Lang.GetUnformatted(value.DisplayName);
			value.Group = Lang.GetUnformatted(value.Group);
			foreach (Ability ability in value.Abilities)
			{
				ability.DisplayName = Lang.GetUnformatted(ability.DisplayName);
				ability.Description = Lang.GetUnformatted(ability.Description);
			}
		}
	}

	public bool OnHotbarSwitch(KeyCombination keys)
	{
		ICoreAPI api = Api;
		ICoreAPI obj = ((api is ICoreClientAPI) ? api : null);
		IClientPlayer obj2 = ((obj != null) ? ((ICoreClientAPI)obj).World.Player : null);
		if (!(((obj2 != null) ? ((IPlayer)obj2).InventoryManager.GetOwnInventory("xskillshotbar") : null) is XSkillsPlayerInventory xSkillsPlayerInventory))
		{
			return false;
		}
		xSkillsPlayerInventory.SwitchInventories();
		return true;
	}
}
