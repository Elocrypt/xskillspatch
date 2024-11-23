using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class ItemSlotCooking : ItemSlotWatertight
{
	public override int MaxSlotStackSize
	{
		get
		{
			ICoreAPI val = ((ItemSlot)this).Inventory?.Api;
			if (val == null)
			{
				return ((ItemSlot)this).MaxSlotStackSize;
			}
			IWorldAccessor world = val.World;
			object obj;
			if (world == null)
			{
				obj = null;
			}
			else
			{
				IBlockAccessor blockAccessor = world.BlockAccessor;
				obj = ((blockAccessor != null) ? blockAccessor.GetBlockEntity(((ItemSlot)this).Inventory.Pos) : null);
			}
			BlockEntity val2 = (BlockEntity)obj;
			if (val2 == null)
			{
				return ((ItemSlot)this).MaxSlotStackSize;
			}
			BlockEntityBehaviorOwnable obj2 = ((val2 != null) ? val2.GetBehavior<BlockEntityBehaviorOwnable>() : null);
			object obj3;
			if (obj2 == null)
			{
				obj3 = null;
			}
			else
			{
				IPlayer owner = obj2.Owner;
				obj3 = ((owner != null) ? owner.Entity : null);
			}
			EntityPlayer val3 = (EntityPlayer)obj3;
			if (val3 == null)
			{
				return ((ItemSlot)this).MaxSlotStackSize;
			}
			XLeveling modSystem = val.ModLoader.GetModSystem<XLeveling>(true);
			if (!(((modSystem != null) ? modSystem.GetSkill("cooking", false) : null) is Cooking cooking))
			{
				return ((ItemSlot)this).MaxSlotStackSize;
			}
			PlayerSkillSet behavior = ((Entity)val3).GetBehavior<PlayerSkillSet>();
			object obj4;
			if (behavior == null)
			{
				obj4 = null;
			}
			else
			{
				PlayerSkill obj5 = behavior[((Skill)cooking).Id];
				obj4 = ((obj5 != null) ? obj5[cooking.CanteenCookId] : null);
			}
			PlayerAbility val4 = (PlayerAbility)obj4;
			if (val4 == null)
			{
				return ((ItemSlot)this).MaxSlotStackSize;
			}
			return (int)((float)((ItemSlot)this).MaxSlotStackSize * (1f + val4.FValue(0, 0f)));
		}
	}

	public ItemSlotCooking(InventoryBase inventory)
		: base(inventory, 6f)
	{
	}

	public override void ActivateSlot(ItemSlot sourceSlot, ref ItemStackMoveOperation op)
	{
		InventoryBase inventory = ((ItemSlot)this).Inventory;
		BlockEntity obj = ((inventory != null) ? inventory.Api.World.BlockAccessor.GetBlockEntity(((ItemSlot)this).inventory.Pos) : null);
		BlockEntityBehaviorOwnable blockEntityBehaviorOwnable = ((obj != null) ? obj.GetBehavior<BlockEntityBehaviorOwnable>() : null);
		if (op.ActingPlayer != null && blockEntityBehaviorOwnable != null)
		{
			blockEntityBehaviorOwnable.Owner = op.ActingPlayer;
		}
		base.capacityLitres = ((ItemSlot)this).MaxSlotStackSize;
		((ItemSlot)this).ActivateSlot(sourceSlot, ref op);
	}
}
