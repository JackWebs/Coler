using System;
using System.Collections.Generic;
using System.Linq;
using coler.Model;
using coler.Model.ColorGen;
using coler.Model.ColorGen.Interface;
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

        public PixelData[][] Points { get; set; }

        public int CanvasWidth { get; set; } = 1920;
        public int CanvasHeight { get; set; } = 1080;

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

        public IColorGen GetColorGen(int genType)
        {
            switch (genType)
            {
                case 0:

                    return null;

                case 1:

                    return new ColorGen1();

                case 2:

                    return new ColorGen2();

                case 3:

                    return new ColorGen3();

                case 4:

                    return new ColorGen4();

                case 5:

                    return new ColorGen5();

                default:
                    return null;
            }
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