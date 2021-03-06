﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coler.Model;
using coler.Model.ColorGen.Parameters;
using coler.Model.Enum;

namespace coler.BusinessLogic.Subsystems.ColorGenFunctions
{
    public class FunctionGen2
    {
        private readonly ParametersGen2 _parameters;

        public FunctionGen2(ParametersGen2 parameters)
        {
            _parameters = parameters;
        }

        public (int, int, int) GeneratePixel(int x, int y, Random rng)
        {
            int colorRed = GenerateColor(x, y, _parameters.RedParameter, rng);
            int colorGreen = GenerateColor(x, y, _parameters.GreenParameter, rng);
            int colorBlue = GenerateColor(x, y, _parameters.BlueParameter, rng);

            return (colorRed, colorGreen, colorBlue);
        }

        public int GenerateColor(int x, int y, int parameter, Random rng)
        {
            int halfWidth = _parameters.CanvasWidth / 2;
            int halfHeight = _parameters.CanvasHeight / 2;

            int xVal = (int)Math.Abs(x * (1 / (double)_parameters.XParameter) - halfWidth) * rng.Next(0, 255);
            int yVal = (int)Math.Abs(y * (1 / (double)_parameters.YParameter) - halfHeight) * rng.Next(0, 255);

            switch (parameter)
            {
                case 0:

                    return xVal % 255;

                case 1:

                    return yVal % 255;

                case 2:

                    int colorVal = (int)new[] { xVal, yVal }.Average();

                    return colorVal % 255;
            }

            return 0;
        }
    }
}