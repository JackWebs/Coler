using System;
using coler.Model.Enum;

namespace coler.Model.ColorGen
{
    public interface IColorGenFunction
    {
        int Id { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        double XParameter { get; set; }
        double YParameter { get; set; }
        int[] ColorParameters { get; set; }

        int GenerateColor(int x, int y, int parameter, EnColor color, Random rng = null, PixelData point = null);
        void UpdateDimensions(int width, int height);
        void UpdateParameters(double xParameter, double yParameter);
    }
}