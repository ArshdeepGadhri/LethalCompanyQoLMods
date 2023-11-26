using HarmonyLib;

namespace ReturnTimeIncrease.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class TimeOfDayPatch
    {
        [HarmonyPatch(nameof(TimeOfDay.SetShipLeaveEarlyServerRpc))]
        [HarmonyPostfix]
        static void CreatePatch(ref float ___normalizedTimeOfDay)
        {
            ___normalizedTimeOfDay += 0.07f;
        }
    }
}
