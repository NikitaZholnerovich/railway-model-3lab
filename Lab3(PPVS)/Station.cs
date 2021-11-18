using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3_PPVS_
{
    public class Station
    {
        public StationTypeEnum Type { get; set; }
        public string Name { get; set; }
        public int Node { get; set; }
        public List<Train> TrainsOnStation { get; set; } = new List<Train>();
        public List<Movable> Movables { get; set; } = new List<Movable>();

    }
}
