﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ExtraEnemyCustomization.CustomProjectiles
{
    public class CustomProjectile
    {
        [JsonIgnore]
        private static Dictionary<byte, GameObject> _ProjectilePrefabs = new Dictionary<byte, GameObject>();

        public byte ID = 10;
        public ProjectileType BaseProjectile = ProjectileType.TargetingSmall;
        public float Speed = 5.0f;
        public float HomingStrength = 8.0f;
        public Color LightColor = Color.yellow;
        public float LightRange = 5.0f;
        public float Damage = 1.0f;
        public float Infection = 0.1f;

        public static void GenerateProjectile(CustomProjectile projectileInfo)
        {
            if (Enum.IsDefined(typeof(ProjectileType), projectileInfo.ID))
            {
                Logger.Error($"ProjectileID Conflict with Official ID!: {projectileInfo.ID}");
                return;
            }

            if (_ProjectilePrefabs.ContainsKey(projectileInfo.ID))
            {
                Logger.Error($"ProjectileID Conflict!: {projectileInfo.ID}");
                return;
            }

            var basePrefab = ProjectileManager.Current.m_projectilePrefabs[(int)projectileInfo.BaseProjectile];
            var newPrefab = GameObject.Instantiate(basePrefab);
            GameObject.DontDestroyOnLoad(newPrefab);
            var projectileBase = newPrefab.GetComponent<ProjectileBase>();
            if(projectileBase != null)
            {
                Logger.DevMessage($"Changed Base");
                projectileBase.m_maxDamage = projectileInfo.Damage;
                projectileBase.m_maxInfection = projectileInfo.Infection;

                var targeting = projectileBase.TryCast<ProjectileTargeting>();
                if(targeting != null)
                {
                    Logger.DevMessage($"Changed Targeting");
                    targeting.Speed = projectileInfo.Speed;
                    targeting.TargetStrength = projectileInfo.HomingStrength;
                    targeting.LightColor = projectileInfo.LightColor;
                    targeting.LightRange = projectileInfo.LightRange;
                }
            }
            newPrefab.active = false;
            newPrefab.name = "GaneratedProjectilePrefab_" + projectileInfo.ID;
            _ProjectilePrefabs.Add(projectileInfo.ID, newPrefab);
            Logger.DevMessage($"Added Projectile!: {projectileInfo.ID}");
        }

        public static GameObject GetProjectile(byte id)
        {
            if(_ProjectilePrefabs.TryGetValue(id, out var prefab))
            {
                return prefab;
            }
            return null;
        }
    }
}
