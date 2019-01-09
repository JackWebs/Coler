using System.Collections.ObjectModel;

namespace coler.Model.GenImage
{
    public class GenImageList
    {
        public ObservableCollection<GenImage> Images { get; set; }

        public GenImageList()
        {
            Images = new ObservableCollection<GenImage>();
        }
    }
}
