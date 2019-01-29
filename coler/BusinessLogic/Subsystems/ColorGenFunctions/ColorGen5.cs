using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using coler.BusinessLogic.Manager;
using coler.Model;
using coler.Model.ColorGen;
using coler.Model.Enum;

namespace coler.BusinessLogic.Subsystems.ColorGenFunctions
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
            if (rng == null || point == null) return 0;

            var probability = 1 / (XParameter * XParameter);
            var radius = YParameter;

            bool returnColor;

            if (rng.NextDouble() < probability)
            {
                //CreateSquare(x, y, color, radius);

                returnColor = true;
            }
            else
            {
                returnColor = false;
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

        /*private void CreateSquare(int xCoord, int yCoord, EnColor color)
        {
            var colorManager = ColorGenManager.Instance;
            var points = colorManager.Points;
        }

        private void CreateCircle(int xCoord, int yCoord, EnColor color, double radius)
        {
            var colorManager = ColorGenManager.Instance;
            var points = colorManager.Points;

            var width = points.Max(xPoint => xPoint.CoordX);
            var height = points.Max(yPoint => yPoint.CoordY);

            var xMin = (int) Math.Max(0, xCoord - radius);
            var yMin = (int) Math.Max(0, yCoord - radius);

            var xMax = (int) Math.Min(width, xCoord + radius);
            var yMax = (int) Math.Min(height, yCoord + radius);

            var count = xMax - xMin;

            var selectedPoints = new List<PixelData>();

            for (var y = yMin; y < yMax; y++)
            {
                selectedPoints.AddRange(points.GetRange((y*width) + xMin, count));
            }

            Parallel.ForEach(selectedPoints, point =>
            {
                if (point.CoordX == xCoord && point.CoordY == yCoord) return;
                var xDiff = Math.Abs(xCoord - point.CoordX);
                var yDiff = Math.Abs(yCoord - point.CoordY);

                var diff = Math.Sqrt(Math.Abs(xDiff * xDiff - yDiff * yDiff)) / radius;
                var colorValue = (int) (255 * diff);

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
        }*/
    }
}
