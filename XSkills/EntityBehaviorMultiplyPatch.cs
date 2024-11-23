using System;
using System.Reflection;
using System.Text;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(EntityBehaviorMultiply))]
public class EntityBehaviorMultiplyPatch
{
	[HarmonyPatch(/*Could not decode attribute arguments.*/)]
	public static void Postfix1(EntityBehaviorMultiply __instance, ref float __result)
	{
		Entity entity = ((EntityBehavior)__instance).entity;
		IPlayer val = ((entity == null) ? null : entity.GetBehavior<XSkillsAnimalBehavior>()?.Feeder);
		if (val == null || !(XLeveling.Instance(((EntityBehavior)__instance).entity.World.Api).GetSkill("husbandry", false) is Husbandry husbandry))
		{
			return;
		}
		EntityPlayer entity2 = val.Entity;
		object obj;
		if (entity2 == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)entity2).GetBehavior<PlayerSkillSet>();
			obj = ((behavior != null) ? behavior[((Skill)husbandry).Id] : null);
		}
		PlayerSkill val2 = (PlayerSkill)obj;
		if (val2 != null)
		{
			PlayerAbility val3 = val2[husbandry.MassHusbandryId];
			if (val3 != null)
			{
				float val4 = 1f + val3.FValue(0, 0f) + val3.FValue(1, 0f) * (float)val2.Level + val3.FValue(2, 0f) * (float)((TreeAttribute)((EntityBehavior)__instance).entity.WatchedAttributes).GetInt("generation", 0);
				val4 = Math.Min(val4, val3.FValue(3, 0f));
				__result *= val4;
			}
		}
	}

	[HarmonyPatch(/*Could not decode attribute arguments.*/)]
	public static void Postfix2(EntityBehaviorMultiply __instance, ref float __result)
	{
		Entity entity = ((EntityBehavior)__instance).entity;
		IPlayer val = ((entity == null) ? null : entity.GetBehavior<XSkillsAnimalBehavior>()?.Feeder);
		if (val == null || !(XLeveling.Instance(((EntityBehavior)__instance).entity.World.Api).GetSkill("husbandry", false) is Husbandry husbandry))
		{
			return;
		}
		EntityPlayer entity2 = val.Entity;
		object obj;
		if (entity2 == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)entity2).GetBehavior<PlayerSkillSet>();
			obj = ((behavior != null) ? behavior[((Skill)husbandry).Id] : null);
		}
		PlayerSkill val2 = (PlayerSkill)obj;
		if (val2 != null)
		{
			PlayerAbility val3 = val2[husbandry.MassHusbandryId];
			if (val3 != null)
			{
				float val4 = 1f + val3.FValue(0, 0f) + val3.FValue(1, 0f) * (float)val2.Level + val3.FValue(2, 0f) * (float)((TreeAttribute)((EntityBehavior)__instance).entity.WatchedAttributes).GetInt("generation", 0);
				val4 = Math.Min(val4, val3.FValue(3, 0f));
				__result *= val4;
			}
		}
	}

	[HarmonyPatch("TryGetPregnant")]
	public static void Prefix(EntityBehaviorMultiply __instance, out bool __state)
	{
		__state = __instance.IsPregnant;
	}

	[HarmonyPatch("TryGetPregnant")]
	public static void Postfix(EntityBehaviorMultiply __instance, bool __state)
	{
		if (__state || !__instance.IsPregnant)
		{
			return;
		}
		Entity entity = ((EntityBehavior)__instance).entity;
		IPlayer val = ((entity == null) ? null : entity.GetBehavior<XSkillsAnimalBehavior>()?.Feeder);
		if (val == null || !(XLeveling.Instance(((EntityBehavior)__instance).entity.World.Api).GetSkill("husbandry", false) is Husbandry husbandry))
		{
			return;
		}
		EntityPlayer entity2 = val.Entity;
		object obj;
		if (entity2 == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)entity2).GetBehavior<PlayerSkillSet>();
			obj = ((behavior != null) ? behavior[((Skill)husbandry).Id] : null);
		}
		PlayerSkill val2 = (PlayerSkill)obj;
		if (val2 != null)
		{
			PlayerAbility val3 = val2[husbandry.BreederId];
			if (val3 != null)
			{
				float val4 = val3.FValue(0, 0f) + val3.FValue(1, 0f) * (float)val2.Level + val3.FValue(2, 0f) * (float)((TreeAttribute)((EntityBehavior)__instance).entity.WatchedAttributes).GetInt("generation", 0);
				val4 = Math.Min(val4, val3.FValue(3, 0f));
				float num = (float)typeof(EntityBehaviorMultiply).GetProperty("PregnancyDays", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
				__instance.TotalDaysPregnancyStart = ((EntityBehavior)__instance).entity.World.Calendar.TotalDays - (double)(num * val4);
			}
		}
	}

	[HarmonyPatch("GetInfoText")]
	public static bool Prefix(EntityBehaviorMultiply __instance, StringBuilder infotext)
	{
		IWorldAccessor obj = ((EntityBehavior)__instance).entity?.World;
		IWorldAccessor obj2 = ((obj is IClientWorldAccessor) ? obj : null);
		IPlayer val = (IPlayer)(object)((obj2 != null) ? ((IClientWorldAccessor)obj2).Player : null);
		if (val == null)
		{
			return true;
		}
		if (!(XLeveling.Instance(((EntityBehavior)__instance).entity.World.Api).GetSkill("husbandry", false) is Husbandry husbandry))
		{
			return true;
		}
		EntityPlayer entity = val.Entity;
		object obj3;
		if (entity == null)
		{
			obj3 = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
			obj3 = ((behavior != null) ? behavior[((Skill)husbandry).Id][husbandry.BreederId] : null);
		}
		if (obj3 == null || ((PlayerAbility)obj3).Tier <= 0)
		{
			return true;
		}
		if (__instance.IsPregnant)
		{
			float num = (float)typeof(EntityBehaviorMultiply).GetProperty("PregnancyDays", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
			double num2 = ((EntityBehavior)__instance).entity.World.Calendar.TotalDays - __instance.TotalDaysPregnancyStart;
			infotext.AppendLine(Lang.Get("Is pregnant", Array.Empty<object>()) + $" ({num2:N1}/{num:N1})");
		}
		else if (((EntityBehavior)__instance).entity.Alive)
		{
			ITreeAttribute treeAttribute = ((TreeAttribute)((EntityBehavior)__instance).entity.WatchedAttributes).GetTreeAttribute("hunger");
			if (treeAttribute != null)
			{
				float @float = treeAttribute.GetFloat("saturation", 0f);
				infotext.AppendLine(Lang.Get("Portions eaten: {0}", new object[1] { @float }));
			}
			double num3 = ((EntityBehaviorMultiplyBase)__instance).TotalDaysCooldownUntil - ((EntityBehavior)__instance).entity.World.Calendar.TotalDays;
			if (num3 <= 0.0)
			{
				infotext.AppendLine(Lang.Get("Ready to mate", Array.Empty<object>()));
			}
			else
			{
				infotext.AppendLine(Lang.Get("xskills:ready-to-mate", new object[1] { num3 }));
			}
		}
		return false;
	}
}
