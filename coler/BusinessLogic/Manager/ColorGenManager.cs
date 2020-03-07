using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        public IColorGen GetColorGen(EnGenType genType)
        {
            switch (genType)
            {
                case EnGenType.NotSet:

                    return null;

                case EnGenType.Gradient:

                    return new ColorGen1();

                case EnGenType.Lines:

                    return new ColorGen2();

                case EnGenType.Cascade1:

                    return new ColorGen3();

                case EnGenType.Cascade2:

                    return new ColorGen4();

                case EnGenType.BitsPieces:

                    return new ColorGen5();

                default:
                    return null;
            }
        }

        public void ClearPoints()
        {
            foreach (PixelData[] column in Points)
            {
                foreach (PixelData point in column)
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