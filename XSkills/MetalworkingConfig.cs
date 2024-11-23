using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using ProtoBuf;
using XLib.XLeveling;

namespace XSkills;

[ProtoContract]
public class MetalworkingConfig : CustomSkillConfig
{
	[ProtoMember(1)]
	[DefaultValue(1f)]
	public float expBase = 1f;

	[ProtoMember(2)]
	[DefaultValue(0.02f)]
	public float expPerHit = 0.02f;

	[ProtoMember(3)]
	[DefaultValue(0.75f)]
	public float helveHammerPenalty = 0.75f;

	[ProtoMember(4)]
	[DefaultValue(true)]
	public bool useVanillaBits = true;

	[ProtoMember(5)]
	[DefaultValue(21)]
	public int bitsForIngot = 21;

	[ProtoMember(6)]
	[DefaultValue(0.5f)]
	public float chiselRecipesRatio = 0.5f;

	[ProtoMember(7)]
	[DefaultValue(0.5f)]
	public bool adjustRecipes;

	[ProtoMember(8)]
	[DefaultValue(false)]
	public bool allowFinishingTouchExploit;

	public override Dictionary<string, string> Attributes
	{
		get
		{
			CultureInfo cultureInfo = new CultureInfo("en-US");
			return new Dictionary<string, string>
			{
				{
					"expBase",
					expBase.ToString(cultureInfo)
				},
				{
					"expPerHit",
					expPerHit.ToString(cultureInfo)
				},
				{
					"helveHammerPenalty",
					helveHammerPenalty.ToString(cultureInfo)
				},
				{
					"useVanillaBits",
					useVanillaBits.ToString(cultureInfo)
				},
				{
					"bitsForIngot",
					bitsForIngot.ToString(cultureInfo)
				},
				{
					"chiselRecipesRatio",
					chiselRecipesRatio.ToString(cultureInfo)
				},
				{
					"allowFinishingTouchExploit",
					allowFinishingTouchExploit.ToString(cultureInfo)
				}
			};
		}
		set
		{
			NumberStyles style = NumberStyles.Any;
			CultureInfo provider = new CultureInfo("en-US");
			value.TryGetValue("expBase", out var value2);
			if (value2 != null)
			{
				float.TryParse(value2, style, provider, out expBase);
			}
			value.TryGetValue("expPerHit", out value2);
			if (value2 != null)
			{
				float.TryParse(value2, style, provider, out expPerHit);
			}
			value.TryGetValue("helveHammerPenalty", out value2);
			if (value2 != null)
			{
				float.TryParse(value2, style, provider, out helveHammerPenalty);
			}
			value.TryGetValue("useVanillaBits", out value2);
			if (value2 != null)
			{
				bool.TryParse(value2, out useVanillaBits);
			}
			value.TryGetValue("bitsForIngot", out value2);
			if (value2 != null)
			{
				int.TryParse(value2, style, provider, out bitsForIngot);
			}
			value.TryGetValue("chiselRecipesRatio", out value2);
			if (value2 != null)
			{
				float.TryParse(value2, style, provider, out chiselRecipesRatio);
			}
			value.TryGetValue("allowFinishingTouchExploit", out value2);
			if (value2 != null)
			{
				bool.TryParse(value2, out allowFinishingTouchExploit);
			}
			if (expPerHit > 1f)
			{
				expPerHit *= 0.01f;
			}
			if (helveHammerPenalty > 1f)
			{
				helveHammerPenalty *= 0.01f;
			}
			if (chiselRecipesRatio > 1f)
			{
				chiselRecipesRatio *= 0.01f;
			}
		}
	}
}
