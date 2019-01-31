using System;
using System.Linq;
using coler.Model.Enum;

namespace coler.Model.ColorGen
{
    /*public class ColorGen2 : ColorGenBase, IColorGenFunction
    {
        public ColorGen2()
        {
            Id = 2;
            ColorParameters = new[] { 0, 1, 2};
        }

        public int GenerateColor(int x, int y, int parameter, EnColor color, Random rng = null, PixelData point = null)
        {
            var halfWidth = Width / 2;
            var halfHeight = Height / 2;

            var xVal = (int) Math.Abs(x * (1 / XParameter) - halfWidth) * rng.Next(0, 255);
            var yVal = (int) Math.Abs(y * (1 / YParameter) - halfHeight) * rng.Next(0, 255);

            switch (parameter)
            {
                case 0:

                    return xVal % 255;

                case 1:

                    return yVal % 255;

                case 2:

                    var colorVal = (int)new[] { xVal, yVal }.Average();

                    return colorVal % 255;
            }

            return 0;
        }
    }*/
}
