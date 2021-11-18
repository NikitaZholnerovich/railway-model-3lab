using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3_PPVS_
{
    public class Train
    {
        public int ServiceLife { get; set; }
        public int Force { get; set; }
        public List<TrainCar> Cars { get; set; } = new List<TrainCar>();
        public string Name { get; set; }
        public List<Station> Route { get; set; } = new List<Station>();

    }
}
