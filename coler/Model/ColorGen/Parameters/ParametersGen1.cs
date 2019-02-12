using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using coler.BusinessLogic.Manager;
using coler.Model.Parameter;
using coler.Properties;
using MyToolkit.Collections;

namespace coler.Model.ColorGen.Parameters
{
    public class ParametersGen1 : INotifyPropertyChanged
    {
        private ObservableDictionary<int, ParameterBase> _parameters;

        public ObservableDictionary<int, ParameterBase> Parameters
        {
            get => _parameters;
            set
            {
                if (Equals(value, _parameters)) return;
                _parameters = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RedParameter));
                OnPropertyChanged(nameof(GreenParameter));
                OnPropertyChanged(nameof(BlueParameter));
            }
        }

        public int CanvasWidth { get; set; }
        public int CanvasHeight { get; set; }

        public int RedParameter => ((ParameterInt)Parameters[0]).Value;
        public int GreenParameter => ((ParameterInt)Parameters[1]).Value;
        public int BlueParameter => ((ParameterInt)Parameters[2]).Value;

        public ParametersGen1()
        {
            var valueRange = new[] {0, 1, 2, 3};

            Parameters = new ObservableDictionary<int, ParameterBase>
            {
                {0, new ParameterInt
                {
                    Name = "Red Parameter",
                    ValueRange = valueRange
                } },
                {1, new ParameterInt
                {
                    Name = "Green Parameter",
                    ValueRange = valueRange
                } },
                {2, new ParameterInt
                {
                    Name = "Blue Parameter",
                    ValueRange = valueRange
                } }
            };

            foreach (var parameter in Parameters.Values)
            {
                parameter.PropertyChanged += (sender, args) =>
                {
                    OnPropertyChanged(nameof(RedParameter));
                    OnPropertyChanged(nameof(GreenParameter));
                    OnPropertyChanged(nameof(BlueParameter));
                };
            }
        }

        public void Randomize(Random rng)
        {
            foreach (var parameter in Parameters.Values)
            {
                parameter.Randomize(rng);
            }
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