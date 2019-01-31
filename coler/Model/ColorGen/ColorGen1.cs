using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using coler.Annotations;
using coler.BusinessLogic.Subsystems.ColorGenFunctions;
using coler.Model.ColorGen.Parameters;
using coler.Model.Enum;

namespace coler.Model.ColorGen
{
    public class ColorGen1 : INotifyPropertyChanged
    {
        private ParametersGen1 _parameters;
        private FunctionGen1 _function;

        public ParametersGen1 Parameters
        {
            get => _parameters;
            set
            {
                if (Equals(value, _parameters)) return;
                _parameters = value;
                OnPropertyChanged();
            }
        }

        public FunctionGen1 Function
        {
            get => _function;
            set
            {
                if (Equals(value, _function)) return;
                _function = value;
                OnPropertyChanged();
            }
        }

        public ColorGen1()
        {
            Parameters = new ParametersGen1();
            Function = new FunctionGen1(Parameters);
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
