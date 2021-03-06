﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace coler.Globals
{
    public static class Utils
    {
        public static T DeserializeXml<T>(string xmlFilePath)
        {
            string xmltext = File.ReadAllText(xmlFilePath);

            XmlReader xReader = XmlReader.Create(new StringReader(xmltext));
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            T deserializedData = (T)deserializer.Deserialize(xReader);

            return deserializedData;
        }

        public static void SerializeToXml(object instance, string path)
        {
            using FileStream fileStream = new FileStream(path, FileMode.Create);
            using StreamWriter sw = new StreamWriter(fileStream);
            using XmlTextWriter writer = new XmlTextWriter(sw);

            XmlSerializer serializer = new XmlSerializer(instance.GetType());

            writer.Formatting = Formatting.Indented;
            writer.Indentation = 4;

            writer.WriteStartDocument();

            serializer.Serialize(writer, instance);

            writer.WriteEndDocument();
            writer.Close();
        }

        public static Bitmap Transparent2Color(Bitmap bmp1, Color target)
        {
            Bitmap bmp2 = new Bitmap(bmp1.Width, bmp1.Height);
            Rectangle rect = new Rectangle(Point.Empty, bmp1.Size);
            using (Graphics g = Graphics.FromImage(bmp2))
            {
                g.Clear(target);
                g.DrawImageUnscaledAndClipped(bmp1, rect);
            }
            return bmp2;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            Rectangle destRect = new Rectangle(0, 0, width, height);
            Bitmap destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using ImageAttributes wrapMode = new ImageAttributes();

                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return destImage;
        }

        public static Bitmap ResizeImage(Image image, double scale)
        {
            int width = (int)(image.Width * scale);
            int height = (int)(image.Height * scale);

            Rectangle destRect = new Rectangle(0, 0, width, height);
            Bitmap destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static void CreateDirectories()
        {
            void CreateDirectory(string directory)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            CreateDirectory(Constants.RootImageDirectory);
            CreateDirectory(Constants.BufferDirectory);
            CreateDirectory(Constants.TemplateDirectory);
            CreateDirectory(Constants.SavedImageDirectory);

            CreateDirectory(Constants.RootFileDirectory);
            CreateDirectory(Constants.ConfigDirectory);
            CreateDirectory(Constants.ThumbnailDirectory);
        }
    }
}