using System.Collections.Generic;

namespace Lab3_PPVS_
{
    public class TrainCar
    {
        public int Number { get; set; }
        public int Carrying { get; set; }

        public List<Movable> Load { get; set; } = new List<Movable>();
    }
}
