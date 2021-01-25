﻿using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Horizontal Squelch Open Mapping")]
    public class Circular_Horizontal_Squelch_Open_Mapping : IFilter
    {
        public static Vector2 ToUnit(Vector2 input)
        {
            if (Info.Driver.OutputMode is AbsoluteOutputMode absoluteOutputMode)
            {
                var area = absoluteOutputMode.Input;
                var size = new Vector2(area.Width, area.Height);
                var half = size / 2;
                var display = (Info.Driver.OutputMode as AbsoluteOutputMode).Output;
                var pxpermmw = display.Width / area.Width;
                var pxpermmh = display.Height / area.Height;
                return new Vector2(
                    (input.X / pxpermmw - half.X) / half.X,
                    (input.Y / pxpermmh - half.Y) / half.Y
                    );
            }
            else
            {
                return default;
            }
        }

        private static Vector2 FromUnit(Vector2 input)
        {
            if (Info.Driver.OutputMode is AbsoluteOutputMode absoluteOutputMode)
            {
                var area = absoluteOutputMode.Input;
                var size = new Vector2(area.Width, area.Height);
                var half = size / 2;
                var display = (Info.Driver.OutputMode as AbsoluteOutputMode).Output;
                var pxpermmw = display.Width / area.Width;
                var pxpermmh = display.Height / area.Height;
                return new Vector2(
                    ((input.X * half.X) + half.X) * pxpermmw,
                    ((input.Y * half.Y) + half.Y) * pxpermmh
                );
            }
            else
            {
                return default;
            }
        }

        public static Vector2 CircleToSquare(Vector2 input)
        {
            var u = input.X;
            var v = input.Y;

            var v2 = MathF.Pow(v, 2);

            return new Vector2(
            u / MathF.Sqrt(1 - v2),
            v
            );
        }

        public static Vector2 Clamp(Vector2 input)
        {
            return new Vector2(
            Math.Clamp(input.X, -1, 1),
            Math.Clamp(input.Y, -1, 1)
            );
        }
        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(CircleToSquare(ToUnit(input))));


        public FilterStage FilterStage => FilterStage.PostTranspose;

    }
}