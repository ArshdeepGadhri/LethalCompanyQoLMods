using HarmonyLib;

namespace ReturnTimeIncrease.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class VisibleClock
    {
        [HarmonyPatch(nameof(HUDManager.SetClockVisible))]
        [HarmonyPostfix]
        static void CreatePatch(ref HUDElement ___Clock)
        {
            ___Clock.targetAlpha += 0.03f;
        }
    }
}