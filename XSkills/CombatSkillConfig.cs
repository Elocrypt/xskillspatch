using System.Collections.Generic;
using ProtoBuf;
using XLib.XLeveling;

namespace XSkills;

[ProtoContract]
public class CombatSkillConfig : CustomSkillConfig
{
	[ProtoMember(1)]
	public bool enableAbilitiesInPvP;

	public override Dictionary<string, string> Attributes
	{
		get
		{
			return new Dictionary<string, string> { 
			{
				"enableAbilitiesInPvP",
				enableAbilitiesInPvP.ToString()
			} };
		}
		set
		{
			value.TryGetValue("enableAbilitiesInPvP", out var value2);
			if (value2 != null)
			{
				bool.TryParse(value2, out enableAbilitiesInPvP);
			}
		}
	}
}
