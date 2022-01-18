﻿using CellMenu;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EECustom.Utils.Integrations.Inject
{
    [HarmonyPatch(typeof(CM_PageRundown_New), nameof(CM_PageRundown_New.OnEnable))]
    internal static class Inject_CM_PageRundown_New
    {
        private static bool _isInjected = false;

        public static void Postfix()
        {
            if (_isInjected)
                return;

            object hotreloader = MTFOUtil.HotReloaderField?.GetValue(null) ?? null;
            if (hotreloader is null)
                return;

            var buttonField = hotreloader.GetType().GetField("button", BindingFlags.NonPublic | BindingFlags.Instance);
            if (buttonField is null)
                return;

            var button = (CM_Item)buttonField.GetValue(hotreloader);
            button.add_OnBtnPressCallback((Action<int>)MTFOUtil.OnHotReloaded);
            _isInjected = true;
        }
    }
}
