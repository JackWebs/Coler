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
            var colorRed = GenerateColor(x, y, EnColor.Red, rng, point);
            var colorGreen = GenerateColor(x, y, EnColor.Green, rng, point);
            var colorBlue = GenerateColor(x, y, EnColor.Blue, rng, point);

            return (colorRed, colorGreen, colorBlue);
        }

        public int GenerateColor(int x, int y, EnColor color, Random rng, PixelData point)
        {
            if (rng == null || point == null) return 0;

            var probability = (double)1 / (_parameters.XParameter * _parameters.XParameter);

            bool returnColor;

            var parameterY = _parameters.YParameter;

            lock (rng)
            {
                if (rng.NextDouble() < probability)
                {
                    var radius = rng.Next(1, parameterY);

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
            var colorManager = ColorGenManager.Instance;
            var points = colorManager.Points;

            var width = points.Length;
            var height = points[0].Length;

            var xMin = Math.Max(0, xCenter - (radius + 1));
            var yMin = Math.Max(0, yCenter - (radius + 1));

            var xMax = Math.Min(width, xCenter + (radius + 1));
            var yMax = Math.Min(height, yCenter + (radius + 1));

            var selectedPoints = new List<PixelData>();

            for (var x = xMin; x < xMax; x++)
            {
                for (var y = yMin; y < yMax; y++)
                {
                    selectedPoints.Add(points[x][y]);
                }
            }

            Parallel.ForEach(selectedPoints, point =>
            {
                var colorValue = 255;

                if (_parameters.HasGradient)
                {
                    var xDiff = 1 - Math.Abs(xCenter - point.CoordX) / (double)radius;
                    var yDiff = 1 - Math.Abs(yCenter - point.CoordY) / (double)radius;

                    var diff = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
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
            var colorManager = ColorGenManager.Instance;
            var points = colorManager.Points;

            var width = points.Length;
            var height = points[0].Length;

            var xMin = Math.Max(0, xCenter - (radius + 1));
            var yMin = Math.Max(0, yCenter - (radius + 1));

            var selectedPoints = new List<PixelData>();

            for (var y = -radius; y <= radius; y++)
            {
                for (var x = -radius; x <= radius; x++)
                {
                    if (x * x + y * y > radius * radius) continue;

                    var realX = Math.Max(0, Math.Min(xMin + x, width-1));
                    var realY = Math.Max(0, Math.Min(yMin + y, height-1));

                    selectedPoints.Add(points[realX][realY]);
                }
            }

            Parallel.ForEach(selectedPoints, point =>
            {
                var colorValue = 255;

                if (_parameters.HasGradient)
                {
                    var xDiff = 1 - Math.Abs(xCenter - point.CoordX) / (double)radius;
                    var yDiff = 1 - Math.Abs(yCenter - point.CoordY) / (double)radius;

                    var diff = 1 - Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
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
