using coler.Globals;
using System.IO;
using System.Windows;

namespace coler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            CreateDirectoryIfNotExist(FilePaths.RootImageDirectory);
            CreateDirectoryIfNotExist(FilePaths.BufferDirectory);
            CreateDirectoryIfNotExist(FilePaths.TemplateDirectory);

            CreateDirectoryIfNotExist(FilePaths.RootFileDirectory);
            CreateDirectoryIfNotExist(FilePaths.ConfigDirectory);
        }

        private void CreateDirectoryIfNotExist(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
