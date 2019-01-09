using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using coler.Annotations;
using coler.BusinessLogic;
using coler.Model.GenImage;
using GalaSoft.MvvmLight;

namespace coler.ViewModel
{
    public class ViewImageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Fields

        private GenImageManager _genImageManager;

        #region Backing Fields

        private ObservableCollection<GenImage> _images;

        private int _imageWidth;

        #endregion

        #endregion

        #region Properties

        public ObservableCollection<GenImage> Images
        {
            get => _images;
            set
            {
                if (Equals(value, _images)) return;
                _images = value;
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

        #endregion

        #region Getter Properties



        #endregion

        public ViewImageViewModel()
        {
            Initialize();
        }

        #region Initialization

        private void Initialize()
        {
            _genImageManager = GenImageManager.Instance;

            Images = _genImageManager.GetImageList();

            ImageWidth = 200;
        }

        #endregion

        #region Commands



        #endregion

        #region Private Methods



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
