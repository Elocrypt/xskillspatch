using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using Vintagestory.ServerMods;
using XLib.XEffects;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsPlayerBehavior : EntityBehavior
{
	private readonly Survival survival;

	private readonly Combat combat;

	private readonly Husbandry husbandry;

	private readonly TemporalAdaptation adaptation;

	private double oldStability;

	private float oldHealth;

	private float oldOxygen;

	private float timeSinceUpdate;

	internal Action<int> NudistSlotNotified;

	internal float HoursSlept { get; set; }

	protected EntityBehaviorTemporalStabilityAffected TemporalAffected => base.entity.GetBehavior<EntityBehaviorTemporalStabilityAffected>();

	protected EntityBehaviorHealth Health => base.entity.GetBehavior<EntityBehaviorHealth>();

	public override string PropertyName()
	{
		return "XSkillsPlayer";
	}

	public XSkillsPlayerBehavior(Entity entity)
		: base(entity)
	{
		XLeveling obj = XLeveling.Instance(entity.Api);
		survival = ((obj != null) ? obj.GetSkill("survival", false) : null) as Survival;
		XLeveling obj2 = XLeveling.Instance(entity.Api);
		combat = ((obj2 != null) ? obj2.GetSkill("combat", false) : null) as Combat;
		XLeveling obj3 = XLeveling.Instance(entity.Api);
		husbandry = ((obj3 != null) ? obj3.GetSkill("husbandry", false) : null) as Husbandry;
		XLeveling obj4 = XLeveling.Instance(entity.Api);
		adaptation = ((obj4 != null) ? obj4.GetSkill("temporaladaptation", false) : null) as TemporalAdaptation;
		ITreeAttribute treeAttribute = ((TreeAttribute)entity.WatchedAttributes).GetTreeAttribute("health");
		ITreeAttribute treeAttribute2 = ((TreeAttribute)entity.WatchedAttributes).GetTreeAttribute("oxygen");
		EntityBehaviorTemporalStabilityAffected temporalAffected = TemporalAffected;
		oldStability = ((temporalAffected != null) ? temporalAffected.OwnStability : 1.0);
		oldHealth = ((treeAttribute != null) ? treeAttribute.GetFloat("currenthealth", 0f) : 0f);
		oldOxygen = ((treeAttribute2 != null) ? treeAttribute2.GetFloat("currentoxygen", 0f) : 0f);
		HoursSlept = 0f;
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		((EntityBehavior)this).Initialize(properties, attributes);
		EntityBehavior behavior = base.entity.GetBehavior("health");
		EntityBehaviorHealth val = (EntityBehaviorHealth)(object)((behavior is EntityBehaviorHealth) ? behavior : null);
		if (val != null)
		{
			val.onDamaged += new OnDamagedDelegate(OnDamage);
		}
	}

	protected float OnDamageInternal(float damage, DamageSource dmgSource)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Invalid comparison between Unknown and I4
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Invalid comparison between Unknown and I4
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Invalid comparison between Unknown and I4
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Invalid comparison between Unknown and I4
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Invalid comparison between Unknown and I4
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Invalid comparison between Unknown and I4
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Invalid comparison between Unknown and I4
		if ((int)dmgSource.Source == 1)
		{
			Combat obj = combat;
			CombatSkillConfig obj2 = ((obj != null) ? ((Skill)obj).Config : null) as CombatSkillConfig;
			if (obj2 == null || !obj2.enableAbilitiesInPvP)
			{
				return damage;
			}
		}
		if ((int)dmgSource.Source == 7 && (int)dmgSource.Type == 6)
		{
			AffectedEntityBehavior behavior = base.entity.GetBehavior<AffectedEntityBehavior>();
			XEffectsSystem modSystem = base.entity.Api.ModLoader.GetModSystem<XEffectsSystem>(true);
			if (behavior != null && modSystem != null)
			{
				PlayerSkillSet behavior2 = base.entity.GetBehavior<PlayerSkillSet>();
				object obj3;
				if (behavior2 == null)
				{
					obj3 = null;
				}
				else
				{
					PlayerSkill obj4 = behavior2[((Skill)survival).Id];
					obj3 = ((obj4 != null) ? obj4[survival.HealerId] : null);
				}
				PlayerAbility val = (PlayerAbility)obj3;
				if (val != null && val.Tier > 0)
				{
					Effect obj5 = ((modSystem != null) ? modSystem.CreateEffect("hot") : null);
					HotEffect val2 = (HotEffect)(object)((obj5 is HotEffect) ? obj5 : null);
					if (val2 != null)
					{
						((Effect)val2).Duration = val.Value(1, 0);
						val2.Heal = damage * val.FValue(0, 0f) / ((Effect)val2).Duration * ((Effect)val2).Interval;
						behavior.AddEffect((Effect)(object)val2);
						behavior.MarkDirty();
					}
				}
			}
		}
		if ((int)dmgSource.Source != 1 && (int)dmgSource.Source != 2 && (int)dmgSource.Source != 8 && (int)dmgSource.Source != 9 && (int)dmgSource.Source != 10)
		{
			return damage;
		}
		Entity sourceEntity = dmgSource.SourceEntity;
		if (adaptation != null && (int)dmgSource.Source == 10 && dmgSource.SourceEntity == null && (int)dmgSource.Type == 7)
		{
			PlayerSkillSet behavior3 = base.entity.GetBehavior<PlayerSkillSet>();
			object obj6;
			if (behavior3 == null)
			{
				obj6 = null;
			}
			else
			{
				PlayerSkill obj7 = behavior3[((Skill)adaptation).Id];
				obj6 = ((obj7 != null) ? obj7[adaptation.TimelessId] : null);
			}
			if (((PlayerAbility)obj6).Tier > 0)
			{
				damage = 0f;
			}
		}
		if (husbandry != null && ((sourceEntity != null) ? sourceEntity.GetBehavior<XSkillsAnimalBehavior>() : null) != null)
		{
			PlayerSkillSet behavior4 = base.entity.GetBehavior<PlayerSkillSet>();
			object obj8;
			if (behavior4 == null)
			{
				obj8 = null;
			}
			else
			{
				PlayerSkill obj9 = behavior4[((Skill)husbandry).Id];
				obj8 = ((obj9 != null) ? obj9[husbandry.HunterId] : null);
			}
			PlayerAbility val3 = (PlayerAbility)obj8;
			if (val3 != null)
			{
				damage *= 1f - val3.SkillDependentFValue(0);
			}
		}
		if (adaptation != null && sourceEntity != null)
		{
			PlayerSkillSet behavior5 = base.entity.GetBehavior<PlayerSkillSet>();
			object obj10;
			if (behavior5 == null)
			{
				obj10 = null;
			}
			else
			{
				PlayerSkill obj11 = behavior5[((Skill)adaptation).Id];
				obj10 = ((obj11 != null) ? obj11[adaptation.ShifterId] : null);
			}
			PlayerAbility val4 = (PlayerAbility)obj10;
			if (val4 != null && (double)(val4.FValue(0, 0f) * (1f - ((TreeAttribute)base.entity.WatchedAttributes).GetFloat("temporalStability", 0f))) > base.entity.World.Rand.NextDouble())
			{
				damage = 0f;
			}
		}
		if (survival != null)
		{
			ITreeAttribute treeAttribute = ((TreeAttribute)base.entity.WatchedAttributes).GetTreeAttribute("hunger");
			PlayerSkillSet behavior6 = base.entity.GetBehavior<PlayerSkillSet>();
			object obj12;
			if (behavior6 == null)
			{
				obj12 = null;
			}
			else
			{
				PlayerSkill obj13 = behavior6[((Skill)survival).Id];
				obj12 = ((obj13 != null) ? obj13[survival.MeatShieldId] : null);
			}
			PlayerAbility val5 = (PlayerAbility)obj12;
			if (val5 != null && treeAttribute != null)
			{
				float @float = treeAttribute.GetFloat("currentsaturation", 0f);
				float num = damage * (float)val5.Value(1, 0);
				if (@float > num)
				{
					treeAttribute.SetFloat("currentsaturation", @float - num);
					base.entity.WatchedAttributes.MarkPathDirty("hunger");
					damage *= 1f - val5.FValue(0, 0f);
				}
			}
		}
		return damage;
	}

	public float OnDamage(float damage, DamageSource dmgSource)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		damage = OnDamageInternal(damage, dmgSource);
		if ((int)dmgSource.Type != 6)
		{
			double num = damage;
			ITreeAttribute treeAttribute = ((TreeAttribute)base.entity.WatchedAttributes).GetTreeAttribute("health");
			if (num >= ((treeAttribute != null) ? new double?(treeAttribute.GetDecimal("currenthealth", 0.0)) : null))
			{
				BeforeDeath();
			}
		}
		return damage;
	}

	protected void ApplyAbilitiesOxygen()
	{
		if (base.entity.WatchedAttributes == null || survival == null)
		{
			return;
		}
		PlayerSkillSet behavior = base.entity.GetBehavior<PlayerSkillSet>();
		object obj;
		if (behavior == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkill obj2 = behavior[((Skill)survival).Id];
			obj = ((obj2 != null) ? obj2[survival.DiverId] : null);
		}
		PlayerAbility val = (PlayerAbility)obj;
		if (((val != null) ? val.Tier : 0) >= 1)
		{
			ITreeAttribute treeAttribute = ((TreeAttribute)base.entity.WatchedAttributes).GetTreeAttribute("oxygen");
			TreeAttribute val2 = (TreeAttribute)(object)((treeAttribute is TreeAttribute) ? treeAttribute : null);
			float num = val2.GetFloat("currentoxygen", 0f);
			float @float = val2.GetFloat("maxoxygen", 0f);
			float num2 = oldOxygen - num;
			if (num2 > 0f)
			{
				num2 *= val.FValue(0, 0f);
				num = Math.Min(num + num2, @float);
				val2.SetFloat("currentoxygen", num);
			}
			oldOxygen = num;
		}
	}

	protected void ApplyAbilitiesHealth()
	{
		if (base.entity.WatchedAttributes == null || base.entity.World == null)
		{
			return;
		}
		IGameCalendar calendar = base.entity.World.Calendar;
		EntityBehaviorHealth health = Health;
		float num;
		if (health != null)
		{
			num = health.Health;
		}
		else
		{
			ITreeAttribute treeAttribute = ((TreeAttribute)base.entity.WatchedAttributes).GetTreeAttribute("health");
			num = ((treeAttribute != null) ? treeAttribute.GetFloat("currenthealth", 0f) : 0f);
		}
		if (oldHealth < 0.01f)
		{
			oldHealth = num;
		}
		float num2 = num - oldHealth;
		int lightLevel = base.entity.World.BlockAccessor.GetLightLevel(base.entity.Pos.AsBlockPos, (EnumLightLevelType)4);
		float num3 = 32f;
		if (survival != null)
		{
			PlayerSkillSet behavior = base.entity.GetBehavior<PlayerSkillSet>();
			PlayerSkill val = ((behavior != null) ? behavior[((Skill)survival).Id] : null);
			if (val != null)
			{
				float num4 = calendar.SpeedOfTime * timeSinceUpdate / (calendar.HoursPerDay * 60f * 60f);
				val.AddExperience(num4, true);
				PlayerAbility val2 = val[survival.PhotosynthesisId];
				if (val2 != null && val2.Tier > 0 && num2 > 0f && health != null)
				{
					int lightLevel2 = base.entity.World.BlockAccessor.GetLightLevel(base.entity.Pos.AsBlockPos, (EnumLightLevelType)3);
					if ((double)lightLevel > (double)num3 * 0.25)
					{
						num = (health.Health = num + num2 * val2.FValue(0, 0f) * ((float)lightLevel / num3));
						health.MarkDirty();
					}
					else if ((double)lightLevel2 < (double)num3 * 0.25)
					{
						num = (health.Health = num - num2 * val2.FValue(1, 0f) * (1f - (float)lightLevel2 / (num3 * 0.25f)));
						health.MarkDirty();
					}
				}
			}
		}
		if (combat != null)
		{
			PlayerSkillSet behavior2 = base.entity.GetBehavior<PlayerSkillSet>();
			PlayerSkill val3 = ((behavior2 != null) ? behavior2[((Skill)combat).Id] : null);
			if (val3 != null)
			{
				PlayerAbility val4 = val3[combat.VampireId];
				if (val4 != null && val4.Tier > 0 && num2 > 0f && health != null && (double)lightLevel > (double)num3 * 0.25)
				{
					num = (health.Health = num - num2 * val4.FValue(1, 0f) * ((float)lightLevel / num3));
					health.MarkDirty();
				}
			}
		}
		oldHealth = num;
	}

	protected void ApplyAbilitiesStability()
	{
		if (adaptation == null)
		{
			return;
		}
		EntityBehaviorTemporalStabilityAffected temporalAffected = TemporalAffected;
		PlayerSkillSet behavior = base.entity.GetBehavior<PlayerSkillSet>();
		PlayerSkill val = ((behavior != null) ? behavior[((Skill)adaptation).Id] : null);
		if (val != null)
		{
			double ownStability = temporalAffected.OwnStability;
			double num = oldStability - ownStability;
			if (num != 0.0)
			{
				float num2 = (float)((double)val[adaptation.FastForwardId].FValue(0, 0f) * (1.0 - temporalAffected.OwnStability));
				base.entity.Stats.Set("hungerrate", "ability-ff", num2, false);
				base.entity.Stats.Set("miningSpeedMul", "ability-ff", num2, false);
			}
			if (num > 0.0)
			{
				float num3 = (float)num * 50f;
				float num4 = 1f;
				val.AddExperience(num3, true);
				PlayerAbility val2 = val[adaptation.TemporalStableId];
				num4 *= 1f - val2.SkillDependentFValue(0);
				val2 = val[adaptation.CavemanId];
				num4 *= 1f - val2.SkillDependentFValue(0) * Math.Max(1f - (float)((int)base.entity.Pos.Y / TerraGenConfig.seaLevel), 0f);
				val2 = val[adaptation.TemporalAdaptedId];
				num4 *= 1f - val2.SkillDependentFValue(0) * (1f - (float)ownStability);
				temporalAffected.TempStabChangeVelocity *= temporalAffected.TempStabChangeVelocity * (double)num4;
				temporalAffected.OwnStability += num * (double)(1f - num4);
			}
			else if (num < 0.0)
			{
				PlayerAbility val3 = val.PlayerAbilities[adaptation.TemporalRecoveryId];
				num *= (double)val3.FValue(0, 0f);
				temporalAffected.OwnStability -= num;
			}
			oldStability = ownStability;
		}
	}

	public void ApplyMovementAbilities()
	{
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		if (survival == null || base.entity.World == null)
		{
			return;
		}
		PlayerSkillSet behavior = base.entity.GetBehavior<PlayerSkillSet>();
		object obj;
		if (behavior == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkill obj2 = behavior[((Skill)survival).Id];
			obj = ((obj2 != null) ? obj2[survival.OnTheRoadId] : null);
		}
		PlayerAbility val = (PlayerAbility)obj;
		if (val == null)
		{
			return;
		}
		if (val.Tier > 0)
		{
			EntityPos sidedPos = base.entity.SidedPos;
			if (sidedPos != null)
			{
				int num = (int)(sidedPos.Y - 0.05000000074505806);
				int num2 = (int)(sidedPos.Y + 0.009999999776482582);
				Block block = base.entity.World.BlockAccessor.GetBlock(new BlockPos((int)sidedPos.X, num, (int)sidedPos.Z, sidedPos.Dimension));
				Block block2 = base.entity.World.BlockAccessor.GetBlock(new BlockPos((int)sidedPos.X, num2, (int)sidedPos.Z, sidedPos.Dimension));
				if (block.WalkSpeedMultiplier * ((num == num2) ? 1f : block2.WalkSpeedMultiplier) <= 1f)
				{
					base.entity.Stats.Set("walkspeed", "ability-ontheroads", 0f, false);
				}
				else
				{
					base.entity.Stats.Set("walkspeed", "ability-ontheroads", val.FValue(0, 0f), false);
				}
			}
		}
		else
		{
			base.entity.Stats.Remove("walkspeed", "ability-ontheroads");
		}
	}

	public override void OnGameTick(float deltaTime)
	{
		if (base.entity != null)
		{
			timeSinceUpdate += deltaTime;
			if (timeSinceUpdate >= 1f)
			{
				ApplyAbilitiesHealth();
				ApplyAbilitiesStability();
				ApplyAbilitiesOxygen();
				ApplyMovementAbilities();
				timeSinceUpdate = 0f;
			}
		}
	}

	public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
	{
		ITreeAttribute treeAttribute = ((TreeAttribute)base.entity.WatchedAttributes).GetTreeAttribute("health");
		PlayerSkillSet behavior = base.entity.GetBehavior<PlayerSkillSet>();
		PlayerSkill val = ((behavior != null) ? behavior[((Skill)combat).Id] : null);
		Entity entity = base.entity;
		Entity obj = ((entity is EntityPlayer) ? entity : null);
		object obj2;
		if (obj == null)
		{
			obj2 = null;
		}
		else
		{
			IPlayer player = ((EntityPlayer)obj).Player;
			if (player == null)
			{
				obj2 = null;
			}
			else
			{
				IPlayerInventoryManager inventoryManager = player.InventoryManager;
				obj2 = ((inventoryManager != null) ? inventoryManager.GetOwnInventory("xskillshotbar") : null);
			}
		}
		if (obj2 is XSkillsPlayerInventory xSkillsPlayerInventory)
		{
			xSkillsPlayerInventory.Linked = true;
		}
		if (val == null || treeAttribute == null)
		{
			return;
		}
		float @float = treeAttribute.GetFloat("currenthealth", 0f);
		if (damage < 0f)
		{
			return;
		}
		oldHealth -= damage;
		PlayerAbility val2 = val[combat.AdrenalineRushId];
		if (!(@float > 0f) || !(@float / treeAttribute.GetFloat("maxhealth", 0f) <= val2.FValue(0, 0f)))
		{
			return;
		}
		AffectedEntityBehavior behavior2 = base.entity.GetBehavior<AffectedEntityBehavior>();
		if (behavior2 == null || behavior2.IsAffectedBy("adrenalinerush") || behavior2.IsAffectedBy("exhaustion") || behavior2.IsAffectedBy("bloodlust"))
		{
			return;
		}
		XEffectsSystem modSystem = ((Skill)combat).XLeveling.Api.ModLoader.GetModSystem<XEffectsSystem>(true);
		Effect obj3 = ((modSystem != null) ? modSystem.CreateEffect("adrenalinerush") : null);
		Condition val3 = (Condition)(object)((obj3 is Condition) ? obj3 : null);
		if (val3 != null)
		{
			((Effect)val3).Duration = val2.Value(3, 0);
			((Effect)val3).MaxStacks = 1;
			((Effect)val3).Stacks = 1;
			val3.SetIntensity("walkspeed", val2.FValue(1, 0f));
			val3.SetIntensity("receivedDamageMultiplier", 1f - val2.FValue(2, 0f));
			Effect obj4 = val3.Effect("trigger");
			TriggerEffect val4 = (TriggerEffect)(object)((obj4 is TriggerEffect) ? obj4 : null);
			if (val4 != null)
			{
				val4.EffectDuration = val2.Value(4, 0);
				val4.EffectIntensity = -0.2f;
			}
			behavior2.AddEffect((Effect)(object)val3);
			behavior2.MarkDirty();
		}
	}

	protected virtual void BeforeDeath()
	{
		PlayerSkillSet behavior = base.entity.GetBehavior<PlayerSkillSet>();
		if (behavior == null)
		{
			return;
		}
		PlayerSkill val = behavior[((Skill)survival).Id];
		if (val == null)
		{
			return;
		}
		PlayerAbility obj = val[survival.SoulboundBagId];
		if (obj != null && obj.Tier > 0)
		{
			Entity entity = base.entity;
			Entity obj2 = ((entity is EntityPlayer) ? entity : null);
			if (((obj2 != null) ? ((EntityPlayer)obj2).Player.InventoryManager.GetOwnInventory("xskillshotbar") : null) is XSkillsPlayerInventory xSkillsPlayerInventory)
			{
				xSkillsPlayerInventory.Linked = false;
			}
		}
	}

	public override void OnEntityDeath(DamageSource damageSourceForDeath)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		if ((int)base.entity.Api.Side != 1 || survival == null)
		{
			return;
		}
		PlayerSkillSet behavior = base.entity.GetBehavior<PlayerSkillSet>();
		if (behavior == null)
		{
			return;
		}
		PlayerSkill val = behavior[((Skill)survival).Id];
		if (val != null)
		{
			Entity causeEntity = damageSourceForDeath.GetCauseEntity();
			PlayerSkillSet val2 = ((causeEntity != null) ? causeEntity.GetBehavior<PlayerSkillSet>() : null);
			if ((!behavior.Sparring || val2 == null || !val2.Sparring) && !((double)(((Skill)survival).XLeveling.Config.expLossCooldown * ((Skill)survival).XLeveling.Api.World.Calendar.SpeedOfTime * ((Skill)survival).XLeveling.Api.World.Calendar.CalendarSpeedMul / 3600f) + val.PlayerSkillSet.LastDeath >= ((Skill)survival).XLeveling.Api.World.Calendar.TotalHours))
			{
				val.AddExperience((0f - val.Experience) * (((Skill)survival).Config as SurvivalSkillConfig).expLoss, true);
			}
		}
	}
}
