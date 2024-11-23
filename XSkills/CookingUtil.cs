using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class CookingUtil
{
	public static BlockEntityBehaviorOwnable GetOwnableFromInventory(InventoryBase inventory)
	{
		object obj;
		if (inventory == null)
		{
			obj = null;
		}
		else
		{
			BlockEntity blockEntity = inventory.Api.World.BlockAccessor.GetBlockEntity(inventory.Pos);
			obj = ((blockEntity != null) ? blockEntity.GetBehavior<BlockEntityBehaviorOwnable>() : null);
		}
		BlockEntityBehaviorOwnable blockEntityBehaviorOwnable = (BlockEntityBehaviorOwnable)obj;
		if (blockEntityBehaviorOwnable == null)
		{
			return null;
		}
		if (blockEntityBehaviorOwnable.ShouldResolveOwner())
		{
			blockEntityBehaviorOwnable.ResolveOwner();
		}
		return blockEntityBehaviorOwnable;
	}

	public static IPlayer GetOwnerFromInventory(InventoryBase inventory)
	{
		return GetOwnableFromInventory(inventory)?.Owner;
	}

	public static int SetMaxServingSize(BlockCookingContainer container, ISlotProvider cookingSlotsProvider)
	{
		int maxServingSize = container.MaxServingSize;
		IPlayer ownerFromInventory = GetOwnerFromInventory((InventoryBase)(object)((cookingSlotsProvider is InventoryBase) ? cookingSlotsProvider : null));
		if (((ownerFromInventory != null) ? ownerFromInventory.Entity : null) == null)
		{
			return maxServingSize;
		}
		XLeveling modSystem = ((Entity)ownerFromInventory.Entity).Api.ModLoader.GetModSystem<XLeveling>(true);
		if (!(((modSystem != null) ? modSystem.GetSkill("cooking", false) : null) is Cooking cooking))
		{
			return maxServingSize;
		}
		PlayerSkillSet behavior = ((Entity)ownerFromInventory.Entity).GetBehavior<PlayerSkillSet>();
		PlayerSkill obj = ((behavior != null) ? behavior[((Skill)cooking).Id] : null);
		PlayerAbility val = ((obj != null) ? obj[cooking.CanteenCookId] : null);
		if (val != null)
		{
			container.MaxServingSize += (int)((float)container.MaxServingSize * val.FValue(0, 0f));
		}
		return maxServingSize;
	}

	public static float GetCookingTimeMultiplier(BlockEntity entity)
	{
		IPlayer val = entity.GetBehavior<BlockEntityBehaviorOwnable>()?.Owner;
		object obj;
		if (val == null)
		{
			obj = null;
		}
		else
		{
			EntityPlayer entity2 = val.Entity;
			if (entity2 == null)
			{
				obj = null;
			}
			else
			{
				XLeveling modSystem = ((Entity)entity2).Api.ModLoader.GetModSystem<XLeveling>(true);
				obj = ((modSystem != null) ? modSystem.GetSkill("cooking", false) : null);
			}
		}
		if (!(obj is Cooking cooking))
		{
			return 1f;
		}
		PlayerSkillSet behavior = ((Entity)val.Entity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val2 = ((behavior != null) ? behavior[((Skill)cooking).Id] : null);
		if (val2 == null)
		{
			return 1f;
		}
		PlayerAbility obj2 = val2[cooking.FastFoodId];
		float num = 1f - ((obj2 != null) ? obj2.SkillDependentFValue(0) : 0f);
		PlayerAbility obj3 = val2[cooking.WellDoneId];
		float num2 = 1f + ((obj3 != null) ? obj3.FValue(3, 0f) : 0f);
		return num * num2;
	}
}
