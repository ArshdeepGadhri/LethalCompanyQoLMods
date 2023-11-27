using System.Diagnostics;
using HarmonyLib;
using UnityEngine.InputSystem;

namespace ReturnTimeIncrease.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class UserInformationHudPatch
    {

        [HarmonyPatch("PingScan_performed")]
        [HarmonyPrefix]
        private static void CreatePatch(ref HUDManager __instance, InputAction.CallbackContext context)
        {
            if (!GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom)
            { 
                float health = (float)GameNetworkManager.Instance.localPlayerController.health;
                float insanityLvl = (float)GameNetworkManager.Instance.localPlayerController.insanityLevel * 2;
                float maxInsanityLevel = (float)GameNetworkManager.Instance.localPlayerController.maxInsanityLevel;

                int profitQuota = TimeOfDay.Instance.profitQuota;
                int quotaFulfilled = TimeOfDay.Instance.quotaFulfilled;
                int daysUntilDeadline = TimeOfDay.Instance.daysUntilDeadline;

                var DisplayGlobalNotification = AccessTools.Method(typeof(HUDManager), "DisplayGlobalNotification");
                DisplayGlobalNotification.Invoke(__instance, new object[] { $"HP: {health:F0}%  -  Ins: {insanityLvl:F0}%\nDays Left: {daysUntilDeadline}  -  Quota: ${quotaFulfilled}/{profitQuota}" });

                //__instance.DisplayTip("Player Information", $"HP: {health:F0}/100\nINS: {insanityLvl:F0}/{maxInsanityLevel:F0}");
            }
        }
    }
}
