using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using coler.Annotations;

namespace coler.Model.GenImage
{
    public class GenImageList : INotifyPropertyChanged
    {
        private ObservableCollection<GenImage> _images;

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

        public GenImageList()
        {
            Images = new ObservableCollection<GenImage>();
        }

        #region INotify

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    #endregion
}
