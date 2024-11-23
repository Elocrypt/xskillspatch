using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using XLib.XEffects;
using XLib.XLeveling;

namespace XSkills;

public abstract class CollectingBehavior : DropBonusBehavior
{
	public CollectingBehavior(Block block)
		: base(block)
	{
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref EnumHandling handling)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		if (Skill == null || byPlayer == null)
		{
			return;
		}
		base.OnBlockBroken(world, pos, byPlayer, ref handling);
		PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val = ((behavior != null) ? behavior[((Skill)Skill).Id] : null);
		if (val == null)
		{
			return;
		}
		AffectedEntityBehavior behavior2 = ((Entity)byPlayer.Entity).GetBehavior<AffectedEntityBehavior>();
		ItemStack itemstack = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack;
		Item val2 = ((itemstack != null) ? itemstack.Item : null);
		if (behavior2 == null || val2 == null)
		{
			return;
		}
		PlayerAbility val3 = val[Skill.MiningSpeedId];
		if (val3 == null || val3.Tier <= 0)
		{
			return;
		}
		EnumTool tool;
		if ((EnumTool?)Skill.Tool == ((CollectibleObject)val2).Tool)
		{
			tool = Skill.Tool;
		}
		else
		{
			if (!((RegistryObject)val2).Code.Path.Contains("paxel"))
			{
				return;
			}
			tool = (EnumTool)4;
		}
		float speed = Math.Min((float)val3.Value(0, 0) * 0.01f + (float)(val3.Value(1, 0) * val.Level) * 0.001f, (float)val3.Value(2, 0) * 0.01f);
		XEffectsSystem modSystem = ((Skill)Skill).XLeveling.Api.ModLoader.GetModSystem<XEffectsSystem>(true);
		Effect obj = ((modSystem != null) ? modSystem.CreateEffect("momentum") : null);
		MomentumEffect val4 = (MomentumEffect)(object)((obj is MomentumEffect) ? obj : null);
		if (val4 != null)
		{
			((Effect)val4).Duration = val3.Value(4, 0);
			((Effect)val4).MaxStacks = val3.Value(3, 0);
			val4.Speed = speed;
			val4.Tool = tool;
			behavior2.AddEffect((Effect)(object)val4);
		}
	}
}
