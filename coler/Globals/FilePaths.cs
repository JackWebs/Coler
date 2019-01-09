using System.IO;

namespace coler.Globals
{
    public static class FilePaths
    {
        public static string RootDirectory { get; } = @"C:\Personal Repos\Coler";
        public static string BufferDirectory { get; } = Path.Combine(RootDirectory, "Buffer");
        public static string TemplateDirectory { get; } = Path.Combine(RootDirectory, "Template");
        public static string ConfigDirectory { get; } = Path.Combine(RootDirectory, "Config");
        public static string GenImageListFilePath { get; } = Path.Combine(ConfigDirectory, "GenImageList.xml");

    }
}
