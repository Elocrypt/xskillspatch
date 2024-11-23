using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockSmeltedContainer))]
internal class BlockSmeltedContainerPatch
{
	[HarmonyPrefix]
	[HarmonyPatch("OnHeldInteractStep")]
	public static void OnHeldInteractStepPrefix(EntityAgent byEntity, BlockSelection blockSel, ref int __state)
	{
		if (byEntity != null && blockSel != null)
		{
			BlockEntity blockEntity = ((Entity)byEntity).World.BlockAccessor.GetBlockEntity(blockSel.Position);
			BlockEntityToolMold val = (BlockEntityToolMold)(object)((blockEntity is BlockEntityToolMold) ? blockEntity : null);
			BlockEntityIngotMold val2 = (BlockEntityIngotMold)(object)((blockEntity is BlockEntityIngotMold) ? blockEntity : null);
			if (val != null)
			{
				__state = val.fillLevel;
			}
			else if (val2 != null)
			{
				__state = val2.fillLevelLeft + val2.fillLevelRight;
			}
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch("OnHeldInteractStep")]
	public static void OnHeldInteractStepPostfix(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, int __state)
	{
		if (slot == null || byEntity == null || blockSel == null)
		{
			return;
		}
		BlockEntity blockEntity = ((Entity)byEntity).World.BlockAccessor.GetBlockEntity(blockSel.Position);
		BlockEntityToolMold val = (BlockEntityToolMold)(object)((blockEntity is BlockEntityToolMold) ? blockEntity : null);
		BlockEntityIngotMold val2 = (BlockEntityIngotMold)(object)((blockEntity is BlockEntityIngotMold) ? blockEntity : null);
		int num = 0;
		if (val != null)
		{
			num = val.fillLevel - __state;
		}
		else if (val2 != null)
		{
			num = val2.fillLevelLeft + val2.fillLevelRight - __state;
		}
		XLeveling obj = XLeveling.Instance(((Entity)byEntity).Api);
		Metalworking metalworking = ((obj != null) ? obj.GetSkill("metalworking", false) : null) as Metalworking;
		PlayerSkillSet behavior = ((Entity)byEntity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val3 = ((behavior != null) ? behavior.PlayerSkills[((Skill)metalworking).Id] : null);
		if (metalworking == null || val3 == null || num <= 0)
		{
			return;
		}
		if (val2 != null)
		{
			val3.AddExperience((float)num * 0.0075f * 0.25f, true);
			return;
		}
		val3.AddExperience((float)num * 0.0075f, true);
		int num2 = val3.PlayerAbilities[metalworking.SmelterId].Value(0, 0);
		ItemStack[] moldedStacks = val.GetMoldedStacks(val.metalContent);
		for (int i = 0; i < moldedStacks.Length; i++)
		{
			if (moldedStacks[i].Collectible.CombustibleProps != null)
			{
				return;
			}
		}
		if (num2 <= 0)
		{
			return;
		}
		int num3 = 100 / num2;
		if (num3 > 1)
		{
			int num4 = num / (num3 - 1);
			val.fillLevel += num4;
			__state += num4 * num3;
			if ((val.fillLevel + 1) / num3 > (__state + 1) / num3)
			{
				val.fillLevel++;
			}
			if (val.IsFull)
			{
				int num5 = ((CollectibleObject)((BlockEntity)val).Block).Attributes["requiredUnits"].AsInt(100);
				slot.Itemstack.Attributes.SetInt("units", slot.Itemstack.Attributes.GetInt("units", 0) + val.fillLevel % num5);
				val.fillLevel -= val.fillLevel % num5;
			}
			slot.MarkDirty();
			((BlockEntity)val).MarkDirty(false, (IPlayer)null);
		}
	}
}
