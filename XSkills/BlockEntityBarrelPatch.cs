using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityBarrel))]
public class BlockEntityBarrelPatch
{
	[HarmonyPatch("OnReceivedClientPacket")]
	public static void Postfix(BlockEntityBarrel __instance, IPlayer player, int packetid)
	{
		if (packetid == 7)
		{
			InventoryBase inventory = ((BlockEntityContainer)__instance).Inventory;
			if (inventory != null)
			{
				ItemSlot obj = inventory[1];
				if (obj != null)
				{
					ItemStack itemstack = obj.Itemstack;
					if (itemstack != null)
					{
						itemstack.Attributes.RemoveAttribute("usage");
					}
				}
			}
		}
		if (packetid != 1337 || (!__instance.CurrentRecipe.Code.Contains("soakedhide") && !__instance.CurrentRecipe.Code.Contains("preparedhide") && !__instance.CurrentRecipe.Code.Contains("leather-plain")))
		{
			return;
		}
		XLeveling obj2 = XLeveling.Instance(((BlockEntity)__instance).Api);
		if (!(((obj2 != null) ? obj2.GetSkill("husbandry", false) : null) is Husbandry husbandry))
		{
			return;
		}
		EntityPlayer entity = player.Entity;
		object obj3;
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
				PlayerSkill obj4 = behavior[((Skill)husbandry).Id];
				obj3 = ((obj4 != null) ? obj4[husbandry.TannerId] : null);
			}
		}
		PlayerAbility val = (PlayerAbility)obj3;
		if (val == null)
		{
			return;
		}
		InventoryBase inventory2 = ((BlockEntityContainer)__instance).Inventory;
		if (inventory2 == null)
		{
			return;
		}
		ItemSlot obj5 = inventory2[1];
		if (obj5 != null)
		{
			ItemStack itemstack2 = obj5.Itemstack;
			if (itemstack2 != null)
			{
				itemstack2.Attributes.SetFloat("usage", 1f - val.SkillDependentFValue(0));
			}
		}
	}
}
