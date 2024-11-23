using System;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(EntityBehaviorHarvestable))]
public class EntityBehaviorHarvestablePatch
{
	[HarmonyPatch("SetHarvested")]
	public static void Prefix(EntityBehaviorHarvestable __instance, IPlayer byPlayer, ref float dropQuantityMultiplier)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		if ((int)((EntityBehavior)__instance).entity.World.Side == 2 || ((byPlayer != null) ? byPlayer.Entity : null) == null)
		{
			return;
		}
		Entity entity = ((EntityBehavior)__instance).entity;
		if (((entity != null) ? entity.GetBehavior<XSkillsAnimalBehavior>() : null) != null)
		{
			return;
		}
		XLeveling obj = XLeveling.Instance(((Entity)byPlayer.Entity).Api);
		if (((obj != null) ? obj.GetSkill("combat", false) : null) is Combat combat)
		{
			object obj2;
			if (byPlayer == null)
			{
				obj2 = null;
			}
			else
			{
				PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
				obj2 = ((behavior != null) ? behavior[((Skill)combat).Id] : null);
			}
			PlayerSkill val = (PlayerSkill)obj2;
			if (val != null)
			{
				dropQuantityMultiplier += val[combat.LooterId].SkillDependentFValue(0);
			}
		}
	}

	[HarmonyPatch("SetHarvested")]
	public static void Postfix(EntityBehaviorHarvestable __instance, IPlayer byPlayer, InventoryGeneric ___inv)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Expected O, but got Unknown
		if ((int)((EntityBehavior)__instance).entity.World.Side == 2 || ((byPlayer != null) ? byPlayer.Entity : null) == null || ((InventoryBase)___inv).Empty)
		{
			return;
		}
		Entity entity = ((EntityBehavior)__instance).entity;
		if (((entity != null) ? entity.GetBehavior<XSkillsAnimalBehavior>() : null) == null)
		{
			return;
		}
		XLeveling obj = XLeveling.Instance(((Entity)byPlayer.Entity).Api);
		if (!(((obj != null) ? obj.GetSkill("husbandry", false) : null) is Husbandry husbandry))
		{
			return;
		}
		object obj2;
		if (byPlayer == null)
		{
			obj2 = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj2 = ((behavior != null) ? behavior[((Skill)husbandry).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj2;
		if (val == null)
		{
			return;
		}
		int @int = ((TreeAttribute)((EntityBehavior)__instance).entity.WatchedAttributes).GetInt("generation", 0);
		for (int i = 0; i < ((InventoryBase)___inv).Count; i++)
		{
			PlayerAbility val2 = null;
			ItemStack itemstack = ((InventoryBase)___inv)[i].Itemstack;
			if (((itemstack != null) ? ((RegistryObject)itemstack.Collectible).FirstCodePart(0) : null) == "hide")
			{
				val2 = val[husbandry.FurrierId];
			}
			else
			{
				ItemStack itemstack2 = ((InventoryBase)___inv)[i].Itemstack;
				if (((itemstack2 != null) ? ((RegistryObject)itemstack2.Collectible).FirstCodePart(1) : null) == "raw")
				{
					val2 = val[husbandry.ButcherId];
				}
				else
				{
					ItemStack itemstack3 = ((InventoryBase)___inv)[i].Itemstack;
					if (((itemstack3 != null) ? ((RegistryObject)itemstack3.Collectible).FirstCodePart(0) : null) == "fat")
					{
						val2 = val[husbandry.ButcherId];
					}
					else
					{
						ItemStack itemstack4 = ((InventoryBase)___inv)[i].Itemstack;
						if (((itemstack4 != null) ? ((RegistryObject)itemstack4.Collectible).FirstCodePart(0) : null) == "feather")
						{
							val2 = val[husbandry.FurrierId];
						}
					}
				}
			}
			if (val2 != null && val2.Tier > 0)
			{
				float num = (1f + val2.SkillDependentFValue(0) + val2.FValue(3, 0f) * (float)Math.Min(@int, val2.Value(4, 0))) * (float)((InventoryBase)___inv)[i].Itemstack.StackSize;
				int num2 = (int)num;
				num2 += (((double)(num - (float)num2) > ((EntityBehavior)__instance).entity.World.Rand.NextDouble()) ? 1 : 0);
				((InventoryBase)___inv)[i].Itemstack.StackSize = num2;
			}
			if (((InventoryBase)___inv)[i].Itemstack == null)
			{
				continue;
			}
			((InventoryBase)___inv)[i].Itemstack.Collectible.UpdateAndGetTransitionState(((EntityBehavior)__instance).entity.World, ((InventoryBase)___inv)[i], (EnumTransitionType)0);
			ITreeAttribute attributes = ((InventoryBase)___inv)[i].Itemstack.Attributes;
			ITreeAttribute obj3 = ((attributes is TreeAttribute) ? attributes : null);
			ITreeAttribute val3 = ((obj3 != null) ? ((TreeAttribute)obj3).GetTreeAttribute("transitionstate") : null);
			val2 = val[husbandry.PreserverId];
			if (val3 != null && val2 != null && val2.Tier > 0)
			{
				IAttribute obj4 = val3["freshHours"];
				FloatArrayAttribute val4 = (FloatArrayAttribute)(object)((obj4 is FloatArrayAttribute) ? obj4 : null);
				IAttribute obj5 = val3["transitionedHours"];
				FloatArrayAttribute val5 = (FloatArrayAttribute)(object)((obj5 is FloatArrayAttribute) ? obj5 : null);
				if (((ArrayAttribute<float>)(object)val5).value.Length >= 1)
				{
					float num3 = ((ArrayAttribute<float>)(object)val4).value[0] * val2.SkillDependentFValue(0);
					((ArrayAttribute<float>)(object)val5).value[0] -= num3;
				}
			}
		}
		TreeAttribute val6 = new TreeAttribute();
		((InventoryBase)___inv).ToTreeAttributes((ITreeAttribute)(object)val6);
		((TreeAttribute)((EntityBehavior)__instance).entity.WatchedAttributes)["harvestableInv"] = (IAttribute)(object)val6;
		((EntityBehavior)__instance).entity.WatchedAttributes.MarkPathDirty("harvestableInv");
		((EntityBehavior)__instance).entity.WatchedAttributes.MarkPathDirty("harvested");
	}
}
