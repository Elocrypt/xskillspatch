using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.Common;
using Vintagestory.GameContent;
using XLib.XEffects;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsEntityBehavior : EntityBehavior
{
	protected Combat combat;

	protected Farming farming;

	protected Husbandry husbandry;

	protected Mining mining;

	protected Digging digging;

	protected Forestry forestry;

	protected Metalworking metalworking;

	protected Cooking cooking;

	protected TemporalAdaptation temporalAdaptation;

	protected float xp;

	public override string PropertyName()
	{
		return "XSkillsEntity";
	}

	public XSkillsEntityBehavior(Entity entity)
		: base(entity)
	{
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		XLeveling obj = XLeveling.Instance(entity.Api);
		combat = ((obj != null) ? obj.GetSkill("combat", false) : null) as Combat;
		XLeveling obj2 = XLeveling.Instance(entity.Api);
		farming = ((obj2 != null) ? obj2.GetSkill("farming", false) : null) as Farming;
		XLeveling obj3 = XLeveling.Instance(entity.Api);
		husbandry = ((obj3 != null) ? obj3.GetSkill("husbandry", false) : null) as Husbandry;
		XLeveling obj4 = XLeveling.Instance(entity.Api);
		mining = ((obj4 != null) ? obj4.GetSkill("mining", false) : null) as Mining;
		XLeveling obj5 = XLeveling.Instance(entity.Api);
		digging = ((obj5 != null) ? obj5.GetSkill("digging", false) : null) as Digging;
		XLeveling obj6 = XLeveling.Instance(entity.Api);
		forestry = ((obj6 != null) ? obj6.GetSkill("forestry", false) : null) as Forestry;
		XLeveling obj7 = XLeveling.Instance(entity.Api);
		metalworking = ((obj7 != null) ? obj7.GetSkill("metalworking", false) : null) as Metalworking;
		XLeveling obj8 = XLeveling.Instance(entity.Api);
		cooking = ((obj8 != null) ? obj8.GetSkill("cooking", false) : null) as Cooking;
		XLeveling obj9 = XLeveling.Instance(entity.Api);
		temporalAdaptation = ((obj9 != null) ? obj9.GetSkill("temporaladaptation", false) : null) as TemporalAdaptation;
		EntityBehavior behavior = base.entity.GetBehavior("health");
		EntityBehaviorHealth val = (EntityBehaviorHealth)(object)((behavior is EntityBehaviorHealth) ? behavior : null);
		if (val != null)
		{
			val.onDamaged += new OnDamagedDelegate(OnDamage);
		}
		xp = 0f;
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		((EntityBehavior)this).Initialize(properties, attributes);
		xp = attributes["xp"].AsFloat(0f);
	}

	public override void OnEntityDeath(DamageSource damageSourceForDeath)
	{
		if (damageSourceForDeath == null)
		{
			return;
		}
		Entity sourceEntity = damageSourceForDeath.SourceEntity;
		EntityPlayer val = (EntityPlayer)(((object)((sourceEntity is EntityPlayer) ? sourceEntity : null)) ?? ((object)(/*isinst with value type is only supported in some contexts*/ ?? /*isinst with value type is only supported in some contexts*/)));
		if (combat == null || val == null)
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)val).GetBehavior<PlayerSkillSet>();
		PlayerSkill val2 = ((behavior != null) ? behavior[((Skill)combat).Id] : null);
		if (val2 == null)
		{
			return;
		}
		val2.AddExperience(xp, true);
		PlayerAbility val3 = val2[combat.FreshFleshId];
		if (val3.Tier > 0 && !((TreeAttribute)base.entity.Attributes).GetBool("isMechanical", false))
		{
			((EntityAgent)val).ReceiveSaturation((float)val3.Value(0, 0), (EnumFoodCategory)2, 10f, 1f);
		}
		val3 = val2[combat.BloodlustId];
		if (val3.Tier <= 0)
		{
			return;
		}
		AffectedEntityBehavior behavior2 = ((Entity)val).GetBehavior<AffectedEntityBehavior>();
		if (behavior2 != null && !behavior2.IsAffectedBy("adrenalineRush") && !behavior2.IsAffectedBy("exhaustion"))
		{
			XEffectsSystem modSystem = ((Skill)combat).XLeveling.Api.ModLoader.GetModSystem<XEffectsSystem>(true);
			Effect obj = ((modSystem != null) ? modSystem.CreateEffect("bloodlust") : null);
			Condition val4 = (Condition)(object)((obj is Condition) ? obj : null);
			if (val4 != null)
			{
				((Effect)val4).Duration = val3.Value(2, 0);
				((Effect)val4).MaxStacks = val3.Value(3, 0);
				((Effect)val4).Stacks = 1;
				val4.SetIntensity("meleeWeaponsDamage", val3.FValue(0, 0f));
				val4.SetIntensity("receivedDamageMultiplier", val3.FValue(1, 0f));
				behavior2.AddEffect((Effect)(object)val4);
				behavior2.MarkDirty();
			}
		}
	}

	public float OnDamage(float damage, DamageSource dmgSource)
	{
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Invalid comparison between Unknown and I4
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected I4, but got Unknown
		Entity sourceEntity = dmgSource.SourceEntity;
		EntityPlayer val = (EntityPlayer)(((object)((sourceEntity is EntityPlayer) ? sourceEntity : null)) ?? ((object)(/*isinst with value type is only supported in some contexts*/ ?? /*isinst with value type is only supported in some contexts*/)));
		if (combat == null || val == null)
		{
			return damage;
		}
		PlayerSkillSet behavior = ((Entity)val).GetBehavior<PlayerSkillSet>();
		PlayerSkill val2 = ((behavior != null) ? behavior[((Skill)combat).Id] : null);
		if (val2 == null)
		{
			return damage;
		}
		PlayerAbility val3 = null;
		int num = 0;
		switch (val.Player.InventoryManager.ActiveTool)
		{
		case 3L:
			val3 = val2[combat.SwordsmanId];
			break;
		case 6L:
			val3 = val2[combat.SpearmanId];
			break;
		case 7L:
		case 14L:
			val3 = val2[combat.ArcherId];
			break;
		case 0L:
		{
			PlayerSkill obj8 = behavior[((Skill)husbandry).Id];
			num = ((obj8 != null) ? obj8.Level : 0);
			break;
		}
		case 1L:
		{
			PlayerSkill obj3 = behavior[((Skill)mining).Id];
			num = ((obj3 != null) ? obj3.Level : 0);
			break;
		}
		case 2L:
		{
			PlayerSkill obj7 = behavior[((Skill)forestry).Id];
			num = ((obj7 != null) ? obj7.Level : 0);
			break;
		}
		case 5L:
		case 11L:
		{
			PlayerSkill obj6 = behavior[((Skill)metalworking).Id];
			num = ((obj6 != null) ? obj6.Level : 0);
			break;
		}
		case 9L:
		case 10L:
		case 13L:
		{
			PlayerSkill obj4 = behavior[((Skill)farming).Id];
			num = ((obj4 != null) ? obj4.Level : 0);
			break;
		}
		case 4L:
		{
			val3 = val2[combat.ShovelKnightId];
			if ((double)val3.FValue(0, 0f) > ((Entity)val).World.Rand.NextDouble())
			{
				damage *= (float)val3.Value(1, 0);
			}
			val3 = null;
			PlayerSkill obj5 = behavior[((Skill)digging).Id];
			num = ((obj5 != null) ? obj5.Level : 0);
			break;
		}
		case null:
		{
			if ((int)dmgSource.Type == 4)
			{
				val3 = val2[combat.SpearmanId];
				break;
			}
			ItemSlot activeHotbarSlot = val.Player.InventoryManager.ActiveHotbarSlot;
			object obj;
			if (activeHotbarSlot == null)
			{
				obj = null;
			}
			else
			{
				ItemStack itemstack = activeHotbarSlot.Itemstack;
				if (itemstack == null)
				{
					obj = null;
				}
				else
				{
					Item item = itemstack.Item;
					obj = ((item != null) ? ((RegistryObject)item).FirstCodePart(0) : null);
				}
			}
			if ((string?)obj == "rollingpin")
			{
				PlayerSkill obj2 = behavior[((Skill)cooking).Id];
				num = ((obj2 != null) ? obj2.Level : 0);
			}
			break;
		}
		}
		if (val3 != null)
		{
			damage *= 1f + val3.SkillDependentFValue(0);
		}
		else if (num != 0)
		{
			val3 = val2[combat.ToolMasteryId];
			if (val3 != null)
			{
				damage *= 1f + val3.SkillDependentFValue(num);
			}
		}
		ItemSlot activeHotbarSlot2 = val.Player.InventoryManager.ActiveHotbarSlot;
		if (((activeHotbarSlot2 != null) ? activeHotbarSlot2.Itemstack : null) == null)
		{
			IInventory ownInventory = val.Player.InventoryManager.GetOwnInventory("character");
			InventoryCharacter val4 = (InventoryCharacter)(object)((ownInventory is InventoryCharacter) ? ownInventory : null);
			if (val4 != null && ((InventoryBase)val4).Count >= 15)
			{
				PlayerAbility val5 = val2[combat.MonkId];
				val3 = val2[combat.IronFistId];
				if (val3.Tier > 0 || val5.Tier > 0)
				{
					ItemStack itemstack2 = ((InventoryBase)val4)[12].Itemstack;
					Item obj9 = ((itemstack2 != null) ? itemstack2.Item : null);
					float num2 = ((float?)((ItemWearable)(((obj9 is ItemWearable) ? obj9 : null)?)).ProtectionModifiers.ProtectionTier) ?? 0f;
					ItemStack itemstack3 = ((InventoryBase)val4)[13].Itemstack;
					Item obj10 = ((itemstack3 != null) ? itemstack3.Item : null);
					float num3 = num2 + (((float?)((ItemWearable)(((obj10 is ItemWearable) ? obj10 : null)?)).ProtectionModifiers.ProtectionTier) ?? 0f);
					ItemStack itemstack4 = ((InventoryBase)val4)[14].Itemstack;
					Item obj11 = ((itemstack4 != null) ? itemstack4.Item : null);
					float num4 = num3 + (((float?)((ItemWearable)(((obj11 is ItemWearable) ? obj11 : null)?)).ProtectionModifiers.ProtectionTier) ?? 0f);
					num4 /= 3f;
					damage = ((val3.Tier <= 0) ? (damage * Math.Max((float)val5.Value(0, 0) - num4 / 3f * (float)val5.Value(0, 0), 1f)) : (damage * (num4 * (float)val3.Value(0, 0))));
				}
				val3 = val2[combat.MonkId];
			}
		}
		if (temporalAdaptation != null)
		{
			PlayerSkillSet behavior2 = ((Entity)val).GetBehavior<PlayerSkillSet>();
			PlayerSkill val6 = ((behavior2 != null) ? behavior2[((Skill)temporalAdaptation).Id] : null);
			SystemTemporalStability modSystem = ((Skill)combat).XLeveling.Api.ModLoader.GetModSystem<SystemTemporalStability>(true);
			if (val6 != null && modSystem != null)
			{
				val3 = val6[temporalAdaptation.StableWarriorId];
				PlayerAbility val7 = val6[temporalAdaptation.TemporalUnstableId];
				float num5 = Math.Clamp(modSystem.GetTemporalStability(((Entity)val).Pos.XYZ), 0f, 1f);
				damage = ((val7.Tier <= 0) ? (damage * (1f + 0.01f * (float)val3.Value(0, 0) * num5)) : (damage * (1f + 0.0001f * (float)val3.Value(0, 0) * (float)(100 + val7.Value(0, 0)) * (1f - num5))));
			}
		}
		val3 = val2[combat.BurningRageId];
		if ((double)((float)val3.Value(0, 0) * 0.01f) > ((Entity)val).World.Rand.NextDouble())
		{
			base.entity.Ignite();
		}
		return damage;
	}

	public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		((EntityBehavior)this).OnEntityReceiveDamage(damageSource, ref damage);
		if ((int)base.entity.Api.Side != 1)
		{
			return;
		}
		Entity sourceEntity = damageSource.SourceEntity;
		EntityPlayer val = (EntityPlayer)(object)((sourceEntity is EntityPlayer) ? sourceEntity : null);
		if (combat == null || val == null)
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)val).GetBehavior<PlayerSkillSet>();
		PlayerSkill val2 = ((behavior != null) ? behavior[((Skill)combat).Id] : null);
		if (val2 == null)
		{
			return;
		}
		PlayerAbility val3 = val2[combat.VampireId];
		if (val3.Tier > 0)
		{
			float num = damage * (float)val3.Value(0, 0) * 0.01f;
			EntityBehavior behavior2 = ((Entity)val).GetBehavior("health");
			EntityBehaviorHealth val4 = (EntityBehaviorHealth)(object)((behavior2 is EntityBehaviorHealth) ? behavior2 : null);
			if (val4 != null && val4 != null)
			{
				val4.Health = Math.Min(val4.Health + num, val4.MaxHealth);
			}
		}
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref EnumHandling handling)
	{
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		if (byPlayer == null)
		{
			return null;
		}
		XLeveling obj = XLeveling.Instance(((Entity)byPlayer.Entity).Api);
		if (!(((obj != null) ? obj.GetSkill("combat", false) : null) is Combat combat))
		{
			return null;
		}
		object obj2;
		if (byPlayer == null)
		{
			obj2 = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj2 = ((behavior != null) ? behavior[((Skill)combat).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj2;
		if (val == null)
		{
			return null;
		}
		PlayerAbility val2 = val[combat.LooterId];
		if (val2.Tier <= 0)
		{
			return null;
		}
		float num = val2.SkillDependentFValue(0);
		handling = (EnumHandling)3;
		if (base.entity.Properties.Drops == null)
		{
			return null;
		}
		List<ItemStack> list = new List<ItemStack>();
		float num2 = 1f + num;
		JsonObject attributes = base.entity.Properties.Attributes;
		if ((attributes == null || !attributes["isMechanical"].AsBool(false)) && ((byPlayer != null) ? byPlayer.Entity : null) != null)
		{
			num2 += ((Entity)byPlayer.Entity).Stats.GetBlended("animalLootDropRate");
		}
		for (int i = 0; i < base.entity.Properties.Drops.Length; i++)
		{
			BlockDropItemStack val3 = base.entity.Properties.Drops[i];
			float num3 = 1f;
			if (val3.DropModbyStat != null && ((byPlayer != null) ? byPlayer.Entity : null) != null)
			{
				num3 = ((Entity)byPlayer.Entity).Stats.GetBlended(val3.DropModbyStat);
			}
			ItemStack val4 = val3.GetNextItemStack(num2 * num3);
			if (val4 != null)
			{
				CollectibleObject collectible = val4.Collectible;
				IResolvableCollectible val5 = (IResolvableCollectible)(object)((collectible is IResolvableCollectible) ? collectible : null);
				if (val5 != null)
				{
					DummySlot val6 = new DummySlot(val4);
					val5.Resolve((ItemSlot)(object)val6, world, true);
					val4 = ((ItemSlot)val6).Itemstack;
				}
				list.Add(val4);
				if (val3.LastDrop)
				{
					break;
				}
			}
		}
		return list.ToArray();
	}

	public override void GetInfoText(StringBuilder infotext)
	{
		((EntityBehavior)this).GetInfoText(infotext);
		ICoreAPI api = base.entity.Api;
		ICoreAPI obj = ((api is ICoreClientAPI) ? api : null);
		IPlayer val = (IPlayer)(object)((obj != null) ? ((ICoreClientAPI)obj).World.Player : null);
		if (val == null)
		{
			return;
		}
		XLeveling obj2 = XLeveling.Instance(((Entity)val.Entity).Api);
		if (!(((obj2 != null) ? obj2.GetSkill("combat", false) : null) is Combat combat))
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)val.Entity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val2 = ((behavior != null) ? behavior[((Skill)combat).Id] : null);
		if (val2 != null && ((val2 != null) ? val2[combat.MonsterExpertId] : null).Tier > 0)
		{
			ITreeAttribute treeAttribute = ((TreeAttribute)base.entity.WatchedAttributes).GetTreeAttribute("health");
			if (treeAttribute != null)
			{
				infotext.AppendLine(Lang.Get("Health: {0:0.##}/{1:0.##}", new object[2]
				{
					treeAttribute.GetFloat("currenthealth", 0f),
					treeAttribute.GetFloat("maxhealth", 0f)
				}));
			}
		}
	}
}
