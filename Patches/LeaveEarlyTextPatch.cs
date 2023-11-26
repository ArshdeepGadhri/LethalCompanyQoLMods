using HarmonyLib;
using TMPro;

namespace ReturnTimeIncrease.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class LeaveEarlyTextPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void CreatePatch(ref bool ___hasLoadedSpectateUI, ref TextMeshProUGUI ___holdButtonToEndGameEarlyText)
        {
            if (___hasLoadedSpectateUI && !(StartOfRound.Instance.shipIsLeaving || !StartOfRound.Instance.currentLevel.planetHasTime) && TimeOfDay.Instance.shipLeavingAlertCalled && TimeOfDay.Instance.votedShipToLeaveEarlyThisRound)
            {
                ___holdButtonToEndGameEarlyText.text = "Ship leaving in three hours";
            }
        }
    }
}
