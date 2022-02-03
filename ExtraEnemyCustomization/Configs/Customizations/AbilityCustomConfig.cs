﻿using EECustom.Customizations;
using EECustom.Customizations.Abilities;
using System;
using System.Collections.Generic;

namespace EECustom.Configs.Customizations
{
    public sealed class AbilityCustomConfig : CustomizationConfig
    {
        public BirthingCustom[] BirthingCustom { get; set; } = Array.Empty<BirthingCustom>();
        public FogSphereCustom[] FogSphereCustom { get; set; } = Array.Empty<FogSphereCustom>();
        public HealthRegenCustom[] HealthRegenCustom { get; set; } = Array.Empty<HealthRegenCustom>();
        public InfectionAttackCustom[] InfectionAttackCustom { get; set; } = Array.Empty<InfectionAttackCustom>();
        public KnockbackAttackCustom[] KnockbackAttackCustom { get; set; } = Array.Empty<KnockbackAttackCustom>();
        public ExplosiveAttackCustom[] ExplosiveAttackCustom { get; set; } = Array.Empty<ExplosiveAttackCustom>();
        public BleedAttackCustom[] BleedAttackCustom { get; set; } = Array.Empty<BleedAttackCustom>();
        public DoorBreakerCustom[] DoorBreakerCustom { get; set; } = Array.Empty<DoorBreakerCustom>();
        public ScoutScreamingCustom[] ScoutScreamingCustom { get; set; } = Array.Empty<ScoutScreamingCustom>();

        public override string FileName => "Ability";
        public override CustomizationConfigType Type => CustomizationConfigType.Ability;

        public override EnemyCustomBase[] GetAllSettings()
        {
            var list = new List<EnemyCustomBase>();
            list.AddRange(BirthingCustom);
            list.AddRange(FogSphereCustom);
            list.AddRange(HealthRegenCustom);
            list.AddRange(InfectionAttackCustom);
            list.AddRange(KnockbackAttackCustom);
            list.AddRange(ExplosiveAttackCustom);
            list.AddRange(BleedAttackCustom);
            list.AddRange(DoorBreakerCustom);
            list.AddRange(ScoutScreamingCustom);
            return list.ToArray();
        }
    }
}