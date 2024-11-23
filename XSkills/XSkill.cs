using System.Collections.Generic;
using XLib.XLeveling;

namespace XSkills;

public class XSkill : Skill
{
	protected static List<string> MissingConfig;

	public XSkill(string name, string displayName = null, string group = null, int expBase = 200, float expMult = 1.33f, int maxLevel = 25)
		: base(name, displayName, group, expBase, expMult, maxLevel)
	{
		if (MissingConfig == null)
		{
			MissingConfig = new List<string>();
		}
		foreach (string item in MissingConfig)
		{
			if (item == ((Skill)this).Name)
			{
				return;
			}
		}
		MissingConfig.Add(((Skill)this).Name);
	}

	public override void OnConfigReceived()
	{
		MissingConfig.Remove(((Skill)this).Name);
		if (MissingConfig.Count == 0)
		{
			XSkills.DoHarmonyPatch(((Skill)this).XLeveling.Api);
		}
	}
}
