using System.Collections.ObjectModel;
using System.IO;
using coler.Globals;
using coler.Model.GenImage;

namespace coler.BusinessLogic
{
    public class GenImageManager
    {
        private static GenImageManager _instance;

        public static GenImageManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = new GenImageManager();

                return _instance;
            }
        }

        private GenImageList ImageList { get; }

        public GenImageManager()
        {
            ImageList = !File.Exists(FilePaths.GenImageListFilePath) ? new GenImageList() : ReadConfigFile();
        }

        public ObservableCollection<GenImage> GetImageList()
        {
            return ImageList.Images;
        }

        private GenImageList ReadConfigFile()
        {
            return Utils.DeserializeXml<GenImageList>(FilePaths.GenImageListFilePath);
        }

        private void SaveConfigFile()
        {
            Utils.SerializeToXml(ImageList, FilePaths.GenImageListFilePath);
        }

        public void AddImage(GenImage genImage)
        {
            ImageList.Images.Add(genImage);
            SaveConfigFile();
        }
    }
}
