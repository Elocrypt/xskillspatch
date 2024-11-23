using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class XSkillsBerryBushBehavior : BlockBehaviorHarvestable
{
	private Farming farming;

	private float xp;

	public XSkillsBerryBushBehavior(Block block)
		: base(block)
	{
	}

	public override void Initialize(JsonObject properties)
	{
		((BlockBehaviorHarvestable)this).Initialize(properties);
		xp = properties["xp"].AsFloat(0f);
	}

	public override void OnLoaded(ICoreAPI api)
	{
		XLeveling obj = XLeveling.Instance(api);
		farming = ((obj != null) ? obj.GetSkill("farming", false) : null) as Farming;
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		return true;
	}

	public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handled)
	{
		return true;
	}

	public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handled)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Invalid comparison between Unknown and I4
		BlockBehaviorHarvestable behavior = ((CollectibleObject)((BlockBehavior)this).block).GetBehavior<BlockBehaviorHarvestable>();
		float num = (typeof(BlockBehaviorHarvestable).GetField("harvestTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(behavior) as float?) ?? 1f;
		if (farming == null || secondsUsed < num - 0.05f || byPlayer == null)
		{
			return;
		}
		PlayerSkillSet behavior2 = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
		PlayerSkill val = ((behavior2 != null) ? behavior2.PlayerSkills[((Skill)farming).Id] : null);
		if (val == null)
		{
			return;
		}
		val.AddExperience(xp, true);
		if ((int)world.Api.Side != 1)
		{
			return;
		}
		PlayerAbility val2 = val.PlayerAbilities[farming.GathererId];
		if (val2.Tier > 0)
		{
			float num2 = 0.01f * (float)val2.SkillDependentValue(0);
			ItemStack nextItemStack = behavior.harvestedStack.GetNextItemStack(num2);
			if (nextItemStack != null && !byPlayer.InventoryManager.TryGiveItemstack(nextItemStack, false))
			{
				world.SpawnItemEntity(nextItemStack, blockSel.Position.ToVec3d().Add(0.5, 0.5, 0.5), (Vec3d)null);
			}
		}
	}
}
