using System;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace XSkills;

public class QualityUtil
{
	public static float GetQuality(ItemSlot slot)
	{
		if (slot == null)
		{
			return 0f;
		}
		return GetQuality(slot.Itemstack);
	}

	public static float GetQuality(ItemStack stack)
	{
		if (stack == null)
		{
			return 0f;
		}
		return stack.Attributes.GetFloat("quality", 0f);
	}

	public static float GetQuality(IWorldAccessor world, BlockPos pos)
	{
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
		BlockEntityCookedContainer val = (BlockEntityCookedContainer)(object)((blockEntity is BlockEntityCookedContainer) ? blockEntity : null);
		if (val == null)
		{
			return 0f;
		}
		ItemSlot obj = ((BlockEntityContainer)val).Inventory[0];
		return GetQuality((obj != null) ? obj.Itemstack : null);
	}

	public static void AddQualityString(ItemSlot slot, StringBuilder dsc)
	{
		AddQualityString(GetQuality(slot), dsc);
	}

	public static void AddQualityString(float quality, StringBuilder dsc)
	{
		if (quality > 0f)
		{
			string value = QualityString(quality);
			dsc.AppendLine(value);
		}
	}

	public static void PickQuality(ItemStack stack, IWorldAccessor world, BlockPos pos)
	{
		float quality = GetQuality(world, pos);
		if (!(quality <= 0f))
		{
			stack.Attributes.SetFloat("quality", quality);
		}
	}

	public static string QualityString(float quality, bool formatted = true)
	{
		if (quality > 0f)
		{
			if (formatted)
			{
				if (quality < 1f)
				{
					return string.Format("<font color=\"gray\">" + Lang.Get("xskills:quality-bad", Array.Empty<object>()) + "({0:N2})</font>", quality);
				}
				if (quality < 2f)
				{
					return string.Format("<font color=\"white\">" + Lang.Get("xskills:quality-common", Array.Empty<object>()) + "({0:N2})</font>", quality);
				}
				if (quality < 4f)
				{
					return string.Format("<font color=\"green\">" + Lang.Get("xskills:quality-uncommon", Array.Empty<object>()) + "({0:N2})</font>", quality);
				}
				if (quality < 6f)
				{
					return string.Format("<font color=\"blue\">" + Lang.Get("xskills:quality-rare", Array.Empty<object>()) + "({0:N2})</font>", quality);
				}
				if (quality < 8f)
				{
					return string.Format("<font color=\"orange\">" + Lang.Get("xskills:quality-epic", Array.Empty<object>()) + "({0:N2})</font>", quality);
				}
				return string.Format("<font color=\"red\">" + Lang.Get("xskills:quality-legendary", Array.Empty<object>()) + "({0:N2})</font>", quality);
			}
			if (quality < 1f)
			{
				return string.Format(Lang.Get("xskills:quality-bad", Array.Empty<object>()) + "({0:N2})", quality);
			}
			if (quality < 2f)
			{
				return string.Format(Lang.Get("xskills:quality-common", Array.Empty<object>()) + "({0:N2})", quality);
			}
			if (quality < 4f)
			{
				return string.Format(Lang.Get("xskills:quality-uncommon", Array.Empty<object>()) + "({0:N2})", quality);
			}
			if (quality < 6f)
			{
				return string.Format(Lang.Get("xskills:quality-rare", Array.Empty<object>()) + "({0:N2})", quality);
			}
			if (quality < 8f)
			{
				return string.Format(Lang.Get("xskills:quality-epic", Array.Empty<object>()) + "({0:N2})", quality);
			}
			return string.Format(Lang.Get("xskills:quality-legendary", Array.Empty<object>()) + "({0:N2})", quality);
		}
		return "";
	}
}
