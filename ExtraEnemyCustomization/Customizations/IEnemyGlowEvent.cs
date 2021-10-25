﻿using Enemies;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EECustom.Customizations
{
    public interface IEnemyGlowEvent
    {
        bool OnGlow(EnemyAgent agent, ref GlowInfo glowInfo);
    }

    public struct GlowInfo
    {
        public Color Color;
        public Vector4 Position;

        public GlowInfo(Color color, Vector4 position)
        {
            Color = color;
            Position = position;
        }

        public GlowInfo ChangeColor(Color color)
        {
            return new GlowInfo(color, Position);
        }

        public GlowInfo ChangePosition(Vector4 position)
        {
            return new GlowInfo(Color, position);
        }
    }
}
