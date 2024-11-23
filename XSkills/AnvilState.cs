using Vintagestory.API.Common;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class AnvilState
{
	public IPlayer usedBy;

	public SmithingRecipe recipe;

	public ItemStack workItemStack;

	public Metalworking metalworking;

	public int splitCount;

	public int hitCount;

	public IAnvilWorkable anvilItemStack;

	public bool wasIronBloom;

	public bool helveHammer;

	public bool wasPlate;

	public AnvilState(BlockEntityAnvil anvil)
	{
		usedBy = anvil.GetUsedByPlayer();
		recipe = anvil.SelectedRecipe;
		workItemStack = anvil.WorkItemStack;
		XLeveling modSystem = ((BlockEntity)anvil).Api.ModLoader.GetModSystem<XLeveling>(true);
		metalworking = ((modSystem != null) ? modSystem.GetSkill("metalworking", false) : null) as Metalworking;
		splitCount = anvil.GetSplitCount();
		hitCount = anvil.GetHitCount();
		ref IAnvilWorkable reference = ref anvilItemStack;
		CollectibleObject collectible = anvil.WorkItemStack.Collectible;
		reference = (IAnvilWorkable)(object)((collectible is IAnvilWorkable) ? collectible : null);
		wasIronBloom = anvil.WorkItemStack.Item is ItemIronBloom;
		helveHammer = anvil.GetHelveHammered();
		wasPlate = anvil.GetWasPlate();
	}
}
