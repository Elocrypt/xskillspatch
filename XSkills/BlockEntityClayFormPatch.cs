using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityClayForm))]
public class BlockEntityClayFormPatch
{
	public class ClayFormCheckIfFinishedState
	{
		public ItemStack workItemStack;

		public ClayFormingRecipe recipe;

		public Pottery pottery;

		public PlayerSkill playerSkill;
	}

	public static bool Prepare(MethodBase original)
	{
		XSkills instance = XSkills.Instance;
		if (instance == null)
		{
			return false;
		}
		instance.Skills.TryGetValue("pottery", out var value);
		if (!(value is Pottery pottery) || !((Skill)pottery).Enabled)
		{
			return false;
		}
		if (original == null)
		{
			return true;
		}
		if (original.Name == "OnUseOver")
		{
			if (!((Skill)pottery)[pottery.InfallibleId].Enabled && !((Skill)pottery)[pottery.PerfectionistId].Enabled && !((Skill)pottery)[pottery.PerfectFitId].Enabled && !((Skill)pottery)[pottery.LayerLayerId].Enabled)
			{
				return ((Skill)pottery)[pottery.ThriftId].Enabled;
			}
			return true;
		}
		return true;
	}

	[HarmonyTranspiler]
	[HarmonyPatch("OnUseOver", new Type[]
	{
		typeof(IPlayer),
		typeof(Vec3i),
		typeof(BlockFacing),
		typeof(bool)
	})]
	public static IEnumerable<CodeInstruction> OnUseOverTranspiler(IEnumerable<CodeInstruction> instructions)
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		List<CodeInstruction> list = new List<CodeInstruction>(instructions);
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].opcode == OpCodes.Call)
			{
				MethodInfo methodInfo = list[i].operand as MethodInfo;
				if (methodInfo.Name == "OnCopyLayer")
				{
					num = i;
				}
				else if (methodInfo.Name == "OnAdd")
				{
					num3 = i;
				}
				else if (methodInfo.Name == "OnRemove")
				{
					num2 = i;
				}
			}
		}
		Type typeFromHandle = typeof(BlockEntityClayFormPatch);
		if (num2 >= 0)
		{
			list.Insert(num2, new CodeInstruction(OpCodes.Ldarg_1, (object)null));
			list[num2 + 1] = new CodeInstruction(OpCodes.Call, (object)typeFromHandle.GetMethod("OnRemove"));
			if (num3 > num2)
			{
				num3++;
			}
			if (num > num2)
			{
				num++;
			}
		}
		if (num3 >= 0)
		{
			list.Insert(num3, new CodeInstruction(OpCodes.Ldarg_1, (object)null));
			list[num3 + 1] = new CodeInstruction(OpCodes.Call, (object)typeFromHandle.GetMethod("OnAdd", new Type[6]
			{
				typeof(BlockEntityClayForm),
				typeof(int),
				typeof(Vec3i),
				typeof(BlockFacing),
				typeof(int),
				typeof(IPlayer)
			}));
			if (num > num3)
			{
				num++;
			}
		}
		if (num >= 0)
		{
			list.Insert(num, new CodeInstruction(OpCodes.Ldarg_1, (object)null));
			list[num + 1] = new CodeInstruction(OpCodes.Call, (object)typeFromHandle.GetMethod("OnCopyLayer"));
		}
		return list;
	}

	[HarmonyPrefix]
	[HarmonyPatch("CheckIfFinished")]
	public static void CheckIfFinishedPrefix(BlockEntityClayForm __instance, out ClayFormCheckIfFinishedState __state, IPlayer byPlayer, ItemStack ___workItemStack)
	{
		__state = new ClayFormCheckIfFinishedState();
		__state.workItemStack = ___workItemStack;
		__state.recipe = __instance.SelectedRecipe;
		__state.pottery = XLeveling.Instance(((Entity)byPlayer.Entity).Api).GetSkill("pottery", false) as Pottery;
		if (__state.pottery == null)
		{
			return;
		}
		ClayFormCheckIfFinishedState obj = __state;
		PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
		obj.playerSkill = ((behavior != null) ? behavior[((Skill)__state.pottery).Id] : null);
		if (__state.playerSkill == null)
		{
			return;
		}
		PlayerAbility val = __state.playerSkill[__state.pottery.FastPotterId];
		if (val != null && val.Tier > 0)
		{
			float num = PotteryHelper.FinishedProportion(__instance);
			if ((double)(Math.Min((float)val.Value(0, 0) + (float)val.Value(1, 0) * 0.1f, val.Value(2, 0)) * 0.01f * num * num) >= ((Entity)byPlayer.Entity).World.Rand.NextDouble())
			{
				PotteryHelper.FinishRecipe(__instance, byPlayer.InventoryManager.ActiveHotbarSlot, (EntityAgent)(object)byPlayer.Entity);
			}
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch("CheckIfFinished")]
	public static void CheckIfFinishedPostfix(BlockEntityClayForm __instance, ClayFormCheckIfFinishedState __state, IPlayer byPlayer, ItemStack ___workItemStack)
	{
		if (___workItemStack != null || byPlayer == null || __state.playerSkill == null)
		{
			return;
		}
		int num = PotteryHelper.CountVoxels(__state.recipe);
		__state.playerSkill.AddExperience(1f + (float)num * 0.002f, true);
		if ((double)((float)__state.playerSkill[__state.pottery.JackPotId].SkillDependentValue(0) * 0.01f) >= ((Entity)byPlayer.Entity).World.Rand.NextDouble())
		{
			ItemStack val = ((RecipeBase<ClayFormingRecipe>)(object)__state.recipe).Output.ResolvedItemstack.Clone();
			if (!byPlayer.InventoryManager.TryGiveItemstack(val, false))
			{
				((Entity)byPlayer.Entity).World.SpawnItemEntity(val, ((Entity)byPlayer.Entity).Pos.XYZ.Add(0.5, 0.5, 0.5), (Vec3d)null);
			}
		}
	}

	public static bool OnAdd(BlockEntityClayForm clayForm, int layer, Vec3i voxelPos, BlockFacing facing, int radius, IPlayer player)
	{
		if (voxelPos.Y == layer && facing.IsVertical)
		{
			return OnAdd(clayForm, layer, voxelPos, radius, player);
		}
		if (clayForm.Voxels[voxelPos.X, voxelPos.Y, voxelPos.Z])
		{
			Vec3i val = voxelPos.AddCopy(facing);
			if (layer >= 0 && layer < 16 && clayForm.InBounds(val, LayerBounds(layer, clayForm.SelectedRecipe)))
			{
				return OnAdd(clayForm, layer, val, radius, player);
			}
			return false;
		}
		return OnAdd(clayForm, layer, voxelPos, radius, player);
	}

	public static bool OnAdd(BlockEntityClayForm clayForm, int layer, Vec3i voxelPos, int radius, IPlayer byPlayer)
	{
		bool result = false;
		bool flag = false;
		Pottery pottery = XLeveling.Instance(((Entity)byPlayer.Entity).Api).GetSkill("pottery", false) as Pottery;
		object obj;
		if (pottery == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj = ((behavior != null) ? behavior[((Skill)pottery).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj;
		if (val != null)
		{
			PlayerAbility obj2 = val[pottery.PerfectionistId];
			if (obj2 != null && obj2.Tier > 0)
			{
				PlayerAbility obj3 = val[pottery.InfallibleId];
				if (radius <= ((obj3 != null) ? new int?(obj3.Value(0, 0)) : null))
				{
					flag = true;
				}
			}
		}
		Cuboidi val2 = LayerBounds(layer, clayForm.SelectedRecipe);
		for (int i = -(int)Math.Ceiling((float)radius / 2f); i <= radius / 2; i++)
		{
			for (int j = -(int)Math.Ceiling((float)radius / 2f); j <= radius / 2; j++)
			{
				Vec3i val3 = voxelPos.AddCopy(i, 0, j);
				if (!clayForm.InBounds(val3, val2) || val3.Y != layer)
				{
					continue;
				}
				if (!clayForm.Voxels[val3.X, val3.Y, val3.Z])
				{
					if (!((LayeredVoxelRecipe<ClayFormingRecipe>)(object)clayForm.SelectedRecipe).Voxels[val3.X, val3.Y, val3.Z] && flag)
					{
						continue;
					}
					clayForm.AvailableVoxels--;
					if (clayForm.AvailableVoxels < 0)
					{
						PotteryHelper.AddClay(clayForm, byPlayer.InventoryManager.ActiveHotbarSlot, (EntityAgent)(object)byPlayer.Entity);
					}
					result = true;
				}
				clayForm.Voxels[val3.X, val3.Y, val3.Z] = true;
			}
		}
		return result;
	}

	public static bool OnRemove(BlockEntityClayForm clayForm, int layer, Vec3i voxelPos, BlockFacing facing, int radius, IPlayer byPlayer)
	{
		bool flag = false;
		if (voxelPos.Y != layer)
		{
			return flag;
		}
		bool flag2 = false;
		Pottery pottery = XLeveling.Instance(((Entity)byPlayer.Entity).Api).GetSkill("pottery", false) as Pottery;
		object obj;
		if (pottery == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj = ((behavior != null) ? behavior[((Skill)pottery).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj;
		if (val != null)
		{
			PlayerAbility obj2 = val[pottery.PerfectFitId];
			if (obj2 != null && obj2.Tier > 0)
			{
				PlayerAbility obj3 = val[pottery.InfallibleId];
				if (radius <= ((obj3 != null) ? new int?(obj3.Value(0, 0)) : null))
				{
					flag2 = true;
				}
			}
		}
		for (int i = -(int)Math.Ceiling((float)radius / 2f); i <= radius / 2; i++)
		{
			for (int j = -(int)Math.Ceiling((float)radius / 2f); j <= radius / 2; j++)
			{
				Vec3i val2 = voxelPos.AddCopy(i, 0, j);
				if (val2.X >= 0 && val2.X < 16 && val2.Y >= 0 && val2.Y <= 16 && val2.Z >= 0 && val2.Z < 16 && !(((LayeredVoxelRecipe<ClayFormingRecipe>)(object)clayForm.SelectedRecipe).Voxels[val2.X, val2.Y, val2.Z] && flag2))
				{
					bool flag3 = clayForm.Voxels[val2.X, val2.Y, val2.Z];
					flag = flag || flag3;
					clayForm.Voxels[val2.X, val2.Y, val2.Z] = false;
					if (flag3)
					{
						clayForm.AvailableVoxels++;
					}
				}
			}
		}
		return flag;
	}

	public static bool OnCopyLayer(BlockEntityClayForm clayForm, int layer, IPlayer byPlayer)
	{
		if (layer <= 0 || layer > 15)
		{
			return false;
		}
		bool result = false;
		int num = 4;
		bool flag = false;
		Pottery pottery = XLeveling.Instance(((Entity)byPlayer.Entity).Api).GetSkill("pottery", false) as Pottery;
		object obj;
		if (pottery == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj = ((behavior != null) ? behavior[((Skill)pottery).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj;
		if (val != null)
		{
			PlayerAbility val2 = val[pottery.LayerLayerId];
			num += ((val2 != null) ? val2.Value(0, 0) : 0);
			val2 = val[pottery.InfallibleId];
			if (val2 != null && val2.Tier >= 2)
			{
				flag = true;
			}
		}
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				if (clayForm.Voxels[i, layer - 1, j] && !clayForm.Voxels[i, layer, j] && (!flag || ((LayeredVoxelRecipe<ClayFormingRecipe>)(object)clayForm.SelectedRecipe).Voxels[i, layer, j]))
				{
					num--;
					clayForm.Voxels[i, layer, j] = true;
					clayForm.AvailableVoxels--;
					result = true;
					if (clayForm.AvailableVoxels < 0)
					{
						PotteryHelper.AddClay(clayForm, byPlayer.InventoryManager.ActiveHotbarSlot, (EntityAgent)(object)byPlayer.Entity);
					}
				}
				if (num == 0)
				{
					return result;
				}
			}
		}
		return result;
	}

	public static Cuboidi LayerBounds(int layer, ClayFormingRecipe SelectedRecipe)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Cuboidi val = new Cuboidi(8, 8, 8, 8, 8, 8);
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				if (((LayeredVoxelRecipe<ClayFormingRecipe>)(object)SelectedRecipe).Voxels[i, layer, j])
				{
					val.X1 = Math.Min(val.X1, i);
					val.X2 = Math.Max(val.X2, i);
					val.Z1 = Math.Min(val.Z1, j);
					val.Z2 = Math.Max(val.Z2, j);
				}
			}
		}
		return val;
	}
}
