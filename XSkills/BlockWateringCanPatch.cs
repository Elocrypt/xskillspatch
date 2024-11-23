using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockWateringCan))]
public class BlockWateringCanPatch
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
		if (original.Name == "OnHeldInteractStep")
		{
			return ((Skill)farming)[farming.ExtensiveFarmingId].Enabled;
		}
		return ((Skill)farming).Enabled;
	}

	[HarmonyPostfix]
	[HarmonyPatch("OnHeldInteractStep")]
	public static void Postfix(bool __result, float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel)
	{
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Expected O, but got Unknown
		if (!__result)
		{
			return;
		}
		IWorldAccessor world = ((Entity)byEntity).World;
		Block block = world.BlockAccessor.GetBlock(blockSel.Position);
		float @float = slot.Itemstack.TempAttributes.GetFloat("secondsUsed", 0f);
		XLeveling obj = XLeveling.Instance(((Entity)byEntity).Api);
		if (!(((obj != null) ? obj.GetSkill("farming", false) : null) is Farming farming))
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)byEntity).GetBehavior<PlayerSkillSet>();
		object obj2;
		if (behavior == null)
		{
			obj2 = null;
		}
		else
		{
			PlayerSkill obj3 = behavior[((Skill)farming).Id];
			obj2 = ((obj3 != null) ? obj3[farming.ExtensiveFarmingId] : null);
		}
		int num = ((PlayerAbility)obj2).Value(0, 0);
		if (num == 0)
		{
			return;
		}
		int x = blockSel.Position.X;
		int y = blockSel.Position.Y;
		int z = blockSel.Position.Z;
		int num2 = 0;
		int num3 = 0;
		if (num % 2 == 0)
		{
			if ((double)x - ((Entity)byEntity).Pos.X >= 0.0)
			{
				num2 = 1;
			}
			if ((double)z - ((Entity)byEntity).Pos.Z >= 0.0)
			{
				num3 = 1;
			}
		}
		x = x - num / 2 + num2;
		z = z - num / 2 + num3;
		for (int i = x; i < x + num; i++)
		{
			for (int j = z; j < z + num; j++)
			{
				if (i == blockSel.Position.X && j == blockSel.Position.Z)
				{
					continue;
				}
				BlockPos val = new BlockPos(i, y, j, blockSel.Position.dimension);
				if (block.CollisionBoxes == null || block.CollisionBoxes.Length == 0)
				{
					block = world.BlockAccessor.GetBlock(blockSel.Position, 2);
					if ((block.CollisionBoxes == null || block.CollisionBoxes.Length == 0) && !((CollectibleObject)block).IsLiquid())
					{
						val = val.DownCopy(1);
					}
				}
				BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(val);
				BlockEntityFarmland val2 = (BlockEntityFarmland)(object)((blockEntity is BlockEntityFarmland) ? blockEntity : null);
				if (val2 != null)
				{
					val2.WaterFarmland(secondsUsed - @float, true);
				}
			}
		}
	}
}
