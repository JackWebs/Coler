using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using coler.Annotations;
using coler.BusinessLogic.Manager;

namespace coler.Model.ColorGen.Parameters
{
    public class ParametersGen1 : INotifyPropertyChanged
    {
        private int _redParameter;
        private int _greenParameter;
        private int _blueParameter;

        public int CanvasWidth { get; set; }
        public int CanvasHeight { get; set; }

        public int RedParameter
        {
            get => _redParameter;
            set
            {
                if (value == _redParameter) return;
                _redParameter = value;
                OnPropertyChanged();
            }
        }

        public int GreenParameter
        {
            get => _greenParameter;
            set
            {
                if (value == _greenParameter) return;
                _greenParameter = value;
                OnPropertyChanged();
            }
        }

        public int BlueParameter
        {
            get => _blueParameter;
            set
            {
                if (value == _blueParameter) return;
                _blueParameter = value;
                OnPropertyChanged();
            }
        }

        public List<int> AvailableValues { get; set; }

        public ParametersGen1()
        {
            AvailableValues = new List<int>{0,1,2,3};
        }

        public void Randomize(Random rng)
        {
            RedParameter = rng.Next(AvailableValues.First(), AvailableValues.Last());
            GreenParameter = rng.Next(AvailableValues.First(), AvailableValues.Last());
            BlueParameter = rng.Next(AvailableValues.First(), AvailableValues.Last());
        }

        public void SetCanvasSize()
        {
            var colorGenManager = ColorGenManager.Instance;

            CanvasWidth = colorGenManager.CanvasWidth;
            CanvasHeight = colorGenManager.CanvasHeight;
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