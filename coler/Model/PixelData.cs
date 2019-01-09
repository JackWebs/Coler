using System;
using coler.Model.Enum;

namespace coler.Model
{
    public class PixelData
    {
        public int CoordX { get; set; }
        public int CoordY { get; set; }

        public int ColorRed { get; set; }
        public int ColorBlue { get; set; }
        public int ColorGreen { get; set; }

        public int GetColor(EnColor color)
        {
            switch (color)
            {
                case EnColor.Red:

                    return ColorRed;

                case EnColor.Green:

                    return ColorGreen;

                case EnColor.Blue:

                    return ColorBlue;

                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }
    }
}
