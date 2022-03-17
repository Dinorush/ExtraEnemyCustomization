﻿using EECustom.Managers.Properties;
using Enemies;
using System;
using System.Collections.Generic;
using System.Text;

namespace EECustom
{
    public static class ProjectileTargetingExtension
    {
        public static bool TryGetOwnerEnemyDataID(this ProjectileBase projectile, out uint id)
        {
            if (projectile == null)
            {
                id = 0u;
                return false;
            }

            return ProjectileOwnerManager.TryGetDataID(projectile.gameObject.GetInstanceID(), out id);
        }

        public static bool TryGetOwnerEnemyDataID(this ProjectileTargeting projectile, out uint id)
        {
            if (projectile == null)
            {
                id = 0u;
                return false;
            }

            return ProjectileOwnerManager.TryGetDataID(projectile.gameObject.GetInstanceID(), out id);
        }

        public static bool TryGetOwner(this ProjectileBase projectile, out EnemyAgent agent)
        {
            if (projectile == null)
            {
                agent = null;
                return false;
            }

            return ProjectileOwnerManager.TryGet(projectile.gameObject.GetInstanceID(), out agent);
        }

        public static bool TryGetOwner(this ProjectileTargeting projectile, out EnemyAgent agent)
        {
            if (projectile == null)
            {
                agent = null;
                return false;
            }

            return ProjectileOwnerManager.TryGet(projectile.gameObject.GetInstanceID(), out agent);
        }
    }
}
