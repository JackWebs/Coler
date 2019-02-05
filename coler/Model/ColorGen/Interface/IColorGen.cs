using System;
using coler.Model.Parameter;
using MyToolkit.Collections;

namespace coler.Model.ColorGen.Interface
{
    public interface IColorGen
    {
        void SetCanvasSize();
        void RandomizeParameters(int seed);

        ObservableDictionary<int, ParameterBase> GetParameters();
        (int r, int g, int b) GeneratePixel(int x, int y, Random rng, PixelData point);
    }
}