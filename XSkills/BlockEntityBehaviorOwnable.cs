using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace XSkills;

public class BlockEntityBehaviorOwnable : BlockEntityBehavior
{
	protected IPlayer owner;

	public string OwnerString { get; set; }

	public IPlayer Owner
	{
		get
		{
			if (owner == null)
			{
				ResolveOwner();
			}
			return owner;
		}
		set
		{
			owner = value;
		}
	}

	public BlockEntityBehaviorOwnable(BlockEntity blockentity)
		: base(blockentity)
	{
		OwnerString = null;
		owner = null;
	}

	public bool ShouldResolveOwner()
	{
		if (OwnerString != null && owner == null)
		{
			return true;
		}
		return false;
	}

	public bool ResolveOwner()
	{
		if (OwnerString == null)
		{
			owner = null;
			return false;
		}
		ICoreAPI api = base.Api;
		owner = ((api != null) ? api.World.PlayerByUid(OwnerString) : null);
		if (owner != null)
		{
			return true;
		}
		return false;
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		((BlockEntityBehavior)this).GetBlockInfo(forPlayer, dsc);
		if (owner != null)
		{
			dsc.AppendLine(Lang.Get("xskills:owner-desc", new object[1] { owner.PlayerName }));
		}
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		((BlockEntityBehavior)this).FromTreeAttributes(tree, worldAccessForResolve);
		OwnerString = tree.GetString("owner", (string)null);
		ResolveOwner();
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		((BlockEntityBehavior)this).ToTreeAttributes(tree);
		if (owner != null)
		{
			tree.SetString("owner", owner.PlayerUID);
		}
		else if (OwnerString != null)
		{
			tree.SetString("owner", OwnerString);
		}
	}
}
