using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coler.Model.Enum;

namespace coler.Model.Parameter
{
    public class ParameterBool : ParameterGeneric<bool>
    {
        public ParameterBool()
        {
            Type = EnParameterType.Boolean;
        }

        public override void Randomize(Random rng)
        {
            Value = rng.NextDouble() > 0.5;
        }
    }
}
