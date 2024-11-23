using System.Text;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockMeal))]
public class BlockMealPatch
{
	[HarmonyPostfix]
	[HarmonyPatch("GetHeldItemInfo")]
	public static void GetHeldItemInfoPostfix(ItemSlot inSlot, StringBuilder dsc)
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
	[HarmonyPatch("OnPickBlock")]
	public static void OnPickBlockPostfix(ItemStack __result, IWorldAccessor world, BlockPos pos)
	{
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
		BlockEntityMeal val = (BlockEntityMeal)(object)((blockEntity is BlockEntityMeal) ? blockEntity : null);
		if (__result != null && val != null)
		{
			ItemSlot obj = ((BlockEntityContainer)val).Inventory[0];
			float? obj2;
			if (obj == null)
			{
				obj2 = null;
			}
			else
			{
				ItemStack itemstack = obj.Itemstack;
				obj2 = ((itemstack != null) ? new float?(itemstack.Attributes.GetFloat("quality", 0f)) : null);
			}
			float? num = obj2;
			float valueOrDefault = num.GetValueOrDefault();
			if (!(valueOrDefault < 0f))
			{
				__result.Attributes.SetFloat("quality", valueOrDefault);
			}
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch("tryFinishEatMeal")]
	internal static void tryFinishEatMealPrefix(BlockMeal __instance, out TryFinishEatMealState __state, ItemSlot slot, EntityAgent byEntity)
	{
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Invalid comparison between Unknown and I4
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Invalid comparison between Unknown and I4
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		__state = new TryFinishEatMealState();
		TryFinishEatMealState obj = __state;
		ItemStack itemstack = slot.Itemstack;
		float? obj2;
		if (itemstack == null)
		{
			obj2 = null;
		}
		else
		{
			ITreeAttribute attributes = itemstack.Attributes;
			obj2 = ((attributes != null) ? new float?(attributes.GetFloat("quality", 0f)) : null);
		}
		float? num = obj2;
		obj.quality = num.GetValueOrDefault();
		__state.quantity = __instance.GetQuantityServings(((Entity)byEntity).World, slot.Itemstack);
		TryFinishEatMealState obj3 = __state;
		float? obj4;
		if (slot == null)
		{
			obj4 = null;
		}
		else
		{
			ItemStack itemstack2 = slot.Itemstack;
			if (itemstack2 == null)
			{
				obj4 = null;
			}
			else
			{
				CollectibleObject collectible = itemstack2.Collectible;
				obj4 = ((collectible != null) ? new float?(collectible.GetTemperature(((Entity)byEntity).World, slot.Itemstack)) : null);
			}
		}
		num = obj4;
		obj3.temperature = num.GetValueOrDefault();
		__state.food0 = (EnumFoodCategory)5;
		__state.food1 = (EnumFoodCategory)5;
		FoodNutritionProperties[] contentNutritionProperties = __instance.GetContentNutritionProperties(((Entity)byEntity).World, slot, byEntity);
		float[] array = new float[5];
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i < contentNutritionProperties.Length; i++)
		{
			if ((int)contentNutritionProperties[i].FoodCategory < 5 && (int)contentNutritionProperties[i].FoodCategory > -1)
			{
				array[contentNutritionProperties[i].FoodCategory] += contentNutritionProperties[i].Satiety;
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] > num2)
			{
				num3 = num2;
				__state.food1 = __state.food0;
				num2 = array[j];
				__state.food0 = (EnumFoodCategory)j;
			}
			else if (array[j] > num3)
			{
				num3 = array[j];
				__state.food1 = (EnumFoodCategory)j;
			}
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch("tryFinishEatMeal")]
	internal static void tryFinishEatMealPostfix(BlockMeal __instance, TryFinishEatMealState __state, ItemSlot slot, EntityAgent byEntity)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		float num = ((slot.Itemstack != null) ? __instance.GetQuantityServings(((Entity)byEntity).World, slot.Itemstack) : 0f);
		float eaten = __state.quantity - num;
		Cooking.ApplyQuality(__state.quality, eaten, __state.temperature, __state.food0, __state.food1, byEntity);
	}

	[HarmonyPostfix]
	[HarmonyPatch("GetContainedInfo")]
	public static void GetContainedInfoPostfix(ref string __result, ItemSlot inSlot)
	{
		float quality = QualityUtil.GetQuality(inSlot);
		if (!(quality <= 0f))
		{
			__result = __result + ", " + QualityUtil.QualityString(quality, formatted: false);
		}
	}
}
