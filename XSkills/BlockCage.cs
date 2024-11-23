using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class BlockCage : Block
{
	private Husbandry husbandry;

	public static Dictionary<string, CapturedEntityTextures> EntitiyTextureIds = new Dictionary<string, CapturedEntityTextures>();

	public static Dictionary<string, MultiTextureMeshRef> MeshRefs = new Dictionary<string, MultiTextureMeshRef>();

	public static Dictionary<string, float> Scalings = new Dictionary<string, float>();

	public override void OnLoaded(ICoreAPI api)
	{
		((Block)this).OnLoaded(api);
		XLeveling obj = XLeveling.Instance(api);
		husbandry = ((obj != null) ? obj.GetSkill("husbandry", false) : null) as Husbandry;
	}

	public bool Empty(ItemStack itemStack)
	{
		if (itemStack == null)
		{
			return false;
		}
		return !itemStack.Attributes.HasAttribute("entityName");
	}

	public string ResolvedEntityName(ItemStack itemStack)
	{
		string @string = itemStack.Attributes.GetString("entityName", (string)null);
		if (@string == null)
		{
			return null;
		}
		return Lang.Get("item-creature-" + @string, Array.Empty<object>());
	}

	public virtual bool IsCatchable(EntityAgent byEntity, Entity entity)
	{
		if (IsCatchable(entity))
		{
			object obj;
			if (byEntity == null)
			{
				obj = null;
			}
			else
			{
				PlayerSkillSet behavior = ((Entity)byEntity).GetBehavior<PlayerSkillSet>();
				if (behavior == null)
				{
					obj = null;
				}
				else
				{
					PlayerSkill obj2 = behavior[((Skill)husbandry).Id];
					obj = ((obj2 != null) ? obj2[husbandry.CatcherId] : null);
				}
			}
			PlayerAbility val = (PlayerAbility)obj;
			if (val == null)
			{
				return false;
			}
			return val.Tier > 0;
		}
		return false;
	}

	public virtual bool IsCatchable(Entity entity)
	{
		return entity.GetBehavior<XSkillsAnimalBehavior>()?.Catchable ?? false;
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		if ((firstEvent && byEntity == null) || byEntity.Controls.Sneak)
		{
			return;
		}
		EntityAgent obj = ((byEntity is EntityPlayer) ? byEntity : null);
		IPlayer val = ((obj != null) ? ((EntityPlayer)obj).Player : null);
		object obj2 = blockSel?.Position;
		if (obj2 == null)
		{
			if (entitySel == null)
			{
				obj2 = null;
			}
			else
			{
				Vec3d position = entitySel.Position;
				obj2 = ((position != null) ? position.AsBlockPos : null);
			}
		}
		BlockPos val2 = (BlockPos)obj2;
		if (val == null || !(val2 != (BlockPos)null) || ((Entity)byEntity).World.Claims.TryAccess(val, val2, (EnumBlockAccessFlags)1))
		{
			if (Empty(slot.Itemstack))
			{
				Capture(slot, byEntity, entitySel?.Entity, ref handling);
			}
			else
			{
				Release(slot, byEntity, blockSel, ref handling);
			}
		}
	}

	public void Capture(ItemSlot slot, EntityAgent byEntity, Entity entity, ref EnumHandHandling handling)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Invalid comparison between Unknown and I4
		if (entity == null || husbandry == null || !entity.Alive)
		{
			return;
		}
		handling = (EnumHandHandling)4;
		EntityBehaviorEmotionStates behavior = entity.GetBehavior<EntityBehaviorEmotionStates>();
		if (behavior != null)
		{
			DamageSource val = new DamageSource();
			val.Source = (EnumDamageSource)8;
			val.SourceEntity = (Entity)(object)byEntity;
			val.Type = (EnumDamageType)2;
			float num = 1f;
			((EntityBehavior)behavior).OnEntityReceiveDamage(val, ref num);
		}
		if (IsCatchable(byEntity, entity))
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			entity.ToBytes(binaryWriter, false);
			binaryWriter.Flush();
			byte[] array = memoryStream.ToArray();
			AssetLocation val2 = entity.Properties.Client.Shape.Base.Clone();
			val2.WithPathPrefix("shapes/").WithPathPrefix(entity.Properties.Client.Shape.Base.Domain + ":").WithPathAppendix(".json");
			slot.Itemstack.Attributes.SetString("entityName", ((RegistryObject)entity).Code.GetName());
			slot.Itemstack.Attributes.SetString("entityClass", ((RegistryObject)entity).Class);
			slot.Itemstack.Attributes.SetBytes("entityData", array);
			slot.Itemstack.Attributes.SetString("entityShape", val2.Path);
			slot.Itemstack.Attributes.SetInt("entityTextureID", ((TreeAttribute)entity.WatchedAttributes).GetInt("textureIndex", 0));
			if ((int)((CollectibleObject)this).api.Side == 1)
			{
				entity.Die((EnumDespawnReason)3, (DamageSource)null);
			}
		}
	}

	public void Release(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, ref EnumHandHandling handling)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		if (blockSel == null)
		{
			return;
		}
		handling = (EnumHandHandling)4;
		if ((int)((CollectibleObject)this).api.Side != 1)
		{
			return;
		}
		string @string = slot.Itemstack.Attributes.GetString("entityClass", (string)null);
		byte[] array = slot.Itemstack.Attributes.GetBytes("entityData", (byte[])null);
		if (array == null)
		{
			string string2 = slot.Itemstack.Attributes.GetString("entityData", (string)null);
			if (string2 != null)
			{
				array = Ascii85.Decode(string2);
			}
		}
		slot.Itemstack.Attributes.RemoveAttribute("entityName");
		slot.Itemstack.Attributes.RemoveAttribute("entityClass");
		slot.Itemstack.Attributes.RemoveAttribute("entityData");
		slot.Itemstack.Attributes.RemoveAttribute("entityShape");
		slot.Itemstack.Attributes.RemoveAttribute("entityTextureID");
		slot.MarkDirty();
		if (@string != null && array != null)
		{
			Entity val = ((CollectibleObject)this).api.World.ClassRegistry.CreateEntity(@string);
			if (val != null)
			{
				BinaryReader binaryReader = new BinaryReader(new MemoryStream(array));
				val.FromBytes(binaryReader, false);
				Vec3d val2 = new Vec3d((double)((float)blockSel.Position.X + 0.5f), (double)((float)blockSel.Position.Y + 0.5f), (double)((float)blockSel.Position.Z + 0.5f));
				val2 = (Vec3d)(blockSel.Face.Index switch
				{
					0 => val2.Add(0.0, 0.0, -1.0), 
					1 => val2.Add(1.0, 0.0, 0.0), 
					2 => val2.Add(0.0, 0.0, 1.0), 
					3 => val2.Add(-1.0, 0.0, 0.0), 
					4 => val2.Add(0.0, 1.0, 0.0), 
					5 => val2.Add(0.0, -1.0, 0.0), 
					_ => val2.Add(0.0, 1.0, 0.0), 
				});
				val.Pos.SetPos(val2);
				val.ServerPos.SetPos(val2);
				val.PositionBeforeFalling.Set(val2);
				((CollectibleObject)this).api.World.SpawnEntity(val);
			}
		}
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		if (!(world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityCage blockEntityCage))
		{
			return false;
		}
		if (!world.Claims.TryAccess(byPlayer, blockSel.Position, (EnumBlockAccessFlags)1))
		{
			return false;
		}
		ItemStack val = null;
		CollectibleObject block = (CollectibleObject)(object)world.BlockAccessor.GetBlock(((RegistryObject)this).CodeWithVariant("side", "south"));
		val = ((block != null) ? new ItemStack(block, 1) : ((base.Drops.Length == 0 || !(base.Drops[0].ResolvedItemstack.Collectible is BlockCage)) ? new ItemStack((Block)(object)this, 1) : new ItemStack(base.Drops[0].ResolvedItemstack.Collectible, 1)));
		if (blockEntityCage.EntityClass != null && blockEntityCage.EntityData != null && blockEntityCage.EntityName != null)
		{
			val.Attributes.SetString("entityName", blockEntityCage.EntityName);
			val.Attributes.SetString("entityClass", blockEntityCage.EntityClass);
			val.Attributes.SetBytes("entityData", blockEntityCage.EntityData);
			if (blockEntityCage.EntityShape != null)
			{
				val.Attributes.SetString("entityShape", blockEntityCage.EntityShape);
			}
			if (blockEntityCage.EntityTextureID != -1)
			{
				val.Attributes.SetInt("entityTextureID", blockEntityCage.EntityTextureID);
			}
		}
		if (byPlayer.InventoryManager.TryGiveItemstack(val, false))
		{
			world.BlockAccessor.SetBlock(0, blockSel.Position);
			world.PlaySoundAt(new AssetLocation("sounds/block/planks"), (double)blockSel.Position.X + 0.5, (double)blockSel.Position.Y, (double)blockSel.Position.Z + 0.5, byPlayer, false, 32f, 1f);
			return true;
		}
		return false;
	}

	public override string GetHeldItemName(ItemStack itemStack)
	{
		string text = ResolvedEntityName(itemStack);
		if (text != null)
		{
			return ((CollectibleObject)this).GetHeldItemName(itemStack) + " (" + text + ")";
		}
		return ((CollectibleObject)this).GetHeldItemName(itemStack);
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		((Block)this).GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		string text = ResolvedEntityName(inSlot.Itemstack);
		if (text != null)
		{
			dsc.AppendLine(text);
		}
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		return ArrayExtensions.Append<WorldInteraction>((WorldInteraction[])(object)new WorldInteraction[1]
		{
			new WorldInteraction
			{
				MouseButton = (EnumMouseButton)2,
				ActionLangCode = "xskills:blockhelp-cage-pickup"
			}
		}, ((Block)this).GetPlacedBlockInteractionHelp(world, selection, forPlayer));
	}

	public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		string text = ResolvedEntityName(inSlot.Itemstack);
		if (text == null)
		{
			return ArrayExtensions.Append<WorldInteraction>((WorldInteraction[])(object)new WorldInteraction[2]
			{
				new WorldInteraction
				{
					MouseButton = (EnumMouseButton)2,
					ActionLangCode = "xskills:blockhelp-cage-catch"
				},
				new WorldInteraction
				{
					MouseButton = (EnumMouseButton)2,
					HotKeyCode = "sneak",
					ActionLangCode = "xskills:blockhelp-cage-place"
				}
			}, ((CollectibleObject)this).GetHeldInteractionHelp(inSlot));
		}
		WorldInteraction[] array = new WorldInteraction[2];
		WorldInteraction val = new WorldInteraction();
		val.MouseButton = (EnumMouseButton)2;
		val.ActionLangCode = Lang.Get("xskills:blockhelp-cage-release", new object[1] { text });
		array[0] = val;
		array[1] = new WorldInteraction
		{
			MouseButton = (EnumMouseButton)2,
			HotKeyCode = "sneak",
			ActionLangCode = "xskills:blockhelp-cage-place"
		};
		return ArrayExtensions.Append<WorldInteraction>((WorldInteraction[])(object)array, ((CollectibleObject)this).GetHeldInteractionHelp(inSlot));
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		ItemStack val = ((Block)this).OnPickBlock(world, pos);
		if (!(world.BlockAccessor.GetBlockEntity(pos) is BlockEntityCage { EntityName: not null } blockEntityCage))
		{
			return val;
		}
		val.Attributes.SetString("entityName", blockEntityCage.EntityName);
		val.Attributes.SetString("entityClass", blockEntityCage.EntityClass);
		val.Attributes.SetBytes("entityData", blockEntityCage.EntityData);
		val.Attributes.SetString("entityShape", blockEntityCage.EntityShape);
		val.Attributes.SetInt("entityTextureID", blockEntityCage.EntityTextureID);
		return val;
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		string @string = itemstack.Attributes.GetString("entityName", (string)null);
		string string2 = itemstack.Attributes.GetString("entityShape", (string)null);
		int @int = itemstack.Attributes.GetInt("entityTextureID", -1);
		if (@string == null || @int == -1 || string2 == null)
		{
			((CollectibleObject)this).OnBeforeRender(capi, itemstack, target, ref renderinfo);
			return;
		}
		string key = @string + "-" + @int;
		if (!MeshRefs.ContainsKey(key))
		{
			MeshData val = default(MeshData);
			capi.Tesselator.TesselateBlock((Block)(object)this, ref val);
			CagedEntityMeshData cagedEntityMeshData = new CagedEntityMeshData(capi, @string, @int, string2);
			cagedEntityMeshData.Generate();
			val.AddMeshData(cagedEntityMeshData.MeshData);
			MeshRefs[key] = capi.Render.UploadMultiTextureMesh(val);
		}
		renderinfo.ModelRef = MeshRefs[key];
	}

	public override void OnCollectTextures(ICoreAPI api, ITextureLocationDictionary textureDict)
	{
		((Block)this).OnCollectTextures(api, textureDict);
		lock (this)
		{
			foreach (EntityProperties entityType in api.World.EntityTypes)
			{
				bool flag = false;
				JsonObject[] behaviorsAsJsonObj = ((EntitySidedProperties)entityType.Client).BehaviorsAsJsonObj;
				foreach (JsonObject val in behaviorsAsJsonObj)
				{
					if (val["code"].AsString((string)null) == "XSkillsAnimal")
					{
						JsonObject obj = val["catchable"];
						if (obj != null && obj.AsBool(false))
						{
							Scalings[entityType.Code.Path] = val["scale"].AsFloat(1f);
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					continue;
				}
				CapturedEntityTextures capturedEntityTextures = new CapturedEntityTextures();
				if (entityType.Client.FirstTexture != null)
				{
					CollectTexture(api, textureDict, capturedEntityTextures, entityType.Client.FirstTexture, entityType.Code);
					if (entityType.Client.FirstTexture.Alternates != null)
					{
						CompositeTexture[] alternates = entityType.Client.FirstTexture.Alternates;
						foreach (CompositeTexture texture in alternates)
						{
							CollectTexture(api, textureDict, capturedEntityTextures, texture, entityType.Code);
						}
					}
				}
				EntitiyTextureIds[entityType.Code.GetName()] = capturedEntityTextures;
			}
		}
	}

	private void CollectTexture(ICoreAPI api, ITextureLocationDictionary textureDict, CapturedEntityTextures entityTextures, CompositeTexture texture, AssetLocation code)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		texture.Bake(api.Assets);
		textureDict.AddTextureLocation(new AssetLocationAndSource(texture.Baked.BakedName, "Entity code ", code, -1));
		entityTextures.TextureIds[entityTextures.TextureIds.Count] = textureDict[new AssetLocationAndSource(texture.Baked.BakedName)];
	}
}
