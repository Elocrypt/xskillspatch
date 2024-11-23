using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using ProtoBuf;
using XLib.XLeveling;

namespace XSkills;

[ProtoContract]
public class SurvivalSkillConfig : CustomSkillConfig
{
	[ProtoMember(1)]
	[DefaultValue(0.5f)]
	public float expLoss = 0.5f;

	[ProtoMember(2)]
	[DefaultValue(3f)]
	public float invSwitchCD = 3f;

	[ProtoMember(3)]
	[DefaultValue(false)]
	public bool allowCatEyesToggle;

	public override Dictionary<string, string> Attributes
	{
		get
		{
			CultureInfo cultureInfo = new CultureInfo("en-US");
			return new Dictionary<string, string>
			{
				{
					"expLoss",
					expLoss.ToString(cultureInfo)
				},
				{
					"invSwitchCD",
					invSwitchCD.ToString(cultureInfo)
				},
				{
					"allowCatEyesToggle",
					allowCatEyesToggle.ToString(cultureInfo)
				}
			};
		}
		set
		{
			NumberStyles style = NumberStyles.Any;
			CultureInfo provider = new CultureInfo("en-US");
			value.TryGetValue("expLoss", out var value2);
			if (value2 != null)
			{
				float.TryParse(value2, style, provider, out expLoss);
			}
			value.TryGetValue("invSwitchCD", out value2);
			if (value2 != null)
			{
				float.TryParse(value2, style, provider, out invSwitchCD);
			}
			value.TryGetValue("allowCatEyesToggle", out value2);
			if (value2 != null)
			{
				bool.TryParse(value2, out allowCatEyesToggle);
			}
		}
	}
}
