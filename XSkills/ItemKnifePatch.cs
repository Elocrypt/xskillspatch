using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(ItemKnife))]
public class ItemKnifePatch
{
	[HarmonyPrefix]
	[HarmonyPatch("OnHeldInteractStart")]
	public static bool OnHeldInteractStartPrefix(EntityAgent byEntity, EntitySelection entitySel)
	{
		if (entitySel == null)
		{
			return true;
		}
		if (entitySel.Entity.HasBehavior("harvestable"))
		{
			return true;
		}
		EntityBehaviorHarvestable behavior = (EntityBehaviorHarvestable)(object)entitySel.Entity.GetBehavior<EntityBehaviorDisassemblable>();
		if (byEntity.Controls.Sneak && behavior != null && behavior.Harvestable)
		{
			return false;
		}
		return true;
	}

	[HarmonyPrefix]
	[HarmonyPatch("OnHeldInteractStop")]
	public static bool OnHeldInteractStopPrefix(EntityAgent byEntity, EntitySelection entitySel)
	{
		if (entitySel == null)
		{
			return true;
		}
		if (entitySel.Entity.HasBehavior("harvestable"))
		{
			return true;
		}
		EntityBehaviorHarvestable behavior = (EntityBehaviorHarvestable)(object)entitySel.Entity.GetBehavior<EntityBehaviorDisassemblable>();
		if (byEntity.Controls.Sneak && behavior != null && behavior.Harvestable)
		{
			return false;
		}
		return true;
	}
}
