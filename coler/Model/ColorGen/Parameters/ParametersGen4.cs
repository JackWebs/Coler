﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using coler.Annotations;
using coler.BusinessLogic.Manager;
using coler.Model.Parameter;
using coler.Properties;
using MyToolkit.Collections;

namespace coler.Model.ColorGen.Parameters
{
    public class ParametersGen4 : INotifyPropertyChanged
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
        public int XParameter => ((ParameterInt)Parameters[3]).Value;
        public int YParameter => ((ParameterInt)Parameters[4]).Value;

        public ParametersGen4()
        {
            int[] valueRange = new[] { 0, 1, 2, 3 };

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
                } },
                {3, new ParameterInt
                {
                    Name = "X Parameter",
                    Value = 1
                } },
                {4, new ParameterInt
                {
                    Name = "Y Parameter",
                    Value = 1
                } },
            };

            foreach (ParameterBase parameter in Parameters.Values)
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
            foreach (ParameterBase parameter in Parameters.Values.Take(3))
            {
                parameter.Randomize(rng);
            }
        }

        public void SetCanvasSize()
        {
            ColorGenManager colorGenManager = ColorGenManager.Instance;

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

        #endregion INotify
    }
}