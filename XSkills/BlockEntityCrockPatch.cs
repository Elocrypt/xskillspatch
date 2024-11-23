using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace XSkills;

[HarmonyPatch(typeof(BlockEntityCrock))]
public class BlockEntityCrockPatch
{
	[HarmonyPatch(/*Could not decode attribute arguments.*/)]
	public static void Postfix(ref InventoryGeneric ___inv)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		int num = ((___inv != null) ? (((InventoryBase)___inv).Count + 1) : 7);
		___inv = new InventoryGeneric(num, (string)null, (ICoreAPI)null, (NewSlotDelegate)null);
	}
}
