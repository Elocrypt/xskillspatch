using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;
using Vintagestory.GameContent;
using Vintagestory.Server;
using XLib.XLeveling;

namespace XSkills;

public class Metalworking : XSkill
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ExperienceEquationDelegate _003C0_003E__QuadraticEquation;
	}

	private List<SmithingRecipe> duplicatable;

	public int SmelterId { get; private set; }

	public int MetalRecoveryId { get; private set; }

	public int HeatingHitsId { get; private set; }

	public int HammerExpertId { get; private set; }

	public int HeavyHitsId { get; private set; }

	public int BlacksmithId { get; private set; }

	public int FinishingTouchId { get; private set; }

	public int DuplicatorId { get; private set; }

	public int SalvagerId { get; private set; }

	public int MasterSmithId { get; private set; }

	public int SenseOfTime { get; private set; }

	public int MachineLearningId { get; private set; }

	public int BloomeryExpertId { get; private set; }

	public int AutomatedSmithingId { get; private set; }

	public Metalworking(ICoreAPI api)
		: base("metalworking", "xskills:skill-metalworking", "xskills:group-processing")
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Expected O, but got Unknown
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Expected O, but got Unknown
		XLeveling obj = XLeveling.Instance(api);
		if (obj != null)
		{
			obj.RegisterSkill((Skill)(object)this);
		}
		((Skill)this).Config = (CustomSkillConfig)(object)new MetalworkingConfig();
		SmelterId = ((Skill)this).AddAbility(new Ability("smelter", "xskills:ability-smelter", "xskills:abilitydesc-smelter", 1, 3, new int[3] { 10, 20, 25 }, false));
		MetalRecoveryId = -1;
		if (!api.ModLoader.IsModEnabled("metalrecovery"))
		{
			MetalRecoveryId = ((Skill)this).AddAbility(new Ability("metalrecovery", "xskills:ability-metalrecovery", "xskills:abilitydesc-metalrecovery", 1, 3, new int[3] { 4, 3, 2 }, false));
		}
		HeatingHitsId = ((Skill)this).AddAbility(new Ability("heatinghits", "xskills:ability-heatinghits", "xskills:abilitydesc-heatinghits", 1, 2, new int[2] { 1, 2 }, false));
		HammerExpertId = ((Skill)this).AddAbility(new Ability("hammerexpert", "xskills:ability-hammerexpert", "xskills:abilitydesc-hammerexpert", 1, 3, new int[9] { 5, 1, 15, 5, 2, 25, 5, 2, 45 }, false));
		HeavyHitsId = ((Skill)this).AddAbility(new Ability("heavyhits", "xskills:ability-heavyhits", "xskills:abilitydesc-heavyhits", 3, 1, 0, false));
		BlacksmithId = ((Skill)this).AddAbility(new Ability("blacksmith", "xskills:ability-blacksmith", "xskills:abilitydesc-blacksmith", 3, 2, new int[4] { 1, 5, 2, 10 }, false));
		((Skill)this).SpecialisationID = ((Skill)this).AddAbility(new Ability("metalworker", "xskills:ability-metalworker", "xskills:abilitydesc-metalworker", 5, 1, new int[1] { 40 }, false));
		FinishingTouchId = ((Skill)this).AddAbility(new Ability("finishingtouch", "xskills:ability-finishingtouch", "xskills:abilitydesc-finishingtouch", 5, 3, new int[9] { 1, 1, 2, 2, 2, 4, 2, 2, 6 }, false));
		DuplicatorId = ((Skill)this).AddAbility(new Ability("duplicator", "xskills:ability-duplicator", "xskills:abilitydesc-duplicator", 5, 3, new int[9] { 5, 0, 5, 5, 1, 15, 5, 1, 25 }, false));
		SalvagerId = ((Skill)this).AddAbility(new Ability("salvager", "xskills:ability-salvager", "xskills:abilitydesc-salvager", 5, 2, new int[6] { 95, 1, 110, 160, 2, 200 }, false));
		MasterSmithId = ((Skill)this).AddAbility(new Ability("mastersmith", "xskills:ability-mastersmith", "xskills:abilitydesc-mastersmith", 7, 2, new int[4] { 2, 3, 4, 4 }, false));
		SenseOfTime = ((Skill)this).AddAbility(new Ability("senseoftime", "xskills:ability-senseoftime", "xskills:abilitydesc-senseoftime", 8, 1, 0, false));
		MachineLearningId = ((Skill)this).AddAbility(new Ability("machinelearning", "xskills:ability-machinelearning", "xskills:abilitydesc-machinelearning", 8, 1, new int[0], false));
		BloomeryExpertId = ((Skill)this).AddAbility(new Ability("bloomeryexpert", "xskills:ability-bloomeryexpert", "xskills:abilitydesc-bloomeryexpert", 8, 1, new int[0], false));
		AutomatedSmithingId = ((Skill)this).AddAbility(new Ability("automatedsmithing", "xskills:ability-automatedsmithing", "xskills:abilitydesc-automatedsmithing", 10, 1, new int[1] { 1 }, false));
		((ICoreAPICommon)api).RegisterEntityBehaviorClass("disassemblable", typeof(EntityBehaviorDisassemblable));
		((ICoreAPICommon)api).RegisterBlockBehaviorClass("XSkillsBloomery", typeof(XSkillsBloomeryBehavior));
		ICoreServerAPI val = (ICoreServerAPI)(object)((api is ICoreServerAPI) ? api : null);
		if (val != null)
		{
			val.Event.PlayerJoin += (PlayerDelegate)delegate
			{
				UpdateBits();
			};
		}
		((Skill)this).ClassExpMultipliers["commoner"] = 0.1f;
		((Skill)this).ClassExpMultipliers["clockmaker"] = 0.2f;
		((Skill)this).ClassExpMultipliers["forager"] = -0.2f;
		((Skill)this).ClassExpMultipliers["hunter"] = -0.15f;
		((Skill)this).ClassExpMultipliers["malefactor"] = -0.15f;
		((Skill)this).ClassExpMultipliers["blackguard"] = 0.15f;
		((Skill)this).ClassExpMultipliers["miner"] = 0.15f;
		((Skill)this).ClassExpMultipliers["archer"] = -0.1f;
		((Skill)this).ClassExpMultipliers["vanguard"] = 0.1f;
		((Skill)this).ClassExpMultipliers["gatherer"] = -0.2f;
		((Skill)this).ClassExpMultipliers["mercenary"] = 0.1f;
		object obj2 = _003C_003EO._003C0_003E__QuadraticEquation;
		if (obj2 == null)
		{
			ExperienceEquationDelegate val2 = Skill.QuadraticEquation;
			_003C_003EO._003C0_003E__QuadraticEquation = val2;
			obj2 = (object)val2;
		}
		((Skill)this).ExperienceEquation = (ExperienceEquationDelegate)obj2;
		((Skill)this).ExpBase = 40f;
		((Skill)this).ExpMult = 10f;
		((Skill)this).ExpEquationValue = 0.8f;
	}

	public void RegisterAnvil()
	{
		ICoreAPI api = ((Skill)this).XLeveling.Api;
		ICoreAPI obj = ((api is ServerCoreAPI) ? api : null);
		object obj2 = ((obj != null) ? ((APIBase)obj).ClassRegistryNative : null);
		if (obj2 == null)
		{
			ICoreAPI api2 = ((Skill)this).XLeveling.Api;
			ICoreAPI obj3 = ((api2 is ClientCoreAPI) ? api2 : null);
			obj2 = ((obj3 != null) ? ((APIBase)obj3).ClassRegistryNative : null);
		}
		ClassRegistry val = (ClassRegistry)obj2;
		if (val != null)
		{
			val.ItemClassToTypeMapping["ItemHammer"] = typeof(ItemHammerPatch);
		}
	}

	public override void OnConfigReceived()
	{
		base.OnConfigReceived();
		UpdateBits();
	}

	public void UpdateBits()
	{
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		if (duplicatable != null)
		{
			return;
		}
		foreach (Item item2 in ((Skill)this).XLeveling.Api.World.Items)
		{
			if (!(((RegistryObject)item2).FirstCodePart(0) == "metalbit"))
			{
				continue;
			}
			AssetLocation code = ((RegistryObject)item2).Code;
			if (!(((code != null) ? code.Domain : null) == "xskills"))
			{
				continue;
			}
			if (((Skill)this).XLeveling.Api is ICoreServerAPI && (((Skill)this).Config as MetalworkingConfig).useVanillaBits)
			{
				GridRecipe val = new GridRecipe();
				val.IngredientPattern = "B";
				val.Height = 1;
				val.Width = 1;
				val.Shapeless = true;
				CraftingRecipeIngredient val2 = new CraftingRecipeIngredient();
				val2.Code = new AssetLocation("xskills", ((RegistryObject)item2).Code.Path);
				val2.Type = (EnumItemClass)1;
				val2.Quantity = 1;
				CraftingRecipeIngredient val3 = new CraftingRecipeIngredient();
				val3.Code = new AssetLocation("game", ((RegistryObject)item2).Code.Path);
				val3.Type = (EnumItemClass)1;
				val3.Quantity = 1;
				val.Ingredients = new Dictionary<string, CraftingRecipeIngredient>();
				val.Ingredients.Add("B", val2);
				val.RecipeGroup = 6;
				val.Output = val3;
				val.Name = new AssetLocation("game", "recipes/grid/metalbit.json");
				if (val.ResolveIngredients(((Skill)this).XLeveling.Api.World))
				{
					ICoreAPI api = ((Skill)this).XLeveling.Api;
					ICoreAPI obj = ((api is ICoreServerAPI) ? api : null);
					if (obj != null)
					{
						((ICoreServerAPI)obj).RegisterCraftingRecipe(val);
					}
				}
			}
			Item item = ((Skill)this).XLeveling.Api.World.GetItem(new AssetLocation("game", ((RegistryObject)item2).Code.Path));
			if (item != null)
			{
				((CollectibleObject)item2).MaterialDensity = ((CollectibleObject)item).MaterialDensity;
				if (((CollectibleObject)item).CombustibleProps != null)
				{
					((CollectibleObject)item2).CombustibleProps = ((CollectibleObject)item).CombustibleProps.Clone();
					((CollectibleObject)item2).CombustibleProps.SmeltedRatio = (((Skill)this).Config as MetalworkingConfig).bitsForIngot;
				}
			}
		}
		float num = (((Skill)this).Config as MetalworkingConfig)?.chiselRecipesRatio ?? 1f;
		if (num != 1f)
		{
			foreach (GridRecipe gridRecipe in ((Skill)this).XLeveling.Api.World.GridRecipes)
			{
				if (!gridRecipe.Name.Path.Contains("metalbit") && !gridRecipe.Name.Path.Contains("ingot"))
				{
					continue;
				}
				CraftingRecipeIngredient val4 = null;
				bool flag = false;
				GridRecipeIngredient[] resolvedIngredients = gridRecipe.resolvedIngredients;
				foreach (GridRecipeIngredient val5 in resolvedIngredients)
				{
					if (val5 == null)
					{
						continue;
					}
					switch (val5.PatternCode)
					{
					case "C":
						if (((CraftingRecipeIngredient)val5).IsTool)
						{
							flag = true;
						}
						break;
					case "A":
					case "I":
						val4 = (CraftingRecipeIngredient)(object)val5;
						break;
					}
				}
				bool? obj2;
				if (val4 == null)
				{
					obj2 = null;
				}
				else
				{
					AssetLocation code2 = val4.Code;
					obj2 = ((code2 != null) ? new bool?(code2.Path.Contains("ingot")) : null);
				}
				if (!(obj2 ?? true) && flag)
				{
					gridRecipe.Output.ResolvedItemstack.StackSize = (int)((float)gridRecipe.Output.ResolvedItemstack.StackSize * num);
					gridRecipe.Output.Quantity = gridRecipe.Output.ResolvedItemstack.StackSize;
					if (gridRecipe.Output.ResolvedItemstack.StackSize == 0)
					{
						gridRecipe.Enabled = false;
					}
				}
			}
		}
		duplicatable = new List<SmithingRecipe>();
		foreach (SmithingRecipe smithingRecipe in ((Skill)this).XLeveling.Api.ModLoader.GetModSystem<RecipeRegistrySystem>(true).SmithingRecipes)
		{
			JsonItemStack output = ((RecipeBase<SmithingRecipe>)(object)smithingRecipe).Output;
			object obj3;
			if (output == null)
			{
				obj3 = null;
			}
			else
			{
				ItemStack resolvedItemstack = output.ResolvedItemstack;
				obj3 = ((resolvedItemstack != null) ? resolvedItemstack.Collectible : null);
			}
			CollectibleObject val6 = (CollectibleObject)obj3;
			if (val6 == null)
			{
				break;
			}
			int num2 = 0;
			if (val6.CombustibleProps?.SmeltedStack?.ResolvedItemstack == null)
			{
				continue;
			}
			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < ((LayeredVoxelRecipe<SmithingRecipe>)(object)smithingRecipe).QuantityLayers; k++)
				{
					for (int l = 0; l < 16; l++)
					{
						if (((LayeredVoxelRecipe<SmithingRecipe>)(object)smithingRecipe).Voxels[j, k, l])
						{
							num2++;
						}
					}
				}
			}
			Ability val7 = ((Skill)this)[DuplicatorId];
			float num3 = 1f + (float)val7.Values[val7.Values.Length - 1] * 0.01f;
			if (((RecipeBase<SmithingRecipe>)(object)smithingRecipe).Output.ResolvedItemstack.StackSize == 0)
			{
				continue;
			}
			int num4 = num2 / 42;
			int num5 = num2 / ((RecipeBase<SmithingRecipe>)(object)smithingRecipe).Output.ResolvedItemstack.StackSize;
			if (num4 == 0)
			{
				if (num5 == 0)
				{
					continue;
				}
				num4 = 1;
				int num6 = (int)((float)(42 / num5) * num3 + 1f);
				((RecipeBase<SmithingRecipe>)(object)smithingRecipe).Output.ResolvedItemstack.Collectible.CombustibleProps.SmeltedRatio = Math.Max(num6, ((RecipeBase<SmithingRecipe>)(object)smithingRecipe).Output.ResolvedItemstack.Collectible.CombustibleProps.SmeltedRatio);
				num2 = num5 * num6;
			}
			((RecipeBase<SmithingRecipe>)(object)smithingRecipe).Output.ResolvedItemstack.Collectible.CombustibleProps.SmeltedStack.ResolvedItemstack.StackSize = num4;
			((RecipeBase<SmithingRecipe>)(object)smithingRecipe).Output.ResolvedItemstack.Collectible.CombustibleProps.SmeltedStack.StackSize = num4;
			if ((float)num2 / (float)(num4 * 42) > num3)
			{
				duplicatable.Add(smithingRecipe);
			}
		}
	}

	internal bool IsDuplicatable(SmithingRecipe recipe)
	{
		return duplicatable.Contains(recipe);
	}
}
