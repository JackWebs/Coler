using System;
using System.Linq;
using coler.Model;
using coler.Model.ColorGen;
using coler.Model.Enum;

namespace coler.BusinessLogic.ColorGenFunctions
{
    public class ColorGen1 : ColorGenBase, IColorGenFunction
    {
        public ColorGen1()
        {
            Id = 1;
            ColorParameters = new[] { 0, 1, 2, 3};
        }

        public int GenerateColor(int x, int y, int parameter, EnColor color, Random rng = null, PixelData point = null)
        {
            int targetX;
            int targetY;

            switch (parameter)
            {
                case 0:

                    targetX = Width - 1;
                    targetY = 0;

                    break;

                case 1:

                    targetX = Width - 1;
                    targetY = Height - 1;

                    break;

                case 2:

                    targetX = 0;
                    targetY = Height - 1;

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

            double valX = posX == 0 ? 255 : (posX / (double)Width) * 255.0;
            double valY = posY == 0 ? 255 : (posY / (double)Height) * 255.0;


            var colorVal = Math.Min((int)new[] { valX, valY }.Average(), 255);

            return colorVal;
        }
    }
}
