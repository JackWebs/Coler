using coler.BusinessLogic.Manager;
using coler.Model;
using coler.Model.ColorGen.Parameters;
using coler.Model.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coler.BusinessLogic.Subsystems.ColorGenFunctions
{
    public class FunctionGen5
    {
        private readonly ParametersGen5 _parameters;

        public FunctionGen5(ParametersGen5 parameters)
        {
            _parameters = parameters;
        }

        public (int, int, int) GeneratePixel(int x, int y, Random rng, PixelData point)
        {
            int colorRed = GenerateColor(x, y, EnColor.Red, rng, point);
            int colorGreen = GenerateColor(x, y, EnColor.Green, rng, point);
            int colorBlue = GenerateColor(x, y, EnColor.Blue, rng, point);

            return (colorRed, colorGreen, colorBlue);
        }

        public int GenerateColor(int x, int y, EnColor color, Random rng, PixelData point)
        {
            if (rng == null || point == null) return 0;

            double probability = (double)1 / (_parameters.XParameter * _parameters.XParameter);

            bool returnColor;

            int parameterY = _parameters.YParameter;

            lock (rng)
            {
                if (rng.NextDouble() < probability)
                {
                    int radius = rng.Next(1, parameterY);

                    switch (_parameters.Shape)
                    {
                        case 0:
                            CreateCircle(x, y, color, radius);
                            break;

                        case 1:
                            CreateSquare(x, y, color, radius);
                            break;

                        default:
                            CreateCircle(x, y, color, radius);
                            break;
                    }

                    returnColor = true;
                }
                else
                {
                    returnColor = false;
                }
            }

            if (returnColor)
            {
                return 255;
            }

            int existingColorValue;

            switch (color)
            {
                case EnColor.Red:

                    existingColorValue = point.ColorRed;

                    break;

                case EnColor.Green:

                    existingColorValue = point.ColorGreen;

                    break;

                case EnColor.Blue:

                    existingColorValue = point.ColorBlue;

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }

            return existingColorValue;
        }

        private void CreateSquare(int xCenter, int yCenter, EnColor color, int radius)
        {
            ColorGenManager colorManager = ColorGenManager.Instance;
            PixelData[][] points = colorManager.Points;

            int width = points.Length;
            int height = points[0].Length;

            int xMin = Math.Max(0, xCenter - (radius + 1));
            int yMin = Math.Max(0, yCenter - (radius + 1));

            int xMax = Math.Min(width, xCenter + (radius + 1));
            int yMax = Math.Min(height, yCenter + (radius + 1));

            List<PixelData> selectedPoints = new List<PixelData>();

            for (int x = xMin; x < xMax; x++)
            {
                for (int y = yMin; y < yMax; y++)
                {
                    selectedPoints.Add(points[x][y]);
                }
            }

            Parallel.ForEach(selectedPoints, point =>
            {
                int colorValue = 255;

                if (_parameters.HasGradient)
                {
                    double xDiff = 1 - Math.Abs(xCenter - point.CoordX) / (double)radius;
                    double yDiff = 1 - Math.Abs(yCenter - point.CoordY) / (double)radius;

                    double diff = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
                    colorValue = (int)Math.Max(0, Math.Min(255 * diff, 255));
                }

                switch (color)
                {
                    case EnColor.Red:

                        point.ColorRed = colorValue;

                        break;

                    case EnColor.Green:

                        point.ColorGreen = colorValue;

                        break;

                    case EnColor.Blue:

                        point.ColorBlue = colorValue;

                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(color), color, null);
                }
            });
        }

        private void CreateCircle(int xCenter, int yCenter, EnColor color, int radius)
        {
            ColorGenManager colorManager = ColorGenManager.Instance;
            PixelData[][] points = colorManager.Points;

            int width = points.Length;
            int height = points[0].Length;

            int xMin = Math.Max(0, xCenter - (radius + 1));
            int yMin = Math.Max(0, yCenter - (radius + 1));

            List<PixelData> selectedPoints = new List<PixelData>();

            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    if (x * x + y * y > radius * radius) continue;

                    int realX = Math.Max(0, Math.Min(xMin + x, width - 1));
                    int realY = Math.Max(0, Math.Min(yMin + y, height - 1));

                    selectedPoints.Add(points[realX][realY]);
                }
            }

            Parallel.ForEach(selectedPoints, point =>
            {
                int colorValue = 255;

                if (_parameters.HasGradient)
                {
                    double xDiff = 1 - Math.Abs(xCenter - point.CoordX) / (double)radius;
                    double yDiff = 1 - Math.Abs(yCenter - point.CoordY) / (double)radius;

                    double diff = 1 - Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
                    colorValue = (int)Math.Max(0, Math.Min(255 * diff, 255));
                }

                switch (color)
                {
                    case EnColor.Red:

                        point.ColorRed = colorValue;

                        break;

                    case EnColor.Green:

                        point.ColorGreen = colorValue;

                        break;

                    case EnColor.Blue:

                        point.ColorBlue = colorValue;

                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(color), color, null);
                }
            });
        }
    }
}