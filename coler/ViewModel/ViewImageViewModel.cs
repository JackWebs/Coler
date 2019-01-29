using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using coler.Annotations;
using coler.BusinessLogic;
using coler.BusinessLogic.Manager;
using coler.Model.GenImage;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace coler.ViewModel
{
    public class ViewImageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Fields

        private GenImageManager _genImageManager;

        #region Backing Fields

        private List<GenImageUi> _images;
        private ObservableCollection<GenImageUi> _displayedImages;

        private int _imageWidth;

        private bool _showSavedImages;

        #endregion

        #endregion

        #region Properties

        public List<GenImageUi> Images
        {
            get => _images;
            set
            {
                if (Equals(value, _images)) return;
                _images = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BufferImages));
                OnPropertyChanged(nameof(SavedImages));
            }
        }

        public ObservableCollection<GenImageUi> DisplayedImages
        {
            get => _displayedImages;
            set
            {
                if (Equals(value, _displayedImages)) return;
                _displayedImages = value;
                OnPropertyChanged();
            }
        }

        public int ImageWidth
        {
            get => _imageWidth;
            set
            {
                if (value.Equals(_imageWidth)) return;
                _imageWidth = value;
                OnPropertyChanged();
            }
        }

        public bool ShowSavedImages
        {
            get => _showSavedImages;
            set
            {
                if (value == _showSavedImages) return;
                _showSavedImages = value;

                UpdateDisplayedImages();

                OnPropertyChanged();
            }
        }

        #endregion

        #region Getter Properties

        public ObservableCollection<GenImageUi> BufferImages => new ObservableCollection<GenImageUi>(Images.Where(x => !x.ImageData.Saved));
        public ObservableCollection<GenImageUi> SavedImages => new ObservableCollection<GenImageUi>(Images.Where(x => x.ImageData.Saved));

        #endregion

        #region Command Properties

        public RelayCommand DeleteImagesCommand { get; private set; }
        public RelayCommand SaveImagesCommand { get; private set; }

        public RelayCommand<GenImageUi> SelectImageCommand { get; private set; }

        #endregion

        public ViewImageViewModel()
        {
            Initialize();
            AssignEvents();
        }

        #region Initialization

        private void Initialize()
        {
            _genImageManager = GenImageManager.Instance;

            InitializeProperties();
            InitializeCommands();
        }

        private void InitializeProperties()
        {
            RefreshImages();

            ImageWidth = 200;
        }

        private void InitializeCommands()
        {
            DeleteImagesCommand = new RelayCommand(DeleteImages);
            SaveImagesCommand = new RelayCommand(SaveImages);

            SelectImageCommand = new RelayCommand<GenImageUi>(SelectImage);
        }

        private void AssignEvents()
        {
            _genImageManager.GetImageList().CollectionChanged += (sender, args) =>
            {
                RefreshImages();
            };
        }

        #endregion

        #region Commands

        public void SelectImage(GenImageUi image)
        {
            image.IsSelected = !image.IsSelected;
        }

        public void SaveImages()
        {
            foreach (var image in BufferImages.Where(x => x.IsSelected))
            {
                _genImageManager.SaveImage(image.ImageData);
            }

            RefreshImages();
        }

        public void DeleteImages()
        {
            for (var i = Images.Count - 1; i >= 0; i--)
            {
                var image = Images[i];

                if (!image.IsSelected) continue;

                _genImageManager.DeleteImage(image.ImageData);
                Images.RemoveAt(i);
            }

            RefreshImages();
        }

        #endregion

        #region Private Methods

        private void RefreshImages()
        {
            var imageList = _genImageManager.GetImageList();
            var genImageUiList = new List<GenImageUi>();

            foreach (var image in imageList)
            {
                genImageUiList.Add(new GenImageUi(image));
            }

            Images = genImageUiList;

            UpdateDisplayedImages();
        }

        private void UpdateDisplayedImages()
        {
            DisplayedImages = ShowSavedImages
                ? SavedImages
                : BufferImages;
        }

        #endregion

        #region Events



        #endregion

        #region INotify

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
