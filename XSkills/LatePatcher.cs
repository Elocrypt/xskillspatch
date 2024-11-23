using System;
using System.Globalization;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace XSkills;

public class LatePatcher : ModSystem
{
	public override double ExecuteOrder()
	{
		return 100.0;
	}

	public override void AssetsLoaded(ICoreAPI api)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		((ModSystem)this).AssetsLoaded(api);
		if (EnumAppSideExtensions.IsClient(api.Side))
		{
			return;
		}
		foreach (EntityProperties entityType2 in api.World.EntityTypes)
		{
			float num = 0f;
			int num2 = -1;
			float num3 = 0f;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			flag = ((BaseSpawnConditions)(entityType2.Server.SpawnConditions?.Runtime?)).Group == "hostile";
			JsonObject[] behaviorsAsJsonObj = ((EntitySidedProperties)entityType2.Server).BehaviorsAsJsonObj;
			JsonObject[] array = behaviorsAsJsonObj;
			foreach (JsonObject val in array)
			{
				switch (val["code"].AsString((string)null))
				{
				case "taskai":
				{
					JsonObject[] array2 = val["aitasks"].AsArray();
					foreach (JsonObject val2 in array2)
					{
						string text = val2["code"].AsString((string)null);
						if (text == "meleeattack" || text == "melee")
						{
							num = Math.Max(val2["damage"].AsFloat(0f), num);
							num2 = Math.Max(val2["damageTier"].AsInt(0), num2);
						}
					}
					break;
				}
				case "health":
					num3 = val["maxhealth"].AsFloat(0f);
					break;
				case "multiply":
					flag2 = true;
					break;
				case "XSkillsEntity":
					val["xp"].AsFloat(0f);
					flag3 = true;
					break;
				case "XSkillsAnimal":
					flag4 = true;
					break;
				}
			}
			if (!flag2 && entityType2.Code.Path.Contains("male"))
			{
				AssetLocation val3 = new AssetLocation(entityType2.Code.Domain, entityType2.Code.Path.Replace("male", "female"));
				EntityProperties entityType = api.World.GetEntityType(val3);
				if (entityType != null)
				{
					array = ((EntitySidedProperties)entityType.Server).BehaviorsAsJsonObj;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i]["code"].AsString((string)null) == "multiply")
						{
							flag2 = true;
						}
					}
				}
			}
			if (num3 > 0f && ((num > 1f && num2 >= 0) || flag2) && !flag3)
			{
				JsonObject val4 = null;
				JsonObject val5 = null;
				int num4 = 0;
				float num5 = num3 * 0.025f + (num - 1f) * 0.05f + (float)num2 * 0.25f + (flag ? 0.25f : 0f);
				if (flag2 && !flag4)
				{
					num5 *= 0.5f;
					num4++;
					val4 = JsonObject.FromJson("{\"code\": \"XSkillsAnimal\", \"xp\": " + num5.ToString(new CultureInfo("en-US")) + ", \"catchable\": \"false\"}");
				}
				num4++;
				val5 = JsonObject.FromJson("{\"code\": \"XSkillsEntity\", \"xp\": " + num5.ToString(new CultureInfo("en-US")) + "}");
				int k = 0;
				JsonObject[] array3 = (JsonObject[])(object)new JsonObject[behaviorsAsJsonObj.Length + num4];
				for (; k < behaviorsAsJsonObj.Length; k++)
				{
					array3[k] = behaviorsAsJsonObj[k];
				}
				if (val5 != null)
				{
					array3[k] = val5;
					k++;
				}
				if (val4 != null)
				{
					array3[k] = val4;
				}
				((EntitySidedProperties)entityType2.Server).BehaviorsAsJsonObj = array3;
			}
		}
	}
}
