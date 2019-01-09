using System.Drawing;
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
            var deserializer = new XmlSerializer(typeof(T));
            var deserializedData = (T)deserializer.Deserialize(xReader);

            return deserializedData;
        }

        public static void SerializeToXml(object instance, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fileStream))
            using (XmlTextWriter writer = new XmlTextWriter(sw))
            {
                XmlSerializer serializer = new XmlSerializer(instance.GetType());

                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;

                writer.WriteStartDocument();

                serializer.Serialize(writer, instance);

                writer.WriteEndDocument();
                writer.Close();
            }
        }

        public static Bitmap Transparent2Color(Bitmap bmp1, System.Drawing.Color target)
        {
            Bitmap bmp2 = new Bitmap(bmp1.Width, bmp1.Height);
            Rectangle rect = new Rectangle(System.Drawing.Point.Empty, bmp1.Size);
            using (Graphics g = Graphics.FromImage(bmp2))
            {
                g.Clear(target);
                g.DrawImageUnscaledAndClipped(bmp1, rect);
            }
            return bmp2;
        }
    }
}
