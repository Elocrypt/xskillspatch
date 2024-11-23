using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using ProtoBuf;
using XLib.XLeveling;

namespace XSkills;

[ProtoContract]
public class MiningSkillConfig : CustomSkillConfig
{
	[ProtoMember(1)]
	public List<string> geologistBlacklist;

	[ProtoMember(2)]
	[DefaultValue(0.5f)]
	public float oreRarityExpMultiplier;

	[ProtoMember(3)]
	[DefaultValue(0.5f)]
	public float oreDepthExpMultiplier;

	public override Dictionary<string, string> Attributes
	{
		get
		{
			CultureInfo cultureInfo = new CultureInfo("en-US");
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string value = string.Join(", ", geologistBlacklist);
			dictionary.Add("geologistBlacklist", value);
			dictionary.Add("oreRarityExpMultiplier", oreRarityExpMultiplier.ToString(cultureInfo));
			dictionary.Add("oreDepthExpMultiplier", oreDepthExpMultiplier.ToString(cultureInfo));
			return dictionary;
		}
		set
		{
			NumberStyles style = NumberStyles.Any;
			CultureInfo provider = new CultureInfo("en-US");
			value.TryGetValue("geologistBlacklist", out var value2);
			if (value2 == null)
			{
				return;
			}
			string[] array = value2.Split(new char[1] { ',' });
			for (int i = 0; i < array.Length; i++)
			{
				string item = array[i].Trim();
				if (!geologistBlacklist.Contains(item))
				{
					geologistBlacklist.Add(item);
				}
			}
			value.TryGetValue("oreRarityExpMultiplier", out value2);
			if (value2 != null)
			{
				float.TryParse(value2, style, provider, out oreRarityExpMultiplier);
			}
			value.TryGetValue("oreDepthExpMultiplier", out value2);
			if (value2 != null)
			{
				float.TryParse(value2, style, provider, out oreDepthExpMultiplier);
			}
			if (oreRarityExpMultiplier > 1f)
			{
				oreRarityExpMultiplier *= 0.01f;
			}
			if (oreDepthExpMultiplier > 1f)
			{
				oreDepthExpMultiplier *= 0.01f;
			}
		}
	}

	public MiningSkillConfig()
	{
		geologistBlacklist = new List<string>();
		oreRarityExpMultiplier = 0.5f;
		oreDepthExpMultiplier = 0.5f;
	}
}
