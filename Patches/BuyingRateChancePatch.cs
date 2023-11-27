using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace ReturnTimeIncrease.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class BuyingRateChancePatch
    {
        [HarmonyPatch(nameof(TimeOfDay.SetBuyingRateForDay))]
        [HarmonyPrefix]
        static bool CreatePatch(ref int ___daysUntilDeadline, ref float ___timeUntilDeadline, ref float ___totalTime, ref QuotaSettings ___quotaVariables)
        {
            var rand = new System.Random();
            float chance = rand.Next(5, 8);
            chance = chance / 100;

            ___daysUntilDeadline = (int)Mathf.Floor(___timeUntilDeadline / ___totalTime);
            if (___daysUntilDeadline == 0)
            {
                StartOfRound.Instance.companyBuyingRate = 1f;
                return false;
            }
            float num = 0.3f;
            float num2 = (1f - num) / (float)___quotaVariables.deadlineDaysAmount;
            StartOfRound.Instance.companyBuyingRate = num2 * (float)(___quotaVariables.deadlineDaysAmount - ___daysUntilDeadline) + num + chance;

            return false;
        }
    }
}
