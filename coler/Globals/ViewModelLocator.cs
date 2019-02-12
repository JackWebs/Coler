using System.Windows;
using coler.UI.ViewModel;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace coler.Globals
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<GenerateImageViewModel>();
            SimpleIoc.Default.Register<ViewImageViewModel>();

            Messenger.Default.Register<NotificationMessage>(this, NotifyUserMethod);
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public GenerateImageViewModel GenerateImageViewModel => ServiceLocator.Current.GetInstance<GenerateImageViewModel>();
        public ViewImageViewModel ViewImageViewModel => ServiceLocator.Current.GetInstance<ViewImageViewModel>();

        private void NotifyUserMethod(NotificationMessage message)
        {
            MessageBox.Show(message.Notification);
        }
    }
}
