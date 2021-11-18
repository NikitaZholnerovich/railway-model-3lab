using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab3_PPVS_
{
    public class Validator
    {
        public void ValidateStation(StationTypeEnum type, string movablesInfo)
        {
            var segments = movablesInfo.Split("|");

            if (type == StationTypeEnum.Passenger && segments[1] != "_")
            {
                throw new ValidationException("Cannot contain goods on passenger station");
            }

            if (type == StationTypeEnum.Freight && segments[0] != "_")
            {
                throw new ValidationException("Cannot contain passenger on freight station");
            }
        }

        public void ValidateStationNode(int node, IEnumerable<Station> stations)
        {
            if (!stations.Any(s => s.Node == node))
            {
                throw new ValidationException($"Station for node {node} does not exist");
            }
        }

        public void ValidateRoute(string routeString, IEnumerable<Station> stations)
        {
            var routeNodes = routeString.Split("-").Select(t => int.Parse(t));

            foreach (var node in routeNodes)
            {
                if (!stations.Any(s => s.Node == node))
                {
                    throw new ValidationException($"Route is not correct. Node {node} does not exist");
                }
            }
        }
    }
}
