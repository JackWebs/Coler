using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
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

            RemoveBrokenImages();
            FixThumbnails();
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
                if (!string.IsNullOrEmpty(genImage.ThumbnailFilePath)) continue;

                var image = new Bitmap(genImage.SourceFilePath);
                var fileName = genImage.DateCreated.ToString("yyyy-MM-dd hh-mm-ss") + ".png";

                genImage.ThumbnailFilePath = Path.Combine(FilePaths.ThumbnailDirectory, fileName);

                Utils.ResizeImage(image, 0.5).Save(genImage.ThumbnailFilePath, ImageFormat.Png);
            }
        }
    }
}
