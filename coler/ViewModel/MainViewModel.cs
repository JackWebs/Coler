using System.ComponentModel;
using System.Runtime.CompilerServices;
using coler.Annotations;
using GalaSoft.MvvmLight;

namespace coler.ViewModel
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Fields

        

        #region Backing Fields

        

        #endregion

        #endregion

        #region Properties
        


        #endregion

        #region Getter Properties
        


        #endregion

        public MainViewModel()
        {
            Initialize();
        }

        #region Initialization

        private void Initialize()
        {
            
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