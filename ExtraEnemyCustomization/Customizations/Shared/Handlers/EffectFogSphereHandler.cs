﻿using EECustom.Attributes;
using EECustom.Utils;
using UnityEngine;

namespace EECustom.Customizations.Shared.Handlers
{
    [InjectToIl2Cpp]
    internal sealed class EffectFogSphereHandler : MonoBehaviour
    {
        public FogSphereHandler Handler;
        public EV_Sphere EVSphere;

        private Timer _updateTimer = new(0.1f);
        private bool _updatedOnce = false;

        internal void Update()
        {
            if (Handler.m_sphere != null && _updateTimer.TickAndCheckDone())
            {
                var sphere = Handler.m_sphere;
                var range = Mathf.Sqrt(1.0f / sphere.m_data.InvRangeSqr);
                EVSphere.minRadius = range * 0.8f;
                EVSphere.maxRadius = range;
                _updateTimer.Reset();
                _updatedOnce = true;
            }

            if (Handler.m_sphere == null && _updatedOnce)
            {
                GameObject.Destroy(this);
            }
        }

        internal void OnDestroy()
        {
            EffectVolumeManager.UnregisterVolume(EVSphere);

            Handler = null;
            EVSphere = null;
        }
    }
}