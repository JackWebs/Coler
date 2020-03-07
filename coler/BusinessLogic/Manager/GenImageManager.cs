using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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

        public GenImageList GetImageList()
        {
            return ImageList;
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

            if (ImageList.Images.Count(x => !x.Saved) > Constants.MaxBufferImages)
            {
                GenImage[] orderedImages = ImageList.Images.OrderBy(x => x.DateCreated).ToArray();
                int numberOfImagesToDelete = ImageList.Images.Count - Constants.MaxBufferImages;

                ImageList.Images = new ObservableCollection<GenImage>(orderedImages.Skip(numberOfImagesToDelete));

                IEnumerable<GenImage> imagesToDelete = orderedImages.Take(numberOfImagesToDelete);

                foreach (GenImage image in imagesToDelete)
                {
                    DeleteImage(image);
                }
            }

            SaveConfigFile();
        }

        public void SaveImage(GenImage genImage)
        {
            if (File.Exists(genImage.SourceFilePath))
            {
                string oldSourceFilePath = genImage.SourceFilePath;

                genImage.SourceFilePath = Path.Combine(Constants.SavedImageDirectory, genImage.FileName);
                File.Copy(oldSourceFilePath, genImage.SourceFilePath);

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
            ObservableCollection<GenImage> imageList = GetImageList().Images;

            for (int i = imageList.Count - 1; i >= 0; i--)
            {
                GenImage genImage = imageList[i];

                if (!string.IsNullOrEmpty(genImage.SourceFilePath)) continue;

                imageList.RemoveAt(i);
            }
        }

        private void FixThumbnails()
        {
            foreach (GenImage genImage in GetImageList().Images)
            {
                if (File.Exists(genImage.ThumbnailFilePath)) continue;

                Bitmap image = new Bitmap(genImage.SourceFilePath);
                Utils.ResizeImage(image, 0.5).Save(genImage.ThumbnailFilePath, ImageFormat.Png);
            }
        }
    }
}