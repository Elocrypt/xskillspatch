using System;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(EntityPlayer))]
public class EntityPlayerPatch
{
	[HarmonyPatch(/*Could not decode attribute arguments.*/)]
	public static void Postfix(EntityPlayer __instance, ref byte[] __result)
	{
		IBlockAccessor blockAccessor = ((Entity)__instance).World.BlockAccessor;
		EntityPos pos = ((Entity)__instance).Pos;
		int lightLevel = blockAccessor.GetLightLevel((pos != null) ? pos.AsBlockPos : null, (EnumLightLevelType)3);
		XLeveling obj = XLeveling.Instance(((Entity)__instance).Api);
		if (!(((obj != null) ? obj.GetSkill("survival", false) : null) is Survival survival))
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)__instance).GetBehavior<PlayerSkillSet>();
		object obj2;
		if (behavior == null)
		{
			obj2 = null;
		}
		else
		{
			PlayerSkill obj3 = behavior[((Skill)survival).Id];
			obj2 = ((obj3 != null) ? obj3[survival.LuminiferousId] : null);
		}
		PlayerAbility val = (PlayerAbility)obj2;
		if (val != null)
		{
			byte[] array = new byte[3]
			{
				(byte)val.Value(0, 0),
				(byte)val.Value(1, 0),
				(byte)((float)val.Value(2, 0) * (1f - (float)lightLevel / 32f))
			};
			byte b = 0;
			if (__result == null)
			{
				__result = array;
				return;
			}
			b = __result[2];
			float num = array[2] + b;
			float num2 = (float)(int)b / num;
			array[0] = (byte)((float)(int)__result[0] * num2 + (float)(int)array[0] * (1f - num2));
			array[1] = (byte)((float)(int)__result[1] * num2 + (float)(int)array[1] * (1f - num2));
			array[2] = Math.Max(b, array[2]);
			__result = array;
		}
	}
}
