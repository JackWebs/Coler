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
            Utils.CreateDirectories();
        }
    }
}
