﻿using Agents;
using Player;
using System;

namespace EECustom.Events
{
    public static class PlayerDamageEvents
    {
        public static Action<PlayerAgent, Agent, float> OnDamage = null;
        public static Action<PlayerAgent, Agent, float> OnMeleeDamage = null;
        public static Action<PlayerAgent, Agent, float> OnTentacleDamage = null;
    }
}