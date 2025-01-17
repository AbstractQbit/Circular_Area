﻿using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Lamé-based Mapping Inverse")]
    public class Circular_Lamé_based_Mapping_Inverse : CircularBase
    {
        public static Vector2 CircleToSquare(Vector2 input)
        {
            double x = input.X;
            double y = input.Y;

            double x2 = Math.Pow(x, 2);
            double y2 = Math.Pow(y, 2);

            double absx = Math.Abs(x);
            double absy = Math.Abs(y);

            var circle = new Vector2(
            (float)((x / Math.Sqrt(x2 + y2)) * Math.Pow((absx * (2 / ((1 - absx) * (1 - absy))) + absy * (2 / ((1 - absx) * (1 - absy)))), (0.5 * (1 - absx) * (1 - absy)))),
            (float)((y / Math.Sqrt(x2 + y2)) * Math.Pow((absx * (2 / ((1 - absx) * (1 - absy))) + absy * (2 / ((1 - absx) * (1 - absy)))), (0.5 * (1 - absx) * (1 - absy))))
            );

            return No_NaN(circle, input);
        }

        public override event Action<IDeviceReport> Emit;

        public override void Consume(IDeviceReport value)
        {
            if (value is ITabletReport report)
            {
                report.Position = Filter(report.Position);
                value = report;
            }

            Emit?.Invoke(value);
        }

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(CircleToSquare(ToUnit(input))));

        public override PipelinePosition Position => PipelinePosition.PostTransform;
    }
}