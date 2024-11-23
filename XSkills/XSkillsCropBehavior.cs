using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsCropBehavior : BlockBehavior
{
	private Farming farming;

	private float xp;

	public XSkillsCropBehavior(Block block)
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

	public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack, ref EnumHandling handling)
	{
		return true;
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
	{
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Invalid comparison between Unknown and I4
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Expected O, but got Unknown
		List<ItemStack> list = new List<ItemStack>();
		if (farming == null)
		{
			list.ToArray();
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
		val.AddExperience(xp, true);
		PlayerAbility val2 = val[farming.DemetersBlessId];
		PlayerAbility val3 = val[farming.GreenThumbId];
		for (int i = 0; i < base.block.Drops.Length; i++)
		{
			if (xp > 0f && base.block.Drops[i].ResolvedItemstack.GetName().Contains("seeds"))
			{
				if (val2 != null)
				{
					float num = val2.SkillDependentFValue(0);
					ItemStack nextItemStack = base.block.Drops[i].GetNextItemStack(num);
					if (nextItemStack != null)
					{
						list.Add(nextItemStack);
					}
				}
			}
			else if (val3 != null)
			{
				float num2 = val3.SkillDependentFValue(0);
				ItemStack nextItemStack2 = base.block.Drops[i].GetNextItemStack(num2);
				if (nextItemStack2 != null)
				{
					list.Add(nextItemStack2);
				}
			}
		}
		if (xp == 0f && list.Count == 0)
		{
			if (val[farming.RepottingId].Tier > 0)
			{
				int num3 = ((RegistryObject)base.block).Code.Path.IndexOf("-");
				int num4 = ((RegistryObject)base.block).Code.Path.LastIndexOf("-");
				if (num4 != num3)
				{
					AssetLocation val4 = new AssetLocation(((RegistryObject)base.block).Code.Domain, "seeds-" + ((RegistryObject)base.block).Code.Path.Substring(num3 + 1, num4 - num3 - 1));
					Item item = world.GetItem(val4);
					if (item != null)
					{
						handling = (EnumHandling)2;
						list.Add(new ItemStack(item, 1));
					}
				}
			}
		}
		else
		{
			BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(new BlockPos(pos.X, pos.Y - 1, pos.Z, pos.dimension));
			BlockEntityFarmland val5 = (BlockEntityFarmland)(object)((blockEntity is BlockEntityFarmland) ? blockEntity : null);
			PlayerAbility val6 = val[farming.CompostingId];
			BlockCropProperties cropProps = base.block.CropProps;
			if (val5 != null && val6.Tier > 0 && cropProps != null)
			{
				for (EnumSoilNutrient val7 = (EnumSoilNutrient)0; (int)val7 <= 2; val7 = (EnumSoilNutrient)(val7 + 1))
				{
					float num5 = ((cropProps.RequiredNutrient != val7) ? ((float)(int)Math.Min((float)val6.Value(1, 0) * 0.01f * cropProps.NutrientConsumption, (float)val6.Value(1, 0) * 0.01f * (float)val5.OriginalFertility[val7])) : Math.Min((float)val6.Value(0, 0) * 0.01f * cropProps.NutrientConsumption, (float)val6.Value(0, 0) * 0.01f * (float)val5.OriginalFertility[val7]));
					val5.Nutrients[val7] = Math.Max(Math.Min(val5.Nutrients[val7] + num5, (float)val5.OriginalFertility[val7] * 1.1f), val5.Nutrients[val7]);
				}
				((BlockEntity)val5).MarkDirty(false, (IPlayer)null);
			}
			val6 = val[farming.CrossBreedingId];
			if (val6 != null && val6.Tier > 0)
			{
				float num6 = val6.FValue(0, 0f);
				int num7 = 0;
				Block block = world.BlockAccessor.GetBlock(new BlockPos(pos.X + 1, pos.Y, pos.Z, pos.dimension));
				BlockCrop val8 = (BlockCrop)(object)((block is BlockCrop) ? block : null);
				if (val8 != null && val8 != base.block && val8.CurrentCropStage >= ((Block)val8).CropProps.GrowthStages)
				{
					num6 *= 1.2f;
					num7++;
				}
				Block block2 = world.BlockAccessor.GetBlock(new BlockPos(pos.X - 1, pos.Y, pos.Z, pos.dimension));
				val8 = (BlockCrop)(object)((block2 is BlockCrop) ? block2 : null);
				if (val8 != null && val8 != base.block && val8.CurrentCropStage >= ((Block)val8).CropProps.GrowthStages)
				{
					num6 *= 1.2f;
					num7++;
				}
				Block block3 = world.BlockAccessor.GetBlock(new BlockPos(pos.X, pos.Y, pos.Z + 1, pos.dimension));
				val8 = (BlockCrop)(object)((block3 is BlockCrop) ? block3 : null);
				if (val8 != null && val8 != base.block && val8.CurrentCropStage >= ((Block)val8).CropProps.GrowthStages)
				{
					num6 *= 1.2f;
					num7++;
				}
				Block block4 = world.BlockAccessor.GetBlock(new BlockPos(pos.X, pos.Y, pos.Z - 1, pos.dimension));
				val8 = (BlockCrop)(object)((block4 is BlockCrop) ? block4 : null);
				if (val8 != null && val8 != base.block && val8.CurrentCropStage >= ((Block)val8).CropProps.GrowthStages)
				{
					num6 *= 1.2f;
					num7++;
				}
				if (num7 > 0 && (double)num6 > world.Rand.NextDouble())
				{
					ItemStack item2 = new ItemStack((Item)(object)farming.Seeds[world.Rand.Next(farming.Seeds.Count - 1)], 1);
					list.Add(item2);
				}
			}
		}
		return list.ToArray();
	}
}
