using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coler.Model.Parameter
{
    public class ParameterInt : ParameterGeneric<int>
    {
        public override void Randomize(Random rng)
        {
            Value = rng.Next(ValueRange.First(), ValueRange.Last());
        }
    }
}
