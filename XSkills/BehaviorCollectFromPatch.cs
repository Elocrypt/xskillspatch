using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BehaviorCollectFrom))]
public class BehaviorCollectFromPatch
{
	[HarmonyPatch("OnBlockInteractStart")]
	public static void Prefix(BehaviorCollectFrom __instance, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (!world.Claims.TryAccess(byPlayer, blockSel.Position, (EnumBlockAccessFlags)2) || ((RegistryObject)((BlockBehavior)__instance).block).Code.Path.Contains("empty") || ((BlockBehavior)__instance).block.Drops == null || ((BlockBehavior)__instance).block.Drops.Length <= 1)
		{
			return;
		}
		XLeveling obj = XLeveling.Instance(world.Api);
		if (!(((obj != null) ? obj.GetSkill("husbandry", false) : null) is Husbandry husbandry))
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val = ((behavior != null) ? behavior[((Skill)husbandry).Id] : null);
		PlayerAbility val2 = ((val != null) ? val[husbandry.RancherId] : null);
		if (val2 == null)
		{
			return;
		}
		BlockDropItemStack val3 = ((BlockBehavior)__instance).block.Drops[0];
		val.AddExperience(0.1f * ((BlockBehavior)__instance).block.Drops[0].Quantity.avg, true);
		if (val2.Tier >= 1)
		{
			ItemStack nextItemStack = val3.GetNextItemStack(val2.FValue(0, 0f));
			if (((nextItemStack != null) ? nextItemStack.StackSize : 0) >= 1 && !byPlayer.InventoryManager.TryGiveItemstack(nextItemStack, false))
			{
				world.SpawnItemEntity(val3.GetNextItemStack(1f), blockSel.Position.ToVec3d().Add(0.5, 0.5, 0.5), (Vec3d)null);
			}
		}
	}
}
