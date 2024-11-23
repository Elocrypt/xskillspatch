using System;
using System.Reflection;
using HarmonyLib;

namespace XSkills;

public class ManualPatch
{
	internal static void PatchMethod(Harmony harmony, Type type, Type patch, string methodName, string prefixName, string postfixName)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		if (harmony != null && !(type == null) && !(patch == null) && methodName != null)
		{
			Type type2 = type;
			MethodInfo methodInfo;
			do
			{
				methodInfo = type2.GetMethod(methodName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) ?? type2.GetMethod("get_" + methodName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				type2 = type2.BaseType;
			}
			while (type2 != null && methodInfo == null);
			if (!(methodInfo == null))
			{
				BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
				MethodInfo methodInfo2 = ((prefixName != null) ? patch.GetMethod(prefixName, bindingAttr) : null);
				MethodInfo methodInfo3 = ((postfixName != null) ? patch.GetMethod(postfixName, bindingAttr) : null);
				HarmonyMethod val = ((!(methodInfo2 != null)) ? ((HarmonyMethod)null) : new HarmonyMethod(methodInfo2));
				HarmonyMethod val2 = ((!(methodInfo3 != null)) ? ((HarmonyMethod)null) : new HarmonyMethod(methodInfo3));
				harmony.Patch((MethodBase)methodInfo, val, val2, (HarmonyMethod)null, (HarmonyMethod)null);
			}
		}
	}

	internal static void PatchMethod(Harmony harmony, Type type, Type patch, string method)
	{
		PatchMethod(harmony, type, patch, method, method + "Prefix", method + "Postfix");
	}

	internal static void PatchConstructor(Harmony harmony, Type type, Type patch)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		if (harmony != null && !(type == null))
		{
			Type type2 = type;
			ConstructorInfo constructor;
			do
			{
				constructor = type2.GetConstructor(new Type[0]);
				type2 = type2.BaseType;
			}
			while (type2 != null && constructor == null);
			if (!(constructor == null))
			{
				MethodInfo method = patch.GetMethod("ConstructorPrefix", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				MethodInfo method2 = patch.GetMethod("ConstructorPostfix", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				HarmonyMethod val = ((!(method != null)) ? ((HarmonyMethod)null) : new HarmonyMethod(method));
				HarmonyMethod val2 = ((!(method2 != null)) ? ((HarmonyMethod)null) : new HarmonyMethod(method2));
				harmony.Patch((MethodBase)constructor, val, val2, (HarmonyMethod)null, (HarmonyMethod)null);
			}
		}
	}
}
