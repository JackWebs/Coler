using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coler.Model.Parameter
{
    public abstract class ParameterGeneric<T> : ParameterBase
    {
        private T _value;
        private T[] _valueRange;

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(value, _value)) return;
                _value = value;
                OnPropertyChanged();
            }
        }

        public T[] ValueRange
        {
            get => _valueRange;
            set
            {
                if (Equals(value, _valueRange)) return;
                _valueRange = value;
                OnPropertyChanged();
            }
        }
    }
}
