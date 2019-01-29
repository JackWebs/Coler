using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using coler.Globals;
using coler.Model.GenImage;

namespace coler.BusinessLogic.Manager
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
            ImageList = !File.Exists(Constants.GenImageListFilePath) ? new GenImageList() : ReadConfigFile();

            RemoveBrokenImages();
            FixThumbnails();
        }

        public ObservableCollection<GenImage> GetImageList()
        {
            return ImageList.Images;
        }

        private GenImageList ReadConfigFile()
        {
            return Utils.DeserializeXml<GenImageList>(Constants.GenImageListFilePath);
        }

        private void SaveConfigFile()
        {
            Utils.SerializeToXml(ImageList, Constants.GenImageListFilePath);
        }

        public void AddImage(GenImage genImage)
        {
            ImageList.Images.Add(genImage);
            SaveConfigFile();
        }

        public void SaveImage(GenImage genImage)
        {
            if (File.Exists(genImage.SourceFilePath))
            {
                var oldSourceFilePath = genImage.SourceFilePath;

                genImage.SourceFilePath = Path.Combine(Constants.SavedImageDirectory, genImage.FileName);
                File.Move(oldSourceFilePath, genImage.SourceFilePath);

                genImage.Saved = true;
            }

            SaveConfigFile();
        }

        public void DeleteImage(GenImage genImage)
        {
            if (File.Exists(genImage.SourceFilePath))
            {
                File.Delete(genImage.SourceFilePath);
            }

            ImageList.Images.Remove(genImage);
            SaveConfigFile();   
        }

        private void RemoveBrokenImages()
        {
            var imageList = GetImageList();

            for (int i = imageList.Count - 1; i >= 0; i--)
            {
                var genImage = imageList[i];

                if (!string.IsNullOrEmpty(genImage.SourceFilePath)) continue;

                imageList.RemoveAt(i);
            }
        }

        private void FixThumbnails()
        {
            foreach(var genImage in GetImageList())
            {
                if (File.Exists(genImage.ThumbnailFilePath)) continue;

                var image = new Bitmap(genImage.SourceFilePath);
                Utils.ResizeImage(image, 0.5).Save(genImage.ThumbnailFilePath, ImageFormat.Png);
            }
        }
    }
}
