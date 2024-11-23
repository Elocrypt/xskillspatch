using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace XSkills;

public static class BlockEntityTroughExtension
{
	public static IPlayer GetOwner(this BlockEntityTrough trough)
	{
		ItemStack itemstack = ((BlockEntityContainer)trough).Inventory[0].Itemstack;
		object obj;
		if (itemstack == null)
		{
			obj = null;
		}
		else
		{
			ITreeAttribute attributes = itemstack.Attributes;
			obj = ((attributes != null) ? attributes.GetString("owner", (string)null) : null);
		}
		string text = (string)obj;
		if (text == null)
		{
			return null;
		}
		return ((BlockEntity)trough).Api.World.PlayerByUid(text);
	}
}
