using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.Common;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsPlayerInventory : InventoryBasePlayer
{
	protected Cooking cooking;

	private ItemSlot[] slots;

	private ItemSlot[] buffer;

	public static string BackgroundColor { get; set; } = "#BEBEBE";

	public bool Linked { get; internal set; }

	public string SwitchWithName { get; set; }

	public float SwitchCD { get; set; }

	public double LastSwitch { get; set; }

	public override int Count => slots.Length;

	public override ItemSlot this[int slotId]
	{
		get
		{
			if (slotId < 0)
			{
				return null;
			}
			if (slotId >= ((InventoryBase)this).Count)
			{
				if (buffer.Length <= slotId)
				{
					SetBufferSize(slotId + 1);
				}
				return buffer[slotId];
			}
			return slots[slotId];
		}
		set
		{
			if (slotId < ((InventoryBase)this).Count && ((InventoryBase)this).Count >= 0)
			{
				slots[slotId] = value;
			}
		}
	}

	public XSkillsPlayerInventory(string className, string playerUID, ICoreAPI api)
		: base(className, playerUID, api)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		if (className == null)
		{
			((InventoryBase)this).className = "xskillshotbar";
		}
		((InventoryBase)this).baseWeight = 0.8f;
		SwitchWithName = "hotbar";
		slots = (ItemSlot[])(object)new ItemSlot[0];
		buffer = (ItemSlot[])(object)new ItemSlot[0];
		Linked = true;
		LastSwitch = 0.0;
		SwitchCD = 3f;
		for (int i = 0; i < ((InventoryBase)this).Count; i++)
		{
			slots[i] = new ItemSlot((InventoryBase)(object)this);
			slots[i].HexBackgroundColor = BackgroundColor;
		}
	}

	public XSkillsPlayerInventory(string inventoryID, ICoreAPI api)
		: base(inventoryID, api)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		if (((InventoryBase)this).className == null)
		{
			((InventoryBase)this).className = "xskillshotbar";
		}
		SwitchWithName = "hotbar";
		slots = (ItemSlot[])(object)new ItemSlot[0];
		buffer = (ItemSlot[])(object)new ItemSlot[0];
		Linked = true;
		LastSwitch = 0.0;
		SwitchCD = 3f;
		for (int i = 0; i < ((InventoryBase)this).Count; i++)
		{
			slots[i] = new ItemSlot((InventoryBase)(object)this);
			slots[i].HexBackgroundColor = BackgroundColor;
		}
	}

	public override void DidModifyItemSlot(ItemSlot slot, ItemStack extractedStack = null)
	{
		int slotId = ((InventoryBase)this).GetSlotId(slot);
		if (slotId < 0)
		{
			return;
		}
		((InventoryBase)this).MarkSlotDirty(slotId);
		((InventoryBase)this).OnItemSlotModified(slot);
		ItemStack itemstack = slot.Itemstack;
		if (itemstack != null)
		{
			CollectibleObject collectible = itemstack.Collectible;
			if (collectible != null)
			{
				collectible.OnModifiedInInventorySlot(((InventoryBase)this).Api.World, slot, extractedStack);
			}
		}
	}

	public void SetSize(int size)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		ItemSlot[] array = slots;
		slots = (ItemSlot[])(object)new ItemSlot[size];
		for (int i = 0; i < ((InventoryBase)this).Count; i++)
		{
			slots[i] = new ItemSlot((InventoryBase)(object)this);
			slots[i].HexBackgroundColor = BackgroundColor;
			if (i < array.Length)
			{
				slots[i].Itemstack = array[i].Itemstack;
				slots[i].MarkDirty();
			}
		}
		IWorldAccessor world = ((InventoryBase)this).Api.World;
		IPlayer val = ((world != null) ? world.PlayerByUid(base.playerUID) : null);
		if (((val != null) ? val.Entity : null) == null)
		{
			return;
		}
		for (int j = ((InventoryBase)this).Count; j < array.Length; j++)
		{
			ItemSlot val2 = array[j];
			if (val2.Itemstack != null)
			{
				((InventoryBase)this).Api.World.SpawnItemEntity(val2.Itemstack, ((Entity)val.Entity).Pos.XYZ, (Vec3d)null);
				val2.Itemstack = null;
			}
		}
		for (int k = 0; k < buffer.Length; k++)
		{
			if (buffer[k].Itemstack != null)
			{
				if (k >= ((InventoryBase)this).Count || slots[k].Itemstack != null)
				{
					((InventoryBase)this).Api.World.SpawnItemEntity(buffer[k].Itemstack, ((Entity)val.Entity).Pos.XYZ, (Vec3d)null);
					buffer[k].Itemstack = null;
				}
				else
				{
					slots[k].Itemstack = buffer[k].Itemstack;
					slots[k].MarkDirty();
				}
			}
		}
		SetBufferSize(0);
	}

	private void SetBufferSize(int size)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		ItemSlot[] array = buffer;
		buffer = (ItemSlot[])(object)new ItemSlot[size];
		for (int i = 0; i < buffer.Length; i++)
		{
			buffer[i] = new ItemSlot((InventoryBase)(object)this);
			buffer[i].HexBackgroundColor = BackgroundColor;
			if (i < array.Length)
			{
				buffer[i].Itemstack = array[i].Itemstack;
			}
		}
	}

	public override void OnOwningEntityDeath(Vec3d pos)
	{
		XLeveling obj = XLeveling.Instance(((InventoryBase)this).Api);
		if (((obj != null) ? obj.GetSkill("survival", false) : null) is Survival survival)
		{
			IPlayer obj2 = ((InventoryBase)this).Api.World.PlayerByUid(base.playerUID);
			object obj3;
			if (obj2 == null)
			{
				obj3 = null;
			}
			else
			{
				EntityPlayer entity = obj2.Entity;
				if (entity == null)
				{
					obj3 = null;
				}
				else
				{
					PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
					if (behavior == null)
					{
						obj3 = null;
					}
					else
					{
						PlayerSkill obj4 = behavior[((Skill)survival).Id];
						obj3 = ((obj4 != null) ? obj4[survival.SoulboundBagId] : null);
					}
				}
			}
			if (obj3 != null && ((PlayerAbility)obj3).Tier > 0)
			{
				return;
			}
		}
		((InventoryBase)this).OnOwningEntityDeath(pos);
	}

	public void SwitchInventories()
	{
		if (((InventoryBase)this).Api.World == null)
		{
			return;
		}
		double totalHours = ((InventoryBase)this).Api.World.Calendar.TotalHours;
		if (LastSwitch + (double)SwitchCD / 360.0 > totalHours)
		{
			return;
		}
		LastSwitch = totalHours;
		IPlayer val = ((InventoryBase)this).Api.World.PlayerByUid(base.playerUID);
		if (((val != null) ? val.Entity : null) == null)
		{
			return;
		}
		IInventory ownInventory = val.InventoryManager.GetOwnInventory(SwitchWithName);
		if (ownInventory == null)
		{
			return;
		}
		int num = Math.Min(((InventoryBase)this).Count, ((IReadOnlyCollection<ItemSlot>)ownInventory).Count);
		for (int i = 0; i < num; i++)
		{
			ItemSlot val2 = ownInventory[i];
			object obj = ((InventoryBase)this).TryFlipItems(i, val2);
			ICoreAPI api = ((InventoryBase)this).Api;
			ICoreAPI obj2 = ((api is ICoreClientAPI) ? api : null);
			if (obj2 != null)
			{
				((ICoreClientAPI)obj2).Network.SendPacketClient(obj);
			}
			val2.MarkDirty();
			((InventoryBase)this)[i].MarkDirty();
		}
		if (buffer.Length == 0)
		{
			return;
		}
		for (int j = 0; j < buffer.Length; j++)
		{
			ItemSlot val3 = buffer[j];
			if (val3.Itemstack != null)
			{
				((InventoryBase)this).Api.World.SpawnItemEntity(val3.Itemstack, ((Entity)val.Entity).Pos.XYZ, (Vec3d)null);
				val3.Itemstack = null;
			}
		}
		SetBufferSize(0);
	}

	public override float GetSuitability(ItemSlot sourceSlot, ItemSlot targetSlot, bool isMerge)
	{
		float suitability = ((InventoryBase)this).GetSuitability(sourceSlot, targetSlot, isMerge);
		float num = 0f;
		if (sourceSlot.Inventory is InventoryGeneric)
		{
			ItemStack itemstack = sourceSlot.Itemstack;
			if (itemstack == null || !itemstack.Collectible.Tool.HasValue)
			{
				num = 1f;
			}
		}
		return suitability + num + ((sourceSlot is ItemSlotOutput || sourceSlot is ItemSlotCraftingOutput) ? 1f : 0f);
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

	public override void FromTreeAttributes(ITreeAttribute tree)
	{
		int asInt = tree.GetAsInt("qslots", 0);
		SetBufferSize(asInt);
		buffer = ((InventoryBase)this).SlotsFromTreeAttributes(tree, buffer, (List<ItemSlot>)null);
		if (slots == null)
		{
			slots = (ItemSlot[])(object)new ItemSlot[0];
		}
		if (buffer == null)
		{
			buffer = (ItemSlot[])(object)new ItemSlot[0];
		}
		for (int i = 0; i < buffer.Length; i++)
		{
			buffer[i].HexBackgroundColor = BackgroundColor;
		}
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		if (slots.Length == 0 && buffer.Length != 0)
		{
			((InventoryBase)this).SlotsToTreeAttributes(buffer, tree);
		}
		else
		{
			((InventoryBase)this).SlotsToTreeAttributes(slots, tree);
		}
	}
}
