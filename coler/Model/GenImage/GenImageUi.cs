using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using coler.Annotations;
using coler.Properties;

namespace coler.Model.GenImage
{
    public class GenImageUi : INotifyPropertyChanged
    {
        private GenImage _imageData;
        private bool _isSelected;

        public GenImage ImageData
        {
            get => _imageData;
            set
            {
                if (Equals(value, _imageData)) return;
                _imageData = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public GenImageUi(GenImage imageData)
        {
            ImageData = imageData;
        }

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
