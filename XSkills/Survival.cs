using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;
using Vintagestory.GameContent;
using Vintagestory.Server;
using XLib.XLeveling;

namespace XSkills;

public class Survival : XSkill
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static OnPlayerAbilityTierChangedDelegate _003C0_003E__OnSteeplechaser;

		public static ExperienceEquationDelegate _003C1_003E__QuadraticEquation;
	}

	private ICoreClientAPI capi;

	private NightVisionRenderer nightVisionRenderer;

	private IShaderProgram nightVisionShaderProg;

	public int LongLifeId { get; private set; }

	public int HugeStomachId { get; private set; }

	public int WellRestedId { get; private set; }

	public int NudistId { get; private set; }

	public int MeatShieldId { get; private set; }

	public int DiverId { get; private set; }

	public int AllRounderId { get; protected set; }

	public int PhotosynthesisId { get; private set; }

	public int StrongBackId { get; private set; }

	public int OnTheRoadId { get; private set; }

	public int ScoutId { get; private set; }

	public int HealerId { get; private set; }

	public int SteeplechaserId { get; private set; }

	public int SprinterId { get; private set; }

	public int AbundanceAdaptationId { get; private set; }

	public int SoulboundBagId { get; private set; }

	public int LuminiferousId { get; private set; }

	public int CatEyesId { get; private set; }

	public Survival(ICoreAPI api)
		: base("survival", "xskills:skill-survival", "xskills:group-survival")
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Expected O, but got Unknown
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Expected O, but got Unknown
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Expected O, but got Unknown
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Expected O, but got Unknown
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Expected O, but got Unknown
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Expected O, but got Unknown
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Expected O, but got Unknown
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Expected O, but got Unknown
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Expected O, but got Unknown
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Expected O, but got Unknown
		XLeveling obj = XLeveling.Instance(api);
		if (obj != null)
		{
			obj.RegisterSkill((Skill)(object)this);
		}
		((Skill)this).Config = (CustomSkillConfig)(object)new SurvivalSkillConfig();
		LongLifeId = ((Skill)this).AddAbility(new Ability("longlife", "xskills:ability-longlife", "xskills:abilitydesc-longlife", 1, 3, new int[9] { 5, 1, 15, 10, 2, 30, 10, 2, 50 }, false));
		HugeStomachId = ((Skill)this).AddAbility(new Ability("hugestomach", "xskills:ability-hugestomach", "xskills:abilitydesc-hugestomach", 1, 3, new int[3] { 500, 1000, 1500 }, false));
		WellRestedId = ((Skill)this).AddAbility(new Ability("wellrested", "xskills:ability-wellrested", "xskills:abilitydesc-wellrested", 1, 2, new int[4] { 6, 480, 12, 600 }, false));
		NudistId = ((Skill)this).AddAbility(new Ability("nudist", "xskills:ability-nudist", "xskills:abilitydesc-nudist", 3, 2, new int[16]
		{
			6, 2, 3, 1, 10, 3, 4, 1, 10, 2,
			5, 1, 20, 4, 8, 2
		}, false));
		MeatShieldId = ((Skill)this).AddAbility(new Ability("meatshield", "xskills:ability-meatshield", "xskills:abilitydesc-meatshield", 3, 3, new int[6] { 10, 20, 10, 15, 30, 10 }, false));
		DiverId = ((Skill)this).AddAbility(new Ability("diver", "xskills:ability-diver", "xskills:abilitydesc-diver", 3, 2, new int[2] { 50, 75 }, false));
		AllRounderId = ((Skill)this).AddAbility(new Ability("allrounder", "xskills:ability-allrounder", "xskills:abilitydesc-allrounder", 5, 1, new int[1] { 1 }, false));
		PhotosynthesisId = ((Skill)this).AddAbility(new Ability("photosynthesis", "xskills:ability-photosynthesis", "xskills:abilitydesc-photosynthesis", 5, 3, new int[6] { 15, 25, 40, 50, 50, 50 }, false));
		StrongBackId = ((Skill)this).AddAbility(new Ability("strongback", "xskills:ability-strongback", "xskills:abilitydesc-strongback", 5, 2, new int[2] { 3, 6 }, false));
		OnTheRoadId = ((Skill)this).AddAbility(new Ability("ontheroad", "xskills:ability-ontheroad", "xskills:abilitydesc-ontheroad", 5, 1, new int[1] { 10 }, false));
		ScoutId = ((Skill)this).AddAbility((Ability)new StatAbility("scout", "walkspeed", "xskills:ability-scout", "xskills:abilitydesc-scout", 5, 1, new int[1] { 5 }, false));
		HealerId = ((Skill)this).AddAbility(new Ability("healer", "xskills:ability-healer", "xskills:abilitydesc-healer", 5, 2, new int[4] { 25, 30, 50, 30 }, false));
		SteeplechaserId = ((Skill)this).AddAbility(new Ability("steeplechaser", "xskills:ability-steeplechaser", "xskills:abilitydesc-steeplechaser", 6, 2, new int[2] { 100, 250 }, false));
		SprinterId = ((Skill)this).AddAbility((Ability)new StatsAbility("sprinter", new string[2] { "walkspeed", "hungerrate" }, "xskills:ability-sprinter", "xskills:abilitydesc-sprinter", 6, 2, new int[4] { 5, 10, 10, 20 }, false));
		AbundanceAdaptationId = ((Skill)this).AddAbility((Ability)new StatsAbility("abundanceadaptation", new string[2] { "healingeffectivness", "hungerrate" }, "xskills:ability-abundanceadaptation", "xskills:abilitydesc-abundanceadaptation", 6, 2, new int[4] { 5, 10, 10, 20 }, false));
		SoulboundBagId = -1;
		if (api.World.Config.GetString("deathPunishment", "drop") != "keep")
		{
			SoulboundBagId = ((Skill)this).AddAbility(new Ability("soulboundbag", "xskills:ability-soulboundbag", "xskills:abilitydesc-soulboundbag", 7, 1, new int[0], false));
		}
		LuminiferousId = ((Skill)this).AddAbility(new Ability("luminiferous", "xskills:ability-luminiferous", "xskills:abilitydesc-luminiferous", 8, 3, new int[9] { 4, 2, 10, 4, 2, 15, 4, 2, 20 }, false));
		CatEyesId = ((Skill)this).AddAbility(new Ability("cateyes", "xskills:ability-cateyes", "xskills:abilitydesc-cateyes", 8, 2, new int[4] { 6, 2000, 8, 2000 }, false));
		Ability obj2 = ((Skill)this)[LongLifeId];
		obj2.OnPlayerAbilityTierChanged = (OnPlayerAbilityTierChangedDelegate)Delegate.Combine((Delegate?)(object)obj2.OnPlayerAbilityTierChanged, (Delegate?)new OnPlayerAbilityTierChangedDelegate(OnLongLife));
		Ability obj3 = ((Skill)this)[HugeStomachId];
		obj3.OnPlayerAbilityTierChanged = (OnPlayerAbilityTierChangedDelegate)Delegate.Combine((Delegate?)(object)obj3.OnPlayerAbilityTierChanged, (Delegate?)new OnPlayerAbilityTierChangedDelegate(OnHugeStomach));
		Ability obj4 = ((Skill)this)[NudistId];
		obj4.OnPlayerAbilityTierChanged = (OnPlayerAbilityTierChangedDelegate)Delegate.Combine((Delegate?)(object)obj4.OnPlayerAbilityTierChanged, (Delegate?)new OnPlayerAbilityTierChangedDelegate(OnNudist));
		Ability obj5 = ((Skill)this)[StrongBackId];
		obj5.OnPlayerAbilityTierChanged = (OnPlayerAbilityTierChangedDelegate)Delegate.Combine((Delegate?)(object)obj5.OnPlayerAbilityTierChanged, (Delegate?)new OnPlayerAbilityTierChangedDelegate(OnStrongBack));
		Ability obj6 = ((Skill)this)[SteeplechaserId];
		OnPlayerAbilityTierChangedDelegate onPlayerAbilityTierChanged = obj6.OnPlayerAbilityTierChanged;
		object obj7 = _003C_003EO._003C0_003E__OnSteeplechaser;
		if (obj7 == null)
		{
			OnPlayerAbilityTierChangedDelegate val = OnSteeplechaser;
			_003C_003EO._003C0_003E__OnSteeplechaser = val;
			obj7 = (object)val;
		}
		obj6.OnPlayerAbilityTierChanged = (OnPlayerAbilityTierChangedDelegate)Delegate.Combine((Delegate?)(object)onPlayerAbilityTierChanged, (Delegate?)obj7);
		ICoreAPI obj8 = ((api is ServerCoreAPI) ? api : null);
		object obj9 = ((obj8 != null) ? ((APIBase)obj8).ClassRegistryNative : null);
		if (obj9 == null)
		{
			ICoreAPI obj10 = ((api is ClientCoreAPI) ? api : null);
			obj9 = ((obj10 != null) ? ((APIBase)obj10).ClassRegistryNative : null);
		}
		ClassRegistry val2 = (ClassRegistry)obj9;
		if (val2 != null)
		{
			val2.RegisterInventoryClass("xskillshotbar", typeof(XSkillsPlayerInventory));
		}
		((Skill)this).ClassExpMultipliers["commoner"] = 0.1f;
		((Skill)this).ClassExpMultipliers["hunter"] = 0.2f;
		((Skill)this).ClassExpMultipliers["malefactor"] = 0.15f;
		((Skill)this).ClassExpMultipliers["clockmaker"] = -0.2f;
		((Skill)this).ClassExpMultipliers["blackguard"] = -0.05f;
		((Skill)this).ClassExpMultipliers["miner"] = -0.05f;
		((Skill)this).ClassExpMultipliers["forager"] = -0.15f;
		((Skill)this).ClassExpMultipliers["archer"] = 0.05f;
		((Skill)this).ClassExpMultipliers["vanguard"] = 0.05f;
		((Skill)this).ClassExpMultipliers["gatherer"] = -0.15f;
		((Skill)this).ClassExpMultipliers["vanguard"] = 0.1f;
		object obj11 = _003C_003EO._003C1_003E__QuadraticEquation;
		if (obj11 == null)
		{
			ExperienceEquationDelegate val3 = Skill.QuadraticEquation;
			_003C_003EO._003C1_003E__QuadraticEquation = val3;
			obj11 = (object)val3;
		}
		((Skill)this).ExperienceEquation = (ExperienceEquationDelegate)obj11;
		((Skill)this).ExpBase = 10f;
		((Skill)this).ExpMult = 5f;
		((Skill)this).ExpEquationValue = 0.4f;
	}

	public void OnHugeStomach(PlayerAbility playerAbility, int oldTier)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		IPlayer player = playerAbility.PlayerSkill.PlayerSkillSet.Player;
		if (player == null)
		{
			return;
		}
		EntityPlayer entity = player.Entity;
		if (((entity != null) ? new EnumAppSide?(((Entity)entity).Api.Side) : null) == (EnumAppSide?)1)
		{
			EntityBehaviorHunger behavior = ((Entity)player.Entity).GetBehavior<EntityBehaviorHunger>();
			if (behavior != null)
			{
				float num = (float)(1500 + playerAbility.Value(0, 0)) / behavior.MaxSaturation;
				behavior.MaxSaturation = 1500 + playerAbility.Value(0, 0);
				behavior.FruitLevel *= num;
				behavior.GrainLevel *= num;
				behavior.VegetableLevel *= num;
				behavior.DairyLevel *= num;
				behavior.ProteinLevel *= num;
				behavior.Saturation *= num;
				behavior.UpdateNutrientHealthBoost();
			}
		}
	}

	public void OnLongLife(PlayerAbility playerAbility, int oldTier)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		IPlayer player = playerAbility.PlayerSkill.PlayerSkillSet.Player;
		if (player == null)
		{
			return;
		}
		EntityPlayer entity = player.Entity;
		if (((entity != null) ? new EnumAppSide?(((Entity)entity).Api.Side) : null) == (EnumAppSide?)1)
		{
			EntityBehavior behavior = ((Entity)player.Entity).GetBehavior("health");
			EntityBehaviorHealth val = (EntityBehaviorHealth)(object)((behavior is EntityBehaviorHealth) ? behavior : null);
			if (val != null)
			{
				((Entity)player.Entity).Stats.Set("maxhealthExtraPoints", "longlife", 0.01f * (float)playerAbility.SkillDependentValue(0) * val.BaseMaxHealth, false);
			}
		}
	}

	public void OnStrongBack(PlayerAbility playerAbility, int oldTier)
	{
		IPlayer player = playerAbility.PlayerSkill.PlayerSkillSet.Player;
		if (player == null)
		{
			return;
		}
		IPlayerInventoryManager inventoryManager = player.InventoryManager;
		PlayerInventoryManager val = (PlayerInventoryManager)(object)((inventoryManager is PlayerInventoryManager) ? inventoryManager : null);
		XSkillsPlayerInventory xSkillsPlayerInventory = val.GetOwnInventory("xskillshotbar") as XSkillsPlayerInventory;
		if (xSkillsPlayerInventory == null)
		{
			try
			{
				xSkillsPlayerInventory = new XSkillsPlayerInventory("xskillshotbar", player.PlayerUID, ((Entity)player.Entity).Api);
				val.Inventories.Add(((InventoryBase)xSkillsPlayerInventory).InventoryID, (InventoryBase)(object)xSkillsPlayerInventory);
			}
			catch (Exception)
			{
				return;
			}
		}
		xSkillsPlayerInventory.SetSize(playerAbility.Value(0, 0));
		xSkillsPlayerInventory.SwitchCD = (((Skill)this).Config as SurvivalSkillConfig).invSwitchCD;
	}

	public void OnNudist(PlayerAbility playerAbility, int oldTier)
	{
		PlayerSkillSet playerSkillSet = playerAbility.PlayerSkill.PlayerSkillSet;
		IPlayer val = ((playerSkillSet != null) ? playerSkillSet.Player : null);
		if (val == null)
		{
			return;
		}
		IPlayerInventoryManager inventoryManager = val.InventoryManager;
		InventoryCharacter inv = default(InventoryCharacter);
		ref InventoryCharacter reference = ref inv;
		IInventory obj = ((inventoryManager != null) ? inventoryManager.GetOwnInventory("character") : null);
		reference = (InventoryCharacter)(object)((obj is InventoryCharacter) ? obj : null);
		if (inv == null)
		{
			return;
		}
		OnNudist(inv);
		EntityPlayer entity = val.Entity;
		XSkillsPlayerBehavior xSkillsPlayerBehavior = ((entity != null) ? ((Entity)entity).GetBehavior<XSkillsPlayerBehavior>() : null);
		if (xSkillsPlayerBehavior == null)
		{
			return;
		}
		if (xSkillsPlayerBehavior.NudistSlotNotified == null && playerAbility.Tier > 0)
		{
			xSkillsPlayerBehavior.NudistSlotNotified = delegate
			{
				OnNudist(inv);
			};
			((InventoryBase)inv).SlotModified += xSkillsPlayerBehavior.NudistSlotNotified;
		}
		else if (xSkillsPlayerBehavior.NudistSlotNotified != null && playerAbility.Tier <= 0)
		{
			((InventoryBase)inv).SlotModified -= xSkillsPlayerBehavior.NudistSlotNotified;
			xSkillsPlayerBehavior.NudistSlotNotified = null;
		}
	}

	public static void OnNudist(InventoryCharacter inv)
	{
		object obj;
		if (inv == null)
		{
			obj = null;
		}
		else
		{
			IPlayer player = ((InventoryBasePlayer)inv).Player;
			obj = ((player != null) ? player.Entity : null);
		}
		Entity val = (Entity)obj;
		if (val == null)
		{
			return;
		}
		float num = 0f;
		if (((InventoryBase)inv).Count <= 14)
		{
			return;
		}
		if (((InventoryBase)inv)[0].Itemstack != null)
		{
			num += 0.5f;
		}
		if (((InventoryBase)inv)[1].Itemstack != null)
		{
			num += 1f;
		}
		if (((InventoryBase)inv)[2].Itemstack != null)
		{
			num += 1.25f;
		}
		if (((InventoryBase)inv)[3].Itemstack != null)
		{
			num += 1.5f;
		}
		if (((InventoryBase)inv)[4].Itemstack != null)
		{
			num += 0.5f;
		}
		if (((InventoryBase)inv)[5].Itemstack != null)
		{
			num += 0.75f;
		}
		if (((InventoryBase)inv)[8].Itemstack != null)
		{
			num += 1f;
		}
		if (((InventoryBase)inv)[9].Itemstack != null)
		{
			num += 0.5f;
		}
		if (((InventoryBase)inv)[11].Itemstack != null)
		{
			num += 1.25f;
		}
		if (((InventoryBase)inv)[12].Itemstack != null)
		{
			num += 1.5f;
		}
		if (((InventoryBase)inv)[13].Itemstack != null)
		{
			num += 2f;
		}
		if (((InventoryBase)inv)[14].Itemstack != null)
		{
			num += 2f;
		}
		XLeveling obj2 = XLeveling.Instance(((InventoryBase)inv).Api);
		if (!(((obj2 != null) ? obj2.GetSkill("survival", false) : null) is Survival survival))
		{
			return;
		}
		PlayerSkillSet behavior = val.GetBehavior<PlayerSkillSet>();
		object obj3;
		if (behavior == null)
		{
			obj3 = null;
		}
		else
		{
			PlayerSkill obj4 = behavior[((Skill)survival).Id];
			obj3 = ((obj4 != null) ? obj4[survival.NudistId] : null);
		}
		PlayerAbility val2 = (PlayerAbility)obj3;
		if (val2 != null)
		{
			val.Stats.Set("walkspeed", "ability-nudist", val2.FValue(0, 0f) - num * val2.FValue(1, 0f), false);
			val.Stats.Set("maxhealthExtraPoints", "ability-nudist", (float)val2.Value(2, 0) - num * (float)val2.Value(3, 0), false);
			val.Stats.Set("hungerrate", "ability-nudist", 0f - val2.FValue(4, 0f) + num * val2.FValue(5, 0f), false);
			EntityBehaviorBodyTemperature behavior2 = val.GetBehavior<EntityBehaviorBodyTemperature>();
			if (behavior2 != null)
			{
				typeof(EntityBehaviorBodyTemperature).GetField("bodyTemperatureResistance", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(behavior2, StringUtil.ToFloat(((InventoryBase)inv).Api.World.Config.GetString("bodyTemperatureResistance", (string)null), 0f) + Math.Min((float)val2.Value(7, 0) * num - (float)val2.Value(6, 0), 0f));
			}
		}
	}

	public static void OnSteeplechaser(PlayerAbility playerAbility, int oldTier)
	{
		float num = 1f;
		if (oldTier > 0)
		{
			num /= 1f + (float)playerAbility.Ability.Value(oldTier, 0) * 0.01f;
		}
		if (playerAbility.Tier > 0)
		{
			num *= 1f + (float)playerAbility.Value(0, 0) * 0.01f;
		}
		EntityBehaviorControlledPhysics behavior = ((Entity)playerAbility.PlayerSkill.PlayerSkillSet.Player.Entity).GetBehavior<EntityBehaviorControlledPhysics>();
		if (behavior != null)
		{
			behavior.stepHeight *= num;
		}
	}

	public override void FromConfig(SkillConfig config)
	{
		((Skill)this).FromConfig(config);
		InitNightVision();
	}

	private void InitNightVision()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		ref ICoreClientAPI reference = ref capi;
		ICoreAPI api = ((Skill)this).XLeveling.Api;
		reference = (ICoreClientAPI)(object)((api is ICoreClientAPI) ? api : null);
		if (capi == null)
		{
			return;
		}
		capi.Event.ReloadShader += new ActionBoolReturn(LoadShader);
		LoadShader();
		nightVisionRenderer = new NightVisionRenderer(capi, nightVisionShaderProg);
		capi.Event.RegisterRenderer((IRenderer)(object)nightVisionRenderer, (EnumRenderStage)11, (string)null);
		if (!(((Skill)this).Config as SurvivalSkillConfig).allowCatEyesToggle)
		{
			return;
		}
		capi.Input.RegisterHotKey("cateyestoggle", "Cat eyes toggle", (GlKeys)98, (HotkeyType)4, false, false, false);
		capi.Input.SetHotKeyHandler("cateyestoggle", (ActionConsumable<KeyCombination>)delegate
		{
			if (((EntityAgent)((IPlayer)capi.World.Player).Entity).Controls.Sneak)
			{
				if ((nightVisionRenderer.Mode & EnumNightVisionMode.Compress) == 0)
				{
					nightVisionRenderer.Mode |= EnumNightVisionMode.Compress;
					capi.ShowChatMessage("Compress: on");
				}
				else
				{
					nightVisionRenderer.Mode &= ~EnumNightVisionMode.Compress;
					capi.ShowChatMessage("Compress: off");
				}
			}
			else
			{
				switch (nightVisionRenderer.Mode & EnumNightVisionMode.Filter)
				{
				case EnumNightVisionMode.FilterNone:
					nightVisionRenderer.Mode = (nightVisionRenderer.Mode & ~EnumNightVisionMode.Filter) | EnumNightVisionMode.FilterGray;
					capi.ShowChatMessage("Filter: Gray");
					break;
				case EnumNightVisionMode.FilterGray:
					nightVisionRenderer.Mode = (nightVisionRenderer.Mode & ~EnumNightVisionMode.Filter) | EnumNightVisionMode.FilterSepia;
					capi.ShowChatMessage("Filter: Sepia");
					break;
				case EnumNightVisionMode.FilterSepia:
					nightVisionRenderer.Mode = (nightVisionRenderer.Mode & ~EnumNightVisionMode.Filter) | EnumNightVisionMode.FilterGreen;
					capi.ShowChatMessage("Filter: Green");
					break;
				case EnumNightVisionMode.FilterGreen:
					nightVisionRenderer.Mode = (nightVisionRenderer.Mode & ~EnumNightVisionMode.Filter) | EnumNightVisionMode.FilterBlue;
					capi.ShowChatMessage("Filter: Blue");
					break;
				case EnumNightVisionMode.FilterBlue:
					nightVisionRenderer.Mode = (nightVisionRenderer.Mode & ~EnumNightVisionMode.Filter) | EnumNightVisionMode.FilterRed;
					capi.ShowChatMessage("Filter: Red");
					break;
				case EnumNightVisionMode.FilterRed:
					nightVisionRenderer.Mode = (nightVisionRenderer.Mode & ~EnumNightVisionMode.Filter) | EnumNightVisionMode.Deactivated;
					capi.ShowChatMessage("Filter: Deactivated");
					break;
				case EnumNightVisionMode.Deactivated:
					nightVisionRenderer.Mode = (nightVisionRenderer.Mode & ~EnumNightVisionMode.Filter) | EnumNightVisionMode.FilterNone;
					capi.ShowChatMessage("Filter: None");
					break;
				}
			}
			LoadShader();
			return true;
		});
	}

	public bool LoadShader()
	{
		nightVisionShaderProg = capi.Shader.NewShaderProgram();
		nightVisionShaderProg.VertexShader = capi.Shader.NewShader((EnumShaderType)35633);
		nightVisionShaderProg.FragmentShader = capi.Shader.NewShader((EnumShaderType)35632);
		nightVisionShaderProg.VertexShader.Code = GetVertexShaderCode();
		if (nightVisionRenderer != null)
		{
			nightVisionShaderProg.FragmentShader.Code = GetFragmentShaderCode(nightVisionRenderer.Mode);
		}
		else
		{
			nightVisionShaderProg.FragmentShader.Code = GetFragmentShaderCode(EnumNightVisionMode.Default);
		}
		capi.Shader.RegisterMemoryShaderProgram("nightvision", nightVisionShaderProg);
		nightVisionShaderProg.Compile();
		if (nightVisionRenderer != null)
		{
			nightVisionRenderer.Shader = nightVisionShaderProg;
		}
		return true;
	}

	public static string GetVertexShaderCode()
	{
		return "\r\n                #version 330 core\r\n                #extension GL_ARB_explicit_attrib_location: enable\r\n                layout(location = 0) in vec3 vertex;\r\n\r\n                out vec2 uv;\r\n\r\n                void main(void)\r\n                {\r\n                    gl_Position = vec4(vertex.xy, 0, 1);\r\n                    uv = (vertex.xy + 1.0) / 2.0;\r\n                }";
	}

	public static string GetFragmentShaderCode(EnumNightVisionMode mode)
	{
		string text = "#version 330 core\r\n";
		if ((mode & EnumNightVisionMode.FilterGray) > EnumNightVisionMode.FilterNone)
		{
			text += "#define GRAY 1\r\n";
		}
		if ((mode & EnumNightVisionMode.FilterSepia) > EnumNightVisionMode.FilterNone)
		{
			text += "#define SEPIA 1\r\n";
		}
		if ((mode & EnumNightVisionMode.FilterGreen) > EnumNightVisionMode.FilterNone)
		{
			text += "#define GREEN 1\r\n";
		}
		if ((mode & EnumNightVisionMode.FilterRed) > EnumNightVisionMode.FilterNone)
		{
			text += "#define RED 1\r\n";
		}
		if ((mode & EnumNightVisionMode.FilterBlue) > EnumNightVisionMode.FilterNone)
		{
			text += "#define BLUE 1\r\n";
		}
		if ((mode & EnumNightVisionMode.Compress) > EnumNightVisionMode.FilterNone)
		{
			text += "#define COMPRESS 1\r\n";
		}
		return text + "\r\n                uniform sampler2D primaryScene;\r\n                uniform float intensity;\r\n                uniform float brightness;\r\n                in vec2 uv;\r\n                out vec4 outColor;\r\n                void main () {\r\n\r\n                    vec4 color = texture(primaryScene, uv);\r\n                    //default mix with sepia, optional gray\r\n\t                #if GRAY\r\n                        vec3 mixColor = vec3(dot(color.rgb, vec3(0.2126, 0.7152, 0.0722)));\r\n                    #elif SEPIA\r\n\t                    vec3 mixColor = vec3(\r\n\t\t                    (color.r * 0.393) + (color.g * 0.769) + (color.b * 0.189),\r\n\t\t                    (color.r * 0.349) + (color.g * 0.686) + (color.b * 0.168),\r\n\t\t                    (color.r * 0.272) + (color.g * 0.534) + (color.b * 0.131));\r\n                    #elif GREEN\r\n\t                    vec3 mixColor = vec3(\r\n\t\t                    0.0f,\r\n\t\t                    (color.r * 0.2126) + (color.g * 0.7152) + (color.b * 0.0722),\r\n\t\t                    0.0f);\r\n                    #elif RED\r\n\t                    vec3 mixColor = vec3(\r\n\t\t                    (color.r * 0.2126) + (color.g * 0.7152) + (color.b * 0.0722),\r\n\t\t                    0.0f,\r\n\t\t                    0.0f);\r\n                    #elif BLUE\r\n\t                    vec3 mixColor = vec3(\r\n\t\t                    0.0f,\r\n\t\t                    0.0f,\t\t                    \r\n                            (color.r * 0.2126) + (color.g * 0.7152) + (color.b * 0.0722));\r\n\t                #else\r\n                        vec3 mixColor = color.rgb;\r\n                    #endif\r\n\r\n                    float inten = intensity;\r\n\t                #if COMPRESS\r\n                        float bright = ((color.r * 0.2126) + (color.g * 0.7152) + (color.b * 0.0722));\r\n                        inten = inten * (1.0 - min(sqrt(bright), 1.0));\r\n                        //inten = inten * (1.0 - min(bright * (1.0 + brightness) * 0.5, 1.0));\r\n                    #endif\r\n                    \r\n                    float scale = 1.0 + brightness * inten;\r\n                    outColor.r = min((color.r * (1.0 - inten) + mixColor.r * inten) * scale, 1.0);\r\n                    outColor.g = min((color.g * (1.0 - inten) + mixColor.g * inten) * scale, 1.0);\r\n                    outColor.b = min((color.b * (1.0 - inten) + mixColor.b * inten) * scale, 1.0);\r\n                    outColor.a = color.a;\r\n                }";
	}
}
