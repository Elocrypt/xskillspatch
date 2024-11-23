using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace XSkills;

public class XSkillsItemHoe : ItemHoe
{
	private SkillItem[] toolModes;

	public override void OnLoaded(ICoreAPI api)
	{
		((ItemHoe)this).OnLoaded(api);
		HoeUtil.OnLoaded(api, ref toolModes);
	}

	public override void OnUnloaded(ICoreAPI api)
	{
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
		if (HoeUtil.DoTill(((CollectibleObject)this).api, slot, byEntity, blockSel))
		{
			((ItemHoe)this).DoTill(secondsUsed, slot, byEntity, blockSel, entitySel);
		}
	}

	public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
	{
		return ArrayExtensions.Append<WorldInteraction>(HoeUtil.GetHeldInteractionHelp(((CollectibleObject)this).api, inSlot), ((ItemHoe)this).GetHeldInteractionHelp(inSlot));
	}
}
