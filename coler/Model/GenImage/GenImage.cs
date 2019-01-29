using System;
using System.IO;
using System.Xml.Serialization;
using coler.Globals;

namespace coler.Model.GenImage
{
    public class GenImage
    {
        public string SourceFilePath { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Saved { get; set; } = false;

        public GenImage()
        {

        }

        public GenImage(DateTime dateCreated)
        {
            DateCreated = dateCreated;
            SourceFilePath = Path.Combine(Constants.BufferDirectory, FileName);
        }

        [XmlIgnore]
        public string ThumbnailFilePath => Path.Combine(Constants.ThumbnailDirectory, FileName);
        [XmlIgnore]
        public string FileName => DateCreated.ToString("yyyy-MM-dd hh-mm-ss") + ".png";
        [XmlIgnore]
        public string DateCreatedString => $"{DateCreated.ToShortDateString()} {DateCreated.ToShortTimeString()}";
    }
}
