using System;
using System.Collections.Generic;
using System.Linq;
using coler.Model;
using coler.Model.ColorGen;
using coler.Model.ColorGen.Functions;
using coler.Model.Enum;

namespace coler.BusinessLogic
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
        public List<PixelData> Points { get; set; }

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

            Points = new List<PixelData>();
        }

        public IColorGenFunction GetColorGen(int id)
        {
            return ColorGens.FirstOrDefault(x => x.Id == id);
        }

        public int GetPointColor(int x, int y, EnColor color)
        {
            var selectedPoint = Points.FirstOrDefault(point => point.CoordX == x && point.CoordY == y);

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
    }
}
