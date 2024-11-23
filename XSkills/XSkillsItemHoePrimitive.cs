using System.Reflection;
using PrimitiveSurvival.ModSystem;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace XSkills;

public class XSkillsItemHoePrimitive : ItemHoe
{
	private SkillItem[] toolModes;

	private ItemHoeExtended dummyHoe;

	public override void OnLoaded(ICoreAPI api)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (dummyHoe == null)
		{
			dummyHoe = new ItemHoeExtended();
		}
		((CollectibleObject)dummyHoe).Attributes = ((CollectibleObject)this).Attributes;
		((RegistryObject)dummyHoe).Class = ((RegistryObject)this).Class;
		((RegistryObject)dummyHoe).Code = ((RegistryObject)this).Code;
		((CollectibleObject)dummyHoe).Durability = ((CollectibleObject)this).Durability;
		typeof(ItemHoeExtended).GetField("api", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dummyHoe, api);
		((CollectibleObject)dummyHoe).OnLoaded(api);
		((ItemHoe)this).OnLoaded(api);
		HoeUtil.OnLoaded(api, ref toolModes);
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		ItemHoeExtended obj = dummyHoe;
		if (obj != null)
		{
			((CollectibleObject)obj).OnUnloaded(api);
		}
		((CollectibleObject)this).OnUnloaded(api);
		HoeUtil.OnUnloaded(toolModes);
	}

	public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
	{
		return HoeUtil.GetToolModes(((CollectibleObject)this).api, toolModes, forPlayer);
	}

	public override int GetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		return HoeUtil.GetToolMode(((CollectibleObject)this).api, slot, byPlayer);
	}

	public override void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel, int toolMode)
	{
		slot.Itemstack.Attributes.SetInt("toolMode", toolMode);
	}

	public override void DoTill(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (!byEntity.Controls.Sneak && HoeUtil.DoTill(((CollectibleObject)this).api, slot, byEntity, blockSel))
		{
			((ItemHoe)this).DoTill(secondsUsed, slot, byEntity, blockSel, entitySel);
		}
	}

	public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
	{
		return ArrayExtensions.Append<WorldInteraction>(HoeUtil.GetHeldInteractionHelp(((CollectibleObject)this).api, inSlot), ((CollectibleObject)dummyHoe).GetHeldInteractionHelp(inSlot));
	}

	public override void OnHeldInteractStart(ItemSlot itemslot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
	{
		((CollectibleObject)dummyHoe).OnHeldInteractStart(itemslot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		((CollectibleObject)dummyHoe).OnHeldInteractStop(secondsUsed, slot, byEntity, blockSel, entitySel);
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		if (blockSel == null)
		{
			return false;
		}
		if (secondsUsed > 0.6f && ((TreeAttribute)((Entity)byEntity).Attributes).GetInt("didtill", 0) == 0 && (int)((Entity)byEntity).World.Side == 1 && !byEntity.Controls.ShiftKey)
		{
			((ItemHoe)this).DoTill(secondsUsed, slot, byEntity, blockSel, entitySel);
			((TreeAttribute)((Entity)byEntity).Attributes).SetInt("didtill", 1);
		}
		((CollectibleObject)dummyHoe).OnHeldInteractStep(secondsUsed, slot, byEntity, blockSel, entitySel);
		return secondsUsed < 1f;
	}

	public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return ((CollectibleObject)dummyHoe).OnHeldInteractCancel(secondsUsed, slot, byEntity, blockSel, entitySel, cancelReason);
	}
}
