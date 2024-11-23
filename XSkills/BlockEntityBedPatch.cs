using System;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XEffects;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityBed))]
public class BlockEntityBedPatch
{
	[HarmonyPatch("RestPlayer")]
	public static void Prefix(BlockEntityBed __instance, double ___hoursTotal)
	{
		double num = ((BlockEntity)__instance).Api.World.Calendar.TotalHours - ___hoursTotal;
		if (num > 0.0)
		{
			EntityAgent mountedBy = __instance.MountedBy;
			if (((mountedBy != null) ? ((Entity)mountedBy).GetBehavior("XSkillsPlayer") : null) is XSkillsPlayerBehavior xSkillsPlayerBehavior)
			{
				xSkillsPlayerBehavior.HoursSlept += (float)num;
			}
		}
	}

	[HarmonyPatch("DidUnmount")]
	public static void Postfix(BlockEntityBed __instance, EntityAgent entityAgent)
	{
		if (entityAgent == null)
		{
			return;
		}
		EntityBehavior behavior = ((Entity)entityAgent).GetBehavior("tiredness");
		EntityBehaviorTiredness val = (EntityBehaviorTiredness)(object)((behavior is EntityBehaviorTiredness) ? behavior : null);
		XSkillsPlayerBehavior xSkillsPlayerBehavior = ((Entity)entityAgent).GetBehavior("XSkillsPlayer") as XSkillsPlayerBehavior;
		if (val == null || xSkillsPlayerBehavior == null)
		{
			return;
		}
		float num = 1f - val.Tiredness / ((BlockEntity)__instance).Api.World.Calendar.HoursPerDay;
		if (num < 0f || !(XLeveling.Instance(((BlockEntity)__instance).Api).GetSkill("survival", false) is Survival survival))
		{
			return;
		}
		PlayerSkillSet behavior2 = ((Entity)entityAgent).GetBehavior<PlayerSkillSet>();
		object obj;
		if (behavior2 == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkill obj2 = behavior2[((Skill)survival).Id];
			obj = ((obj2 != null) ? obj2[survival.WellRestedId] : null);
		}
		PlayerAbility val2 = (PlayerAbility)obj;
		if (val2 != null && val2.Tier >= 1)
		{
			XEffectsSystem modSystem = ((BlockEntity)__instance).Api.ModLoader.GetModSystem<XEffectsSystem>(true);
			Effect val3 = ((modSystem != null) ? modSystem.CreateEffect("rested") : null);
			if (val3 != null && !(xSkillsPlayerBehavior.HoursSlept < 1f))
			{
				val3.Update(num * val2.FValue(0, 0f), 0);
				val3.Duration = (float)val2.Value(1, 0) * Math.Min(xSkillsPlayerBehavior.HoursSlept / 8f, 1f);
				xSkillsPlayerBehavior.HoursSlept = 0f;
				XEffectsHelper.AddEffect((Entity)(object)entityAgent, val3);
			}
		}
	}
}
