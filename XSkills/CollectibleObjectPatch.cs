using System.Linq;
using System.Text;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(CollectibleObject))]
public class CollectibleObjectPatch
{
	public class TryEatStopState
	{
		public float quality;

		public float temperature;

		public TryEatStopState()
		{
			quality = 0f;
			temperature = 0f;
		}
	}

	[HarmonyPatch("OnBlockBrokenWith")]
	public static void Prefix(ref Block __state, IWorldAccessor world, BlockSelection blockSel)
	{
		__state = world.BlockAccessor.GetBlock(blockSel.Position);
	}

	[HarmonyPatch("OnBlockBrokenWith")]
	public static void Postfix(CollectibleObject __instance, Block __state, IWorldAccessor world, Entity byEntity, ItemSlot itemslot)
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		IPlayer val = null;
		if (byEntity is EntityPlayer)
		{
			Entity obj = ((byEntity is EntityPlayer) ? byEntity : null);
			val = ((obj != null) ? ((EntityPlayer)obj).Player : null);
		}
		ItemStack itemstack = itemslot.Itemstack;
		if (itemstack == null || val == null || __state == null)
		{
			return;
		}
		DropBonusBehavior dropBonusBehavior = null;
		BlockBehavior[] blockBehaviors = __state.BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			dropBonusBehavior = blockBehaviors[i] as DropBonusBehavior;
			if (dropBonusBehavior != null)
			{
				break;
			}
		}
		Item item = itemstack.Item;
		if (item == null || dropBonusBehavior == null || (dropBonusBehavior.Tool != ((CollectibleObject)item).Tool && !((RegistryObject)item).Code.Path.Contains("paxel")) || __instance.DamagedBy == null || !__instance.DamagedBy.Contains((EnumItemDamageSource)0))
		{
			return;
		}
		if (dropBonusBehavior.Skill == null)
		{
			if (dropBonusBehavior is XSkillsCharcoalBehavior xSkillsCharcoalBehavior)
			{
				XLeveling obj2 = XLeveling.Instance(world.Api);
				xSkillsCharcoalBehavior.Forestry = ((obj2 != null) ? obj2.GetSkill("forestry", false) : null) as Forestry;
			}
			if (dropBonusBehavior.Skill == null)
			{
				return;
			}
		}
		PlayerSkillSet behavior = byEntity.GetBehavior<PlayerSkillSet>();
		object obj3;
		if (behavior == null)
		{
			obj3 = null;
		}
		else
		{
			PlayerSkill obj4 = behavior[((Skill)dropBonusBehavior.Skill).Id];
			obj3 = ((obj4 != null) ? obj4[dropBonusBehavior.Skill.DurabilityId] : null);
		}
		PlayerAbility val2 = (PlayerAbility)obj3;
		if (val2 != null && (double)val2.SkillDependentFValue(0) >= world.Rand.NextDouble())
		{
			int @int = itemstack.Attributes.GetInt("durability", __instance.GetMaxDurability(itemstack));
			@int++;
			itemstack.Attributes.SetInt("durability", @int);
			itemslot.MarkDirty();
		}
	}

	[HarmonyPatch("GetHeldItemInfo")]
	public static void Postfix(ItemSlot inSlot, StringBuilder dsc)
	{
		float? obj;
		if (inSlot == null)
		{
			obj = null;
		}
		else
		{
			ItemStack itemstack = inSlot.Itemstack;
			obj = ((itemstack != null) ? itemstack.Attributes.TryGetFloat("quality") : null);
		}
		float? num = obj;
		QualityUtil.AddQualityString(num.GetValueOrDefault(), dsc);
	}

	[HarmonyPostfix]
	[HarmonyPatch("GetMaxDurability")]
	public static void Postfix0(ref int __result, ItemStack itemstack)
	{
		float valueOrDefault = ((itemstack != null) ? itemstack.Attributes.TryGetFloat("quality") : null).GetValueOrDefault();
		if (valueOrDefault > 0f && __result > 1)
		{
			__result = (int)((float)__result * (1f + valueOrDefault * 0.05f));
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch("GetAttackPower")]
	public static void Postfix1(ref float __result, IItemStack withItemStack)
	{
		float valueOrDefault = ((withItemStack != null) ? withItemStack.Attributes.TryGetFloat("quality") : null).GetValueOrDefault();
		if (valueOrDefault > 0f)
		{
			__result *= 1f + valueOrDefault * 0.02f;
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch("GetMiningSpeed")]
	public static void Postfix2(ref float __result, IItemStack itemstack)
	{
		float valueOrDefault = ((itemstack != null) ? itemstack.Attributes.TryGetFloat("quality") : null).GetValueOrDefault();
		if (valueOrDefault > 0f)
		{
			__result *= 1f + valueOrDefault * 0.02f;
		}
	}

	[HarmonyPatch("OnCreatedByCrafting")]
	public static void Postfix(ItemSlot[] allInputslots, ItemSlot outputSlot)
	{
		if (outputSlot.Itemstack == null || outputSlot.Itemstack.Collectible.GetMaxDurability(outputSlot.Itemstack) <= 1)
		{
			return;
		}
		float num = 0f;
		int num2 = 0;
		bool flag = false;
		foreach (ItemSlot val in allInputslots)
		{
			if (val.Itemstack == null)
			{
				continue;
			}
			float? num3 = val.Itemstack.Attributes.TryGetFloat("quality");
			if (outputSlot.Itemstack.Collectible == val.Itemstack.Collectible)
			{
				num += num3.GetValueOrDefault() * 8f;
				num2 += 8;
				continue;
			}
			JsonObject attributes = val.Itemstack.Collectible.Attributes;
			if (attributes != null && attributes.IsTrue("useQuality"))
			{
				flag = true;
				num += num3.GetValueOrDefault();
				num2++;
			}
			else if (num3.HasValue)
			{
				flag = true;
				num += 2f * num3.GetValueOrDefault();
				num2 += 2;
			}
		}
		if (num2 > 0 && flag)
		{
			num /= (float)num2;
			if (num > 0.05f)
			{
				outputSlot.Itemstack.Attributes.SetFloat("quality", num);
			}
		}
	}

	[HarmonyPatch("TryMergeStacks")]
	public static bool Prefix(out ItemStack __state, ItemStackMergeOperation op)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		__state = op.SourceSlot.Itemstack;
		if ((int)((ItemStackMoveOperation)op).CurrentPriority != 0)
		{
			return true;
		}
		JsonObject attributes = op.SourceSlot.Itemstack.Collectible.Attributes;
		if (attributes == null || !attributes.IsTrue("useQuality"))
		{
			return true;
		}
		if (op.SourceSlot.Itemstack.Attributes.GetDecimal("quality", 0.0) == op.SinkSlot.Itemstack.Attributes.GetDecimal("quality", 0.0))
		{
			return true;
		}
		return XSkills.Instance.XLeveling.Config.mergeQualities;
	}

	[HarmonyPatch("TryMergeStacks")]
	public static void Postfix(ItemStack __state, ItemStackMergeOperation op)
	{
		if (((ItemStackMoveOperation)op).MovedQuantity <= 0 || ((__state != null) ? __state.Attributes : null) == null)
		{
			return;
		}
		ItemStack itemstack = op.SinkSlot.Itemstack;
		if (((itemstack != null) ? itemstack.Attributes : null) != null)
		{
			float num = (__state.Attributes.GetFloat("quality", 0f) * (float)((ItemStackMoveOperation)op).MovedQuantity + op.SinkSlot.Itemstack.Attributes.GetFloat("quality", 0f) * (float)(op.SinkSlot.Itemstack.StackSize - ((ItemStackMoveOperation)op).MovedQuantity)) / (float)op.SinkSlot.Itemstack.StackSize;
			if (num > 0f)
			{
				op.SinkSlot.Itemstack.Attributes.SetFloat("quality", num);
			}
		}
	}

	[HarmonyPatch("tryEatStop")]
	public static void Prefix(out TryEatStopState __state, ItemSlot slot, EntityAgent byEntity)
	{
		__state = new TryEatStopState();
		if (byEntity != null && ((slot != null) ? slot.Itemstack : null) != null)
		{
			TryEatStopState obj = __state;
			ITreeAttribute attributes = slot.Itemstack.Attributes;
			obj.quality = ((attributes != null) ? attributes.GetFloat("quality", 0f) : 0f);
			__state.temperature = slot.Itemstack.Collectible.GetTemperature(((Entity)byEntity).World, slot.Itemstack);
		}
	}

	[HarmonyPatch("tryEatStop")]
	public static void Postfix(CollectibleObject __instance, TryEatStopState __state, float secondsUsed, ItemSlot slot, EntityAgent byEntity)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (byEntity != null && slot != null && __state != null)
		{
			FoodNutritionProperties nutritionProperties = __instance.GetNutritionProperties(((Entity)byEntity).World, slot.Itemstack, (Entity)(object)byEntity);
			if (((Entity)byEntity).World is IServerWorldAccessor && nutritionProperties != null && secondsUsed >= 0.95f)
			{
				Cooking.ApplyQuality(__state.quality, 1f, __state.temperature, nutritionProperties.FoodCategory, (EnumFoodCategory)5, byEntity);
			}
		}
	}

	[HarmonyPatch("DoSmelt")]
	public static void Prefix(out DoSmeltState __state, ItemSlot outputSlot)
	{
		__state = new DoSmeltState();
		DoSmeltState obj = __state;
		ItemStack itemstack = outputSlot.Itemstack;
		obj.stackSize = ((itemstack != null) ? itemstack.StackSize : 0);
		DoSmeltState obj2 = __state;
		ItemStack itemstack2 = outputSlot.Itemstack;
		obj2.quality = ((itemstack2 != null) ? itemstack2.Attributes.GetFloat("quality", 0f) : 0f);
	}

	[HarmonyPatch("DoSmelt")]
	public static void Postfix(DoSmeltState __state, IWorldAccessor world, ISlotProvider cookingSlotsProvider, ItemSlot outputSlot)
	{
		InventoryBase val = (InventoryBase)(object)((cookingSlotsProvider is InventoryBase) ? cookingSlotsProvider : null);
		if (val != null)
		{
			BlockEntity obj = ((world != null) ? world.BlockAccessor.GetBlockEntity(val.Pos) : null);
			BlockEntityBehaviorOwnable blockEntityBehaviorOwnable = ((obj != null) ? obj.GetBehavior<BlockEntityBehaviorOwnable>() : null);
			ItemStack itemstack = outputSlot.Itemstack;
			int num = ((itemstack != null) ? itemstack.StackSize : 0) - __state.stackSize;
			if (blockEntityBehaviorOwnable?.Owner != null && num > 0)
			{
				DoSmeltCooking(blockEntityBehaviorOwnable.Owner, outputSlot, num, __state.quality);
			}
		}
	}

	internal static bool DoSmeltCooking(IPlayer byPlayer, ItemSlot outputSlot, int cooked, float quality)
	{
		ItemStack itemstack = outputSlot.Itemstack;
		if (((itemstack != null) ? itemstack.Collectible.NutritionProps : null) == null)
		{
			return false;
		}
		EntityPlayer entity = byPlayer.Entity;
		object obj;
		if (entity == null)
		{
			obj = null;
		}
		else
		{
			XLeveling modSystem = ((Entity)entity).Api.ModLoader.GetModSystem<XLeveling>(true);
			obj = ((modSystem != null) ? modSystem.GetSkill("cooking", false) : null);
		}
		if (!(obj is Cooking cooking))
		{
			return true;
		}
		cooking.ApplyAbilities(outputSlot, byPlayer, quality, cooked);
		return true;
	}
}
