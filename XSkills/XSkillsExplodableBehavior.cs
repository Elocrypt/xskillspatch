using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsExplodableBehavior : CollectingBehavior
{
	protected Mining mining;

	protected TemporalAdaptation temporalAdaptation;

	public override CollectingSkill Skill => mining;

	public override EnumTool? Tool => (EnumTool)1;

	public override PlayerAbility DropBonusAbility(PlayerSkill playerSkill)
	{
		return null;
	}

	public XSkillsExplodableBehavior(Block block)
		: base(block)
	{
	}

	public override void OnLoaded(ICoreAPI api)
	{
		XLeveling obj = XLeveling.Instance(api);
		mining = ((obj != null) ? obj.GetSkill("mining", false) : null) as Mining;
		XLeveling obj2 = XLeveling.Instance(api);
		temporalAdaptation = ((obj2 != null) ? obj2.GetSkill("temporaladaptation", false) : null) as TemporalAdaptation;
	}

	public override void OnBlockExploded(IWorldAccessor world, BlockPos pos, BlockPos explosionCenter, EnumBlastType blastType, ref EnumHandling handling)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		if (mining == null)
		{
			return;
		}
		IPlayer playerCausingExplosion = mining.GetPlayerCausingExplosion(explosionCenter);
		if (playerCausingExplosion == null)
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)playerCausingExplosion.Entity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val = ((behavior != null) ? behavior.PlayerSkills[((Skill)mining).Id] : null);
		if (val == null)
		{
			return;
		}
		float num = (this as XSkillsOreBehavior)?.GetXP(world, pos, playerCausingExplosion) ?? xp;
		val.AddExperience(num * 0.1f, true);
		handling = (EnumHandling)2;
		((IBlockAccessor)world.BulkBlockAccessor).SetBlock(0, pos);
		double num2 = ((BlockBehavior)this).block.ExplosionDropChance(world, pos, blastType);
		PlayerAbility val2 = val.PlayerAbilities[mining.BombermanId];
		num2 += (1.0 - num2) * (double)val2.FValue(0, 0f);
		if (world.Rand.NextDouble() < num2)
		{
			val2 = val.PlayerAbilities[mining.BlasterId];
			ItemStack[] array = ((val2.Tier <= 0) ? ((BlockBehavior)this).block.GetDrops(world, pos, (IPlayer)null, 1f) : ((BlockBehavior)this).block.GetDrops(world, pos, playerCausingExplosion, 1f));
			if (array == null)
			{
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (((BlockBehavior)this).block.SplitDropStacks)
				{
					for (int j = 0; j < array[i].StackSize; j++)
					{
						ItemStack val3 = array[i].Clone();
						val3.StackSize = 1;
						world.SpawnItemEntity(val3, new Vec3d((double)pos.X + 0.5, (double)pos.Y + 0.5, (double)pos.Z + 0.5), (Vec3d)null);
					}
				}
				else
				{
					world.SpawnItemEntity(array[i], new Vec3d((double)pos.X + 0.5, (double)pos.Y + 0.5, (double)pos.Z + 0.5), (Vec3d)null);
				}
			}
		}
		if (((BlockBehavior)this).block.EntityClass != null)
		{
			BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
			if (blockEntity != null)
			{
				blockEntity.OnBlockBroken((IPlayer)null);
			}
		}
	}
}
