using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class ItemHammerPatch : ItemHammer
{
	private static void Callback(EntityAgent byEntity)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		if ((int)byEntity.Controls.HandUse != 2)
		{
			return;
		}
		EntityAgent obj = byEntity;
		EntityAgent obj2 = ((obj is EntityPlayer) ? obj : null);
		IPlayer val = ((obj2 != null) ? ((EntityPlayer)obj2).Player : null);
		if (val != null)
		{
			((Entity)val.Entity).World.PlaySoundAt(new AssetLocation("sounds/effect/anvilhit"), val, val, true, 32f, 1f);
			((Entity)byEntity).World.RegisterCallback((Action<float>)delegate
			{
				Callback(byEntity);
			}, 628);
		}
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		((CollectibleObject)this).OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
		if ((int)handling == 1 || (int)handling == 4)
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)byEntity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val = ((behavior != null) ? behavior.FindSkill("metalworking", false) : null);
		if (val == null || !(val.Skill is Metalworking metalworking) || val[metalworking.SalvagerId].Tier <= 0)
		{
			return;
		}
		EntityBehaviorDisassemblable entityBehaviorDisassemblable = ((entitySel != null) ? entitySel.Entity.GetBehavior<EntityBehaviorDisassemblable>() : null);
		if (byEntity.Controls.Sneak && entitySel != null && entityBehaviorDisassemblable != null && ((EntityBehaviorHarvestable)entityBehaviorDisassemblable).Harvestable)
		{
			((Entity)byEntity).World.RegisterCallback((Action<float>)delegate
			{
				Callback(byEntity);
			}, 464);
			handling = (EnumHandHandling)4;
		}
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		bool flag = ((CollectibleObject)this).OnHeldInteractStep(secondsUsed, slot, byEntity, blockSel, entitySel);
		if (flag)
		{
			return flag;
		}
		EntityBehaviorDisassemblable entityBehaviorDisassemblable = ((entitySel != null) ? entitySel.Entity.GetBehavior<EntityBehaviorDisassemblable>() : null);
		if (entitySel != null && entityBehaviorDisassemblable != null && ((EntityBehaviorHarvestable)entityBehaviorDisassemblable).Harvestable)
		{
			if ((int)((Entity)byEntity).World.Side == 2)
			{
				ModelTransform val = new ModelTransform();
				val.EnsureDefaultValues();
				((ModelTransformNoDefaults)val).Rotation.X = 270f;
				((ModelTransformNoDefaults)val).Rotation.Y = (float)Math.Abs(Math.Sin(Math.Max(0f, secondsUsed * 5f - 0.25f)) * 110.0);
				byEntity.Controls.UsingHeldItemTransformBefore = val;
			}
			return secondsUsed < ((EntityBehaviorHarvestable)entityBehaviorDisassemblable).GetHarvestDuration((Entity)(object)byEntity) + 0.15f;
		}
		return false;
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		((CollectibleObject)this).OnHeldInteractStop(secondsUsed, slot, byEntity, blockSel, entitySel);
		EntityBehaviorDisassemblable entityBehaviorDisassemblable = ((entitySel != null) ? entitySel.Entity.GetBehavior<EntityBehaviorDisassemblable>() : null);
		if (entityBehaviorDisassemblable == null || !((EntityBehaviorHarvestable)entityBehaviorDisassemblable).Harvestable || !(secondsUsed >= ((EntityBehaviorHarvestable)entityBehaviorDisassemblable).GetHarvestDuration((Entity)(object)byEntity) - 0.1f))
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)byEntity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val = ((behavior != null) ? behavior.FindSkill("metalworking", false) : null);
		if (val == null || !(val.Skill is Metalworking metalworking))
		{
			return;
		}
		PlayerAbility val2 = val[metalworking.SalvagerId];
		if (val2.Tier <= 0)
		{
			return;
		}
		EntityAgent obj = ((byEntity is EntityPlayer) ? byEntity : null);
		((EntityBehaviorHarvestable)entityBehaviorDisassemblable).SetHarvested((obj != null) ? ((EntityPlayer)obj).Player : null, val2.SkillDependentFValue(0));
		if (slot != null)
		{
			ItemStack itemstack = slot.Itemstack;
			if (itemstack != null)
			{
				itemstack.Collectible.DamageItem(((Entity)byEntity).World, (Entity)(object)byEntity, slot, 3);
			}
		}
	}
}
