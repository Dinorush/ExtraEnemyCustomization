﻿using EECustom.Configs;
using EECustom.Configs.Customizations;
using EECustom.Customizations;
using EECustom.CustomSettings;
using EECustom.Utils;
using Enemies;
using System;
using System.Collections.Generic;
using System.IO;

namespace EECustom.Managers
{
    public class ConfigManager
    {
        public static string BasePath { get; private set; }

        public static void Initialize()
        {
            Current = new ConfigManager();

            if (MTFOUtil.IsLoaded && MTFOUtil.HasCustomContent)
            {
                try
                {
                    BasePath = Path.Combine(MTFOUtil.CustomPath, "ExtraEnemyCustomization");

                    Logger.Debug("Loading Model.json...");
                    if (TryLoadConfig(BasePath, "Model.json", out ModelCustomConfig modelConfig))
                        Current.ModelCustom = modelConfig;

                    Logger.Debug("Loading Ability.json...");
                    if (TryLoadConfig(BasePath, "Ability.json", out AbilityCustomConfig abilityConfig))
                        Current.AbilityCustom = abilityConfig;

                    Logger.Debug("Loading Projectile.json...");
                    if (TryLoadConfig(BasePath, "Projectile.json", out ProjectileCustomConfig projConfig))
                        Current.ProjectileCustom = projConfig;

                    Logger.Debug("Loading Tentacle.json...");
                    if (TryLoadConfig(BasePath, "Tentacle.json", out TentacleCustomConfig tentacleConfig))
                        Current.TentacleCustom = tentacleConfig;

                    Logger.Debug("Loading Detection.json...");
                    if (TryLoadConfig(BasePath, "Detection.json", out DetectionCustomConfig detectionConfig))
                        Current.DetectionCustom = detectionConfig;

                    Logger.Debug("Loading ScoutWave.json");
                    if (TryLoadConfig(BasePath, "ScoutWave.json", out ScoutWaveConfig scoutWaveConfig))
                    {
                        CustomScoutWaveManager.AddScoutSetting(scoutWaveConfig.Expeditions);
                        CustomScoutWaveManager.AddTargetSetting(scoutWaveConfig.TargetSettings);
                        CustomScoutWaveManager.AddWaveSetting(scoutWaveConfig.WaveSettings);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error($"Error Occured While reading ExtraEnemyCustomization.json file: {e}");
                }
            }
            else
            {
                Logger.Warning("No Custom content were found, No Customization will be applied");
            }

            Current.GenerateBuffer();
        }

        public static bool TryLoadConfig<T>(string basePath, string fileName, out T config)
        {
            var path = Path.Combine(basePath, fileName);
            if (File.Exists(path))
            {
                try
                {
                    config = JSON.Deserialize<T>(File.ReadAllText(path));
                    return true;
                }
                catch (Exception e)
                {
                    Logger.Error($"Exception Occured While reading {path} file: {e}");
                    config = default;
                    return false;
                }
            }
            else
            {
                Logger.Warning($"File: {path} is not exist, ignoring this config...");
                config = default;
                return false;
            }
        }

        public static ConfigManager Current { get; private set; }

        public ModelCustomConfig ModelCustom { get; private set; } = new ModelCustomConfig();
        public AbilityCustomConfig AbilityCustom { get; private set; } = new AbilityCustomConfig();
        public ProjectileCustomConfig ProjectileCustom { get; private set; } = new ProjectileCustomConfig();
        public TentacleCustomConfig TentacleCustom { get; private set; } = new TentacleCustomConfig();
        public DetectionCustomConfig DetectionCustom { get; private set; } = new DetectionCustomConfig();

        private readonly List<EnemyCustomBase> _CustomizationBuffer = new List<EnemyCustomBase>();

        private void GenerateBuffer()
        {
            _CustomizationBuffer.Clear();
            _CustomizationBuffer.AddRange(ModelCustom.GetAllSettings());
            _CustomizationBuffer.AddRange(AbilityCustom.GetAllSettings());
            _CustomizationBuffer.AddRange(ProjectileCustom.GetAllSettings());
            _CustomizationBuffer.AddRange(TentacleCustom.GetAllSettings());
            _CustomizationBuffer.AddRange(DetectionCustom.GetAllSettings());
            foreach (var custom in _CustomizationBuffer)
            {
                custom.Initialize();
                custom.LogDev("Init!");
                custom.LogVerbose(custom.Target.ToDebugString());
            }
        }

        public void Customize_Prespawn(EnemyAgent agent)
        {
            foreach (var custom in _CustomizationBuffer)
            {
                if (!custom.Enabled)
                    continue;

                if (custom.Target.IsMatch(agent) && custom.HasPrespawnBody)
                {
                    custom.LogDev($"Prespawn effect to {agent.name}");
                    custom.Prespawn(agent);
                    custom.LogVerbose($"Finished!");
                }
            }
        }

        public void Customize_Postspawn(EnemyAgent agent)
        {
            foreach (var custom in _CustomizationBuffer)
            {
                if (!custom.Enabled)
                    continue;

                if (custom.Target.IsMatch(agent) && custom.HasPostspawnBody)
                {
                    custom.LogDev($"Postspawn effect to {agent.name}");
                    custom.Postspawn(agent);
                    custom.LogVerbose($"Finished!");
                }
            }
        }
    }
}