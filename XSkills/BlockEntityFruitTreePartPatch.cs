using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityFruitTreePart))]
public class BlockEntityFruitTreePartPatch
{
	public static bool Prepare(MethodBase original)
	{
		XSkills instance = XSkills.Instance;
		if (instance == null)
		{
			return false;
		}
		instance.Skills.TryGetValue("farming", out var value);
		if (!(value is Farming farming) || !((Skill)farming).Enabled)
		{
			return false;
		}
		if (original == null)
		{
			return true;
		}
		Ability obj = ((Skill)farming)[farming.OrchardistId];
		if (obj == null)
		{
			return false;
		}
		return obj.Enabled;
	}

	[HarmonyPrefix]
	[HarmonyPatch("OnBlockInteractStop")]
	public static void OnBlockInteractStopPrefix(BlockEntityFruitTreePart __instance, out bool __state)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		__state = (int)__instance.FoliageState == 4;
	}

	[HarmonyPostfix]
	[HarmonyPatch("OnBlockInteractStop")]
	public static void OnBlockInteractStopPostfix(BlockEntityFruitTreePart __instance, bool __state, IPlayer byPlayer)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)__instance.FoliageState == 4 || !__state)
		{
			return;
		}
		XLeveling modSystem = ((BlockEntity)__instance).Api.ModLoader.GetModSystem<XLeveling>(true);
		if (!(((modSystem != null) ? modSystem.GetSkill("farming", false) : null) is Farming farming))
		{
			return;
		}
		EntityPlayer entity = byPlayer.Entity;
		object obj;
		if (entity == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
			obj = ((behavior != null) ? behavior[((Skill)farming).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj;
		if (val == null)
		{
			return;
		}
		float num = (((Skill)farming).Config as FarmingConfig)?.treeHarvestExp ?? 0f;
		if (num > 0f)
		{
			val.AddExperience(num, true);
		}
		PlayerAbility val2 = val[farming.OrchardistId];
		if (val2 != null && val2.Tier <= 0)
		{
			return;
		}
		float num2 = val2.SkillDependentFValue(0);
		AssetLocation val3 = AssetLocation.Create(((CollectibleObject)((BlockEntity)__instance).Block).Attributes["branchBlock"].AsString((string)null), ((RegistryObject)((BlockEntity)__instance).Block).Code.Domain);
		Block block = ((BlockEntity)__instance).Api.World.GetBlock(val3);
		BlockDropItemStack[] fruitStacks = ((BlockFruitTreeBranch)((block is BlockFruitTreeBranch) ? block : null)).TypeProps[__instance.TreeType].FruitStacks;
		foreach (BlockDropItemStack val4 in fruitStacks)
		{
			ItemStack nextItemStack = val4.GetNextItemStack(num2);
			if (nextItemStack != null)
			{
				if (!byPlayer.InventoryManager.TryGiveItemstack(nextItemStack, true))
				{
					((BlockEntity)__instance).Api.World.SpawnItemEntity(nextItemStack, ((Entity)byPlayer.Entity).Pos.XYZ.Add(0.0, 0.5, 0.0), (Vec3d)null);
				}
				if (val4.LastDrop)
				{
					break;
				}
			}
		}
	}
}
