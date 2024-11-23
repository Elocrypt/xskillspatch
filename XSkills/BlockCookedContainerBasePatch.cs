using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockCookedContainerBase))]
public class BlockCookedContainerBasePatch
{
	[HarmonyPrefix]
	[HarmonyPatch("ServeIntoBowl")]
	public static void ServeIntoBowlPrefix(out QualityState __state, BlockPos pos, ItemSlot potslot, IWorldAccessor world)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		__state = new QualityState();
		if ((int)world.Side != 2)
		{
			BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
			IBlockEntityMealContainer val = (IBlockEntityMealContainer)(object)((blockEntity is IBlockEntityMealContainer) ? blockEntity : null);
			if (val != null)
			{
				QualityState obj = __state;
				ItemStack itemstack = potslot.Itemstack;
				obj.quality = ((itemstack != null) ? itemstack.Attributes.GetFloat("quality", 0f) : 0f);
				QualityState obj2 = __state;
				ItemStack itemstack2 = val.inventory[0].Itemstack;
				obj2.oldQuality = ((itemstack2 != null) ? itemstack2.Attributes.GetFloat("quality", 0f) : 0f);
				__state.oldQuantity = val.QuantityServings;
			}
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch("ServeIntoBowl")]
	public static void ServeIntoBowlPostfix(QualityState __state, BlockPos pos, IWorldAccessor world)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)world.Side == 2)
		{
			return;
		}
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
		IBlockEntityMealContainer val = (IBlockEntityMealContainer)(object)((blockEntity is IBlockEntityMealContainer) ? blockEntity : null);
		if (val == null)
		{
			return;
		}
		float num = val.QuantityServings - __state.oldQuantity;
		if (num <= 0f)
		{
			return;
		}
		float num2 = (__state.oldQuality * __state.oldQuantity + __state.quality * num) / (__state.oldQuantity + num);
		if (!(num2 <= 0f))
		{
			ItemStack itemstack = val.inventory[0].Itemstack;
			if (itemstack != null)
			{
				itemstack.Attributes.SetFloat("quality", num2);
			}
		}
	}

	[HarmonyPrefix]
	[HarmonyPatch("ServeIntoStack")]
	public static void ServeIntoStackPrefix(out QualityState __state, ItemSlot bowlSlot, ItemSlot potslot, IWorldAccessor world)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		__state = new QualityState();
		if ((int)world.Side != 2 && ((bowlSlot != null) ? bowlSlot.Itemstack : null) != null && ((potslot != null) ? potslot.Itemstack : null) != null)
		{
			__state.quality = potslot.Itemstack.Attributes.GetFloat("quality", 0f);
			__state.oldQuality = bowlSlot.Itemstack.Attributes.GetFloat("quality", 0f);
			QualityState obj = __state;
			CollectibleObject collectible = bowlSlot.Itemstack.Collectible;
			CollectibleObject obj2 = ((collectible is IBlockMealContainer) ? collectible : null);
			obj.oldQuantity = ((obj2 != null) ? ((IBlockMealContainer)obj2).GetQuantityServings(world, bowlSlot.Itemstack) : 0f);
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch("ServeIntoStack")]
	public static void ServeIntoStackPostfix(QualityState __state, ItemSlot bowlSlot, ItemSlot potslot, IWorldAccessor world)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)world.Side == 2 || ((bowlSlot != null) ? bowlSlot.Itemstack : null) == null || ((potslot != null) ? potslot.Itemstack : null) == null)
		{
			return;
		}
		CollectibleObject collectible = bowlSlot.Itemstack.Collectible;
		IBlockMealContainer val = (IBlockMealContainer)(object)((collectible is IBlockMealContainer) ? collectible : null);
		if (val == null)
		{
			return;
		}
		float num = val.GetQuantityServings(world, bowlSlot.Itemstack) - __state.oldQuantity;
		if (!(num <= 0f))
		{
			float num2 = (__state.oldQuality * __state.oldQuantity + __state.quality * num) / (__state.oldQuantity + num);
			if (!(num2 <= 0f))
			{
				bowlSlot.Itemstack.Attributes.SetFloat("quality", num2);
			}
		}
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
