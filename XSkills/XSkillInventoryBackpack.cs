using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.Common;
using XLib.XLeveling;

namespace XSkills;

public class XSkillInventoryBackpack : InventoryPlayerBackPacks
{
	protected Cooking cooking;

	public override int Count
	{
		get
		{
			IPlayer player = ((InventoryBasePlayer)this).Player;
			if (((player != null) ? player.InventoryManager.GetOwnInventory("xskillshotbar") : null) is XSkillsPlayerInventory { Linked: not false } xSkillsPlayerInventory)
			{
				return ((InventoryPlayerBackPacks)this).Count + ((InventoryBase)xSkillsPlayerInventory).Count;
			}
			return ((InventoryPlayerBackPacks)this).Count;
		}
	}

	public override ItemSlot this[int slotId]
	{
		get
		{
			int num = base.backPackContents.Count + base.backPackSlots.Length;
			if (slotId >= num)
			{
				int num2 = slotId - num;
				IPlayer player = ((InventoryBasePlayer)this).Player;
				if (((player != null) ? player.InventoryManager.GetOwnInventory("xskillshotbar") : null) is XSkillsPlayerInventory xSkillsPlayerInventory && num2 < ((InventoryBase)xSkillsPlayerInventory).Count && xSkillsPlayerInventory.Linked)
				{
					return ((InventoryBase)xSkillsPlayerInventory)[num2];
				}
			}
			return ((InventoryPlayerBackPacks)this)[slotId];
		}
		set
		{
			int num = base.backPackContents.Count + base.backPackSlots.Length;
			if (slotId >= num)
			{
				int num2 = slotId - num;
				IPlayer player = ((InventoryBasePlayer)this).Player;
				if (((player != null) ? player.InventoryManager.GetOwnInventory("xskillshotbar") : null) is XSkillsPlayerInventory xSkillsPlayerInventory && num2 < ((InventoryBase)xSkillsPlayerInventory).Count && xSkillsPlayerInventory.Linked)
				{
					((InventoryBase)xSkillsPlayerInventory)[num2] = value;
					return;
				}
			}
			((InventoryPlayerBackPacks)this)[slotId] = value;
		}
	}

	public XSkillInventoryBackpack(string inventoryId, ICoreAPI api)
		: base(inventoryId, api)
	{
	}

	public XSkillInventoryBackpack(string className, string playerUID, ICoreAPI api)
		: base(className, playerUID, api)
	{
	}

	public override object ActivateSlot(int slotId, ItemSlot sourceSlot, ref ItemStackMoveOperation op)
	{
		int num = base.backPackContents.Count + base.backPackSlots.Length;
		if (slotId >= num && op.ShiftDown)
		{
			int num2 = slotId - num;
			IPlayer player = ((InventoryBasePlayer)this).Player;
			if (((player != null) ? player.InventoryManager.GetOwnInventory("xskillshotbar") : null) is XSkillsPlayerInventory xSkillsPlayerInventory && num2 < ((InventoryBase)xSkillsPlayerInventory).Count && xSkillsPlayerInventory.Linked)
			{
				return ((InventoryBase)xSkillsPlayerInventory).ActivateSlot(num2, sourceSlot, ref op);
			}
		}
		return ((InventoryPlayerBackPacks)this).ActivateSlot(slotId, sourceSlot, ref op);
	}

	public override float GetTransitionSpeedMul(EnumTransitionType transType, ItemStack stack)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		float transitionSpeedMul = ((InventoryBase)this).GetTransitionSpeedMul(transType, stack);
		if (cooking == null)
		{
			XLeveling obj = XLeveling.Instance(((InventoryBase)this).Api);
			cooking = ((obj != null) ? obj.GetSkill("cooking", false) : null) as Cooking;
			if (cooking == null)
			{
				return transitionSpeedMul;
			}
		}
		EntityPlayer entity = ((InventoryBasePlayer)this).Player.Entity;
		object obj2;
		if (entity == null)
		{
			obj2 = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
			if (behavior == null)
			{
				obj2 = null;
			}
			else
			{
				PlayerSkill obj3 = behavior[((Skill)cooking).Id];
				obj2 = ((obj3 != null) ? obj3[cooking.SaltyBackpackId] : null);
			}
		}
		PlayerAbility val = (PlayerAbility)obj2;
		if (val == null)
		{
			return transitionSpeedMul;
		}
		if ((int)transType != 0)
		{
			return transitionSpeedMul;
		}
		return transitionSpeedMul * val.FValue(0, 1f);
	}
}
