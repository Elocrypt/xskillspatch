using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsAnimalBehavior : EntityBehavior
{
	protected Husbandry husbandry;

	protected float xp;

	public IPlayer Feeder { get; internal set; }

	public bool Catchable { get; set; }

	public override string PropertyName()
	{
		return "XSkillsAnimal";
	}

	public XSkillsAnimalBehavior(Entity entity)
		: base(entity)
	{
		XLeveling obj = XLeveling.Instance(entity.Api);
		husbandry = ((obj != null) ? obj.GetSkill("husbandry", false) : null) as Husbandry;
		xp = 0f;
		Catchable = false;
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		((RegistryObject)base.entity).Code.Path.Contains("bear");
		((EntityBehavior)this).Initialize(properties, attributes);
		xp = attributes["xp"].AsFloat(0f);
		Catchable = attributes["catchable"].AsBool(false);
		EntityBehavior behavior = base.entity.GetBehavior("health");
		EntityBehaviorHealth val = (EntityBehaviorHealth)(object)((behavior is EntityBehaviorHealth) ? behavior : null);
		if (val != null)
		{
			val.onDamaged += new OnDamagedDelegate(OnDamage);
		}
	}

	public float OnDamage(float damage, DamageSource dmgSource)
	{
		Entity sourceEntity = dmgSource.SourceEntity;
		EntityPlayer val = (EntityPlayer)(((object)((sourceEntity is EntityPlayer) ? sourceEntity : null)) ?? ((object)(/*isinst with value type is only supported in some contexts*/ ?? /*isinst with value type is only supported in some contexts*/)));
		if (husbandry == null || val == null)
		{
			return damage;
		}
		PlayerSkillSet behavior = ((Entity)val).GetBehavior<PlayerSkillSet>();
		object obj;
		if (behavior == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkill obj2 = behavior[((Skill)husbandry).Id];
			obj = ((obj2 != null) ? obj2[husbandry.HunterId] : null);
		}
		PlayerAbility val2 = (PlayerAbility)obj;
		if (val2 == null)
		{
			return damage;
		}
		damage *= 1f + val2.SkillDependentFValue(0);
		return damage;
	}

	public override void OnEntityDeath(DamageSource damageSourceForDeath)
	{
		Entity sourceEntity = damageSourceForDeath.SourceEntity;
		EntityPlayer val = (EntityPlayer)(((object)((sourceEntity is EntityPlayer) ? sourceEntity : null)) ?? ((object)(/*isinst with value type is only supported in some contexts*/ ?? /*isinst with value type is only supported in some contexts*/)));
		if (husbandry != null && val != null)
		{
			PlayerSkillSet behavior = ((Entity)val).GetBehavior<PlayerSkillSet>();
			PlayerSkill val2 = ((behavior != null) ? behavior[((Skill)husbandry).Id] : null);
			if (val2 != null)
			{
				float num = 1f + (float)Math.Min(((TreeAttribute)base.entity.WatchedAttributes).GetInt("generation", 0), 20) * 0.05f;
				val2.AddExperience(num * xp, true);
			}
		}
	}
}
