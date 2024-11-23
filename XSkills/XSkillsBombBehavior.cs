using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsBombBehavior : BlockBehavior
{
	private Mining mining;

	public XSkillsBombBehavior(Block block)
		: base(block)
	{
	}

	public override void OnBlockExploded(IWorldAccessor world, BlockPos pos, BlockPos explosionCenter, EnumBlastType blastType, ref EnumHandling handling)
	{
		if (mining == null)
		{
			XLeveling obj = XLeveling.Instance(world.Api);
			mining = ((obj != null) ? obj.GetSkill("mining", false) : null) as Mining;
			if (mining == null)
			{
				return;
			}
		}
		if (!(pos == (BlockPos)null))
		{
			IPlayer playerCausingExplosion = mining.GetPlayerCausingExplosion(pos);
			if (playerCausingExplosion != null)
			{
				mining.RegisterExplosion(pos, playerCausingExplosion);
			}
		}
	}

	public override void OnBlockRemoved(IWorldAccessor world, BlockPos pos, ref EnumHandling handling)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		if (mining == null)
		{
			XLeveling obj = XLeveling.Instance(world.Api);
			mining = ((obj != null) ? obj.GetSkill("mining", false) : null) as Mining;
			if (mining == null)
			{
				return;
			}
		}
		BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(pos);
		BlockEntityBomb val = (BlockEntityBomb)(object)((blockEntity is BlockEntityBomb) ? blockEntity : null);
		if (val == null)
		{
			return;
		}
		TreeAttribute val2 = new TreeAttribute();
		((BlockEntity)val).ToTreeAttributes((ITreeAttribute)(object)val2);
		IAttribute attribute = val2.GetAttribute("ignitedByPlayerUid");
		if (attribute != null && attribute.GetValue() != null)
		{
			string text = ((object)attribute).ToString();
			IPlayer val3 = world.PlayerByUid(text);
			if (val3 != null && val.IsLit)
			{
				mining.RegisterExplosion(pos, val3);
			}
		}
	}
}
