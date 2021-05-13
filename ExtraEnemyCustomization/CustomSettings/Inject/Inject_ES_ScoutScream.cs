﻿using Enemies;
using HarmonyLib;
using SNetwork;

namespace EECustom.CustomSettings.Inject
{
    [HarmonyPatch(typeof(ES_ScoutScream), "CommonUpdate")]
    internal class Inject_ES_ScoutScream
    {
        //TODO: Check if it's work
        [HarmonyWrapSafe]
        private static void Postfix(ES_ScoutScream __instance)
        {
            if (__instance.m_state == ES_ScoutScream.ScoutScreamState.Done)
            {
                if (__instance.m_stateDoneTimer >= 0.0f)
                {
                    __instance.m_stateDoneTimer = -1.0f;
                    if (SNet.IsMaster)
                    {
                        CustomScoutWaveManager.TriggerScoutWave(__instance.m_enemyAgent);
                    }
                }
            }
        }
    }
}