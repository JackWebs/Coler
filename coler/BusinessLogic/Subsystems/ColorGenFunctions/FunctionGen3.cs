using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coler.Model.ColorGen.Parameters;

namespace coler.BusinessLogic.Subsystems.ColorGenFunctions
{
    public class FunctionGen3
    {
        private readonly ParametersGen3 _parameters;

        public FunctionGen3(ParametersGen3 parameters)
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

            //int xyDiff = Math.Max(Math.Abs(x - y), 1);
            int xyDiff = Math.Max((int)new[] { x, y }.Average(), 1);

            var randCol = rng.Next(0, 256);

            //randCol = 1;

            int salt = 0;

            switch (parameter)
            {
                case 0:

                    return (randCol * xyDiff) % 255;

                case 1:

                    salt = (int)new[] { _parameters.CanvasWidth, xyDiff }.Average();
                    //salt = Width / xyDiff

                    return (randCol * salt) % 255;

                case 2:

                    salt = (int)new[] { _parameters.CanvasHeight, xyDiff }.Average();
                    //salt = Height / xyDiff

                    return (randCol * _parameters.CanvasHeight / xyDiff) % 255;

                default:

                    return 0;
            }
        }
    }
}
