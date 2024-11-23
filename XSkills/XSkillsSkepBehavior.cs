using System;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsSkepBehavior : BlockBehavior
{
	private Farming farming;

	private float xp;

	public float HarvestTime { get; set; }

	public BlockDropItemStack HarvestedStack { get; set; }

	public XSkillsSkepBehavior(Block block)
		: base(block)
	{
	}

	public override void Initialize(JsonObject properties)
	{
		xp = properties["xp"].AsFloat(0f);
		HarvestTime = properties["harvestTime"].AsFloat(1f);
		((CollectibleBehavior)this).Initialize(properties);
	}

	public override void OnLoaded(ICoreAPI api)
	{
		XLeveling obj = XLeveling.Instance(api);
		farming = ((obj != null) ? obj.GetSkill("farming", false) : null) as Farming;
		if (base.block.Drops.Length != 0)
		{
			HarvestedStack = base.block.Drops[0];
		}
		else
		{
			HarvestedStack = null;
		}
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer, ref EnumHandling handling)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		if ((int)world.Api.Side != 2)
		{
			return (WorldInteraction[])(object)new WorldInteraction[0];
		}
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(selection.Position);
		BlockEntityBeehive val = (BlockEntityBeehive)(object)((blockEntity is BlockEntityBeehive) ? blockEntity : null);
		if (val == null || !val.Harvestable || farming == null)
		{
			return (WorldInteraction[])(object)new WorldInteraction[0];
		}
		EntityPlayer entity = forPlayer.Entity;
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
				PlayerSkill obj2 = behavior[((Skill)farming).Id];
				obj = ((obj2 != null) ? obj2[farming.BeemasterId] : null);
			}
		}
		if (((PlayerAbility)obj).Tier < 1)
		{
			return (WorldInteraction[])(object)new WorldInteraction[0];
		}
		return (WorldInteraction[])(object)new WorldInteraction[1]
		{
			new WorldInteraction
			{
				ActionLangCode = Lang.Get("xskills:beehive-harvest", Array.Empty<object>()),
				HotKeyCode = null,
				MouseButton = (EnumMouseButton)2,
				Itemstacks = null
			}
		};
	}

	public override string GetPlacedBlockInfo(IWorldAccessor world, BlockPos pos, IPlayer forPlayer)
	{
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
		BlockEntityBeehive val = (BlockEntityBeehive)(object)((blockEntity is BlockEntityBeehive) ? blockEntity : null);
		if (val == null || val.Harvestable)
		{
			return "";
		}
		if (farming == null)
		{
			return "";
		}
		EntityPlayer entity = forPlayer.Entity;
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
				PlayerSkill obj2 = behavior[((Skill)farming).Id];
				obj = ((obj2 != null) ? obj2[((Skill)farming).SpecialisationID] : null);
			}
		}
		if (((PlayerAbility)obj).Tier < 1)
		{
			return "";
		}
		double num = (typeof(BlockEntityBeehive).GetField("harvestableAtTotalHours", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(val) as double?).GetValueOrDefault() / (double)world.Calendar.HoursPerDay;
		double totalDays = world.Calendar.TotalDays;
		float num2 = (float)(num - totalDays);
		if (num2 < 0f)
		{
			return "";
		}
		return Lang.Get("xskills:harvestable-in-days", new object[1] { num2 });
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		if (blockSel == null || byPlayer == null)
		{
			return false;
		}
		if (!world.Claims.TryAccess(byPlayer, blockSel.Position, (EnumBlockAccessFlags)2))
		{
			return false;
		}
		object obj;
		if (byPlayer == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj = ((behavior != null) ? behavior[((Skill)farming).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj;
		if (val == null)
		{
			return false;
		}
		PlayerAbility val2 = val[farming.BeemasterId];
		if (val2 == null || val2.Tier <= 0)
		{
			return false;
		}
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position);
		BlockEntityBeehive val3 = (BlockEntityBeehive)(object)((blockEntity is BlockEntityBeehive) ? blockEntity : null);
		if (val3 == null || !val3.Harvestable)
		{
			return false;
		}
		handling = (EnumHandling)2;
		world.PlaySoundAt(new AssetLocation("game:sounds/block/plant"), (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, byPlayer, true, 32f, 1f);
		return true;
	}

	public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Invalid comparison between Unknown and I4
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		if (blockSel == null)
		{
			return false;
		}
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position);
		BlockEntityBeehive val = (BlockEntityBeehive)(object)((blockEntity is BlockEntityBeehive) ? blockEntity : null);
		if (val == null || !val.Harvestable)
		{
			return false;
		}
		handling = (EnumHandling)2;
		IPlayer obj = ((byPlayer is IClientPlayer) ? byPlayer : null);
		if (obj != null)
		{
			((IClientPlayer)obj).TriggerFpAnimation((EnumHandInteract)1);
		}
		if (world.Rand.NextDouble() < 0.1)
		{
			world.PlaySoundAt(new AssetLocation("game:sounds/block/plant"), (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, byPlayer, true, 32f, 1f);
		}
		if ((int)world.Side != 2)
		{
			return secondsUsed < HarvestTime;
		}
		return true;
	}

	public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		if (blockSel == null || byPlayer == null || secondsUsed < HarvestTime - 0.05f || HarvestedStack == null || (int)world.Side == 2)
		{
			return;
		}
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
				obj = ((behavior != null) ? behavior[((Skill)farming).Id] : null);
			}
		}
		PlayerSkill val = (PlayerSkill)obj;
		if (val == null)
		{
			return;
		}
		PlayerAbility val2 = val[farming.BeemasterId];
		if (val2 == null || val2.Tier <= 0)
		{
			return;
		}
		PlayerAbility val3 = val[farming.BeekeeperId];
		if (val3 == null)
		{
			return;
		}
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position);
		BlockEntityBeehive val4 = (BlockEntityBeehive)(object)((blockEntity is BlockEntityBeehive) ? blockEntity : null);
		if (val4 != null && val4.Harvestable)
		{
			handling = (EnumHandling)2;
			ItemStack nextItemStack = HarvestedStack.GetNextItemStack(1f);
			nextItemStack.StackSize += val3.Value(0, 0);
			val.AddExperience(xp, true);
			if (!byPlayer.InventoryManager.TryGiveItemstack(nextItemStack, false))
			{
				world.SpawnItemEntity(nextItemStack, blockSel.Position.ToVec3d().Add(0.5, 0.5, 0.5), (Vec3d)null);
			}
			ITreeAttribute val5 = (ITreeAttribute)new TreeAttribute();
			((BlockEntity)val4).ToTreeAttributes(val5);
			val5.SetInt("harvestable", 0);
			val5.SetDouble("harvestableAtTotalHours", world.Calendar.TotalHours + 12.0 * (3.0 + world.Rand.NextDouble() * 8.0));
			val5.SetInt("hiveHealth", 0);
			((BlockEntity)val4).FromTreeAttributes(val5, world);
			((BlockEntity)val4).MarkDirty(false, (IPlayer)null);
			world.PlaySoundAt(new AssetLocation("game:sounds/block/plant"), (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, byPlayer, true, 32f, 1f);
		}
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref EnumHandling handling)
	{
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		if (farming == null)
		{
			return;
		}
		object obj;
		if (byPlayer == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj = ((behavior != null) ? behavior[((Skill)farming).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj;
		if (val == null)
		{
			return;
		}
		Block block = base.block;
		BlockSkep val2 = (BlockSkep)(object)((block is BlockSkep) ? block : null);
		if (val2 == null || val2.IsEmpty())
		{
			return;
		}
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
		BlockEntityBeehive val3 = (BlockEntityBeehive)(object)((blockEntity is BlockEntityBeehive) ? blockEntity : null);
		if (val3 == null || !val3.Harvestable)
		{
			return;
		}
		BlockReinforcement reinforcment = world.Api.ModLoader.GetModSystem<ModSystemBlockReinforcement>(true).GetReinforcment(pos);
		if (reinforcment != null && reinforcment.Strength > 0)
		{
			return;
		}
		val.AddExperience(xp, true);
		PlayerAbility val4 = val[farming.BeekeeperId];
		if (val4 != null)
		{
			for (int num = val4.Value(0, 0); num > 0; num--)
			{
				world.SpawnItemEntity(new ItemStack(base.block.Drops[0].ResolvedItemstack.Item, 1), pos.ToVec3d().Add(0.5, 0.5, 0.5), (Vec3d)null);
			}
		}
	}
}
