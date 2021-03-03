﻿using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Horizontal Squelch Open Mapping")]
    public class Circular_Horizontal_Squelch_Open_Mapping : CircularBase, IFilter
    {
        public static Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;

            float umax = (float)(u * 9);
            float vmax = (float)(v * 9);

            double v2 = Math.Pow(v, 2);

            var circle = new Vector2(
            (float)(u / Math.Sqrt(1 - v2)),
            (float)(v)
            );
            if ((circle.X >= 0 || circle.X <= 0) && (circle.Y >= 0 || circle.Y <= 0))
            {
                return new Vector2(
                circle.X,
                circle.Y
                );
            }
            else
            {
                return new Vector2(
                Math.Clamp(umax, -1, 1),
                Math.Clamp(vmax, -1, 1)
                );
            }
        }
        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(CircleToSquare(ToUnit(input))));

        public FilterStage FilterStage => FilterStage.PostTranspose;
    }
}