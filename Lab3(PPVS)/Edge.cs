using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3_PPVS_
{
   public class Edge
    {
        public Node fromStation { get; set; }
        public Node toStation { get; set; }
        public int weight { get; set; }
        public Dictionary<Train, int> TrainPositions { get; set; } = new Dictionary<Train, int>();
       

        public Edge (Node fromStation, Node toStation, int weight)
        {

            this.fromStation = fromStation;
            this.toStation = toStation;
            this.weight = weight;
        }

        public override string ToString()
        {
            return $"({fromStation}),({toStation})";
        }



    }
}
