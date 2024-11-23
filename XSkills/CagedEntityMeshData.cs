using Newtonsoft.Json;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace XSkills;

public class CagedEntityMeshData : ITexPositionSource
{
	protected ICoreClientAPI capi;

	protected string entityName;

	protected int entityTextureId;

	protected string entityShape;

	public Shape Shape { get; protected set; }

	public MeshData MeshData { get; protected set; }

	public Size2i AtlasSize
	{
		get
		{
			ICoreClientAPI obj = capi;
			if (obj == null)
			{
				return null;
			}
			return ((ITextureAtlasAPI)obj.BlockTextureAtlas).Size;
		}
	}

	public TextureAtlasPosition this[string textureCode]
	{
		get
		{
			if (capi == null)
			{
				return null;
			}
			BlockCage.EntitiyTextureIds.TryGetValue(entityName, out var value);
			if (value == null)
			{
				return null;
			}
			int num = value.TextureIds[entityTextureId];
			return ((ITextureAtlasAPI)capi.BlockTextureAtlas).Positions[num];
		}
	}

	public CagedEntityMeshData(ICoreClientAPI capi, string entityName, int entityTextureId, string entityShape)
	{
		this.capi = capi;
		this.entityName = entityName;
		this.entityTextureId = entityTextureId;
		this.entityShape = entityShape;
		Shape = null;
		MeshData = null;
	}

	public MeshData Generate()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		MeshData val = null;
		if (capi != null && entityShape != null)
		{
			Shape = ((ICoreAPI)capi).Assets.TryGet(new AssetLocation(entityShape), true).ToObject<Shape>((JsonSerializerSettings)null);
			capi.Tesselator.TesselateShapeWithJointIds("cage", Shape, ref val, (ITexPositionSource)(object)this, new Vec3f(), (int?)null, (string[])null);
			if (!BlockCage.Scalings.TryGetValue(entityName, out var value))
			{
				ModelTransform noTransform = ModelTransform.NoTransform;
				((ModelTransformNoDefaults)noTransform).Scale = value;
				val.ModelTransform(noTransform);
			}
			val.Translate(0f, 0.0625f - (1f - value) / 2f, 0f);
		}
		MeshData = val;
		return MeshData;
	}
}
