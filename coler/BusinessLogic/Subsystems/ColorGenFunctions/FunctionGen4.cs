using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coler.Model;
using coler.Model.ColorGen.Parameters;
using coler.Model.Enum;

namespace coler.BusinessLogic.Subsystems.ColorGenFunctions
{
    public class FunctionGen4
    {
        private readonly ParametersGen4 _parameters;

        public FunctionGen4(ParametersGen4 parameters)
        {
            _parameters = parameters;
        }

        public (int, int, int) GeneratePixel(int x, int y, Random rng)
        {
            var colorRed = GenerateColor(x, y, _parameters.RedParameter, rng);
            var colorGreen = GenerateColor(x, y, _parameters.GreenParameter, rng);
            var colorBlue = GenerateColor(x, y, _parameters.BlueParameter, rng);

            return (colorRed, colorGreen, colorBlue);
        }

        public int GenerateColor(int x, int y, int parameter, Random rng)
        {
            if (rng == null) return 0;

            int xyDiff = (int)Math.Max(Math.Abs(x * (1 / (double)_parameters.XParameter) - y * (1 / (double)_parameters.YParameter)), 1);

            var randCol = rng.Next(0, 256);

            int salt = 0;

            switch (parameter)
            {
                case 0:

                    return (randCol * xyDiff) % 255;

                case 1:

                    salt = _parameters.CanvasWidth / xyDiff;

                    return (randCol * salt) % 255;

                case 2:

                    salt = _parameters.CanvasHeight / xyDiff;

                    return (randCol * _parameters.CanvasHeight / xyDiff) % 255;

                default:

                    return 0;
            }
        }
    }
}
