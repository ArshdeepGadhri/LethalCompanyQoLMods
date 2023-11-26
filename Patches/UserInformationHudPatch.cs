using HarmonyLib;

namespace ReturnTimeIncrease.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class UserInformationHudPatch
    {

        [HarmonyPatch("PingScan_performed")]
        [HarmonyPrefix]
        private static void CreatePatch(ref HUDManager __instance)
        {
            if (!GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom)
            { 
            float health = (float)GameNetworkManager.Instance.localPlayerController.health;
            float insanityLvl = (float)GameNetworkManager.Instance.localPlayerController.insanityLevel;
            float maxInsanityLevel = (float)GameNetworkManager.Instance.localPlayerController.maxInsanityLevel;

            var DisplayGlobalNotification = AccessTools.Method(typeof(HUDManager), "DisplayGlobalNotification");
            DisplayGlobalNotification.Invoke(__instance, new object[] { $"Health Points: {health:F0}/100\nInsanity Level: {insanityLvl:F0}/{maxInsanityLevel:F0}" });

            //__instance.DisplayTip("Player Information", $"HP: {health:F0}/100\nINS: {insanityLvl:F0}/{maxInsanityLevel:F0}");
            }
        }
    }
}
