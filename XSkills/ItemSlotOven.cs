using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using XLib.XLeveling;

namespace XSkills;

public class ItemSlotOven : ItemSlotSurvival
{
	public override int MaxSlotStackSize
	{
		get
		{
			if (((ItemSlot)this).Inventory == null)
			{
				return ((ItemSlot)this).MaxSlotStackSize;
			}
			BlockEntity blockEntity = ((ItemSlot)this).Inventory.Api.World.BlockAccessor.GetBlockEntity(((ItemSlot)this).Inventory.Pos);
			if (blockEntity == null)
			{
				return ((ItemSlot)this).MaxSlotStackSize;
			}
			BlockEntityBehaviorOwnable obj = ((blockEntity != null) ? blockEntity.GetBehavior<BlockEntityBehaviorOwnable>() : null);
			object obj2;
			if (obj == null)
			{
				obj2 = null;
			}
			else
			{
				IPlayer owner = obj.Owner;
				obj2 = ((owner != null) ? owner.Entity : null);
			}
			EntityPlayer val = (EntityPlayer)obj2;
			if (val == null)
			{
				return ((ItemSlot)this).MaxSlotStackSize;
			}
			XLeveling modSystem = ((Entity)val).Api.ModLoader.GetModSystem<XLeveling>(true);
			if (!(((modSystem != null) ? modSystem.GetSkill("cooking", false) : null) is Cooking cooking))
			{
				return ((ItemSlot)this).MaxSlotStackSize;
			}
			PlayerSkill obj3 = ((Entity)val).GetBehavior<PlayerSkillSet>()[((Skill)cooking).Id];
			PlayerAbility val2 = ((obj3 != null) ? obj3[cooking.CanteenCookId] : null);
			if (val2 == null)
			{
				return ((ItemSlot)this).MaxSlotStackSize;
			}
			return (int)((float)((ItemSlot)this).MaxSlotStackSize * (1f + val2.FValue(0, 0f)));
		}
	}

	public ItemSlotOven(InventoryBase inventory)
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
