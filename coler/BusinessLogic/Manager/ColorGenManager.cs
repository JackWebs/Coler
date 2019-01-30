using System;
using System.Collections.Generic;
using System.Linq;
using coler.BusinessLogic.Subsystems.ColorGenFunctions;
using coler.Model;
using coler.Model.ColorGen;
using coler.Model.Enum;

namespace coler.BusinessLogic.Manager
{
    public class ColorGenManager
    {
        private static ColorGenManager _instance;

        public static ColorGenManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = new ColorGenManager();

                return _instance;
            }   
        }

        public List<IColorGenFunction> ColorGens { get; set; }
        public PixelData[][] Points { get; set; }

        public ColorGenManager()
        {
            ColorGens = new List<IColorGenFunction>
            {
                new ColorGen1(),
                new ColorGen2(),
                new ColorGen3(),
                new ColorGen4(),
                new ColorGen5(),
            };
        }

        public IColorGenFunction GetColorGen(int id)
        {
            return ColorGens.FirstOrDefault(x => x.Id == id);
        }

        public int GetPointColor(int x, int y, EnColor color)
        {
            var selectedPoint = GetPoint(x, y);

            if (selectedPoint == null) return 0;

            switch (color)
            {
                case EnColor.Red:

                    return selectedPoint.ColorRed;

                case EnColor.Green:

                    return selectedPoint.ColorGreen;

                case EnColor.Blue:

                    return selectedPoint.ColorBlue;

                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        public PixelData GetPoint(int x, int y)
        {
            if (x > Points.Length) return null;

            var column = Points[x];

            if (y > column.Length) return null;

            return column[y];
        }

        public void ClearPoints()
        {
            foreach (var column in Points)
            {
                foreach (var point in column)
                {
                    ResetPoint(point);
                }
            }
        }

        private void ResetPoint(PixelData point)
        {
            point.ColorRed = 0;
            point.ColorGreen = 0;
            point.ColorBlue = 0;
        }
    }
}
