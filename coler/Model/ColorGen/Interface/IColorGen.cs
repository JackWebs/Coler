using System;

namespace coler.Model.ColorGen.Interface
{
    public interface IColorGen
    {
        void SetCanvasSize();
        void RandomizeParameters(Random rng);
        (int r, int g, int b) GeneratePixel(int x, int y);
    }
}