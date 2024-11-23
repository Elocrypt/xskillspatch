using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsItemPickaxe : Item
{
	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static DrawSkillIconDelegate _003C0_003E__Drawcreate1_svg;
	}

	private SkillItem[] toolModes;

	public override void OnLoaded(ICoreAPI api)
	{
		((CollectibleObject)this).OnLoaded(api);
		ICoreClientAPI capi = (ICoreClientAPI)(object)((api is ICoreClientAPI) ? api : null);
		if (capi == null)
		{
			return;
		}
		toolModes = ObjectCacheUtil.GetOrCreate<SkillItem[]>(api, "pickaxeToolModes", (CreateCachableObjectDelegate<SkillItem[]>)delegate
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
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Expected O, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
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
			array[1] = new SkillItem
			{
				Code = new AssetLocation("3size"),
				Name = Lang.Get("3x3", Array.Empty<object>())
			}.WithIcon(capi, new DrawSkillIconDelegate(new ItemClay().Drawcreate9_svg));
			array[2] = new SkillItem
			{
				Code = new AssetLocation("vein"),
				Name = Lang.Get("vein", Array.Empty<object>())
			}.WithIcon(capi, capi.Gui.LoadSvgWithPadding(new AssetLocation("textures/icons/heatmap.svg"), 48, 48, 5, (int?)(-1)));
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
		if (!(((obj != null) ? obj.GetSkill("mining", false) : null) is Mining mining))
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
				obj2 = ((behavior != null) ? behavior[((Skill)mining).Id] : null);
			}
		}
		PlayerSkill val = (PlayerSkill)obj2;
		if (val == null)
		{
			return null;
		}
		PlayerAbility val2 = val[mining.TunnelDiggerId];
		PlayerAbility val3 = val[mining.VeinMinerId];
		int num = 1 + ((val2 != null && val2.Tier > 0) ? 1 : 0) + ((val3 != null && val3.Tier > 0) ? 1 : 0);
		if (num == 1)
		{
			return null;
		}
		SkillItem[] array = (SkillItem[])(object)new SkillItem[num];
		array[0] = toolModes[0];
		num = 1;
		if (val2.Tier > 0)
		{
			array[num] = toolModes[1];
			num++;
		}
		if (val3.Tier > 0)
		{
			array[num] = toolModes[2];
		}
		return array;
	}

	public override int GetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		XLeveling obj = XLeveling.Instance(((CollectibleObject)this).api);
		if (!(((obj != null) ? obj.GetSkill("mining", false) : null) is Mining mining))
		{
			return 0;
		}
		object obj2;
		if (byPlayer == null)
		{
			obj2 = null;
		}
		else
		{
			EntityPlayer entity = byPlayer.Entity;
			if (entity == null)
			{
				obj2 = null;
			}
			else
			{
				PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
				obj2 = ((behavior != null) ? behavior[((Skill)mining).Id] : null);
			}
		}
		PlayerSkill val = (PlayerSkill)obj2;
		if (val == null)
		{
			return 0;
		}
		PlayerAbility val2 = val[mining.TunnelDiggerId];
		PlayerAbility val3 = val[mining.VeinMinerId];
		int num = 1 + ((val2 != null && val2.Tier > 0) ? 1 : 0) + ((val3 != null && val3.Tier > 0) ? 1 : 0);
		return GameMath.Min(new int[2]
		{
			slot.Itemstack.Attributes.GetInt("toolMode", 0),
			num
		});
	}

	public override void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel, int toolMode)
	{
		slot.Itemstack.Attributes.SetInt("toolMode", toolMode);
	}

	public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		WorldInteraction[] heldInteractionHelp = ((CollectibleObject)this).GetHeldInteractionHelp(inSlot);
		InventoryBase inventory = inSlot.Inventory;
		InventoryBase obj = ((inventory is InventoryBasePlayer) ? inventory : null);
		IPlayer val = ((obj != null) ? ((InventoryBasePlayer)obj).Player : null);
		XLeveling obj2 = XLeveling.Instance(((CollectibleObject)this).api);
		if (!(((obj2 != null) ? obj2.GetSkill("mining", false) : null) is Mining mining))
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
				obj3 = ((behavior != null) ? behavior[((Skill)mining).Id] : null);
			}
		}
		PlayerSkill val2 = (PlayerSkill)obj3;
		if (val2 == null)
		{
			return heldInteractionHelp;
		}
		PlayerAbility val3 = val2[mining.TunnelDiggerId];
		PlayerAbility val4 = val2[mining.VeinMinerId];
		if (val3 != null && val3.Tier == 0 && val4 != null && val4.Tier == 0)
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
