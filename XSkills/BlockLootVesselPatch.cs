using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockLootVessel))]
public class BlockLootVesselPatch
{
	[HarmonyPostfix]
	[HarmonyPatch("GetDrops")]
	public static void GetDropsPostfix(BlockLootVessel __instance, ref ItemStack[] __result, IWorldAccessor world)
	{
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Expected O, but got Unknown
		string text = ((RegistryObject)__instance).LastCodePart(0);
		float num = (float)world.Rand.NextDouble();
		float num2 = 0.5f;
		string text2;
		switch (text)
		{
		default:
			return;
		case "seed":
			text2 = "farming";
			num = 0.5f;
			break;
		case "food":
			text2 = "cooking";
			num = 1f;
			break;
		case "forage":
			text2 = ((num > 0.66f) ? "survival" : ((!(num > 0.33f)) ? "forestry" : "digging"));
			num = 3f;
			break;
		case "ore":
			text2 = "mining";
			num = 1f;
			break;
		case "tool":
			text2 = ((!(num > 0.5f)) ? "combat" : "metalworking");
			num = 2f;
			break;
		case "farming":
			if (num > 0.5f)
			{
				text2 = "farming";
				num = 0.5f;
			}
			else
			{
				text2 = "husbandry";
				num = 2f;
			}
			break;
		}
		if (text2 == null)
		{
			return;
		}
		string text3;
		switch (text2.Length)
		{
		case 7:
			switch (text2[0])
			{
			case 'f':
				if (!(text2 == "farming"))
				{
					return;
				}
				text3 = "darkgreen";
				break;
			case 'c':
				if (!(text2 == "cooking"))
				{
					return;
				}
				text3 = "cherryred";
				break;
			case 'd':
				if (!(text2 == "digging"))
				{
					return;
				}
				text3 = "orange";
				break;
			case 'p':
				if (!(text2 == "pottery"))
				{
					return;
				}
				text3 = "darkbeige";
				break;
			default:
				return;
			}
			break;
		case 8:
			switch (text2[0])
			{
			default:
				return;
			case 's':
				if (!(text2 == "survival"))
				{
					return;
				}
				text3 = "olive";
				break;
			case 'f':
				if (!(text2 == "forestry"))
				{
					return;
				}
				text3 = "darkolive";
				break;
			}
			break;
		case 6:
			switch (text2[0])
			{
			default:
				return;
			case 'm':
				if (!(text2 == "mining"))
				{
					return;
				}
				text3 = "darkgray";
				break;
			case 'c':
				if (!(text2 == "combat"))
				{
					return;
				}
				text3 = "purpleorange";
				break;
			}
			break;
		case 12:
			if (!(text2 == "metalworking"))
			{
				return;
			}
			text3 = "brickred";
			break;
		case 9:
			if (!(text2 == "husbandry"))
			{
				return;
			}
			text3 = "orangebrown";
			break;
		case 18:
			if (!(text2 == "temporaladaptation"))
			{
				return;
			}
			text3 = "gray";
			break;
		default:
			return;
		}
		num /= 3f;
		if ((double)num < world.Rand.NextDouble())
		{
			return;
		}
		num = (float)world.Rand.NextDouble();
		num2 = ((num > 0.9f) ? (num2 * 1f) : ((num > 0.6f) ? (num2 * 0.5f) : ((!(num > 0.3f)) ? (num2 * 0.125f) : (num2 * 0.25f))));
		AssetLocation val = new AssetLocation("xlib", "skillbook-aged-" + text3);
		Item item = world.GetItem(val);
		if (item == null)
		{
			return;
		}
		XLeveling val2 = XLeveling.Instance(world.Api);
		if (val2 != null)
		{
			Skill skill = val2.GetSkill(text2, false);
			if (skill != null)
			{
				num2 *= skill.ExpBase;
				ItemStack val3 = new ItemStack(item, 1);
				val3.Attributes.SetString("skill", text2);
				val3.Attributes.SetFloat("experience", num2);
				__result = CollectionExtensions.AddToArray<ItemStack>(__result, val3);
			}
		}
	}
}
