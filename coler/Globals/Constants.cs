using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coler.Globals
{
    public static class Constants
    {
        #region FilePaths

        public static string UserPictures { get; } = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        public static string RootImageDirectory { get; } = Path.Combine(UserPictures, @"Coler Images");
        public static string BufferDirectory { get; } = Path.Combine(RootImageDirectory, "Image Buffer");
        public static string TemplateDirectory { get; } = Path.Combine(RootImageDirectory, "Templates");
        public static string SavedImageDirectory { get; } = Path.Combine(RootImageDirectory, "Saved Images");

        public static string AppData { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string RootFileDirectory { get; } = Path.Combine(AppData, @"ColerApp");
        public static string ConfigDirectory { get; } = Path.Combine(RootFileDirectory, "Data");
        public static string ThumbnailDirectory { get; } = Path.Combine(ConfigDirectory, "Thumbnails");
        public static string GenImageListFilePath { get; } = Path.Combine(ConfigDirectory, "GenImageList.xml");

        #endregion

        public static double DoubleEqualityTolerance { get; } = 0.001;
    }
}
