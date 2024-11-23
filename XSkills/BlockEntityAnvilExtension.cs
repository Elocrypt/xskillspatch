using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace XSkills;

public static class BlockEntityAnvilExtension
{
	public static int GetSplitCount(this BlockEntityAnvil anvil)
	{
		ItemStack workItemStack = anvil.WorkItemStack;
		if (workItemStack == null)
		{
			return 0;
		}
		return workItemStack.Attributes.GetInt("splitCounter", 0);
	}

	public static void SetSplitCount(this BlockEntityAnvil anvil, int value)
	{
		ItemStack workItemStack = anvil.WorkItemStack;
		if (workItemStack != null)
		{
			workItemStack.Attributes.SetInt("splitCounter", value);
		}
	}

	public static int GetHitCount(this BlockEntityAnvil anvil)
	{
		ItemStack workItemStack = anvil.WorkItemStack;
		if (workItemStack == null)
		{
			return 0;
		}
		return workItemStack.Attributes.GetInt("hitCounter", 0);
	}

	public static void SetHitCount(this BlockEntityAnvil anvil, int value)
	{
		ItemStack workItemStack = anvil.WorkItemStack;
		if (workItemStack != null)
		{
			workItemStack.Attributes.SetInt("hitCounter", value);
		}
	}

	public static void SetUsedByPlayer(this BlockEntityAnvil anvil, IPlayer player)
	{
		ItemStack workItemStack = anvil.WorkItemStack;
		if (workItemStack != null)
		{
			workItemStack.Attributes.SetString("placedBy", player.PlayerUID);
		}
	}

	public static IPlayer GetUsedByPlayer(this BlockEntityAnvil anvil)
	{
		IWorldAccessor world = ((BlockEntity)anvil).Api.World;
		ItemStack workItemStack = anvil.WorkItemStack;
		return world.PlayerByUid((workItemStack != null) ? workItemStack.Attributes.GetString("placedBy", (string)null) : null);
	}

	public static bool GetHelveHammered(this BlockEntityAnvil anvil)
	{
		ItemStack workItemStack = anvil.WorkItemStack;
		if (workItemStack == null)
		{
			return false;
		}
		return workItemStack.Attributes.GetBool("helveHammered", false);
	}

	public static void SetHelveHammered(this BlockEntityAnvil anvil, bool value)
	{
		ItemStack workItemStack = anvil.WorkItemStack;
		if (workItemStack != null)
		{
			workItemStack.Attributes.SetBool("helveHammered", value);
		}
	}

	public static bool GetWasPlate(this BlockEntityAnvil anvil)
	{
		ItemStack workItemStack = anvil.WorkItemStack;
		if (workItemStack == null)
		{
			return false;
		}
		return workItemStack.Attributes.GetBool("wasPlate", false);
	}

	public static void SetWasPlate(this BlockEntityAnvil anvil, bool value)
	{
		ItemStack workItemStack = anvil.WorkItemStack;
		if (workItemStack != null)
		{
			workItemStack.Attributes.SetBool("wasPlate", value);
		}
	}

	public static Vec3i[] FindFreeVoxels(this BlockEntityAnvil anvil, int count, Vec3i center, int range)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		bool[,,] recipeVoxels = anvil.recipeVoxels;
		List<Vec3i> list = new List<Vec3i>();
		int num = Math.Max(center.X - range, 0);
		int num2 = Math.Min(center.X + range, 16);
		int num3 = Math.Max(center.Y - range, 0);
		int num4 = Math.Min(center.Y + range, ((LayeredVoxelRecipe<SmithingRecipe>)(object)anvil.SelectedRecipe).QuantityLayers);
		int num5 = Math.Max(center.Z - range, 0);
		int num6 = Math.Min(center.Z + range, 16);
		for (int i = num3; i < num4; i++)
		{
			for (int j = num5; j < num6; j++)
			{
				for (int k = num; k < num2; k++)
				{
					if (anvil.Voxels[k, i, j] == 0 && recipeVoxels[k, i, j])
					{
						list.Add(new Vec3i(k, i, j));
						if (list.Count >= count)
						{
							return list.ToArray();
						}
					}
				}
			}
		}
		return list.ToArray();
	}

	public static float FinishedProportion(this BlockEntityAnvil anvil)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		bool[,,] recipeVoxels = anvil.recipeVoxels;
		if (anvil.SelectedRecipe == null)
		{
			return -1f;
		}
		int num4 = Math.Min(6, ((LayeredVoxelRecipe<SmithingRecipe>)(object)anvil.SelectedRecipe).QuantityLayers);
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < num4; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					if ((byte)(recipeVoxels[i, j, k] ? 1 : 0) == 1)
					{
						num2++;
						if (anvil.Voxels[i, j, k] == 1)
						{
							num3++;
							num++;
						}
					}
					else if (anvil.Voxels[i, j, k] == 1)
					{
						num3++;
					}
				}
			}
		}
		if (num3 >= num2)
		{
			return (float)num / (float)num3;
		}
		return (float)num / (float)num2 * -1f;
	}

	public static int FinishRecipe(this BlockEntityAnvil anvil)
	{
		if (anvil.SelectedRecipe == null)
		{
			return -1;
		}
		int num = 0;
		int num2 = Math.Min(6, ((LayeredVoxelRecipe<SmithingRecipe>)(object)anvil.SelectedRecipe).QuantityLayers);
		bool[,,] recipeVoxels = anvil.recipeVoxels;
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				for (int k = 0; k < 16; k++)
				{
					byte b = (byte)(recipeVoxels[i, j, k] ? 1u : 0u);
					if (b != anvil.Voxels[i, j, k])
					{
						num = ((b != 0 || anvil.Voxels[i, j, k] == 2) ? (num - 1) : (num + 1));
					}
					anvil.Voxels[i, j, k] = b;
				}
			}
		}
		return num;
	}
}
