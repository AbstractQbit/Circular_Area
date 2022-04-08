﻿using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Non-axial 2-Pinch Mapping")]
    public class Circular_Non_axial_2_Pinch_Mapping : CircularBase
    {
        public static Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;            
            
            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);

            double absu = Math.Abs(u);
            double absv = Math.Abs(v);

            double sgnu = absu / u;
            double sgnv = absv / v;
            double sgnuv = (absu * absv) / (u * v);

            double cuberoottwo = Math.Pow(2, 0.25f);

            if (Math.Abs(v) < 0.00001)
            {
                var circle = new Vector2(
                        (float)(sgnu * Math.Sqrt(absu)),
                        (float)((sgnuv / (u * cuberoottwo)) * Math.Pow((u2 + v2 - 2 * u2 * v2 - Math.Sqrt((u2 + v2 - 4 * u2 * v2) * (u2 + v2))), 0.25f))
                        );

                return No_NaN(circle, input);
            }
            else
            {
                if (Math.Abs(u) < 0.00001)
                {
                    var circle = new Vector2(
                        (float)((sgnuv / (v * cuberoottwo)) * Math.Pow((u2 + v2 - 2 * u2 * v2 - Math.Sqrt((u2 + v2 - 4 * u2 * v2) * (u2 + v2))), 0.25f)),
                        (float)(sgnv *Math.Sqrt(absv))
                        );

                    return No_NaN(circle, input);
                }
                else
                {
                    var circle = new Vector2(
                        (float)((sgnuv / (v * cuberoottwo)) * Math.Pow((u2 + v2 - 2 * u2 * v2 - Math.Sqrt((u2 + v2 - 4 * u2 * v2) * (u2 + v2))), 0.25f)),
                        (float)((sgnuv / (u * cuberoottwo)) * Math.Pow((u2 + v2 - 2 * u2 * v2 - Math.Sqrt((u2 + v2 - 4 * u2 * v2) * (u2 + v2))), 0.25f))
                        );

                    return No_NaN(circle, input);
                }
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

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(CircleToSquare(ToUnit(input))));

        public override PipelinePosition Position => PipelinePosition.PostTransform;
    }
}