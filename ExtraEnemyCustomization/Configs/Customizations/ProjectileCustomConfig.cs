﻿using EECustom.Customizations;
using EECustom.Customizations.Shooters;
using EECustom.CustomSettings.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECustom.Configs.Customizations
{
    public class ProjectileCustomConfig : CustomizationConfig
    {
        public ShooterFireCustom[] ShooterFireCustom = new ShooterFireCustom[0];
        public CustomProjectile[] ProjectileDefinitions = new CustomProjectile[0];

        public override EnemyCustomBase[] GetAllSettings()
        {
            var list = new List<EnemyCustomBase>();
            list.AddRange(ShooterFireCustom);
            return list.ToArray();
        }
    }
}
