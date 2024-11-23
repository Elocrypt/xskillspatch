using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using ProtoBuf;
using XLib.XLeveling;

namespace XSkills;

[ProtoContract]
public class FarmingConfig : CustomSkillConfig
{
	[ProtoMember(1)]
	[DefaultValue(0.5f)]
	public float treeHarvestExp = 0.5f;

	public override Dictionary<string, string> Attributes
	{
		get
		{
			CultureInfo cultureInfo = new CultureInfo("en-US");
			return new Dictionary<string, string> { 
			{
				"treeHarvestExp",
				treeHarvestExp.ToString(cultureInfo)
			} };
		}
		set
		{
			NumberStyles style = NumberStyles.Any;
			CultureInfo provider = new CultureInfo("en-US");
			value.TryGetValue("treeHarvestExp", out var value2);
			if (value2 != null)
			{
				float.TryParse(value2, style, provider, out treeHarvestExp);
			}
		}
	}
}
