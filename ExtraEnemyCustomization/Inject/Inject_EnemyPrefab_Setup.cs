﻿using EECustom.Managers;
using Enemies;
using HarmonyLib;
using UnityEngine;

namespace EECustom.Inject
{
    [HarmonyPatch(typeof(EnemyPrefabManager), nameof(EnemyPrefabManager.BuildEnemyPrefab))]
    internal static class Inject_EnemyPrefab_Setup
    {
        [HarmonyWrapSafe]
        private static void Postfix(GameObject __result)
        {
            var agent = __result.GetComponent<EnemyAgent>();
            if (agent is null)
            {
                Logger.Error($"Agent is null! : {__result.name}");
                return;
            }
            ConfigManager.Current.Customize_Prespawn(__result.GetComponent<EnemyAgent>());
        }
    }
}