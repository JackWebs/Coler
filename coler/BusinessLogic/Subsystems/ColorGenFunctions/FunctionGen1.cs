using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coler.BusinessLogic.Manager;
using coler.Model;
using coler.Model.ColorGen.Parameters;
using coler.Model.Enum;

namespace coler.BusinessLogic.Subsystems.ColorGenFunctions
{
    public class FunctionGen1
    {
        private readonly ParametersGen1 _parameters;

        public FunctionGen1(ParametersGen1 parameters)
        {
            _parameters = parameters;
        }

        public (int, int, int) GeneratePixel(int x, int y)
        {
            var colorRed = GenerateColor(x, y, _parameters.RedParameter);
            var colorGreen = GenerateColor(x, y, _parameters.GreenParameter);
            var colorBlue = GenerateColor(x, y, _parameters.BlueParameter);

            return (colorRed, colorGreen, colorBlue);
        }

        private int GenerateColor(int x, int y, int parameter)
        {
            int canvasWidth = _parameters.CanvasWidth;
            int canvasHeight = _parameters.CanvasHeight;

            int targetX;
            int targetY;

            switch (parameter)
            {
                case 0:

                    targetX = canvasWidth - 1;
                    targetY = 0;

                    break;

                case 1:

                    targetX = canvasWidth - 1;
                    targetY = canvasHeight - 1;

                    break;

                case 2:

                    targetX = 0;
                    targetY = canvasHeight - 1;

                    break;

                case 3:

                    targetX = 0;
                    targetY = 0;

                    break;

                default:

                    return 0;
            }

            var posX = Math.Abs(x - targetX);
            var posY = Math.Abs(y - targetY);

            double valX = posX == 0 ? 0 : (posX / (double)canvasWidth) * 255.0;
            double valY = posY == 0 ? 0 : (posY / (double)canvasHeight) * 255.0;


            var colorVal = Math.Max(0, Math.Min((int)new[] { valX, valY }.Average(), 255));

            return colorVal;
        }
    }
}
