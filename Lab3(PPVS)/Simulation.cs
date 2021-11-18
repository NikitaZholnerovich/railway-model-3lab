using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab3_PPVS_
{
    public class Simulation
    {
        public void Run()
        {
            Graph graph;

            try
            {
                graph = (new Loader()).Load();
            }
            catch (ValidationException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"File Validation Error:\n{ex.Message}");
                return;
            }

            var currentTick = 1;

            while (graph.Trains.Any(t => t.Route.Count > 1 && t.ServiceLife > 0)) // while all trains have destinations and not out of service
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Tick: {currentTick++}");
                Console.ResetColor();

                foreach (var train in graph.Trains)
                {
                    if (currentTick == 100)
                    {

                        Console.WriteLine("Break;");
                    }

                    if (train.Route.Count == 1) // train arrived at final destination
                    {
                        continue;
                    }

                    if (train.ServiceLife == 0) // out of service
                    {
                        continue;
                    }




                    var fromStation = train.Route[0];
                    var toStation = train.Route[1];

                    var isArrived = toStation.TrainsOnStation.Contains(train);
                    var edge = graph.Edges.FirstOrDefault(e => e.fromStation.Station == fromStation && e.toStation.Station == toStation);

                    if (!isArrived) // train not arrived yet
                    {
                        var isLeft = !fromStation.TrainsOnStation.Contains(train);


                        if (!isLeft) // train has not started moving yet
                        {
                            if (fromStation.Type == StationTypeEnum.Passenger || fromStation.Type == StationTypeEnum.PassengerAndFreight)
                            {
                                LoadCars<PassengerCar, Passengers>(train, fromStation);
                            }

                            if (fromStation.Type == StationTypeEnum.Freight || fromStation.Type == StationTypeEnum.PassengerAndFreight)
                            {
                                LoadCars<FreightCar, Goods>(train, fromStation);
                            }

                            fromStation.TrainsOnStation.Remove(train);
                            edge.TrainPositions.Add(train, 0);

                            Console.WriteLine($"{train.Name} starts moving from {fromStation.Name}");
                        }

                        edge.TrainPositions[train] += GetForce(train);
                        Console.WriteLine($"{train.Name} moved { edge.TrainPositions[train]} in direction to {toStation.Name}");

                        var isArrivedNow = edge.TrainPositions[train] >= edge.weight;
                        if (isArrivedNow)
                        {
                            edge.TrainPositions.Remove(train);
                            toStation.TrainsOnStation.Add(train);

                            Console.WriteLine($"{train.Name} arrived at {toStation.Name}");
                        }
                    }
                    else //train is on station
                    {
                        if (toStation.Type == StationTypeEnum.Passenger || toStation.Type == StationTypeEnum.PassengerAndFreight)
                        {
                            UnloadCars<PassengerCar>(train, toStation);
                        }

                        if (toStation.Type == StationTypeEnum.Freight || toStation.Type == StationTypeEnum.PassengerAndFreight)
                        {
                            UnloadCars<FreightCar>(train, toStation);
                        }

                        //actions complete

                        train.Route = train.Route.Skip(1).ToList();

                        if (train.Route.Count == 1)
                        {
                            Console.WriteLine($"{train.Name} reached it's final destination {train.Route.FirstOrDefault().Name}");
                        }
                    }

                    // decrease service life after each move
                    train.ServiceLife -= 1;

                    if (train.ServiceLife == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{train.Name} is out of service");
                        Console.ResetColor();
                    }
                }
            }
        }

        private int GetForce(Train train)
        {
            // total train weight consists of wight of the cars and cargo
            var totalLoad = train.Cars.Count;

            totalLoad += train.Cars.Sum(c => c.Load.Sum(l => l.Count));

            // calcualete percentage of power
            var percentage = (float)train.Force / (float)totalLoad;

            var currentForce = percentage > 1
                ? train.Force * (percentage - 1)
                : train.Force * percentage;

            return (int)currentForce;
        }

        private void UnloadCars<T>(Train train, Station station) where T : TrainCar
        {
            foreach (var c in train.Cars.OfType<T>())
            {
                var arrivedCarrying = c.Load.Where(a => a.Destination == station.Node);

                Console.WriteLine($"{station.Name}: {train.Name} unloading {arrivedCarrying.Count()} from car {typeof(T).Name} {c.Number}");
                foreach (var ac in arrivedCarrying)
                {
                    ac.Count = 0;
                }
            }
            foreach (var c in train.Cars.OfType<T>())
            {
                c.Load.RemoveAll(e => e.Count == 0);
            }
        }



        private void LoadCars<T, U>(Train train, Station station)
            where T : TrainCar
            where U : Movable, new()
        {
            var carryingCars = train.Cars.OfType<T>();

            foreach (Movable m in station.Movables.OfType<U>())
            {
                if (m.Count == 0)
                {
                    continue;
                }

                if (train.Route.Skip(1).Any(s => s.Node == m.Destination))
                {
                    foreach (var c in carryingCars)
                    {
                        if (m.Count == 0)
                        {
                            break;
                        }

                        var freePlace = c.Carrying - c.Load.Sum(p => p.Count);

                        if (freePlace == 0) // no more place in this car
                        {
                            continue;
                        }

                        var toLoad = Math.Min(freePlace, m.Count);
                        m.Count -= toLoad;

                        c.Load.Add(new U()
                        {
                            Count = toLoad,
                            Destination = m.Destination
                        });

                        Console.WriteLine($"{station.Name}: {train.Name} loaded {toLoad} {typeof(U).Name} to car {c.Number}");
                    }

                }
            }

            station.Movables.RemoveAll(m => (m is T) && m.Count == 0);
        }


        //private void LoadGoods(Train train, Station station)
        //{
        //    var freightCars = train.Cars.Where(c => c.Type == СarTypeEnum.Freight);

        //    foreach (Goods g in station.Movables.Where(m => m is Goods))
        //    {
        //        if (g.Amount == 0)
        //        {
        //            continue;
        //        }

        //        if (train.Route.Skip(1).Any(s => s.Node == g.Destination))
        //        {
        //            foreach (FreightCar fc in freightCars)
        //            {
        //                if (g.Amount == 0)
        //                {
        //                    break;
        //                }

        //                var freePlace = fc.Carrying - fc.Goods.Sum(p => p.Amount);

        //                if (freePlace == 0) // no more free place in this car
        //                {
        //                    continue;
        //                }

        //                var goodsToload = Math.Min(freePlace, g.Amount);
        //                g.Amount -= goodsToload;

        //                fc.Goods.Add(new Goods
        //                {
        //                    Amount = goodsToload,
        //                    Destination = g.Destination
        //                });

        //                Console.WriteLine($"{station.Name}: {train.Name} loaded {goodsToload} goods to car {fc.Number}");
        //            }

        //        }
        //    }

        //    station.Movables.RemoveAll(m => (m is Passengers) && ((Passengers)m).Count == 0);
        //}
    }
}
