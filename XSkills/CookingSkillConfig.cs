using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using ProtoBuf;
using XLib.XLeveling;

namespace XSkills;

[ProtoContract]
public class CookingSkillConfig : CustomSkillConfig
{
	[ProtoMember(1)]
	[DefaultValue(0.0004f)]
	public float expBase = 0.0004f;

	[ProtoMember(2)]
	[DefaultValue(0.1f)]
	public float fruitPressExpPerLitre = 0.1f;

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
					"fruitPressExpPerLitre",
					fruitPressExpPerLitre.ToString(cultureInfo)
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
			value.TryGetValue("fruitPressExpPerLitre", out value2);
			if (value2 != null)
			{
				float.TryParse(value2, style, provider, out fruitPressExpPerLitre);
			}
		}
	}
}
