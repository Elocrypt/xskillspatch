using System.IO;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace XSkills;

public class BlockEntityCage : BlockEntity
{
	public string EntityName { get; internal set; }

	public string EntityClass { get; internal set; }

	public byte[] EntityData { get; internal set; }

	public string EntityShape { get; internal set; }

	public int EntityTextureID { get; internal set; }

	protected CagedEntityMeshData MeshData { get; set; }

	public BlockEntityCage()
	{
		EntityName = null;
		EntityClass = null;
		EntityData = null;
		EntityShape = null;
		EntityTextureID = -1;
	}

	public override void OnBlockBroken(IPlayer byPlayer = null)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)base.Api.Side == 1)
		{
			if (EntityClass == null || EntityData == null)
			{
				return;
			}
			Entity val = base.Api.World.ClassRegistry.CreateEntity(EntityClass);
			if (val == null)
			{
				return;
			}
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(EntityData));
			val.FromBytes(binaryReader, false);
			val.Pos.SetPos(base.Pos.Add(0f, 0.15f, 0f));
			val.ServerPos.SetPos(base.Pos.Add(0f, 0.15f, 0f));
			val.PositionBeforeFalling.Set(base.Pos.Add(0f, 0.15f, 0f));
			base.Api.World.SpawnEntity(val);
		}
		((BlockEntity)this).OnBlockBroken(byPlayer);
	}

	public override void OnBlockPlaced(ItemStack byItemStack = null)
	{
		((BlockEntity)this).OnBlockPlaced(byItemStack);
		if (byItemStack == null)
		{
			EntityName = null;
			EntityClass = null;
			EntityData = null;
			EntityShape = null;
			EntityTextureID = -1;
		}
		else
		{
			EntityName = byItemStack.Attributes.GetString("entityName", (string)null);
			EntityClass = byItemStack.Attributes.GetString("entityClass", (string)null);
			EntityData = byItemStack.Attributes.GetBytes("entityData", (byte[])null);
			EntityShape = byItemStack.Attributes.GetString("entityShape", (string)null);
			EntityTextureID = byItemStack.Attributes.GetInt("entityTextureID", -1);
		}
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		((BlockEntity)this).ToTreeAttributes(tree);
		if (EntityName != null && EntityClass != null && EntityData != null)
		{
			tree.SetString("entityName", EntityName);
			tree.SetString("entityClass", EntityClass);
			tree.SetBytes("entityData", EntityData);
			if (EntityShape != null)
			{
				tree.SetString("entityShape", EntityShape);
			}
			if (EntityTextureID != -1)
			{
				tree.SetInt("entityTextureID", EntityTextureID);
			}
		}
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		((BlockEntity)this).FromTreeAttributes(tree, worldAccessForResolve);
		EntityName = tree.GetString("entityName", (string)null);
		EntityClass = tree.GetString("entityClass", (string)null);
		EntityData = tree.GetBytes("entityData", (byte[])null);
		EntityShape = tree.GetString("entityShape", (string)null);
		EntityTextureID = tree.GetInt("entityTextureID", -1);
		if (EntityData == null)
		{
			string @string = tree.GetString("entityData", (string)null);
			if (@string != null)
			{
				EntityData = Ascii85.Decode(@string);
			}
		}
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		GenerateMeshData();
		if (MeshData.MeshData != null)
		{
			mesher.AddMeshData(MeshData.MeshData, 1);
		}
		return false;
	}

	protected virtual void GenerateMeshData()
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		ICoreAPI api = base.Api;
		MeshData = new CagedEntityMeshData((ICoreClientAPI)(object)((api is ICoreClientAPI) ? api : null), EntityName, EntityTextureID, EntityShape);
		MeshData.Generate();
		string text = ((RegistryObject)((BlockEntity)this).Block).CodeEndWithoutParts(0);
		if (MeshData.MeshData != null)
		{
			float y = 0f;
			if (text.Contains("north"))
			{
				y = 90f;
			}
			if (text.Contains("west"))
			{
				y = 180f;
			}
			if (text.Contains("south"))
			{
				y = 270f;
			}
			ModelTransform val = new ModelTransform();
			val.EnsureDefaultValues();
			((ModelTransformNoDefaults)val).Rotation.Y = y;
			MeshData.MeshData.ModelTransform(val);
		}
	}
}
