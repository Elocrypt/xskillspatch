using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockFruitTreeBranch))]
public class BlockFruitTreeBranchPatch
{
	public static bool Prepare(MethodBase original)
	{
		XSkills instance = XSkills.Instance;
		if (instance == null)
		{
			return false;
		}
		instance.Skills.TryGetValue("forestry", out var value);
		if (!(value is Forestry forestry) || !((Skill)forestry).Enabled)
		{
			return false;
		}
		if (original == null)
		{
			return true;
		}
		Ability obj = ((Skill)forestry)[forestry.GrafterId];
		if (obj == null)
		{
			return false;
		}
		return obj.Enabled;
	}

	[HarmonyPostfix]
	[HarmonyPatch("TryPlaceBlock")]
	public static void TryPlaceBlockPostfix(bool __result, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (!__result)
		{
			return;
		}
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position);
		BlockEntityFruitTreeBranch val = (BlockEntityFruitTreeBranch)(object)((blockEntity is BlockEntityFruitTreeBranch) ? blockEntity : null);
		BlockEntityBehaviorValue blockEntityBehaviorValue = new BlockEntityBehaviorValue((BlockEntity)(object)val);
		XLeveling modSystem = world.Api.ModLoader.GetModSystem<XLeveling>(true);
		if (!(((modSystem != null) ? modSystem.GetSkill("forestry", false) : null) is Forestry forestry))
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
			if (behavior == null)
			{
				obj = null;
			}
			else
			{
				PlayerSkill obj2 = behavior[((Skill)forestry).Id];
				obj = ((obj2 != null) ? obj2[forestry.GrafterId] : null);
			}
		}
		PlayerAbility val2 = (PlayerAbility)obj;
		if (val2 == null || val2.Tier > 0)
		{
			blockEntityBehaviorValue.Value = 1f + val2.FValue(0, 0f);
			((BlockEntity)val).Behaviors.Add((BlockEntityBehavior)(object)blockEntityBehaviorValue);
		}
	}
}
