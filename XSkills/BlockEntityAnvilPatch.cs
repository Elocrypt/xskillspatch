using System;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

internal class BlockEntityAnvilPatch : ManualPatch
{
	public static void Apply(Harmony harmony, Type anvilType)
	{
		Type typeFromHandle = typeof(BlockEntityAnvilPatch);
		ManualPatch.PatchMethod(harmony, anvilType, typeFromHandle, "TryPut");
		ManualPatch.PatchMethod(harmony, anvilType, typeFromHandle, "OnReceivedClientPacket");
		ManualPatch.PatchMethod(harmony, anvilType, typeFromHandle, "OnUpset");
		ManualPatch.PatchMethod(harmony, anvilType, typeFromHandle, "OnSplit");
		ManualPatch.PatchMethod(harmony, anvilType, typeFromHandle, "OnHit");
		ManualPatch.PatchMethod(harmony, anvilType, typeFromHandle, "onHelveHitSuccess");
		ManualPatch.PatchMethod(harmony, anvilType, typeFromHandle, "CheckIfFinished");
		ManualPatch.PatchMethod(harmony, anvilType, typeFromHandle, "recipeVoxels");
	}

	public static void TryPutPrefix(BlockEntityAnvil __instance, ref bool __state, IPlayer byPlayer)
	{
		ItemSlot activeHotbarSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		object obj;
		if (activeHotbarSlot == null)
		{
			obj = null;
		}
		else
		{
			ItemStack itemstack = activeHotbarSlot.Itemstack;
			obj = ((itemstack != null) ? itemstack.Collectible : null);
		}
		CollectibleObject val = (CollectibleObject)obj;
		__state = val is ItemMetalPlate;
	}

	public static void TryPutPostfix(BlockEntityAnvil __instance, bool __state, IPlayer byPlayer)
	{
		__instance.SetUsedByPlayer(byPlayer);
		if (__state)
		{
			__instance.SetWasPlate(__state);
		}
	}

	public static void OnReceivedClientPacketPrefix(BlockEntityAnvil __instance, IPlayer player)
	{
		__instance.SetUsedByPlayer(player);
	}

	public static void OnUpsetPrefix(BlockEntityAnvil __instance, Vec3i voxelPos, BlockFacing towardsFace)
	{
		if (__instance.Voxels[voxelPos.X, voxelPos.Y, voxelPos.Z] == 1)
		{
			__instance.SetHitCount(__instance.GetHitCount() + 1);
		}
	}

	public static void OnSplitPrefix(BlockEntityAnvil __instance, Vec3i voxelPos)
	{
		if (__instance.Voxels[voxelPos.X, voxelPos.Y, voxelPos.Z] != 0)
		{
			__instance.SetHitCount(__instance.GetHitCount() + 1);
			if (__instance.Voxels[voxelPos.X, voxelPos.Y, voxelPos.Z] == 1)
			{
				__instance.SetSplitCount(__instance.GetSplitCount() + 1);
			}
		}
	}

	public static bool OnHitPrefix(BlockEntityAnvil __instance, Vec3i voxelPos)
	{
		if (__instance.Voxels[voxelPos.X, voxelPos.Y, voxelPos.Z] != 0)
		{
			__instance.SetHitCount(__instance.GetHitCount() + 1);
		}
		object obj = __instance.GetUsedByPlayer();
		if (obj == null)
		{
			ICoreAPI api = ((BlockEntity)__instance).Api;
			ICoreAPI obj2 = ((api is ClientCoreAPI) ? api : null);
			obj = ((obj2 != null) ? ((ClientCoreAPI)obj2).World.Player : null);
		}
		IPlayer val = (IPlayer)obj;
		XLeveling modSystem = ((BlockEntity)__instance).Api.ModLoader.GetModSystem<XLeveling>(true);
		Metalworking metalworking = ((modSystem != null) ? modSystem.GetSkill("metalworking", false) : null) as Metalworking;
		if (val == null || metalworking == null)
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
			obj3 = ((behavior != null) ? behavior[((Skill)metalworking).Id] : null);
		}
		PlayerSkill val2 = (PlayerSkill)obj3;
		PlayerAbility val3 = ((val2 != null) ? val2[metalworking.HeavyHitsId] : null);
		if (val3 == null)
		{
			return true;
		}
		if (__instance.Voxels[voxelPos.X, voxelPos.Y, voxelPos.Z] == 2 && val3.Tier > 0)
		{
			__instance.OnSplit(voxelPos);
			return false;
		}
		if (__instance.Voxels[voxelPos.X, voxelPos.Y, voxelPos.Z] == 1)
		{
			val3 = val2[metalworking.MasterSmithId];
			bool[,,] recipeVoxels = __instance.recipeVoxels;
			int range = val3.Value(1, 0);
			int num = val3.Value(0, 0);
			if (num <= 0)
			{
				return true;
			}
			Vec3i[] array = __instance.FindFreeVoxels(num, voxelPos, range);
			if (array.Length == 0)
			{
				return true;
			}
			int num2 = 0;
			int num3 = voxelPos.Y;
			while (num3 >= 0 && num3 >= num3 - 1 && num2 < array.Length)
			{
				for (int i = voxelPos.Z - 1; i <= voxelPos.Z + 1; i++)
				{
					if (num2 >= array.Length)
					{
						break;
					}
					if (i < 0 || i >= 16)
					{
						continue;
					}
					for (int j = voxelPos.X - 1; j <= voxelPos.X + 1; j++)
					{
						if (num2 >= array.Length)
						{
							break;
						}
						if (j >= 0 && j < 16 && __instance.Voxels[j, num3, i] == 1 && !recipeVoxels[j, num3, i])
						{
							Vec3i val4 = array[num2];
							if (val4 != (Vec3i)null)
							{
								__instance.Voxels[j, num3, i] = 0;
								__instance.Voxels[val4.X, val4.Y, val4.Z] = 1;
								num2++;
							}
						}
					}
				}
				num3--;
			}
		}
		return true;
	}

	public static void onHelveHitSuccessPostfix(BlockEntityAnvil __instance, EnumVoxelMaterial mat, Vec3i usableMetalVoxel)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		__instance.SetHelveHammered(value: true);
		__instance.SetHitCount(__instance.GetHitCount() + 1);
		if ((int)mat == 1 && usableMetalVoxel == (Vec3i)null)
		{
			__instance.SetSplitCount(__instance.GetSplitCount() + 1);
		}
	}

	public static bool CheckIfFinishedPrefix(BlockEntityAnvil __instance, out AnvilState __state, IPlayer byPlayer)
	{
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		__state = new AnvilState(__instance);
		if (__state.recipe == null)
		{
			return true;
		}
		PlayerSkill val = null;
		float num = -1f;
		if (__state.metalworking == null || __state.workItemStack == null)
		{
			return true;
		}
		if (byPlayer == null)
		{
			byPlayer = __instance.GetUsedByPlayer();
			object obj;
			if (byPlayer == null)
			{
				obj = null;
			}
			else
			{
				EntityPlayer entity = byPlayer.Entity;
				if (entity == null)
				{
					obj = null;
				}
				else
				{
					PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
					obj = ((behavior != null) ? behavior[((Skill)__state.metalworking).Id] : null);
				}
			}
			val = (PlayerSkill)obj;
			if (val == null)
			{
				return true;
			}
			PlayerAbility obj2 = val[__state.metalworking.MachineLearningId];
			if (obj2 != null && obj2.Tier <= 0)
			{
				return true;
			}
		}
		object obj3 = val;
		if (obj3 == null)
		{
			PlayerSkillSet behavior2 = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj3 = ((behavior2 != null) ? behavior2[((Skill)__state.metalworking).Id] : null);
		}
		val = (PlayerSkill)obj3;
		PlayerAbility val2 = ((val != null) ? val[__state.metalworking.FinishingTouchId] : null);
		if (val2 == null)
		{
			return true;
		}
		if (val2.Tier > 0)
		{
			num = __instance.FinishedProportion();
			if (num < 0f)
			{
				MetalworkingConfig obj4 = ((Skill)__state.metalworking).Config as MetalworkingConfig;
				if (obj4 != null && obj4.allowFinishingTouchExploit)
				{
					num *= -1f;
				}
			}
			float num2 = Math.Min((float)val2.Value(0, 0) + (float)val2.Value(1, 0) * 0.1f, val2.Value(2, 0)) * 0.01f;
			if (num > 0f && (double)(num2 * num * num) >= ((Entity)byPlayer.Entity).World.Rand.NextDouble())
			{
				__state.splitCount += __instance.FinishRecipe();
				num = 1f;
			}
		}
		val2 = val[__state.metalworking.HeatingHitsId];
		if (val2 == null || __state.anvilItemStack == null)
		{
			return true;
		}
		float temperature = __state.workItemStack.Collectible.GetTemperature(((BlockEntity)__instance).Api.World, __state.workItemStack);
		float meltingPoint = __state.workItemStack.Collectible.GetMeltingPoint(((BlockEntity)__instance).Api.World, (ISlotProvider)null, (ItemSlot)new DummySlot(__state.anvilItemStack.GetBaseMaterial(__state.workItemStack)));
		temperature = ((!(meltingPoint > 0f)) ? (temperature + (float)val2.Value(0, 0)) : Math.Min(temperature + (float)val2.Value(0, 0), meltingPoint));
		__state.workItemStack.Collectible.SetTemperature(((BlockEntity)__instance).Api.World, __state.workItemStack, temperature, true);
		val2 = val[__state.metalworking.BlacksmithId];
		if (val2.Tier > 0 && (!__state.wasPlate || !(((RecipeBase<SmithingRecipe>)(object)__state.recipe).Output.ResolvedItemstack.Collectible is ItemMetalPlate)) && !(((RecipeBase<SmithingRecipe>)(object)__state.recipe).Output.ResolvedItemstack.Collectible is ItemIngot))
		{
			float num3 = (float)val2.Value(0, 0) * ((float)Math.Min(val.Level, 20) * 0.25f);
			num3 *= 0.5f;
			num3 = Math.Min(num3 + (float)((Entity)byPlayer.Entity).World.Rand.NextDouble() * num3, val2.Value(1, 0));
			((RecipeBase<SmithingRecipe>)(object)__state.recipe).Output.ResolvedItemstack.Attributes.SetFloat("quality", num3);
		}
		if (num >= 1f || num < 0f || byPlayer == null)
		{
			return true;
		}
		return false;
	}

	public static void CheckIfFinishedPostfix(BlockEntityAnvil __instance, ref AnvilState __state, IPlayer byPlayer)
	{
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Invalid comparison between Unknown and I4
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		SmithingRecipe recipe = __state.recipe;
		float num = ((recipe != null) ? ((RecipeBase<SmithingRecipe>)(object)recipe).Output.ResolvedItemstack.Attributes.GetFloat("quality", 0f) : 0f);
		SmithingRecipe recipe2 = __state.recipe;
		if (recipe2 != null)
		{
			((RecipeBase<SmithingRecipe>)(object)recipe2).Output.ResolvedItemstack.Attributes.RemoveAttribute("quality");
		}
		if (__instance.WorkItemStack != null || __state.metalworking == null)
		{
			return;
		}
		PlayerSkill val = null;
		bool flag = true;
		bool flag2 = false;
		if (byPlayer == null)
		{
			flag2 = true;
			byPlayer = __state.usedBy;
			object obj;
			if (byPlayer == null)
			{
				obj = null;
			}
			else
			{
				EntityPlayer entity = byPlayer.Entity;
				if (entity == null)
				{
					obj = null;
				}
				else
				{
					PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
					obj = ((behavior != null) ? behavior[((Skill)__state.metalworking).Id] : null);
				}
			}
			val = (PlayerSkill)obj;
			if (val == null)
			{
				return;
			}
			PlayerAbility obj2 = val[__state.metalworking.MachineLearningId];
			if (obj2 != null && obj2.Tier <= 0)
			{
				flag = false;
			}
		}
		PlayerSkillSet behavior2 = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
		val = ((behavior2 != null) ? behavior2[((Skill)__state.metalworking).Id] : null);
		if (val == null)
		{
			return;
		}
		if (((Skill)__state.metalworking).Config is MetalworkingConfig metalworkingConfig && !__state.wasPlate)
		{
			float num2 = (((Skill)__state.metalworking).Config as MetalworkingConfig).expBase + (((Skill)__state.metalworking).Config as MetalworkingConfig).expPerHit * (float)__state.hitCount;
			if (__state.helveHammer)
			{
				num2 *= 1f - metalworkingConfig.helveHammerPenalty;
			}
			val.AddExperience(num2, true);
		}
		PlayerAbility val2;
		if (!__state.helveHammer)
		{
			val2 = val[__state.metalworking.HammerExpertId];
			if (val2 == null)
			{
				return;
			}
			IPlayerInventoryManager inventoryManager = byPlayer.InventoryManager;
			object obj3;
			if (inventoryManager == null)
			{
				obj3 = null;
			}
			else
			{
				ItemSlot activeHotbarSlot = inventoryManager.ActiveHotbarSlot;
				obj3 = ((activeHotbarSlot != null) ? activeHotbarSlot.Itemstack : null);
			}
			ItemStack val3 = (ItemStack)obj3;
			if (val3 != null && val3.Collectible?.Tool == (EnumTool?)5)
			{
				int @int = val3.Attributes.GetInt("durability", 0);
				int val4 = @int + (int)((float)__state.hitCount * val2.SkillDependentFValue(0));
				val4 = Math.Min(val4, val3.Collectible.Durability);
				if (val4 > @int)
				{
					val3.Attributes.SetInt("durability", val4);
				}
			}
		}
		if ((int)((BlockEntity)__instance).Api.Side == 2 || !flag)
		{
			return;
		}
		SmithingRecipe recipe3 = __state.recipe;
		object obj4;
		if (recipe3 == null)
		{
			obj4 = null;
		}
		else
		{
			ItemStack resolvedItemstack = ((RecipeBase<SmithingRecipe>)(object)recipe3).Output.ResolvedItemstack;
			obj4 = ((resolvedItemstack != null) ? resolvedItemstack.Collectible.CombustibleProps : null);
		}
		if ((obj4 == null || __state.metalworking.IsDuplicatable(__state.recipe)) && !__state.wasPlate)
		{
			val2 = val[__state.metalworking.DuplicatorId];
			if (val2 == null)
			{
				return;
			}
			if ((double)val2.SkillDependentFValue(0) >= ((BlockEntity)__instance).Api.World.Rand.NextDouble())
			{
				ItemStack val5 = ((RecipeBase<SmithingRecipe>)(object)__state.recipe).Output.ResolvedItemstack.Clone();
				val5.Collectible.SetTemperature(((BlockEntity)__instance).Api.World, val5, __state.workItemStack.Collectible.GetTemperature(((BlockEntity)__instance).Api.World, __state.workItemStack), true);
				if (num > 0f)
				{
					val5.Attributes.SetFloat("quality", num);
				}
				if (flag2 || !byPlayer.InventoryManager.TryGiveItemstack(val5, false))
				{
					((BlockEntity)__instance).Api.World.SpawnItemEntity(val5, ((BlockEntity)__instance).Pos.ToVec3d().Add(0.5, 1.5, 0.5), (Vec3d)null);
				}
			}
		}
		val2 = val[__state.metalworking.MetalRecoveryId];
		if (val2 == null)
		{
			return;
		}
		int num3 = val2.Value(0, 0);
		if (num3 <= 0 || __state.wasIronBloom)
		{
			return;
		}
		int num4 = __state.splitCount / num3;
		string obj5 = ((((Skill)__state.metalworking).Config as MetalworkingConfig).useVanillaBits ? "game" : "xskills");
		string text = ((RegistryObject)__state.anvilItemStack.GetBaseMaterial(__state.workItemStack).Collectible).LastCodePart(0);
		if (text == "steel")
		{
			text = "blistersteel";
		}
		AssetLocation val6 = new AssetLocation(obj5, "metalbit-" + text);
		Item item = ((BlockEntity)__instance).Api.World.GetItem(val6);
		if (item != null && num4 > 0)
		{
			ItemStack val7 = new ItemStack(item, num4);
			if (flag2 || !byPlayer.InventoryManager.TryGiveItemstack(val7, false))
			{
				((BlockEntity)__instance).Api.World.SpawnItemEntity(val7, ((BlockEntity)__instance).Pos.ToVec3d().Add(0.5, 1.5, 0.5), (Vec3d)null);
			}
		}
	}

	public static bool recipeVoxelsPrefix(BlockEntityAnvil __instance, out bool[,,] __result)
	{
		if (__instance.SelectedRecipe == null)
		{
			__result = null;
			return false;
		}
		bool[,,] voxels = ((LayeredVoxelRecipe<SmithingRecipe>)(object)__instance.SelectedRecipe).Voxels;
		bool[,,] array = new bool[voxels.GetLength(0), voxels.GetLength(1), voxels.GetLength(2)];
		switch (__instance.rotation / 90 % 4)
		{
		case 0:
			__result = voxels;
			return false;
		case 1:
		{
			for (int l = 0; l < voxels.GetLength(0); l++)
			{
				for (int m = 0; m < voxels.GetLength(1); m++)
				{
					for (int n = 0; n < voxels.GetLength(2); n++)
					{
						array[n, m, l] = voxels[16 - l - 1, m, n];
					}
				}
			}
			break;
		}
		case 2:
		{
			for (int num = 0; num < voxels.GetLength(0); num++)
			{
				for (int num2 = 0; num2 < voxels.GetLength(1); num2++)
				{
					for (int num3 = 0; num3 < voxels.GetLength(2); num3++)
					{
						array[num, num2, num3] = voxels[16 - num - 1, num2, 16 - num3 - 1];
					}
				}
			}
			break;
		}
		case 3:
		{
			for (int i = 0; i < voxels.GetLength(0); i++)
			{
				for (int j = 0; j < voxels.GetLength(1); j++)
				{
					for (int k = 0; k < voxels.GetLength(2); k++)
					{
						array[k, j, i] = voxels[i, j, 16 - k - 1];
					}
				}
			}
			break;
		}
		}
		__result = array;
		return false;
	}
}
