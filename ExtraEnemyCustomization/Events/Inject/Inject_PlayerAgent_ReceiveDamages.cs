﻿using HarmonyLib;

namespace EECustom.Events.Inject
{
    [HarmonyPatch(typeof(Dam_PlayerDamageLocal))]
    internal class Inject_PlayerAgent_ReceiveDamages
    {
        [HarmonyPostfix]
        [HarmonyWrapSafe]
        [HarmonyPatch(nameof(Dam_PlayerDamageLocal.ReceiveMeleeDamage))]
        private static void Post_Melee(pFullDamageData data, Dam_PlayerDamageBase __instance)
        {
            if (data.source.TryGet(out var inflictor))
            {
                var damage = data.damage.Get(__instance.HealthMax);
                PlayerDamageEvents.OnDamage?.Invoke(__instance.Owner, inflictor, damage);
                PlayerDamageEvents.OnMeleeDamage?.Invoke(__instance.Owner, inflictor, damage);
            }
        }

        [HarmonyPostfix]
        [HarmonyWrapSafe]
        [HarmonyPatch(nameof(Dam_PlayerDamageLocal.ReceiveTentacleAttackDamage))]
        private static void Post_Tentacle(pMediumDamageData data, Dam_PlayerDamageLocal __instance)
        {
            if (data.source.TryGet(out var inflictor))
            {
                var damage = data.damage.Get(__instance.HealthMax);
                PlayerDamageEvents.OnDamage?.Invoke(__instance.Owner, inflictor, damage);
                PlayerDamageEvents.OnTentacleDamage?.Invoke(__instance.Owner, inflictor, damage);
            }
        }
    }
}