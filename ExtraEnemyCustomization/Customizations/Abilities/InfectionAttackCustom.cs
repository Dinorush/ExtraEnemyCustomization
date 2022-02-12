﻿using Agents;
using EECustom.Events;
using EECustom.Utils;
using EECustom.Utils.JsonElements;
using Enemies;
using Player;

namespace EECustom.Customizations.Abilities
{
    public sealed class InfectionAttackCustom : EnemyCustomBase
    {
        public InfectionAttackData MeleeData { get; set; } = new();
        public InfectionAttackData TentacleData { get; set; } = new();

        public override string GetProcessName()
        {
            return "InfectionAttack";
        }

        public override void OnConfigLoaded()
        {
            LocalPlayerDamageEvents.MeleeDamage += OnMelee;
            LocalPlayerDamageEvents.TentacleDamage += OnTentacle;
        }

        public override void OnConfigUnloaded()
        {
            LocalPlayerDamageEvents.MeleeDamage -= OnMelee;
            LocalPlayerDamageEvents.TentacleDamage -= OnTentacle;
        }

        public void OnMelee(PlayerAgent player, Agent inflictor, float damage)
        {
            if (inflictor is not null && inflictor.Type == AgentType.Enemy)
            {
                var enemy = inflictor.Cast<EnemyAgent>();
                if (IsTarget(enemy))
                {
                    ApplyInfection(MeleeData, player);
                }
            }
        }

        public void OnTentacle(PlayerAgent player, Agent inflictor, float damage)
        {
            if (inflictor is not null && inflictor.Type == AgentType.Enemy)
            {
                var enemy = inflictor.Cast<EnemyAgent>();
                if (IsTarget(enemy))
                {
                    ApplyInfection(TentacleData, player);
                }
            }
        }

        private static void ApplyInfection(InfectionAttackData data, PlayerAgent player)
        {
            var infectionAbs = data.Infection.GetAbsValue(PlayerData.MaxInfection);
            if (infectionAbs == 0.0f)
                return;

            if (data.SoundEventID != 0u)
            {
                player.Sound.Post(data.SoundEventID);
            }

            if (data.UseEffect)
            {
                var liquidSetting = ScreenLiquidSettingName.spitterJizz;
                if (infectionAbs < 0.0f)
                {
                    liquidSetting = ScreenLiquidSettingName.disinfectionStation_Apply;
                }
                ScreenLiquidManager.TryApply(liquidSetting, player.Position, data.ScreenLiquidRange, true);
            }

            player.Damage.ModifyInfection(new pInfection()
            {
                amount = infectionAbs,
                effect = pInfectionEffect.None,
                mode = pInfectionMode.Add
            }, true, true);
        }
    }

    public class InfectionAttackData
    {
        public ValueBase Infection { get; set; } = ValueBase.Zero;
        public uint SoundEventID { get; set; } = 0u;
        public bool UseEffect { get; set; } = false;
        public float ScreenLiquidRange { get; set; } = 0.0f;
    }
}