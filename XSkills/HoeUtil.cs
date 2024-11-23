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

internal class HoeUtil
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static DrawSkillIconDelegate _003C0_003E__Drawcreate1_svg;

		public static DrawSkillIconDelegate _003C1_003E__Drawcreate4_svg;
	}

	public static void RegisterItemHoePrimitive(ClassRegistry registry)
	{
		registry.ItemClassToTypeMapping["ItemHoeExtended"] = typeof(XSkillsItemHoePrimitive);
	}

	public static void RegisterItemHoe(ClassRegistry registry)
	{
		registry.ItemClassToTypeMapping["ItemHoe"] = typeof(XSkillsItemHoe);
	}

	public static void OnLoaded(ICoreAPI api, ref SkillItem[] toolModes)
	{
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

	public static void OnUnloaded(SkillItem[] toolModes)
	{
		if (toolModes == null)
		{
			return;
		}
		foreach (SkillItem obj in toolModes)
		{
			if (obj != null)
			{
				obj.Dispose();
			}
		}
	}

	public static int AbilityTier(ICoreAPI api, IPlayer player)
	{
		XLeveling obj = XLeveling.Instance(api);
		if (!(((obj != null) ? obj.GetSkill("farming", false) : null) is Farming farming))
		{
			return 0;
		}
		object obj2;
		if (player == null)
		{
			obj2 = null;
		}
		else
		{
			EntityPlayer entity = player.Entity;
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
		if (obj2 == null)
		{
			return 0;
		}
		return ((PlayerAbility)obj2).Tier;
	}

	public static int AbilityTier(ICoreAPI api, IPlayer player, ref int value)
	{
		XLeveling obj = XLeveling.Instance(api);
		if (!(((obj != null) ? obj.GetSkill("farming", false) : null) is Farming farming))
		{
			return 0;
		}
		object obj2;
		if (player == null)
		{
			obj2 = null;
		}
		else
		{
			EntityPlayer entity = player.Entity;
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
		value = val.Value(0, 0);
		if (val == null)
		{
			return 0;
		}
		return val.Tier;
	}

	public static SkillItem[] GetToolModes(ICoreAPI api, SkillItem[] toolModes, IClientPlayer forPlayer)
	{
		int num = AbilityTier(api, (IPlayer)(object)forPlayer);
		if (num <= 0)
		{
			return null;
		}
		return ArrayConvert.Copy<SkillItem>((IEnumerable<SkillItem>)toolModes, 0L, (long)(num + 1));
	}

	public static int GetToolMode(ICoreAPI api, ItemSlot slot, IPlayer byPlayer)
	{
		int num = AbilityTier(api, byPlayer);
		return GameMath.Min(new int[2]
		{
			slot.Itemstack.Attributes.GetInt("toolMode", 0),
			num
		});
	}

	public static bool DoTill(ICoreAPI api, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel)
	{
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		if (blockSel == null)
		{
			return false;
		}
		IPlayer player = ((EntityPlayer)((byEntity is EntityPlayer) ? byEntity : null)).Player;
		int value = 0;
		if (AbilityTier(api, player, ref value) <= 0)
		{
			return true;
		}
		if (GetToolMode(api, slot, player) <= 0)
		{
			return true;
		}
		int num = 0;
		int x = blockSel.Position.X;
		int y = blockSel.Position.Y;
		int z = blockSel.Position.Z;
		int num2 = 0;
		int num3 = 0;
		if (value % 2 == 0)
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
		x = x - value / 2 + num2;
		z = z - value / 2 + num3;
		AssetLocation val = null;
		for (int i = x; i < x + value; i++)
		{
			for (int j = z; j < z + value; j++)
			{
				Block block = api.World.BlockAccessor.GetBlock(new BlockPos(i, y + 1, j, blockSel.Position.dimension));
				if (block == null || ((CollectibleObject)block).Id != 0)
				{
					continue;
				}
				block = api.World.BlockAccessor.GetBlock(new BlockPos(i, y, j, blockSel.Position.dimension));
				if (!((RegistryObject)block).Code.Path.StartsWith("soil"))
				{
					continue;
				}
				string text = ((RegistryObject)block).LastCodePart(1);
				Block block2 = ((Entity)byEntity).World.GetBlock(new AssetLocation("farmland-dry-" + text));
				if (block2 != null)
				{
					BlockPos val2 = new BlockPos(i, y, j, blockSel.Position.dimension);
					api.World.BlockAccessor.SetBlock(block2.BlockId, val2);
					num++;
					BlockEntity blockEntity = ((Entity)byEntity).World.BlockAccessor.GetBlockEntity(val2);
					if (blockEntity is BlockEntityFarmland)
					{
						((BlockEntityFarmland)blockEntity).OnCreatedFromSoil(block);
					}
					api.World.BlockAccessor.MarkBlockDirty(val2, (IPlayer)null);
					if (player != null && block.Sounds != null && val == (AssetLocation)null)
					{
						val = block.Sounds.Place;
					}
				}
			}
		}
		num = (int)((float)num * 0.5f + 0.6f);
		if (num > 0)
		{
			slot.Itemstack.Collectible.DamageItem(((Entity)byEntity).World, (Entity)(object)byEntity, player.InventoryManager.ActiveHotbarSlot, num);
		}
		if (slot.Empty)
		{
			((Entity)byEntity).World.PlaySoundAt(new AssetLocation("sounds/effect/toolbreak"), ((Entity)byEntity).Pos.X, ((Entity)byEntity).Pos.Y, ((Entity)byEntity).Pos.Z, (IPlayer)null, true, 32f, 1f);
		}
		if (val != (AssetLocation)null)
		{
			((Entity)byEntity).World.PlaySoundAt(val, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, (IPlayer)null, true, 32f, 1f);
		}
		return false;
	}

	public static WorldInteraction[] GetHeldInteractionHelp(ICoreAPI api, ItemSlot inSlot)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		InventoryBase inventory = inSlot.Inventory;
		InventoryBase obj = ((inventory is InventoryBasePlayer) ? inventory : null);
		IPlayer val = ((obj != null) ? ((InventoryBasePlayer)obj).Player : null);
		if (val == null)
		{
			return Array.Empty<WorldInteraction>();
		}
		if (AbilityTier(api, val) <= 0)
		{
			return Array.Empty<WorldInteraction>();
		}
		return (WorldInteraction[])(object)new WorldInteraction[1]
		{
			new WorldInteraction
			{
				ActionLangCode = "blockhelp-selecttoolmode",
				HotKeyCode = "toolmodeselect",
				MouseButton = (EnumMouseButton)255
			}
		};
	}
}
