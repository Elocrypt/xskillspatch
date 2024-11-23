using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsFarmlandBehavior : BlockBehavior
{
	private Farming farming;

	public XSkillsFarmlandBehavior(Block block)
		: base(block)
	{
	}

	public override void OnLoaded(ICoreAPI api)
	{
		XLeveling obj = XLeveling.Instance(api);
		farming = ((obj != null) ? obj.GetSkill("farming", false) : null) as Farming;
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
	{
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		if (farming == null)
		{
			return (ItemStack[])(object)new ItemStack[0];
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
		PlayerAbility val = ((obj != null) ? ((PlayerSkill)obj)[farming.RecyclerId] : null);
		if (val == null)
		{
			return (ItemStack[])(object)new ItemStack[0];
		}
		if (val.Tier <= 0)
		{
			return (ItemStack[])(object)new ItemStack[0];
		}
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
		IFarmlandBlockEntity val2 = (IFarmlandBlockEntity)(object)((blockEntity is IFarmlandBlockEntity) ? blockEntity : null);
		if (val2 == null)
		{
			return (ItemStack[])(object)new ItemStack[0];
		}
		float val3 = Math.Max(val.Value(0, 0), val2.OriginalFertility.Min());
		float val4 = val2.Nutrients.Min();
		val4 = Math.Min(val4, val3);
		AssetLocation val5 = null;
		handling = (EnumHandling)2;
		IEnumerator<KeyValuePair<string, float>> enumerator = BlockEntityFarmland.Fertilities.GetEnumerator();
		float num = 0f;
		string text = null;
		while (enumerator.MoveNext())
		{
			if (val4 >= enumerator.Current.Value - 0.1f && num < enumerator.Current.Value)
			{
				num = enumerator.Current.Value;
				text = enumerator.Current.Key;
			}
		}
		val5 = ((text == null) ? new AssetLocation("game", "soil-verylow-none") : new AssetLocation("game", "soil-" + text + "-none"));
		Block block = world.GetBlock(val5);
		if (block == null)
		{
			return (ItemStack[])(object)new ItemStack[0];
		}
		return (ItemStack[])(object)new ItemStack[1]
		{
			new ItemStack(block, 1)
		};
	}
}
