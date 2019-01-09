using System;

namespace coler.Model.GenImage
{
    public class GenImage
    {
        public string FilePath { get; set; }
        public DateTime DateCreated { get; set; }

        public string DateCreatedString => $"{DateCreated.ToShortDateString()} {DateCreated.ToShortTimeString()}";
    }
}
