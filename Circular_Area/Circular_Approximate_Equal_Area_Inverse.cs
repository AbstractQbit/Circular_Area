﻿using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Approximate Equal Area Inverse")]
    public class Circular_Approximate_Equal_Area_Inverse : CircularBase
    {
        public static Vector2 SquareToCircle(Vector2 input)
        {
            double x = input.X;
            double y = input.Y;

            double x2 = Math.Pow(x, 2);
            double y2 = Math.Pow(y, 2);

            if (x2 > y2)
            {
                var circle = new Vector2(
                (float)(x * Math.Sqrt(1 - (y2 / (2 * x2)))),
                (float)(y / Math.Sqrt(2))
                );

                return No_NaN(circle, input);
            }
            else
            {
                var circle = new Vector2(
                (float)(x / Math.Sqrt(2)),
                (float)(y * Math.Sqrt(1 - (x2 / (2 * y2))))
                );

                return No_NaN(circle, input);
            }
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

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(Expand(SquareToCircle(ToUnit(input)))));

        public override PipelinePosition Position => PipelinePosition.PostTransform;
    }
}