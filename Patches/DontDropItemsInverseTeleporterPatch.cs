using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;


namespace ReturnTimeIncrease.Patches
{
    [HarmonyPatch(typeof(ShipTeleporter))]
    internal class DontDropItemsInverseTeleporterPatch
    {
        [HarmonyPatch("TeleportPlayerOutWithInverseTeleporter")]
        [HarmonyPrefix]
        private static bool CreatePatch(ref int[] ___playersBeingTeleported, int playerObj, Vector3 teleportPos) {
            if (StartOfRound.Instance.allPlayerScripts[playerObj].isPlayerDead)
            {
                //StartCoroutine(teleportBodyOut(playerObj, teleportPos));
                return true; // Run old code
            }
            PlayerControllerB playerControllerB = StartOfRound.Instance.allPlayerScripts[playerObj];
            SetPlayerTeleporterId(___playersBeingTeleported, playerControllerB, -1);
            //playerControllerB.DropAllHeldItems();

            if ((bool)UnityEngine.Object.FindObjectOfType<AudioReverbPresets>())
            {
                UnityEngine.Object.FindObjectOfType<AudioReverbPresets>().audioPresets[2].ChangeAudioReverbForPlayer(playerControllerB);
            }

            playerControllerB.isInElevator = false;
            playerControllerB.isInHangarShipRoom = false;
            playerControllerB.isInsideFactory = true;
            playerControllerB.averageVelocity = 0f;
            playerControllerB.velocityLastFrame = Vector3.zero;
            StartOfRound.Instance.allPlayerScripts[playerObj].TeleportPlayer(teleportPos, false, 0f, false, true);
            StartOfRound.Instance.allPlayerScripts[playerObj].beamOutParticle.Play();
            //shipTeleporterAudio.PlayOneShot(teleporterBeamUpSFX);
            //StartOfRound.Instance.allPlayerScripts[playerObj].movementAudio.PlayOneShot(instance.teleporterBeamUpSFX);
            if (playerControllerB == GameNetworkManager.Instance.localPlayerController)
            {
                HUDManager.Instance.ShakeCamera(ScreenShakeType.Big);
            }
            return false; // Don't run old code
        }

        private static void SetPlayerTeleporterId(int[] playersBeingTeleported, PlayerControllerB playerScript, int teleporterId)
        {
            playerScript.shipTeleporterId = teleporterId;
            playersBeingTeleported[playerScript.playerClientId] = (int)playerScript.playerClientId;
        }
    }
}
