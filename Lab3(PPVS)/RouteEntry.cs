using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3_PPVS_
{
    public class RouteEntry
    {
        public Train Train { get; set; }
        public Station FromStation { get; set; }
        public Station ToStation { get; set; }
        public List<RouteAction> Actions { get; set; } = new List<RouteAction>();


    }
}
