using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ReturnTimeIncrease.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    internal class ShipItemCounterPatch
    {
        private static GameObject _totalCounter;

        private static TextMeshProUGUI _textMesh;

        private static float _displayTimeLeft;

        private const float DisplayTime = 3f;
        
        [HarmonyPatch("PingScan_performed")]
        [HarmonyPrefix]
        private static void OnScan(ref HUDManager __instance, InputAction.CallbackContext context, float ___playerPingingScan)
        {
            var canPlayerScan = AccessTools.Method(typeof(HUDManager), "CanPlayerScan");
            bool playerScan = (bool)canPlayerScan.Invoke(__instance, new object[] { });
            if (!((UnityEngine.Object)(object)GameNetworkManager.Instance.localPlayerController == null) && context.performed && playerScan && !(___playerPingingScan > -0.5f) && (StartOfRound.Instance.inShipPhase || GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom))
            {
                
                if (!(ShipItemCounterPatch._totalCounter))
                {
                    CopyValueCounter();
                }
                float num = CalculateLootValue();
                ((TMP_Text)_textMesh).text = $"Ship Total: ${num:F0}";
                _displayTimeLeft = 5f;
                if (!_totalCounter.activeSelf)
                {
                    ((MonoBehaviour)GameNetworkManager.Instance).StartCoroutine(ShipLootCoroutine());
                }
            }
        }

        private static IEnumerator ShipLootCoroutine()
        {
            _totalCounter.SetActive(true);
            while (_displayTimeLeft > 0f)
            {
                float displayTimeLeft = _displayTimeLeft;
                _displayTimeLeft = 0f;
                yield return (object)new WaitForSeconds(displayTimeLeft);
            }
            _totalCounter.SetActive(false);
        }

        private static float CalculateLootValue()
        {
            List<GrabbableObject> list = (from obj in GameObject.Find("/Environment/HangarShip").GetComponentsInChildren<GrabbableObject>()
                                          where ((UnityEngine.Object)obj).name != "ClipboardManual" && ((UnityEngine.Object)obj).name != "StickyNoteItem"
                                          select obj).ToList();
            //ShipLoot.Log.LogDebug((object)"Calculating total ship scrap value.");
            CollectionExtensions.Do<GrabbableObject>((IEnumerable<GrabbableObject>)list, (Action<GrabbableObject>)delegate (GrabbableObject scrap)
            {
                //ShipLoot.Log.LogDebug((object)$"{((Object)scrap).name} - ${scrap.scrapValue}");
            });
            return list.Sum((GrabbableObject scrap) => scrap.scrapValue);
        }

        private static void CopyValueCounter()
        {
            GameObject val = GameObject.Find("/Systems/UI/Canvas/IngamePlayerHUD/BottomMiddle/ValueCounter");
            //if (!UnityEngine.Object.op_Implicit((UnityEngine.Object)(object)val))
            //{
                //ShipLoot.Log.LogError((object)"Failed to find ValueCounter object to copy!");
            //}
            _totalCounter = UnityEngine.Object.Instantiate<GameObject>(val.gameObject, val.transform.parent, false);
            _totalCounter.transform.Translate(0f, 1f, 0f);
            Vector3 localPosition = _totalCounter.transform.localPosition;
            _totalCounter.transform.localPosition = new Vector3(localPosition.x + 50f, -50f, localPosition.z);
            _textMesh = _totalCounter.GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}
