using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ReturnTimeIncrease.Patches;

namespace ReturnTimeIncrease
{
    [BepInPlugin(Guid, Name, Version)]
    public class VoteUpdate : BaseUnityPlugin
    {
        private const string Guid = "my.art.mods";
        private const string Name = "QoL mods";
        private const string Version = "1.0.1";

        private readonly Harmony harmony = new Harmony(Guid);
        private static VoteUpdate Instance;

        internal ManualLogSource log;

        void Awake()
        {
            if (Instance)
            {
                Instance = this;
            }

            log = BepInEx.Logging.Logger.CreateLogSource(Guid);
            log.LogInfo("QoL Mods (Art) - v:" + Version);

            harmony.PatchAll(typeof(VoteUpdate));
            harmony.PatchAll(typeof(TimeOfDayPatch));
            harmony.PatchAll(typeof(VisibleClock));
            harmony.PatchAll(typeof(DontDropItemsInverseTeleporterPatch));
            harmony.PatchAll(typeof(ShipItemCounterPatch));
            harmony.PatchAll(typeof(UserInformationHudPatch));
            harmony.PatchAll(typeof(LeaveEarlyTextPatch));
        }
    }
}
