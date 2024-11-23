using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

[HarmonyPatch(typeof(BlockSmeltingContainer))]
public class BlockSmeltingContainerPatch
{
	public static bool Prepare(MethodBase original)
	{
		XSkills instance = XSkills.Instance;
		if (instance == null)
		{
			return false;
		}
		instance.Skills.TryGetValue("metalworking", out var value);
		if (!(value is Metalworking metalworking) || !((Skill)metalworking).Enabled)
		{
			return false;
		}
		if (original == null)
		{
			return true;
		}
		if (original.Name == "CanSmelt")
		{
			return ((Skill)metalworking)[metalworking.SenseOfTime].Enabled;
		}
		return true;
	}

	[HarmonyPostfix]
	[HarmonyPatch("DoSmelt")]
	public static void DoSmeltPostfix(IWorldAccessor world, ISlotProvider cookingSlotsProvider, ItemSlot outputSlot)
	{
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		IPlayer ownerFromInventory = CookingUtil.GetOwnerFromInventory((InventoryBase)(object)((cookingSlotsProvider is InventoryBase) ? cookingSlotsProvider : null));
		if (((ownerFromInventory != null) ? ownerFromInventory.Entity : null) == null)
		{
			return;
		}
		XLeveling modSystem = ((Entity)ownerFromInventory.Entity).Api.ModLoader.GetModSystem<XLeveling>(true);
		if (!(((modSystem != null) ? modSystem.GetSkill("metalworking", false) : null) is Metalworking metalworking))
		{
			return;
		}
		PlayerSkillSet behavior = ((Entity)ownerFromInventory.Entity).GetBehavior<PlayerSkillSet>();
		object obj;
		if (behavior == null)
		{
			obj = null;
		}
		else
		{
			PlayerSkill obj2 = behavior[((Skill)metalworking).Id];
			obj = ((obj2 != null) ? obj2[metalworking.SenseOfTime] : null);
		}
		if (obj == null || ((PlayerAbility)obj).Tier <= 0)
		{
			return;
		}
		BlockPos pos = outputSlot.Inventory.Pos;
		Block val = ((pos != (BlockPos)null) ? ((IBlockAccessor)world.BulkBlockAccessor).GetBlock(pos) : null);
		InventoryBase inventory = outputSlot.Inventory;
		InventoryBase obj3 = ((inventory is InventorySmelting) ? inventory : null);
		bool flag = obj3 == null || obj3[1].Empty;
		if (!(val != null && flag))
		{
			return;
		}
		double totalHours = world.Calendar.TotalHours;
		double @double = ((TreeAttribute)((Entity)ownerFromInventory.Entity).Attributes).GetDouble("xskillsCookingMsg", 0.0);
		if (totalHours > @double + 0.333)
		{
			((TreeAttribute)((Entity)ownerFromInventory.Entity).Attributes).SetDouble("xskillsCookingMsg", totalHours);
			world.PlaySoundFor(new AssetLocation("sounds/tutorialstepsuccess.ogg"), ownerFromInventory, true, 32f, 1f);
			string text = Lang.Get("xskills:cooking-finished", new object[1] { val.GetPlacedBlockName(world, pos) + " (" + pos.X + ", " + pos.X + pos.Y + ", " + pos.Z + ")" });
			IPlayer obj4 = ((ownerFromInventory is IServerPlayer) ? ownerFromInventory : null);
			if (obj4 != null)
			{
				((IServerPlayer)obj4).SendMessage(0, text, (EnumChatType)4, (string)null);
			}
		}
	}
}
