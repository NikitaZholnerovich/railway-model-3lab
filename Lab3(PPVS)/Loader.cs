using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lab3_PPVS_
{
    public class Loader
    {
        public Graph Load()
        {
            var validator = new Validator();

            //var fileInput = File.ReadAllLines("Load.txt"); // OK
            //var fileInput = File.ReadAllLines("Load_2.txt"); // Station type error
            //var fileInput = File.ReadAllLines("Load_3.txt"); // Train node error
            var fileInput = File.ReadAllLines("Load_4.txt"); // Route error
            var path = fileInput.TakeWhile(l => !l.StartsWith("-")).ToArray();

            var stations = new List<Station>();
            var stationStrings = fileInput.Skip(path.Length + 1).TakeWhile(l => !l.StartsWith("-")).ToArray();

            foreach (var stationInfo in stationStrings)
            {
                var st = stationInfo.Split(",").ToArray();

                var station = new Station();
                station.Node = int.Parse(st[0]);
                station.Name = st[1];
                station.Type = Enum.Parse<StationTypeEnum>(st[2]);

                var movablesInfo = st[3].Split("|");

                validator.ValidateStation(station.Type, st[3]);

                if (movablesInfo[0] != "_")
                {
                    var passengersInfo = movablesInfo[0].Split(";");
                    station.Movables.AddRange(passengersInfo.Select(i =>
                    {
                        var pi = i.Split("-");
                        return new Passengers
                        {
                            Count = int.Parse(pi[0]),
                            Destination = int.Parse(pi[1])
                        };
                    }));
                }


                if (movablesInfo[1] != "_")
                {
                    var goodsInfo = movablesInfo[1].Split(";");
                    station.Movables.AddRange(goodsInfo.Select(i =>
                    {
                        var pi = i.Split("-");
                        return new Goods
                        {
                            Count = int.Parse(pi[0]),
                            Destination = int.Parse(pi[1])
                        };
                    }));
                }


                stations.Add(station);
            }

            Console.WriteLine("Stations:\n");
            foreach (var station in stations)
            {
                Console.WriteLine($"{station.Node} {station.Name} {station.Type.ToString()} ");
                foreach (var movable in station.Movables)
                {
                    if (movable is Passengers p)
                    {
                        Console.WriteLine($"\t{p.Count} passengers are going to {stations.FirstOrDefault(s => s.Node == p.Destination).Name}");
                    }

                    if (movable is Goods g)
                    {
                        Console.WriteLine($"\t{g.Count} goods are going to {stations.FirstOrDefault(s => s.Node == g.Destination).Name}");
                    }
                }
            }

            var trains = new List<Train>();
            var trainStrings = fileInput.Skip(path.Length + stationStrings.Length + 2).TakeWhile(l => !l.StartsWith("-")).ToArray();

            foreach (var trainInfo in trainStrings)
            {
                var tr = trainInfo.Split(",").ToArray();

                var train = new Train();
                train.Name = tr[1];
                train.Force = int.Parse(tr[2]);
                train.ServiceLife = int.Parse(tr[3]);

                var carNumber = 1;

                foreach (var car in tr[4].Split('|'))
                {
                    var carInfo = car.Split('-');
                    var carType = Enum.Parse<СarTypeEnum>(carInfo[0]);

                    if (carType == СarTypeEnum.Freight)
                    {
                        train.Cars.Add(new FreightCar()
                        {
                            Carrying = int.Parse(carInfo[1]),
                            Number = carNumber++

                        });
                    }
                    else if (carType == СarTypeEnum.Passenger)
                    {
                        train.Cars.Add(new PassengerCar()
                        {
                            Carrying = int.Parse(carInfo[1]),
                            Number = carNumber++

                        });
                    }
                }

                validator.ValidateRoute(tr[5], stations);

                train.Route = tr[5].Split("-").Select(t => stations.FirstOrDefault(s => s.Node == int.Parse(t))).ToList();

                trains.Add(train);

                validator.ValidateStationNode(int.Parse(tr[0]), stations);

                var s = stations.FirstOrDefault(s => s.Node == int.Parse(tr[0]));
                s.TrainsOnStation.Add(train);
            }

            Console.WriteLine("Trains:");
            foreach (var train in trains)
            {
                Console.WriteLine($"\n{train.Name} Force: {train.Force} ServiceLife: {train.ServiceLife}\nCars\n{string.Join("\n\t", train.Cars)}");

                Console.WriteLine($"\n Routes: {string.Join(" --> ", train.Route.Select(r => r.Name))}");
            }




            //creating a graph

            var graph = new Graph();
            var stationNodes = stations.Select(s => new Node() { Station = s });
            graph.AddNodes(stationNodes);

            foreach (var p in path)
            {
                var fromTo = p.Split(' ');
                var fromStation = stationNodes.FirstOrDefault(n => n.Station.Node == int.Parse(fromTo[0]));
                var toStation = stationNodes.FirstOrDefault(n => n.Station.Node == int.Parse(fromTo[1]));
                graph.AddEdge(fromStation, toStation, int.Parse(fromTo[2]));

            }

            graph.Trains = trains;
            return graph;
        }


    }
}
