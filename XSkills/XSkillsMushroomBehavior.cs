using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsMushroomBehavior : BlockBehavior
{
	private Farming farming;

	private float xp;

	public XSkillsMushroomBehavior(Block block)
		: base(block)
	{
	}

	public override void Initialize(JsonObject properties)
	{
		xp = properties["xp"].AsFloat(0f);
		((CollectibleBehavior)this).Initialize(properties);
	}

	public override void OnLoaded(ICoreAPI api)
	{
		XLeveling obj = XLeveling.Instance(api);
		farming = ((obj != null) ? obj.GetSkill("farming", false) : null) as Farming;
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref EnumHandling handling)
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		if (farming == null)
		{
			XLeveling obj = XLeveling.Instance(world.Api);
			farming = ((obj != null) ? obj.GetSkill("farming", false) : null) as Farming;
			if (farming == null)
			{
				return;
			}
		}
		object obj2;
		if (byPlayer == null)
		{
			obj2 = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj2 = ((behavior != null) ? behavior[((Skill)farming).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj2;
		if (val == null)
		{
			return;
		}
		val.AddExperience(xp, true);
		PlayerAbility val2 = val[farming.CarefulHandsId];
		Block block = base.block;
		BlockPlant val3 = (BlockPlant)(object)((block is BlockPlant) ? block : null);
		if (val3 == null || val2 == null || !((RegistryObject)val3).Code.Path.Contains("normal") || val2.Tier <= 0 || byPlayer.InventoryManager.ActiveTool == (EnumTool?)0)
		{
			return;
		}
		handling = (EnumHandling)2;
		AssetLocation val4 = ((RegistryObject)val3).Code.CopyWithPath(((RegistryObject)val3).Code.Path.Replace("normal", "harvested"));
		Block block2 = world.GetBlock(val4);
		if (block2 != null)
		{
			world.BlockAccessor.SetBlock(block2.BlockId, pos);
		}
		else
		{
			world.BlockAccessor.SetBlock(0, pos);
		}
		val2 = val[farming.GathererId];
		if (val2 != null)
		{
			float num = 1f + 0.01f * (float)val2.SkillDependentValue(0);
			for (int i = 0; i < base.block.Drops.Length; i++)
			{
				world.SpawnItemEntity(base.block.Drops[i].GetNextItemStack(num), pos.ToVec3d().Add(0.5, 0.5, 0.5), (Vec3d)null);
			}
		}
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
	{
		List<ItemStack> list = new List<ItemStack>();
		if (farming == null)
		{
			return list.ToArray();
		}
		object obj;
		if (byPlayer == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
			obj = ((behavior != null) ? behavior[((Skill)farming).Id] : null);
		}
		PlayerSkill val = (PlayerSkill)obj;
		if (val == null)
		{
			return list.ToArray();
		}
		if (base.block.Drops.Length == 0)
		{
			return list.ToArray();
		}
		PlayerAbility val2 = val[farming.GathererId];
		if (val2 == null)
		{
			return list.ToArray();
		}
		handling = (EnumHandling)2;
		float num = dropChanceMultiplier + val2.SkillDependentFValue(0);
		for (int i = 0; i < base.block.Drops.Length; i++)
		{
			ItemStack nextItemStack = base.block.Drops[i].GetNextItemStack(num);
			if (nextItemStack != null)
			{
				list.Add(nextItemStack);
			}
		}
		return list.ToArray();
	}
}
