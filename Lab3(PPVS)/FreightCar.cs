using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3_PPVS_
{
    public class FreightCar : TrainCar
    {
        public override string ToString()
        {
            return $"Freigh, Carrying: {Carrying}";
        }
    }
}
