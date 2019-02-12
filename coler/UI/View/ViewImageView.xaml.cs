using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using coler.Model.GenImage;
using coler.UI.ViewModel;

namespace coler.UI.View
{
    /// <summary>
    /// Interaction logic for ViewImageView.xaml
    /// </summary>
    public partial class ViewImageView : UserControl
    {
        public ViewImageView()
        {
            InitializeComponent();
        }

        private void ImageButton_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!((sender as Button)?.DataContext is GenImageUi genImageUi)) return;
            if (!(DataContext is ViewImageViewModel vm)) return;

            vm.SetSelectedImage(genImageUi);
        }

        private void ZoomBorder_OnDoubleClick(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is ViewImageViewModel vm)) return;

            vm.SetSelectedImage(null);
            ZoomBorder.Reset();
        }
    }
}
