﻿using System;
using System.IO;

namespace coler.Globals
{
    public static class FilePaths
    {
        public static string UserPictures { get; } = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        public static string RootImageDirectory { get; } = Path.Combine(UserPictures, @"Coler Images");
        public static string BufferDirectory { get; } = Path.Combine(RootImageDirectory, "Image Buffer");
        public static string TemplateDirectory { get; } = Path.Combine(RootImageDirectory, "Image Templates");

        public static string AppData { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string RootFileDirectory { get; } = Path.Combine(AppData, @"ColerApp");
        public static string ConfigDirectory { get; } = Path.Combine(RootFileDirectory, "Data");
        public static string ThumbnailDirectory { get; } = Path.Combine(ConfigDirectory, "Thumbnails");
        public static string GenImageListFilePath { get; } = Path.Combine(ConfigDirectory, "GenImageList.xml");

        public static void CreateDirectories()
        {
            void CreateDirectory(string directory)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            CreateDirectory(RootImageDirectory);
            CreateDirectory(BufferDirectory);
            CreateDirectory(TemplateDirectory);

            CreateDirectory(RootFileDirectory);
            CreateDirectory(ConfigDirectory);
            CreateDirectory(ThumbnailDirectory);
        }
    }
}
