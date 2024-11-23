using Vintagestory.API.Common;

namespace XSkills;

public class InputSlot : ItemSlot
{
	public InputSlot(InventoryBase inventory)
		: base(inventory)
	{
	}

	public override void ActivateSlot(ItemSlot sourceSlot, ref ItemStackMoveOperation op)
	{
		InventoryBase inventory = ((ItemSlot)this).Inventory;
		BlockEntity obj = ((inventory != null) ? inventory.Api.World.BlockAccessor.GetBlockEntity(((ItemSlot)this).Inventory.Pos) : null);
		BlockEntityBehaviorOwnable blockEntityBehaviorOwnable = ((obj != null) ? obj.GetBehavior<BlockEntityBehaviorOwnable>() : null);
		if (op.ActingPlayer != null && blockEntityBehaviorOwnable != null)
		{
			blockEntityBehaviorOwnable.Owner = op.ActingPlayer;
		}
		((ItemSlot)this).ActivateSlot(sourceSlot, ref op);
	}
}
