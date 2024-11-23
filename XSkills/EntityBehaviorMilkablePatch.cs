using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(EntityBehaviorMilkable))]
public class EntityBehaviorMilkablePatch
{
	[HarmonyPatch("MilkingComplete")]
	public static bool Prefix(EntityBehaviorMilkable __instance, out bool __state, EntityAgent byEntity)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		__state = true;
		if ((int)((EntityBehavior)__instance).entity.World.Side == 2)
		{
			return __state;
		}
		if (!(XLeveling.Instance(((EntityBehavior)__instance).entity.Api).GetSkill("husbandry", false) is Husbandry husbandry))
		{
			return __state;
		}
		PlayerSkillSet behavior = ((Entity)byEntity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val = ((behavior != null) ? behavior[((Skill)husbandry).Id] : null);
		if (val == null)
		{
			return __state;
		}
		PlayerAbility val2 = val[husbandry.CheesyCheeseId];
		if (val2 == null)
		{
			return __state;
		}
		val.AddExperience(0.5f, true);
		if ((double)(float)((double)val2.FValue(0, 0f) + (double)val2.FValue(1, 0f) * (((EntityBehavior)__instance).entity.World.Calendar.TotalHours - (double)((TreeAttribute)((EntityBehavior)__instance).entity.WatchedAttributes).GetFloat("lastMilkedTotalHours", 0f)) / (double)((EntityBehavior)__instance).entity.World.Calendar.HoursPerDay) > ((EntityBehavior)__instance).entity.World.Rand.NextDouble())
		{
			typeof(EntityBehaviorMilkable).GetField("lastMilkedTotalHours", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, ((EntityBehavior)__instance).entity.World.Calendar.TotalHours);
			((TreeAttribute)((EntityBehavior)__instance).entity.WatchedAttributes).SetFloat("lastMilkedTotalHours", (float)((EntityBehavior)__instance).entity.World.Calendar.TotalHours);
			ItemStack val3 = new ItemStack(((Entity)byEntity).World.GetItem(new AssetLocation("game", "cheese-cheddar-4slice")), 1);
			if (!((Entity)byEntity).TryGiveItemStack(val3))
			{
				((Entity)byEntity).World.SpawnItemEntity(val3, ((Entity)byEntity).Pos.XYZ.Add(0.0, 0.5, 0.0), (Vec3d)null);
			}
			__state = false;
			return __state;
		}
		return __state;
	}

	[HarmonyPatch("MilkingComplete")]
	public static void Postfix(EntityBehaviorMilkable __instance, bool __state, ItemSlot slot, EntityAgent byEntity)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		if (!__state || (int)((EntityBehavior)__instance).entity.World.Side != 1 || !(XLeveling.Instance(((EntityBehavior)__instance).entity.Api).GetSkill("husbandry", false) is Husbandry husbandry))
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)byEntity).GetBehavior<PlayerSkillSet>();
		object obj;
		if (behavior == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkill obj2 = behavior[((Skill)husbandry).Id];
			obj = ((obj2 != null) ? obj2[husbandry.RancherId] : null);
		}
		PlayerAbility val = (PlayerAbility)obj;
		if (val == null)
		{
			return;
		}
		if (((EntityBehavior)__instance).entity.World.Rand.NextDouble() < (double)val.FValue(1, 0f))
		{
			int num = ((TreeAttribute)((EntityBehavior)__instance).entity.WatchedAttributes).GetInt("generation", 0) + 1;
			if (num < val.Value(2, 0))
			{
				((TreeAttribute)((EntityBehavior)__instance).entity.WatchedAttributes).SetInt("generation", num);
				((EntityBehavior)__instance).entity.WatchedAttributes.MarkPathDirty("generation");
			}
		}
		ItemStack val2 = new ItemStack(((Entity)byEntity).World.GetItem(new AssetLocation("milkportion")), 1);
		float num2 = 1000f * val.FValue(0, 0f);
		val2.StackSize = (int)num2 + (((double)(num2 - (float)(int)num2) > ((Entity)byEntity).World.Rand.NextDouble()) ? 1 : 0);
		if (val2.StackSize < 1 || TryFillLiquidContainer(slot, val2, byEntity))
		{
			return;
		}
		EntityAgent obj3 = ((byEntity is EntityPlayer) ? byEntity : null);
		object obj4;
		if (obj3 == null)
		{
			obj4 = null;
		}
		else
		{
			IPlayer player = ((EntityPlayer)obj3).Player;
			if (player == null)
			{
				obj4 = null;
			}
			else
			{
				IPlayerInventoryManager inventoryManager = player.InventoryManager;
				obj4 = ((inventoryManager != null) ? inventoryManager.GetHotbarInventory() : null);
			}
		}
		IInventory val3 = (IInventory)obj4;
		if (val3 == null)
		{
			return;
		}
		foreach (ItemSlot item in (IEnumerable<ItemSlot>)val3)
		{
			if (TryFillLiquidContainer(item, val2, byEntity))
			{
				return;
			}
		}
		EntityAgent obj5 = ((byEntity is EntityPlayer) ? byEntity : null);
		object obj6;
		if (obj5 == null)
		{
			obj6 = null;
		}
		else
		{
			IPlayer player2 = ((EntityPlayer)obj5).Player;
			if (player2 == null)
			{
				obj6 = null;
			}
			else
			{
				IPlayerInventoryManager inventoryManager2 = player2.InventoryManager;
				obj6 = ((inventoryManager2 != null) ? inventoryManager2.GetOwnInventory("backpack") : null);
			}
		}
		val3 = (IInventory)obj6;
		if (val3 == null)
		{
			return;
		}
		using IEnumerator<ItemSlot> enumerator = ((IEnumerable<ItemSlot>)val3).GetEnumerator();
		while (enumerator.MoveNext() && !TryFillLiquidContainer(enumerator.Current, val2, byEntity))
		{
		}
	}

	public static bool TryFillLiquidContainer(ItemSlot slot, ItemStack contentStack, EntityAgent byEntity)
	{
		ItemStack itemstack = slot.Itemstack;
		CollectibleObject obj = ((itemstack != null) ? itemstack.Collectible : null);
		BlockLiquidContainerBase val = (BlockLiquidContainerBase)(object)((obj is BlockLiquidContainerBase) ? obj : null);
		if (val == null)
		{
			return false;
		}
		if (slot.Itemstack.StackSize == 1)
		{
			contentStack.StackSize -= val.TryPutLiquid(slot.Itemstack, contentStack, 10f);
			slot.MarkDirty();
			if (contentStack.StackSize <= 0)
			{
				return true;
			}
		}
		else
		{
			ItemStack val2 = slot.TakeOut(1);
			slot.MarkDirty();
			contentStack.StackSize -= val.TryPutLiquid(val2, contentStack, 10f);
			if (!((Entity)byEntity).TryGiveItemStack(val2))
			{
				((Entity)byEntity).World.SpawnItemEntity(val2, ((Entity)byEntity).Pos.XYZ.Add(0.0, 0.5, 0.0), (Vec3d)null);
			}
			if (contentStack.StackSize <= 0)
			{
				return true;
			}
		}
		return false;
	}
}
