using System;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsBloomeryBehavior : BlockBehavior
{
	public XSkillsBloomeryBehavior(Block block)
		: base(block)
	{
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer, ref EnumHandling handling)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		if ((int)world.Api.Side != 2)
		{
			return (WorldInteraction[])(object)new WorldInteraction[0];
		}
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(selection.Position);
		BlockEntityBloomery val = (BlockEntityBloomery)(object)((blockEntity is BlockEntityBloomery) ? blockEntity : null);
		if (val == null || val.IsBurning)
		{
			return (WorldInteraction[])(object)new WorldInteraction[0];
		}
		object? obj = typeof(BlockEntityBloomery).GetProperty("OutSlot", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(val);
		ItemSlot val2 = (ItemSlot)((obj is ItemSlot) ? obj : null);
		if (val2 == null || val2.Itemstack == null)
		{
			return (WorldInteraction[])(object)new WorldInteraction[0];
		}
		XLeveling obj2 = XLeveling.Instance(world.Api);
		if (!(((obj2 != null) ? obj2.GetSkill("metalworking", false) : null) is Metalworking metalworking))
		{
			return (WorldInteraction[])(object)new WorldInteraction[0];
		}
		EntityPlayer entity = forPlayer.Entity;
		object obj3;
		if (entity == null)
		{
			obj3 = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
			if (behavior == null)
			{
				obj3 = null;
			}
			else
			{
				PlayerSkill obj4 = behavior[((Skill)metalworking).Id];
				obj3 = ((obj4 != null) ? obj4[metalworking.BloomeryExpertId] : null);
			}
		}
		if (((PlayerAbility)obj3).Tier < 1)
		{
			return (WorldInteraction[])(object)new WorldInteraction[0];
		}
		WorldInteraction[] array = new WorldInteraction[1];
		WorldInteraction val3 = new WorldInteraction();
		val3.ActionLangCode = Lang.Get("xskills:blockhelp-bloomery-takeresult", new object[1] { val2.Itemstack.GetName() });
		val3.HotKeyCode = null;
		val3.MouseButton = (EnumMouseButton)2;
		val3.Itemstacks = null;
		array[0] = val3;
		return (WorldInteraction[])(object)array;
	}

	public override string GetPlacedBlockInfo(IWorldAccessor world, BlockPos pos, IPlayer forPlayer)
	{
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
		BlockEntityBloomery val = (BlockEntityBloomery)(object)((blockEntity is BlockEntityBloomery) ? blockEntity : null);
		if (val == null || !val.IsBurning)
		{
			return "";
		}
		XLeveling obj = XLeveling.Instance(world.Api);
		if (!(((obj != null) ? obj.GetSkill("metalworking", false) : null) is Metalworking metalworking))
		{
			return "";
		}
		EntityPlayer entity = forPlayer.Entity;
		object obj2;
		if (entity == null)
		{
			obj2 = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
			if (behavior == null)
			{
				obj2 = null;
			}
			else
			{
				PlayerSkill obj3 = behavior[((Skill)metalworking).Id];
				obj2 = ((obj3 != null) ? obj3[((Skill)metalworking).SpecialisationID] : null);
			}
		}
		if (((PlayerAbility)obj2).Tier < 1)
		{
			return "";
		}
		double valueOrDefault = (typeof(BlockEntityBloomery).GetField("burningUntilTotalDays", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(val) as double?).GetValueOrDefault();
		double valueOrDefault2 = (typeof(BlockEntityBloomery).GetField("burningStartTotalDays", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(val) as double?).GetValueOrDefault();
		double totalDays = world.Calendar.TotalDays;
		float num = (float)Math.Min(1.0 - (totalDays - valueOrDefault) / (valueOrDefault2 - valueOrDefault), 1.0);
		return Lang.Get("xskills:progress", new object[1] { num });
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		if (blockSel == null || byPlayer == null)
		{
			return false;
		}
		if (!world.Claims.TryAccess(byPlayer, blockSel.Position, (EnumBlockAccessFlags)2))
		{
			return false;
		}
		XLeveling obj = XLeveling.Instance(world.Api);
		if (!(((obj != null) ? obj.GetSkill("metalworking", false) : null) is Metalworking metalworking))
		{
			return false;
		}
		EntityPlayer entity = byPlayer.Entity;
		object obj2;
		if (entity == null)
		{
			obj2 = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
			if (behavior == null)
			{
				obj2 = null;
			}
			else
			{
				PlayerSkill obj3 = behavior[((Skill)metalworking).Id];
				obj2 = ((obj3 != null) ? obj3[metalworking.BloomeryExpertId] : null);
			}
		}
		if (((PlayerAbility)obj2).Tier < 1)
		{
			return false;
		}
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position);
		BlockEntityBloomery val = (BlockEntityBloomery)(object)((blockEntity is BlockEntityBloomery) ? blockEntity : null);
		if (val == null || val.IsBurning)
		{
			return false;
		}
		object? obj4 = typeof(BlockEntityBloomery).GetProperty("OutSlot", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(val);
		ItemSlot val2 = (ItemSlot)((obj4 is ItemSlot) ? obj4 : null);
		if (val2 == null || val2.Itemstack == null)
		{
			return false;
		}
		if (!byPlayer.InventoryManager.TryGiveItemstack(val2.Itemstack, false))
		{
			world.SpawnItemEntity(val2.Itemstack, blockSel.Position.ToVec3d().Add(0.5, 0.5, 0.5), (Vec3d)null);
		}
		handling = (EnumHandling)2;
		val2.Itemstack = null;
		return true;
	}
}
