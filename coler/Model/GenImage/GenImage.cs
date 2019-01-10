using System;

namespace coler.Model.GenImage
{
    public class GenImage
    {
        public string SourceFilePath { get; set; }
        public string ThumbnailFilePath { get; set; }
        public DateTime DateCreated { get; set; }

        public string DateCreatedString => $"{DateCreated.ToShortDateString()} {DateCreated.ToShortTimeString()}";
    }
}
