﻿using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Concentric Mapping Inverse")]
    public class Circular_Concentric_Mapping_Inverse : CircularBase
    {
        public static Vector2 SquareToCircle(Vector2 input)
        {
            double x = input.X;
            double y = input.Y;

            double absx = Math.Abs(x);
            double absy = Math.Abs(y);

            if (absx >= absy)
            {
                var circle = new Vector2(
                (float)(x * Math.Cos((Math.PI / 4) * (y / x))),
                (float)(x * Math.Sin((Math.PI / 4) * (y / x)))
                );

                return No_NaN(circle, input);
            }
            else
            {
                var circle = new Vector2(
                (float)(y * Math.Cos((Math.PI / 2) - ((Math.PI / 4) * (x / y)))),
                (float)(y * Math.Sin((Math.PI / 2) - ((Math.PI / 4) * (x / y))))
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