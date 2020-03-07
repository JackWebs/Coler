using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using coler.Annotations;
using coler.BusinessLogic.Subsystems.ColorGenFunctions;
using coler.Model.ColorGen.Interface;
using coler.Model.ColorGen.Parameters;
using coler.Model.Enum;
using coler.Model.Parameter;
using coler.Properties;
using MyToolkit.Collections;

namespace coler.Model.ColorGen
{
    public class ColorGen3 : INotifyPropertyChanged, IColorGen
    {
        private ParametersGen3 _parameters;
        private FunctionGen3 _function;

        public ParametersGen3 Parameters
        {
            get => _parameters;
            set
            {
                if (Equals(value, _parameters)) return;
                _parameters = value;
                OnPropertyChanged();
            }
        }

        public FunctionGen3 Function
        {
            get => _function;
            set
            {
                if (Equals(value, _function)) return;
                _function = value;
                OnPropertyChanged();
            }
        }

        public ColorGen3()
        {
            Parameters = new ParametersGen3();
            Function = new FunctionGen3(Parameters);
        }

        public void SetCanvasSize()
        {
            Parameters.SetCanvasSize();
        }

        public void RandomizeParameters(int seed)
        {
            Random rng = new Random(seed);
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

        #endregion INotify
    }
}