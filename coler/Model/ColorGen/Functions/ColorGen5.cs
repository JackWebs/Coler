﻿using System;
using coler.Model.Enum;

namespace coler.Model.ColorGen.Functions
{
    public class ColorGen5 : ColorGenBase, IColorGenFunction
    {
        public ColorGen5()
        {
            Id = 5;
            ColorParameters = new[] { 0, 1, 2, 3};
        }

        public int GenerateColor(int x, int y, int parameter, EnColor color, Random rng = null, PixelData point = null)
        {
            if (rng == null) return 0;

            int xyDiff = (int) Math.Max(Math.Abs(x * XParameter - y * YParameter), 1);

            var randCol = rng.Next(0, 256);

            int salt = 0;

            switch (parameter)
            {
                case 0:

                    return (randCol * xyDiff) % 255;

                case 1:

                    salt = Width / xyDiff;

                    return (randCol * salt) % 255;

                case 2:

                    salt = Height / xyDiff;

                    return (randCol * Height / xyDiff) % 255;

                default:

                    return 0;
            }
        }
    }
}
