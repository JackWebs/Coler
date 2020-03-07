using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using coler.Annotations;
using coler.BusinessLogic.Manager;
using coler.Model.GenImage;
using coler.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace coler.UI.ViewModel
{
    public class ViewImageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Fields

        private GenImageManager _genImageManager;

        #region Backing Fields

        private List<GenImageUi> _images;
        private ObservableCollection<GenImageUi> _displayedImages;

        private BitmapScalingMode _selectedScalingMode = BitmapScalingMode.Fant;
        private List<BitmapScalingMode> _scalingModes;

        private string _selectedImageFilePath;

        private int _imageWidth;

        private bool _showSavedImages;
        private bool _showImageZoom;

        #endregion Backing Fields

        #endregion Fields

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

        public string SelectedImageFilePath
        {
            get => _selectedImageFilePath;
            set
            {
                _selectedImageFilePath = value;
                OnPropertyChanged();
            }
        }

        public BitmapScalingMode SelectedScalingMode
        {
            get => _selectedScalingMode;
            set
            {
                if (value == _selectedScalingMode) return;
                _selectedScalingMode = value;
                OnPropertyChanged();
            }
        }

        public List<BitmapScalingMode> ScalingModes
        {
            get => _scalingModes;
            set
            {
                if (Equals(value, _scalingModes)) return;
                _scalingModes = value;
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

        public bool ShowImageZoom
        {
            get => _showImageZoom;
            set
            {
                if (_showImageZoom.Equals(value)) return;
                _showImageZoom = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Getter Properties

        public ObservableCollection<GenImageUi> BufferImages => new ObservableCollection<GenImageUi>(Images.Where(x => !x.ImageData.Saved));
        public ObservableCollection<GenImageUi> SavedImages => new ObservableCollection<GenImageUi>(Images.Where(x => x.ImageData.Saved));

        #endregion Getter Properties

        #region Command Properties

        public RelayCommand DeleteImagesCommand { get; private set; }
        public RelayCommand SaveImagesCommand { get; private set; }

        public RelayCommand<GenImageUi> SelectImageCommand { get; private set; }

        #endregion Command Properties

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

            ScalingModes = new List<BitmapScalingMode>
            {
                BitmapScalingMode.NearestNeighbor,
                BitmapScalingMode.Linear,
                BitmapScalingMode.Fant
            };
        }

        private void InitializeCommands()
        {
            DeleteImagesCommand = new RelayCommand(DeleteImages);
            SaveImagesCommand = new RelayCommand(SaveImages);

            SelectImageCommand = new RelayCommand<GenImageUi>(SelectImage);
        }

        private void AssignEvents()
        {
            _genImageManager.GetImageList().Images.CollectionChanged += (sender, args) =>
            {
                RefreshImages();
            };

            _genImageManager.GetImageList().PropertyChanged += (sender, args) =>
            {
                RefreshImages();
            };
        }

        #endregion Initialization

        #region Commands

        public void SelectImage(GenImageUi image)
        {
            image.IsSelected = !image.IsSelected;
        }

        public void SaveImages()
        {
            foreach (GenImageUi image in BufferImages.Where(x => x.IsSelected))
            {
                _genImageManager.SaveImage(image.ImageData);
            }

            RefreshImages();
        }

        public void DeleteImages()
        {
            GenImageUi[] imagesToRemove = Images.Where(x => x.IsSelected).ToArray();
            int imageCount = imagesToRemove.Count() - 1;

            for (int i = imageCount; i >= 0; i--)
            {
                GenImageUi image = imagesToRemove[i];

                if (!image.IsSelected) continue;

                _genImageManager.DeleteImage(image.ImageData);
                Images.Remove(image);
            }

            RefreshImages();
        }

        #endregion Commands

        #region Methods

        private void RefreshImages()
        {
            ObservableCollection<GenImage> imageList = _genImageManager.GetImageList().Images;
            List<GenImageUi> genImageUiList = new List<GenImageUi>();

            foreach (GenImage image in imageList)
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

        public void SetSelectedImage(GenImageUi genImageUi)
        {
            if (genImageUi == null)
            {
                SelectedImageFilePath = string.Empty;
                ShowImageZoom = false;
            }
            else
            {
                SelectedImageFilePath = genImageUi.ImageData.SourceFilePath;
                ShowImageZoom = true;
            }
        }

        #endregion Methods



        #region INotify

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotify
    }
}