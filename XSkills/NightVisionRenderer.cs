using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using XLib.XLeveling;

namespace XSkills;

public class NightVisionRenderer : IRenderer, IDisposable
{
	private MeshRef quadRef;

	private ICoreClientAPI capi;

	private float nightVisionIntensity;

	private PlayerAbility ability;

	internal EnumNightVisionMode Mode { get; set; }

	public float NightVisionBrightness { get; set; }

	public IShaderProgram Shader { get; internal set; }

	public double RenderOrder => 0.85;

	public int RenderRange => 1;

	public NightVisionRenderer(ICoreClientAPI capi, IShaderProgram shader)
	{
		this.capi = capi;
		Shader = shader;
		MeshData customQuadModelData = QuadMeshUtil.GetCustomQuadModelData(-1f, -1f, 0f, 2f, 2f);
		customQuadModelData.Rgba = null;
		quadRef = capi.Render.UploadMesh(customQuadModelData);
		nightVisionIntensity = 0f;
		Mode = EnumNightVisionMode.Default;
		NightVisionBrightness = 0f;
		ability = null;
	}

	public void Dispose()
	{
		capi.Render.DeleteMesh(quadRef);
		((IDisposable)Shader).Dispose();
	}

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
	{
		EntityPlayer entity = ((IPlayer)capi.World.Player).Entity;
		if (entity == null)
		{
			return;
		}
		if (ability == null)
		{
			if (!(XLeveling.Instance(((Entity)entity).Api).GetSkill("survival", false) is Survival survival))
			{
				return;
			}
			PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
			object obj;
			if (behavior == null)
			{
				obj = null;
			}
			else
			{
				PlayerSkill obj2 = behavior[((Skill)survival).Id];
				obj = ((obj2 != null) ? obj2[survival.CatEyesId] : null);
			}
			ability = (PlayerAbility)obj;
			if (ability == null)
			{
				return;
			}
		}
		if (ability.Tier <= 0)
		{
			return;
		}
		IShaderProgram currentActiveShader = capi.Render.CurrentActiveShader;
		if (currentActiveShader != null)
		{
			currentActiveShader.Stop();
		}
		float num = (float)ability.Value(1, 0) / 1000f;
		deltaTime = Math.Min(deltaTime, num);
		int num2 = ((IWorldAccessor)capi.World).BlockAccessor.GetLightLevel(((Entity)entity).Pos.AsBlockPos, (EnumLightLevelType)3);
		IPlayer[] playersAround = ((IWorldAccessor)capi.World).GetPlayersAround(((Entity)entity).Pos.XYZ, 32f, 32f, (ActionConsumable<IPlayer>)null);
		foreach (IPlayer val in playersAround)
		{
			EntityPlayer entity2 = val.Entity;
			byte? obj3;
			if (entity2 == null)
			{
				obj3 = null;
			}
			else
			{
				byte[] lightHsv = ((Entity)entity2).LightHsv;
				obj3 = ((lightHsv != null) ? new byte?(lightHsv[2]) : null);
			}
			byte? b = obj3;
			byte valueOrDefault = b.GetValueOrDefault();
			if (valueOrDefault != 0)
			{
				valueOrDefault -= (byte)((Entity)val.Entity).Pos.DistanceTo(((Entity)entity).Pos);
				num2 = Math.Max(num2, valueOrDefault);
			}
		}
		float num3 = ((num2 == 0) ? 1.6f : 1f);
		float num4 = (1f - (float)num2 / 16f) * num3;
		if (num > 0f)
		{
			nightVisionIntensity = (nightVisionIntensity * (num - deltaTime) + num4 * deltaTime) / num;
		}
		else
		{
			nightVisionIntensity = num4;
		}
		if (nightVisionIntensity > 0f && (Mode & EnumNightVisionMode.Deactivated) == 0)
		{
			Shader.Use();
			capi.Render.GlToggleBlend(true, (EnumBlendMode)5);
			capi.Render.GLDisableDepthTest();
			Shader.BindTexture2D("primaryScene", capi.Render.FrameBuffers[0].ColorTextureIds[0], 0);
			Shader.Uniform("intensity", nightVisionIntensity);
			Shader.Uniform("brightness", NightVisionBrightness + (float)ability.Value(0, 0));
			capi.Render.RenderMesh(quadRef);
			Shader.Stop();
		}
		if (currentActiveShader != null)
		{
			currentActiveShader.Use();
		}
	}
}
