using System;
using coler.Model;
using coler.Model.ColorGen;
using coler.Model.Enum;

namespace coler.BusinessLogic.ColorGenFunctions
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
            return 0;
        }
    }
}
