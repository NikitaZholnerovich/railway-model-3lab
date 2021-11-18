using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Lab3_PPVS_
{
    public class PassengerCar : TrainCar
    {
        public override string ToString()
        {
            return $"Passenger, Number Of Seats: {Carrying}";
        }
    }
}
