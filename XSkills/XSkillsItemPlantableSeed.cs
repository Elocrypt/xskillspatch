using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.Common;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsItemPlantableSeed : ItemPlantableSeed
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static DrawSkillIconDelegate _003C0_003E__Drawcreate1_svg;

		public static DrawSkillIconDelegate _003C1_003E__Drawcreate4_svg;
	}

	private SkillItem[] toolModes;

	public override void OnLoaded(ICoreAPI api)
	{
		((ItemPlantableSeed)this).OnLoaded(api);
		ICoreClientAPI capi = (ICoreClientAPI)(object)((api is ICoreClientAPI) ? api : null);
		if (capi == null)
		{
			return;
		}
		toolModes = ObjectCacheUtil.GetOrCreate<SkillItem[]>(api, "hoeToolModes", (CreateCachableObjectDelegate<SkillItem[]>)delegate
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Expected O, but got Unknown
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Expected O, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			SkillItem[] array = new SkillItem[3];
			SkillItem val = new SkillItem
			{
				Code = new AssetLocation("1size"),
				Name = Lang.Get("1x1", Array.Empty<object>())
			};
			ICoreClientAPI obj = capi;
			object obj2 = _003C_003EO._003C0_003E__Drawcreate1_svg;
			if (obj2 == null)
			{
				DrawSkillIconDelegate val2 = ItemClay.Drawcreate1_svg;
				_003C_003EO._003C0_003E__Drawcreate1_svg = val2;
				obj2 = (object)val2;
			}
			array[0] = val.WithIcon(obj, (DrawSkillIconDelegate)obj2);
			SkillItem val3 = new SkillItem
			{
				Code = new AssetLocation("2size"),
				Name = Lang.Get("2x2", Array.Empty<object>())
			};
			ICoreClientAPI obj3 = capi;
			object obj4 = _003C_003EO._003C1_003E__Drawcreate4_svg;
			if (obj4 == null)
			{
				DrawSkillIconDelegate val4 = ItemClay.Drawcreate4_svg;
				_003C_003EO._003C1_003E__Drawcreate4_svg = val4;
				obj4 = (object)val4;
			}
			array[1] = val3.WithIcon(obj3, (DrawSkillIconDelegate)obj4);
			array[2] = new SkillItem
			{
				Code = new AssetLocation("3size"),
				Name = Lang.Get("3x3", Array.Empty<object>())
			}.WithIcon(capi, new DrawSkillIconDelegate(new ItemClay().Drawcreate9_svg));
			return (SkillItem[])(object)array;
		});
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		if (toolModes == null)
		{
			return;
		}
		for (int i = 0; i < toolModes.Length; i++)
		{
			SkillItem obj = toolModes[i];
			if (obj != null)
			{
				obj.Dispose();
			}
		}
	}

	public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
	{
		XLeveling obj = XLeveling.Instance(((CollectibleObject)this).api);
		if (!(((obj != null) ? obj.GetSkill("farming", false) : null) is Farming farming))
		{
			return null;
		}
		object obj2;
		if (forPlayer == null)
		{
			obj2 = null;
		}
		else
		{
			EntityPlayer entity = ((IPlayer)forPlayer).Entity;
			if (entity == null)
			{
				obj2 = null;
			}
			else
			{
				PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
				obj2 = ((behavior != null) ? behavior[((Skill)farming).Id][farming.ExtensiveFarmingId] : null);
			}
		}
		PlayerAbility val = (PlayerAbility)obj2;
		if (val == null || val.Tier <= 0)
		{
			return null;
		}
		return ArrayConvert.Copy<SkillItem>((IEnumerable<SkillItem>)toolModes, 0L, (long)(val.Tier + 1));
	}

	public override int GetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		XLeveling obj = XLeveling.Instance(((CollectibleObject)this).api);
		if (!(((obj != null) ? obj.GetSkill("farming", false) : null) is Farming farming))
		{
			return 0;
		}
		EntityPlayer entity = byPlayer.Entity;
		PlayerAbility val = ((entity != null) ? ((Entity)entity).GetBehavior<PlayerSkillSet>()[((Skill)farming).Id][farming.ExtensiveFarmingId] : null);
		if (val == null)
		{
			return 0;
		}
		return GameMath.Clamp(slot.Itemstack.Attributes.GetInt("toolMode", 0), 0, val.Tier);
	}

	public override void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel, int toolMode)
	{
		slot.Itemstack.Attributes.SetInt("toolMode", toolMode);
	}

	public override void OnHeldInteractStart(ItemSlot itemslot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
	{
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		if (blockSel == null)
		{
			return;
		}
		IPlayer player = ((EntityPlayer)((byEntity is EntityPlayer) ? byEntity : null)).Player;
		string text = ((RegistryObject)itemslot.Itemstack.Collectible).LastCodePart(0);
		Block block = ((Entity)byEntity).World.GetBlock(((RegistryObject)this).CodeWithPath("crop-" + text + "-1"));
		if (block == null)
		{
			return;
		}
		XLeveling obj = XLeveling.Instance(((CollectibleObject)this).api);
		if (!(((obj != null) ? obj.GetSkill("farming", false) : null) is Farming farming))
		{
			((ItemPlantableSeed)this).OnHeldInteractStart(itemslot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
			return;
		}
		PlayerSkillSet behavior = ((Entity)byEntity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val = ((behavior != null) ? behavior[((Skill)farming).Id] : null);
		if (val == null)
		{
			((ItemPlantableSeed)this).OnHeldInteractStart(itemslot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
			return;
		}
		PlayerAbility val2 = val[farming.ExtensiveFarmingId];
		int toolMode = ((CollectibleObject)this).GetToolMode(itemslot, player, blockSel);
		int num = 1;
		if (val2 != null && toolMode > 0)
		{
			num = val2.Ability.Value(toolMode, 0);
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
		AssetLocation val3 = null;
		for (int i = x; i < x + num; i++)
		{
			if (itemslot.StackSize <= 0)
			{
				break;
			}
			for (int j = z; j < z + num && itemslot.StackSize > 0; j++)
			{
				BlockPos val4 = new BlockPos(i, y, j, blockSel.Position.dimension);
				BlockEntity blockEntity = ((Entity)byEntity).World.BlockAccessor.GetBlockEntity(val4);
				BlockEntityFarmland val5 = (BlockEntityFarmland)(object)((blockEntity is BlockEntityFarmland) ? blockEntity : null);
				if (val5 == null || !val5.TryPlant(block))
				{
					continue;
				}
				handHandling = (EnumHandHandling)4;
				if (val3 == (AssetLocation)null)
				{
					val3 = new AssetLocation("sounds/block/plant");
				}
				IPlayer obj2 = ((player is IClientPlayer) ? player : null);
				if (obj2 != null)
				{
					((IClientPlayer)obj2).TriggerFpAnimation((EnumHandInteract)2);
				}
				if (player != null)
				{
					IWorldPlayerData worldData = player.WorldData;
					if (((worldData != null) ? new EnumGameMode?(worldData.CurrentGameMode) : null) == (EnumGameMode?)2)
					{
						goto IL_023f;
					}
				}
				itemslot.TakeOut(1);
				itemslot.MarkDirty();
				goto IL_023f;
				IL_023f:
				val2 = val[farming.CultivatedSeedsId];
				if (val2 != null && val2.Tier > 0)
				{
					if (val5.roomness > 0)
					{
						val5.TryGrowCrop(((CollectibleObject)this).api.World.Calendar.TotalHours);
					}
					if (((Entity)byEntity).World.Rand.NextDouble() < (double)val2.SkillDependentFValue(0))
					{
						val5.TryGrowCrop(((CollectibleObject)this).api.World.Calendar.TotalHours);
					}
				}
			}
		}
		if (val3 != (AssetLocation)null)
		{
			((Entity)byEntity).World.PlaySoundAt(val3, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, (IPlayer)null, true, 32f, 1f);
		}
	}

	public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		WorldInteraction[] heldInteractionHelp = ((ItemPlantableSeed)this).GetHeldInteractionHelp(inSlot);
		InventoryBase inventory = inSlot.Inventory;
		InventoryBase obj = ((inventory is InventoryBasePlayer) ? inventory : null);
		IPlayer val = ((obj != null) ? ((InventoryBasePlayer)obj).Player : null);
		XLeveling obj2 = XLeveling.Instance(((CollectibleObject)this).api);
		if (!(((obj2 != null) ? obj2.GetSkill("farming", false) : null) is Farming farming))
		{
			return heldInteractionHelp;
		}
		object obj3;
		if (val == null)
		{
			obj3 = null;
		}
		else
		{
			EntityPlayer entity = val.Entity;
			if (entity == null)
			{
				obj3 = null;
			}
			else
			{
				PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
				obj3 = ((behavior != null) ? behavior[((Skill)farming).Id][farming.ExtensiveFarmingId] : null);
			}
		}
		if (obj3 == null || ((PlayerAbility)obj3).Tier <= 0)
		{
			return heldInteractionHelp;
		}
		return ArrayExtensions.Append<WorldInteraction>((WorldInteraction[])(object)new WorldInteraction[1]
		{
			new WorldInteraction
			{
				ActionLangCode = "blockhelp-selecttoolmode",
				HotKeyCode = "toolmodeselect",
				MouseButton = (EnumMouseButton)255
			}
		}, heldInteractionHelp);
	}
}
