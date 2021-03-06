﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coler.Model.Enum;

namespace coler.Model.Parameter
{
    public class ParameterInt : ParameterGeneric<int>
    {
        public ParameterInt()
        {
            Type = EnParameterType.Integer;
        }

        public override void Randomize(Random rng)
        {
            Value = rng.Next(ValueRange.First(), ValueRange.Last());
        }
    }
}
