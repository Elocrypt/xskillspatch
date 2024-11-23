using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsSaplingBehavior : BlockBehavior
{
	protected Forestry forestry;

	public XSkillsSaplingBehavior(Block block)
		: base(block)
	{
	}

	public override void OnLoaded(ICoreAPI api)
	{
		XLeveling obj = XLeveling.Instance(api);
		forestry = ((obj != null) ? obj.GetSkill("forestry", false) : null) as Forestry;
	}

	public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack, ref EnumHandling handling)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		if (forestry == null || byPlayer == null || (int)world.Api.Side == 2)
		{
			return ((BlockBehavior)this).DoPlaceBlock(world, byPlayer, blockSel, byItemStack, ref handling);
		}
		Action<IWorldAccessor, BlockPos, float> action = delegate(IWorldAccessor worldAccessor, BlockPos blockPos, float f)
		{
			TreePlantedCallback(byPlayer, worldAccessor, blockPos);
		};
		world.Api.Event.RegisterCallback(action, blockSel.Position, 40);
		return ((BlockBehavior)this).DoPlaceBlock(world, byPlayer, blockSel, byItemStack, ref handling);
	}

	public void TreePlantedCallback(IPlayer player, IWorldAccessor world, BlockPos blockPos)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		object obj;
		if (player == null)
		{
			obj = null;
		}
		else
		{
			EntityPlayer entity = player.Entity;
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
					obj = ((obj2 != null) ? obj2[forestry.TreeNurseryId] : null);
				}
			}
		}
		PlayerAbility val = (PlayerAbility)obj;
		if (val != null && val.Tier > 0 && world.BlockAccessor.GetBlockEntity(blockPos) is XSkillsBlockEntitySapling xSkillsBlockEntitySapling)
		{
			TreeAttribute val2 = new TreeAttribute();
			((BlockEntity)xSkillsBlockEntitySapling).ToTreeAttributes((ITreeAttribute)(object)val2);
			double totalHours = world.Calendar.TotalHours;
			double num = val2.GetDouble("totalHoursTillGrowth", 1.0) - totalHours;
			xSkillsBlockEntitySapling.GrowthTimeMultiplier = val.FValue(0, 1f);
			val2.SetFloat("growthTimeMultiplier", xSkillsBlockEntitySapling.GrowthTimeMultiplier);
			num *= (double)xSkillsBlockEntitySapling.GrowthTimeMultiplier;
			val2.SetDouble("totalHoursTillGrowth", totalHours + num);
			((BlockEntity)xSkillsBlockEntitySapling).FromTreeAttributes((ITreeAttribute)(object)val2, world);
			((BlockEntity)xSkillsBlockEntitySapling).MarkDirty(false, (IPlayer)null);
		}
	}
}
