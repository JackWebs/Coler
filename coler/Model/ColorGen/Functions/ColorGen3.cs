using System;
using System.Linq;
using coler.Model.Enum;

namespace coler.Model.ColorGen.Functions
{
    public class ColorGen3 : ColorGenBase, IColorGenFunction
    {
        public ColorGen3()
        {
            Id = 3;
            ColorParameters = new[] { 0, 1, 2, 3};
        }

        public int GenerateColor(int x, int y, int parameter, EnColor color, Random rng = null, PixelData point = null)
        {
            if (rng == null) return 0;

            //int xyDiff = Math.Max(Math.Abs(x - y), 1);
            int xyDiff = Math.Max((int)new [] {x, y}.Average(), 1);

            var randCol = rng.Next(0, 256);

            //randCol = 1;

            int salt = 0;

            switch (parameter)
            {
                case 0:

                    return (randCol * xyDiff) % 255;

                case 1:

                    salt = (int) new[] {Width, xyDiff}.Average();
                    //salt = Width / xyDiff

                    return (randCol * salt) % 255;

                case 2:

                    salt = (int) new[] { Height, xyDiff }.Average();
                    //salt = Height / xyDiff

                    return (randCol * Height / xyDiff) % 255;

                default:

                    return 0;
            }
        }
    }
}
