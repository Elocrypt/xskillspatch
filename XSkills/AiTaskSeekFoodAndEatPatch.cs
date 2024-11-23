using System;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace XSkills
{
    [HarmonyPatch(typeof(AiTaskSeekFoodAndEat))]
    public class AiTaskSeekFoodAndEatPatch
    {
        [HarmonyPatch("ContinueExecute")]
        public static void Prefix(AiTaskSeekFoodAndEat __instance, IAnimalFoodSource ___targetPoi)
        {
            BlockEntityTrough blockEntityTrough = ___targetPoi as BlockEntityTrough;
            EntityAgent entity = __instance.entity;
            XSkillsAnimalBehavior xskillsAnimalBehavior = (entity != null) ? entity.GetBehavior<XSkillsAnimalBehavior>() : null;
            if (xskillsAnimalBehavior == null || blockEntityTrough == null)
            {
                return;
            }
            xskillsAnimalBehavior.Feeder = blockEntityTrough.GetOwner();
        }

        [HarmonyPatch("FinishExecute")]
        public static void Prefix(AiTaskSeekFoodAndEat __instance, float ___quantityEaten)
        {
            if (___quantityEaten < 1f)
            {
                return;
            }
            EntityAgent entity = __instance.entity;
            IPlayer player;
            if (entity == null)
            {
                player = null;
            }
            else
            {
                XSkillsAnimalBehavior behavior = entity.GetBehavior<XSkillsAnimalBehavior>();
                player = ((behavior != null) ? behavior.Feeder : null);
            }
            IPlayer player2 = player;
            if (player2 == null)
            {
                return;
            }
            Husbandry husbandry = XLeveling.Instance(__instance.entity.World.Api).GetSkill("husbandry", false) as Husbandry;
            if (husbandry == null)
            {
                return;
            }
            EntityPlayer entity2 = player2.Entity;
            PlayerSkill playerSkill;
            if (entity2 == null)
            {
                playerSkill = null;
            }
            else
            {
                PlayerSkillSet behavior2 = entity2.GetBehavior<PlayerSkillSet>();
                playerSkill = ((behavior2 != null) ? behavior2[husbandry.Id] : null);
            }
            PlayerSkill playerSkill2 = playerSkill;
            if (playerSkill2 == null)
            {
                return;
            }
            playerSkill2.AddExperience(0.025f, true);
            PlayerAbility playerAbility = playerSkill2[husbandry.FeederId];
            if (playerAbility == null)
            {
                return;
            }
            if (__instance.entity.World.Rand.NextDouble() < (double)playerAbility.FValue(3, 0f))
            {
                int num = __instance.entity.WatchedAttributes.GetInt("generation", 0) + 1;
                if (num < playerAbility.Value(4, 0))
                {
                    __instance.entity.WatchedAttributes.SetInt("generation", num);
                    __instance.entity.WatchedAttributes.MarkPathDirty("generation");
                }
            }
        }
    }
}