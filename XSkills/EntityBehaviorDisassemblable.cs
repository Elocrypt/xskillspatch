using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class EntityBehaviorDisassemblable : EntityBehaviorHarvestable
{
	private WorldInteraction[] interactions;

	public override string PropertyName()
	{
		return "disassemblable";
	}

	public EntityBehaviorDisassemblable(Entity entity)
		: base(entity)
	{
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		((EntityBehaviorHarvestable)this).Initialize(properties, attributes);
	}

	public override WorldInteraction[] GetInteractionHelp(IClientWorldAccessor world, EntitySelection es, IClientPlayer player, ref EnumHandling handled)
	{
		interactions = ObjectCacheUtil.GetOrCreate<WorldInteraction[]>(((IWorldAccessor)world).Api, "disassemblableEntityInteractions", (CreateCachableObjectDelegate<WorldInteraction[]>)delegate
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			List<ItemStack> list = new List<ItemStack>();
			foreach (Item item in ((IWorldAccessor)world).Items)
			{
				if (!(((RegistryObject)item).Code == (AssetLocation)null) && ((CollectibleObject)item).Tool == (EnumTool?)5)
				{
					list.Add(new ItemStack(item, 1));
				}
			}
			return (WorldInteraction[])(object)new WorldInteraction[1]
			{
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-creature-harvest",
					MouseButton = (EnumMouseButton)2,
					HotKeyCode = "sneak",
					Itemstacks = list.ToArray()
				}
			};
		});
		if (player == null)
		{
			return null;
		}
		PlayerSkillSet behavior = ((Entity)((IPlayer)player).Entity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val = ((behavior != null) ? behavior.FindSkill("metalworking", false) : null);
		if (val == null)
		{
			return null;
		}
		if (!(val.Skill is Metalworking metalworking))
		{
			return null;
		}
		if (val[metalworking.SalvagerId].Tier <= 0)
		{
			return null;
		}
		if (((EntityBehavior)this).entity.Alive || ((EntityBehaviorHarvestable)this).IsHarvested)
		{
			return null;
		}
		return interactions;
	}
}
