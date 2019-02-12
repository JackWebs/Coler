using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using coler.BusinessLogic.Subsystems.ColorGenFunctions;
using coler.Model.ColorGen.Interface;
using coler.Model.ColorGen.Parameters;
using coler.Model.Enum;
using coler.Model.Parameter;
using coler.Properties;
using MyToolkit.Collections;

namespace coler.Model.ColorGen
{
    public class ColorGen4 : INotifyPropertyChanged, IColorGen
    {
        private ParametersGen4 _parameters;
        private FunctionGen4 _function;

        public ParametersGen4 Parameters
        {
            get => _parameters;
            set
            {
                if (Equals(value, _parameters)) return;
                _parameters = value;
                OnPropertyChanged();
            }
        }

        public FunctionGen4 Function
        {
            get => _function;
            set
            {
                if (Equals(value, _function)) return;
                _function = value;
                OnPropertyChanged();
            }
        }

        public ColorGen4()
        {
            Parameters = new ParametersGen4();
            Function = new FunctionGen4(Parameters);
        }

        public void SetCanvasSize()
        {
            Parameters.SetCanvasSize();
        }

        public void RandomizeParameters(int seed)
        {
            var rng = new Random(seed);
            Parameters.Randomize(rng);
        }

        public ObservableDictionary<int, ParameterBase> GetParameters()
        {
            return Parameters.Parameters;
        }

        public (int r, int g, int b) GeneratePixel(int x, int y, Random rng, PixelData point = null)
        {
            return Function.GeneratePixel(x, y, rng);
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
