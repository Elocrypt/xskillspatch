using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;
using Vintagestory.GameContent;
using Vintagestory.Server;
using XLib.XEffects;
using XLib.XLeveling;

namespace XSkills;

public class Cooking : XSkill
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static ExperienceEquationDelegate _003C0_003E__QuadraticEquation;
	}

	protected Dictionary<CookingRecipeStack, List<CookingRecipeStack>> resolvedRecipeStacks = new Dictionary<CookingRecipeStack, List<CookingRecipeStack>>();

	public int CanteenCookId { get; private set; }

	public int FastFoodId { get; private set; }

	public int WellDoneId { get; private set; }

	public int PreserverId { get; private set; }

	public int DilutionId { get; private set; }

	public int DesalinateId { get; private set; }

	public int SaltyBackpackId { get; private set; }

	public int GourmetId { get; private set; }

	public int HappyMealId { get; private set; }

	public int JuicerId { get; private set; }

	public int EggTimerId { get; private set; }

	public Cooking(ICoreAPI api)
		: base("cooking", "xskills:skill-cooking", "xskills:group-processing")
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		XLeveling obj = XLeveling.Instance(api);
		if (obj != null)
		{
			obj.RegisterSkill((Skill)(object)this);
		}
		((Skill)this).Config = (CustomSkillConfig)(object)new CookingSkillConfig();
		CanteenCookId = ((Skill)this).AddAbility(new Ability("canteencook", "xskills:ability-canteencook", "xskills:abilitydesc-canteencook", 1, 3, new int[3] { 34, 67, 100 }, false));
		FastFoodId = ((Skill)this).AddAbility(new Ability("fastfood", "xskills:ability-fastfood", "xskills:abilitydesc-fastfood", 1, 3, new int[9] { 10, 1, 20, 20, 2, 40, 20, 2, 60 }, false));
		WellDoneId = ((Skill)this).AddAbility(new Ability("welldone", "xskills:ability-welldone", "xskills:abilitydesc-welldone", 1, 3, new int[12]
		{
			5, 1, 15, 20, 10, 2, 30, 20, 10, 2,
			50, 20
		}, false));
		DilutionId = ((Skill)this).AddAbility(new Ability("dilution", "xskills:ability-dilution", "xskills:abilitydesc-dilution", 3, 3, new int[9] { 10, 1, 20, 20, 1, 30, 20, 1, 40 }, false));
		DesalinateId = ((Skill)this).AddAbility(new Ability("desalinate", "xskills:ability-desalinate", "xskills:abilitydesc-desalinate", 3, 3, new int[3] { 4, 3, 2 }, false));
		SaltyBackpackId = ((Skill)this).AddAbility(new Ability("saltybackpack", "xskills:ability-saltybackpack", "xskills:abilitydesc-saltybackpack", 3, 3, new int[3] { 75, 66, 50 }, false));
		GourmetId = ((Skill)this).AddAbility(new Ability("gourmet", "xskills:ability-gourmet", "xskills:abilitydesc-gourmet", 3, 2, new int[4] { 1, 5, 2, 10 }, false));
		((Skill)this).SpecialisationID = ((Skill)this).AddAbility(new Ability("chef", "xskills:ability-chef", "xskills:abilitydesc-chef", 5, 1, new int[1] { 40 }, false));
		HappyMealId = ((Skill)this).AddAbility(new Ability("happymeal", "xskills:ability-happymeal", "xskills:abilitydesc-happymeal", 5, 3, new int[9] { 10, 1, 20, 20, 2, 40, 20, 2, 60 }, false));
		JuicerId = ((Skill)this).AddAbility(new Ability("juicer", "xskills:ability-juicer", "xskills:abilitydesc-juicer", 6, 2, new int[2] { 33, 66 }, false));
		EggTimerId = ((Skill)this).AddAbility(new Ability("eggtimer", "xskills:ability-eggtimer", "xskills:abilitydesc-eggtimer", 8, 1, 0, false));
		if (!CookingRecipe.NamingRegistry.ContainsKey("salt"))
		{
			CookingRecipe.NamingRegistry.Add("salt", (ICookingRecipeNamingHelper)(object)new XSkillsCookingRecipeNaming());
		}
		ICoreAPI obj2 = ((api is ServerCoreAPI) ? api : null);
		object obj3 = ((obj2 != null) ? ((APIBase)obj2).ClassRegistryNative : null);
		if (obj3 == null)
		{
			ICoreAPI obj4 = ((api is ClientCoreAPI) ? api : null);
			obj3 = ((obj4 != null) ? ((APIBase)obj4).ClassRegistryNative : null);
		}
		ClassRegistry val = (ClassRegistry)obj3;
		if (val != null)
		{
			val.inventoryClassToTypeMapping["backpack"] = typeof(XSkillInventoryBackpack);
		}
		((Skill)this).ClassExpMultipliers["commoner"] = 0.1f;
		((Skill)this).ClassExpMultipliers["hunter"] = 0.15f;
		((Skill)this).ClassExpMultipliers["malefactor"] = 0.05f;
		((Skill)this).ClassExpMultipliers["clockmaker"] = 0f;
		((Skill)this).ClassExpMultipliers["blackguard"] = -0.15f;
		((Skill)this).ClassExpMultipliers["miner"] = -0.15f;
		((Skill)this).ClassExpMultipliers["forager"] = 0.2f;
		((Skill)this).ClassExpMultipliers["archer"] = 0.1f;
		((Skill)this).ClassExpMultipliers["vanguard"] = -0.2f;
		((Skill)this).ClassExpMultipliers["gatherer"] = 0.2f;
		((Skill)this).ClassExpMultipliers["mercenary"] = -0.1f;
		object obj5 = _003C_003EO._003C0_003E__QuadraticEquation;
		if (obj5 == null)
		{
			ExperienceEquationDelegate val2 = Skill.QuadraticEquation;
			_003C_003EO._003C0_003E__QuadraticEquation = val2;
			obj5 = (object)val2;
		}
		((Skill)this).ExperienceEquation = (ExperienceEquationDelegate)obj5;
		((Skill)this).ExpBase = 40f;
		((Skill)this).ExpMult = 10f;
		((Skill)this).ExpEquationValue = 0.8f;
	}

	public static void ApplyQuality(float quality, float eaten, float temperature, EnumFoodCategory food0, EnumFoodCategory food1, EntityAgent byEntity)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Invalid comparison between Unknown and I4
		if (quality <= 0f || (int)((Entity)byEntity).Api.Side == 2 || eaten <= 0f)
		{
			return;
		}
		float num = eaten * 600f;
		string text;
		if ((int)food0 == 0)
		{
			text = "saturated-hot";
		}
		else if ((int)food0 == 1)
		{
			text = "saturated-miningSpeed";
		}
		else if ((int)food0 == 2)
		{
			text = "saturated-health";
		}
		else if ((int)food0 == 3)
		{
			text = "saturated-hungerrate";
		}
		else
		{
			if ((int)food0 != 4)
			{
				return;
			}
			text = "saturated-expMult";
		}
		XEffectsSystem val = ((byEntity != null) ? ((Entity)byEntity).Api.ModLoader.GetModSystem<XEffectsSystem>(true) : null);
		if (val == null)
		{
			return;
		}
		Effect val2 = val.CreateEffect(text);
		if (val2 != null)
		{
			Effect obj = val2;
			obj.Duration *= num;
			val2.Update(val2.Intensity * quality, 0);
			XEffectsHelper.AddEffect((Entity)(object)byEntity, val2);
		}
		if (temperature >= 50f)
		{
			text = "saturated-heated";
			val2 = val.CreateEffect(text);
			if (val2 != null)
			{
				Effect obj2 = val2;
				obj2.Duration *= num;
				val2.Update(val2.Intensity * quality, 0);
				XEffectsHelper.AddEffect((Entity)(object)byEntity, val2);
			}
		}
	}

	public ItemStack[] ContentStacks(ItemStack itemStack, IWorldAccessor world)
	{
		CollectibleObject collectible = itemStack.Collectible;
		IBlockMealContainer val = (IBlockMealContainer)(object)((collectible is IBlockMealContainer) ? collectible : null);
		CollectibleObject collectible2 = itemStack.Collectible;
		CollectibleObject obj = ((collectible2 is BlockLiquidContainerBase) ? collectible2 : null);
		ItemStack val2 = ((obj != null) ? ((BlockLiquidContainerBase)obj).GetContent(itemStack) : null);
		if (val != null)
		{
			return val.GetContents(world, itemStack);
		}
		if (val2 != null)
		{
			return (ItemStack[])(object)new ItemStack[1] { val2 };
		}
		return (ItemStack[])(object)new ItemStack[1] { itemStack };
	}

	public float IngredientDiversity(ItemStack itemStack, ItemStack[] contentStacks, IWorldAccessor world, out int ingredientCount)
	{
		ingredientCount = 0;
		int num = 1;
		if (itemStack == null)
		{
			return 0f;
		}
		if (contentStacks == null)
		{
			contentStacks = ContentStacks(itemStack, world);
		}
		Dictionary<CollectibleObject, int> dictionary = new Dictionary<CollectibleObject, int>();
		ItemStack[] array = contentStacks;
		foreach (ItemStack val in array)
		{
			if (val != null)
			{
				ingredientCount++;
				if (!dictionary.TryGetValue(val.Collectible, out var value))
				{
					value = 0;
				}
				dictionary[val.Collectible] = value + 1;
			}
		}
		IAttribute obj = itemStack.Attributes["madeWith"];
		string[] array2 = ((ArrayAttribute<string>)(object)((obj is StringArrayAttribute) ? obj : null))?.value;
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
		if (array2 != null && array2.Length != 0)
		{
			num++;
			ingredientCount--;
			string[] array3 = array2;
			foreach (string text in array3)
			{
				if (text != null)
				{
					ingredientCount++;
					if (!dictionary2.TryGetValue(text, out var value2))
					{
						value2 = 0;
					}
					dictionary2[text] = value2 + 1;
				}
			}
		}
		return 1f + (float)(dictionary.Count + dictionary2.Count - num) * 0.1f;
	}

	public float BakeRange(ItemStack outputStack, ItemStack sourceStack, out bool firstStage)
	{
		BakingProperties val = BakingProperties.ReadFrom(outputStack);
		BakingProperties val2 = ((sourceStack != null) ? BakingProperties.ReadFrom(sourceStack) : null);
		float result = 1f;
		firstStage = true;
		if (val2 == null)
		{
			return result;
		}
		result = ((val != null) ? Math.Min(val.LevelFrom - val2.LevelFrom, 1f) : GameMath.Clamp(1f - val2.LevelFrom, 0f, 1f));
		firstStage = val2.LevelFrom <= 0f;
		return result;
	}

	public bool FinishedCooking(ItemSlot outputSlot)
	{
		InventoryBase inventory = outputSlot.Inventory;
		InventoryBase obj = ((inventory is InventorySmelting) ? inventory : null);
		if (obj != null && obj[1].Empty)
		{
			return true;
		}
		foreach (ItemSlot item in outputSlot.Inventory)
		{
			if (item.Empty)
			{
				continue;
			}
			BakingProperties val = BakingProperties.ReadFrom(item.Itemstack);
			if (val != null)
			{
				if (item == outputSlot)
				{
					string resultCode = val.ResultCode;
					if (resultCode == null || resultCode.Contains("charred"))
					{
						return true;
					}
					return false;
				}
			}
			else if (item != outputSlot)
			{
				CombustibleProperties combustibleProps = item.Itemstack.Collectible.CombustibleProps;
				if (combustibleProps == null || !(combustibleProps.BurnDuration > 0f))
				{
					return false;
				}
			}
		}
		return true;
	}

	public void FreshnessAndQuality(ItemStack[] sourceStacks, out float freshness, out float quality)
	{
		freshness = 1f;
		quality = 0f;
		int num = 0;
		foreach (ItemStack val in sourceStacks)
		{
			ITreeAttribute obj = ((val != null) ? val.Attributes : null);
			ITreeAttribute obj2 = ((obj is TreeAttribute) ? obj : null);
			ITreeAttribute val2 = ((obj2 != null) ? ((TreeAttribute)obj2).GetTreeAttribute("transitionstate") : null);
			if (val2 != null)
			{
				IAttribute obj3 = val2["freshHours"];
				FloatArrayAttribute val3 = (FloatArrayAttribute)(object)((obj3 is FloatArrayAttribute) ? obj3 : null);
				IAttribute obj4 = val2["transitionedHours"];
				FloatArrayAttribute val4 = (FloatArrayAttribute)(object)((obj4 is FloatArrayAttribute) ? obj4 : null);
				if (val3 != null && ((ArrayAttribute<float>)(object)val3).value.Length >= 1 && val4 != null && ((ArrayAttribute<float>)(object)val4).value.Length >= 1)
				{
					freshness *= Math.Min(1f - ((ArrayAttribute<float>)(object)val4).value[0] / ((ArrayAttribute<float>)(object)val3).value[0], 1f);
				}
			}
			IAttribute obj5 = val.Attributes["madeWith"];
			IAttribute obj6 = ((obj5 is StringArrayAttribute) ? obj5 : null);
			int num2 = ((obj6 == null) ? 1 : ((ArrayAttribute<string>)(object)obj6).value.Length);
			quality += val.Attributes.GetFloat("quality", 0f) * (float)num2;
			num += num2;
		}
		quality /= num;
	}

	public void ApplyAbilities(ItemSlot outputSlot, IPlayer player, float oldQuality, float cookedAmount = 1f, ItemStack[] sourceStacks = null, float expMult = 1f)
	{
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Expected O, but got Unknown
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Expected O, but got Unknown
		ItemStack val = ((outputSlot != null) ? outputSlot.Itemstack : null);
		if (val == null || player == null)
		{
			return;
		}
		ItemStack val2 = ((sourceStacks != null && sourceStacks.Length == 1) ? sourceStacks[0] : null);
		PlayerSkillSet behavior = ((Entity)player.Entity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val3 = ((behavior != null) ? behavior[((Skill)this).Id] : null);
		if (val3 == null)
		{
			return;
		}
		IWorldAccessor val4 = ((Entity)(player.Entity?)).World;
		if (val4 == null)
		{
			return;
		}
		ItemStack[] array = ContentStacks(val, val4);
		if (array == null || array.Length < 1)
		{
			return;
		}
		bool firstStage;
		float num = BakeRange(val, val2, out firstStage);
		if (num < 0.99f)
		{
			num *= 1.5f;
		}
		float num2 = val.Collectible.NutritionProps?.Satiety ?? 0f;
		float num3 = (float)val.Attributes.GetDecimal("quantityServings", (double)cookedAmount);
		bool flag = ((val2 == null) ? null : val2.Collectible.NutritionProps?.Satiety).GetValueOrDefault() > num2 || ((RegistryObject)val.Collectible).Code.Path.Contains("charred");
		int ingredientCount;
		float num4 = IngredientDiversity(val, array, val4, out ingredientCount);
		bool flag2 = val.Attributes.HasAttribute("madeWith");
		CollectibleObject collectible = val.Collectible;
		IBlockMealContainer val5 = (IBlockMealContainer)(object)((collectible is IBlockMealContainer) ? collectible : null);
		CollectibleObject collectible2 = val.Collectible;
		BlockLiquidContainerBase val6 = (BlockLiquidContainerBase)(object)((collectible2 is BlockLiquidContainerBase) ? collectible2 : null);
		float num5 = expMult * (((Skill)this).Config as CookingSkillConfig).expBase;
		num5 = ((ingredientCount != 1) ? (num5 * (225f * (float)ingredientCount * num4 * num3 * num)) : (num5 * (num2 * num3 * num)));
		if (!flag)
		{
			if (!flag2 || num2 > 0f)
			{
				val3.AddExperience(num5, true);
			}
		}
		else if (firstStage)
		{
			val3.AddExperience(num5 * 0.5f, true);
		}
		PlayerAbility val7 = val3[EggTimerId];
		if (val7 != null && val7.Tier > 0)
		{
			BlockPos pos = outputSlot.Inventory.Pos;
			Block val8 = ((pos != (BlockPos)null) ? ((IBlockAccessor)val4.BulkBlockAccessor).GetBlock(pos) : null);
			if (val8 != null && FinishedCooking(outputSlot))
			{
				double totalHours = val4.Calendar.TotalHours;
				double @double = ((TreeAttribute)((Entity)player.Entity).Attributes).GetDouble("xskillsCookingMsg", 0.0);
				if (totalHours > @double + 0.333)
				{
					((TreeAttribute)((Entity)player.Entity).Attributes).SetDouble("xskillsCookingMsg", totalHours);
					val4.PlaySoundFor(new AssetLocation("sounds/tutorialstepsuccess.ogg"), player, true, 32f, 1f);
					string text = Lang.Get("xskills:cooking-finished", new object[1] { val8.GetPlacedBlockName(val4, pos) + " (" + pos.X + ", " + pos.X + pos.Y + ", " + pos.Z + ")" });
					IPlayer obj = ((player is IServerPlayer) ? player : null);
					if (obj != null)
					{
						((IServerPlayer)obj).SendMessage(0, text, (EnumChatType)4, (string)null);
					}
				}
			}
		}
		val7 = val3[DilutionId];
		float num6 = num3;
		int num7 = 1;
		ItemStack[] array2;
		if (val7 != null && val7.Tier > 0 && firstStage)
		{
			num6 = num3 * (1f + val7.SkillDependentFValue(0));
			if (val6 != null)
			{
				float num8 = 1f + val7.SkillDependentFValue(0);
				array2 = array;
				foreach (ItemStack val9 in array2)
				{
					FoodNutritionProperties nutritionProps = val9.Collectible.NutritionProps;
					if (nutritionProps != null && nutritionProps.Satiety > 0f)
					{
						val9.StackSize = (int)((float)val9.StackSize * num8);
					}
				}
			}
			else if (val5 == null || val5 is BlockPie)
			{
				num7 = (int)num6;
				float num9 = num6 - (float)num7;
				num7 += ((val4.Rand.NextDouble() < (double)num9) ? 1 : 0);
				if ((float)val.StackSize > cookedAmount)
				{
					ItemStack obj2 = val;
					obj2.StackSize += num7 - (int)(cookedAmount + 0.25f);
				}
				else
				{
					val.StackSize = num7;
				}
			}
			else
			{
				val5.SetQuantityServings(val4, val, num6);
			}
		}
		val7 = val3[DesalinateId];
		if (val7.Tier > 0 && ((val5 != null) ? val5.GetRecipeCode(val4, val) : null) == "salt")
		{
			int num10 = (int)(num6 / (float)val7.Value(0, 0));
			Item item = val4.GetItem(new AssetLocation("game:salt"));
			InventoryBase inventory = outputSlot.Inventory;
			InventorySmelting val10 = (InventorySmelting)(object)((inventory is InventorySmelting) ? inventory : null);
			if (item != null && num10 > 0 && val10 != null)
			{
				ItemStack val11 = new ItemStack(item, num10);
				val5.SetContents((string)null, val, (ItemStack[])null, 0f);
				outputSlot.TryPutInto(val4, ((InventoryBase)val10)[1], 1);
				if (outputSlot.Empty)
				{
					outputSlot.Itemstack = val11;
					val5 = null;
					val = val11;
					array = (ItemStack[])(object)new ItemStack[1] { val };
					outputSlot.MarkDirty();
				}
			}
		}
		else if (val5 != null)
		{
			val7 = val3[HappyMealId];
			if ((double?)((val7 != null) ? new float?(val7.SkillDependentFValue(0)) : null) >= val4.Rand.NextDouble() && val5 is BlockCookedContainer)
			{
				ItemStack[] array3 = (ItemStack[])(object)new ItemStack[array.Length + 1];
				int j;
				for (j = 0; j < array.Length; j++)
				{
					array3[j] = array[j];
				}
				IBlockMealContainer obj3 = ((val5 is BlockCookedContainer) ? val5 : null);
				object obj4 = ((obj3 != null) ? ((BlockCookedContainerBase)obj3).GetCookingRecipe(val4, val) : null);
				if (obj4 == null)
				{
					IBlockMealContainer obj5 = ((val5 is BlockMeal) ? val5 : null);
					obj4 = ((obj5 != null) ? ((BlockMeal)obj5).GetCookingRecipe(val4, val) : null);
				}
				CookingRecipe val12 = (CookingRecipe)obj4;
				if (val12 != null)
				{
					ItemStack missingIngredient = GetMissingIngredient(array, val12, val4);
					if (missingIngredient != null)
					{
						array3[j] = missingIngredient;
						val5.SetContents(val12.Code, val, array3, val5.GetQuantityServings(val4, val));
					}
				}
			}
		}
		val7 = val3[WellDoneId];
		array2 = array;
		foreach (ItemStack obj6 in array2)
		{
			ITreeAttribute obj7 = ((obj6 != null) ? obj6.Attributes : null);
			ITreeAttribute obj8 = ((obj7 is TreeAttribute) ? obj7 : null);
			ITreeAttribute val13 = ((obj8 != null) ? ((TreeAttribute)obj8).GetTreeAttribute("transitionstate") : null);
			if (val13 == null)
			{
				continue;
			}
			IAttribute obj9 = val13["freshHours"];
			FloatArrayAttribute val14 = (FloatArrayAttribute)(object)((obj9 is FloatArrayAttribute) ? obj9 : null);
			IAttribute obj10 = val13["transitionedHours"];
			FloatArrayAttribute val15 = (FloatArrayAttribute)(object)((obj10 is FloatArrayAttribute) ? obj10 : null);
			if (((ArrayAttribute<float>)(object)val14).value.Length >= 1 && ((ArrayAttribute<float>)(object)val15).value.Length >= 1)
			{
				if (val5 == null)
				{
					((ArrayAttribute<float>)(object)val14).value[0] = val.Collectible.TransitionableProps[0].FreshHours.avg * (1f + val7.SkillDependentFValue(0));
				}
				else
				{
					((ArrayAttribute<float>)(object)val14).value[0] = ((ArrayAttribute<float>)(object)val14).value[0] * (1f + val7.SkillDependentFValue(0));
				}
			}
		}
		FreshnessAndQuality(sourceStacks ?? array, out var freshness, out var quality);
		val7 = val3[GourmetId];
		if (val7 != null && val7.Tier > 0)
		{
			float num11;
			if (((sourceStacks != null) ? sourceStacks.Length : array.Length) == 1 && quality > 0f)
			{
				num11 = quality * (flag ? 0.2f : 1.1f);
			}
			else
			{
				float num12 = (float)val7.Value(1, 0) - quality;
				num11 = (float)val3.Level * 0.1f + 2f * freshness + (float)ingredientCount * 0.2f + num4;
				num11 *= 0.3125f * (float)val7.Value(0, 0);
				num11 = Math.Min(num11 + (float)val4.Rand.NextDouble() * num11, val7.Value(1, 0));
				num11 = num11 / (float)val7.Value(1, 0) * num12;
				num11 += quality * 1.1f;
			}
			if (val6 == null)
			{
				val.Attributes.SetFloat("quality", (num11 * (float)num7 + oldQuality * (float)(val.StackSize - num7)) / (float)val.StackSize);
			}
			else
			{
				array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].Attributes.SetFloat("quality", num11);
				}
			}
		}
		if (val6 != null)
		{
			((BlockContainer)val6).SetContents(val, array);
		}
	}

	protected CookingRecipeStack GetResolvedIngredient(IWorldAccessor world, CookingRecipeStack recipeStack)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		if (!((JsonItemStack)recipeStack).Code.Path.Contains('*'))
		{
			return recipeStack;
		}
		resolvedRecipeStacks.TryGetValue(recipeStack, out var value);
		if (value == null)
		{
			value = new List<CookingRecipeStack>();
			if ((int)((JsonItemStack)recipeStack).Type == 1)
			{
				foreach (Item item in world.Items)
				{
					if (((RegistryObject)item).WildCardMatch(((JsonItemStack)recipeStack).Code))
					{
						CookingRecipeStack val = recipeStack.Clone();
						((JsonItemStack)val).Code = ((RegistryObject)item).Code;
						((JsonItemStack)val).ResolvedItemstack = new ItemStack(item, 1);
						value.Add(val);
					}
				}
			}
			if ((int)((JsonItemStack)recipeStack).Type == 0 || ((JsonItemStack)recipeStack).Code.Path.Contains("mushroom"))
			{
				foreach (Block block in world.Blocks)
				{
					if (((RegistryObject)block).WildCardMatch(((JsonItemStack)recipeStack).Code))
					{
						CookingRecipeStack val2 = recipeStack.Clone();
						((JsonItemStack)val2).Code = ((RegistryObject)block).Code;
						((JsonItemStack)val2).ResolvedItemstack = new ItemStack(block, 1);
						value.Add(val2);
					}
				}
			}
			resolvedRecipeStacks.Add(recipeStack, value);
		}
		if (value.Count == 0)
		{
			return recipeStack;
		}
		return value[world.Rand.Next(value.Count - 1)];
	}

	public ItemStack GetMissingIngredient(ItemStack[] inputStacks, CookingRecipe recipe, IWorldAccessor world)
	{
		List<ItemStack> list = new List<ItemStack>(inputStacks);
		List<CookingRecipeIngredient> list2 = new List<CookingRecipeIngredient>(recipe.Ingredients);
		int[] array = new int[list2.Count];
		while (list.Count > 0)
		{
			ItemStack val = list[0];
			list.RemoveAt(0);
			if (val == null)
			{
				continue;
			}
			for (int i = 0; i < list2.Count; i++)
			{
				CookingRecipeIngredient val2 = list2[i];
				if (val2.Matches(val))
				{
					array[i]++;
					if (array[i] >= val2.MaxQuantity)
					{
						list2.RemoveAt(i);
					}
					break;
				}
			}
		}
		int num = 0;
		ItemStack val3 = null;
		while (num < 5 && val3 == null && list2.Count > 0)
		{
			num++;
			CookingRecipeIngredient val4 = list2[world.Rand.Next(list2.Count - 1)];
			if (val4.ValidStacks.Length == 0)
			{
				list2.Remove(val4);
				continue;
			}
			CookingRecipeStack recipeStack = val4.ValidStacks[world.Rand.Next(val4.ValidStacks.Length - 1)];
			recipeStack = GetResolvedIngredient(world, recipeStack);
			if (recipeStack != null)
			{
				ItemStack resolvedItemstack = ((JsonItemStack)recipeStack).ResolvedItemstack;
				val3 = ((resolvedItemstack != null) ? resolvedItemstack.Clone() : null);
			}
		}
		if (val3 != null)
		{
			val3.StackSize = 1;
		}
		return val3;
	}
}
