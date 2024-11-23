using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills;

public class PotteryHelper
{
	public static bool AddClay(BlockEntityClayForm cfe, ItemSlot slot, EntityAgent entity)
	{
		if (cfe == null || slot == null)
		{
			return false;
		}
		if (cfe.AvailableVoxels <= 0)
		{
			ItemStack itemstack = slot.Itemstack;
			if (((itemstack != null && itemstack.StackSize != 0) ? 1 : 0) <= (false ? 1 : 0))
			{
				return false;
			}
			slot.TakeOut(1);
			slot.MarkDirty();
			cfe.AvailableVoxels += 25;
			if (((Entity)(entity?)).Api == null)
			{
				return true;
			}
			if (!(XLeveling.Instance(((Entity)entity).Api).GetSkill("pottery", false) is Pottery pottery))
			{
				return true;
			}
			PlayerSkillSet behavior = ((Entity)entity).GetBehavior<PlayerSkillSet>();
			PlayerSkill val = ((behavior != null) ? behavior[((Skill)pottery).Id] : null);
			if (val == null)
			{
				return true;
			}
			PlayerAbility val2 = val[pottery.ThriftId];
			if (val2 == null)
			{
				return true;
			}
			cfe.AvailableVoxels += val2.Value(0, 0);
			return true;
		}
		return false;
	}

	public static void FinishRecipe(BlockEntityClayForm clayForm, ItemSlot slot, EntityAgent entity)
	{
		if (clayForm == null)
		{
			return;
		}
		int num = Math.Min(16, ((LayeredVoxelRecipe<ClayFormingRecipe>)(object)clayForm.SelectedRecipe).QuantityLayers);
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					if (((LayeredVoxelRecipe<ClayFormingRecipe>)(object)clayForm.SelectedRecipe).Voxels[i, j, k] != clayForm.Voxels[i, j, k])
					{
						if (((LayeredVoxelRecipe<ClayFormingRecipe>)(object)clayForm.SelectedRecipe).Voxels[i, j, k])
						{
							if (clayForm.AvailableVoxels < 0 && !AddClay(clayForm, slot, entity))
							{
								return;
							}
							clayForm.AvailableVoxels--;
						}
						else
						{
							clayForm.AvailableVoxels++;
						}
					}
					clayForm.Voxels[i, j, k] = ((LayeredVoxelRecipe<ClayFormingRecipe>)(object)clayForm.SelectedRecipe).Voxels[i, j, k];
				}
			}
		}
	}

	public static float FinishedProportion(BlockEntityClayForm clayForm)
	{
		if (((clayForm != null) ? clayForm.SelectedRecipe : null) == null)
		{
			return 0f;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = Math.Min(16, ((LayeredVoxelRecipe<ClayFormingRecipe>)(object)clayForm.SelectedRecipe).QuantityLayers);
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < num4; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					if (((LayeredVoxelRecipe<ClayFormingRecipe>)(object)clayForm.SelectedRecipe).Voxels[i, j, k])
					{
						num2++;
						if (clayForm.Voxels[i, j, k])
						{
							num3++;
							num++;
						}
					}
					else if (clayForm.Voxels[i, j, k])
					{
						num3++;
					}
				}
			}
		}
		float num5 = ((num3 < num2) ? ((float)num3 / (float)num2) : ((float)num2 / (float)num3));
		float num6 = num / num3;
		return num5 * num6;
	}

	public static int CountVoxels(ClayFormingRecipe recipe)
	{
		int num = 0;
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < ((LayeredVoxelRecipe<ClayFormingRecipe>)(object)recipe).QuantityLayers; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					if (((LayeredVoxelRecipe<ClayFormingRecipe>)(object)recipe).Voxels[i, j, k])
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	public static bool ApplyOnStack(IPlayer byPlayer, IWorldAccessor world, ItemSlot outputSlot)
	{
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		object obj;
		if (byPlayer == null)
		{
			obj = null;
		}
		else
		{
			EntityPlayer entity = byPlayer.Entity;
			if (entity == null)
			{
				obj = null;
			}
			else
			{
				XLeveling modSystem = ((Entity)entity).Api.ModLoader.GetModSystem<XLeveling>(true);
				obj = ((modSystem != null) ? modSystem.GetSkill("pottery", false) : null);
			}
		}
		if (!(obj is Pottery pottery) || world == null)
		{
			return false;
		}
		PlayerSkillSet behavior = ((Entity)byPlayer.Entity).GetBehavior<PlayerSkillSet>();
		object obj2;
		if (behavior == null)
		{
			obj2 = null;
		}
		else
		{
			PlayerSkill obj3 = behavior[((Skill)pottery).Id];
			obj2 = ((obj3 != null) ? obj3[pottery.InspirationId] : null);
		}
		PlayerAbility val = (PlayerAbility)obj2;
		if (val == null)
		{
			return false;
		}
		if (val.Tier <= 0)
		{
			return false;
		}
		string name = ((RegistryObject)outputSlot.Itemstack.Collectible).Code.GetName();
		if (!pottery.InspirationCollectibles.TryGetValue(name, out var value))
		{
			return false;
		}
		if (value == null)
		{
			value = new List<CollectibleObject>();
			pottery.InspirationCollectibles[name] = value;
			int length = name.LastIndexOf('-');
			string value2 = ((!name.EndsWith("-burned") && !name.EndsWith("-burnt")) ? name : name.Substring(0, length));
			foreach (CollectibleObject collectible in world.Collectibles)
			{
				if (((RegistryObject)collectible).Code.Path.Contains(value2) && collectible != outputSlot.Itemstack.Collectible && !((RegistryObject)collectible).Code.Path.Contains("raw"))
				{
					value.Add(collectible);
				}
			}
		}
		if (value.Count <= 0)
		{
			return true;
		}
		if (world.Rand.NextDouble() < (double)(val.FValue(0, 0f) / (float)outputSlot.Itemstack.StackSize))
		{
			CollectibleObject obj4 = value[world.Rand.Next(value.Count - 1)];
			ItemStack val2 = new ItemStack(obj4, outputSlot.Itemstack.StackSize);
			string text = obj4.Attributes["defaultType"].AsString((string)null);
			if (text != null)
			{
				val2.Attributes.SetString("type", text);
			}
			outputSlot.Itemstack = val2;
			outputSlot.MarkDirty();
		}
		return true;
	}
}
