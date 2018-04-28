using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FlightRoutes
{
   public class Program
    {
        public static List<Flight> LoadFlights()
        {
            return new List<Flight>()
        {
            new Flight() { FlightNumber= "001", FromAirportCode= "ATL",  ToAirportCode= "ORD", Distance= 1000, FlightDuration= 2.5f },
            new Flight() { FlightNumber= "002", FromAirportCode= "OAK",  ToAirportCode= "SAN", Distance= 1100, FlightDuration= 2.6f },
            new Flight() { FlightNumber= "002", FromAirportCode= "BOI",  ToAirportCode= "SAN", Distance= 1100, FlightDuration= 2.6f },
            new Flight() { FlightNumber= "003", FromAirportCode= "OAK",  ToAirportCode= "ATL", Distance= 1100, FlightDuration= 2.6f },
            new Flight() { FlightNumber= "004", FromAirportCode= "ATL",  ToAirportCode= "MDW", Distance= 1200, FlightDuration= 2.7f },
            new Flight() { FlightNumber= "005", FromAirportCode= "MDW",  ToAirportCode= "ORD", Distance= 1300, FlightDuration= 2.8f },
            new Flight() { FlightNumber= "006", FromAirportCode= "BOI",  ToAirportCode= "MDW", Distance= 1400, FlightDuration= 2.9f },
            new Flight() { FlightNumber= "007", FromAirportCode= "ORD",  ToAirportCode= "ATL", Distance= 1500, FlightDuration= 3.0f },
            new Flight() { FlightNumber= "008", FromAirportCode= "ATL",  ToAirportCode= "COS", Distance= 1600, FlightDuration= 3.1f },
            new Flight() { FlightNumber= "009", FromAirportCode= "COS",  ToAirportCode= "OAK", Distance= 1700, FlightDuration= 3.2f },
            new Flight() { FlightNumber= "010", FromAirportCode= "COS",  ToAirportCode= "BOI", Distance= 1800, FlightDuration= 3.3f }
            };

        }


        public static List<IEnumerable<Flight>> FindFastestRoute(List<Flight> allFlights, 
            string from, string to)
        {
            var directFlights = allFlights.Where(flight => flight.FromAirportCode == from && flight.ToAirportCode == to).ToList();
            //This will give us minimum hops flight i.e. direct flights
            if (directFlights.Count > 0)
                return new List<IEnumerable<Flight>>() { directFlights };

            List<string> traversed = new List<string>();           
            List<List<Flight>> allPossibleRoutes = new List<List<Flight>>();

           //Pass 1 ..Find out all possible routes with noises
            List<string> destination = new List<string>() { from };
            while (!destination.Contains(to))
            {
                var startingFlights = allFlights.Where(flight => destination.Contains( flight.FromAirportCode) && !destination.Contains(flight.ToAirportCode)).ToList();
                destination = startingFlights.Select(s => s.ToAirportCode).ToList();
                traversed.AddRange(destination);
                allPossibleRoutes.Add(startingFlights);
            }

            //Pass 2 : Get all valid routes via catesian products and filtering
            var result = CartesianProduct(allPossibleRoutes,to).ToList();
    
            return result;

        }
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>( IEnumerable<IEnumerable<T>> sequences,string to)
        {
            if (sequences == null)
            {
                return null;
            }

            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };

            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) => accumulator.SelectMany(
                    accseq => sequence,
                    (accseq, item) =>
                    {
                        if (accseq !=null && (accseq.LastOrDefault()  == null || (accseq.LastOrDefault() as Flight)?.ToAirportCode == (item as Flight)?.FromAirportCode))
                            return accseq.Concat(new[] { item });
                        else
                            return null;
                        }  
                    )).Where(item=>item != null && (item.LastOrDefault() as Flight)?.ToAirportCode == to);
        }
        static void Main(string[] args)
        {
            //Write your code here
            List<Flight> flights = LoadFlights(); //Magic method which loads all the flights between the airports

            var fastestRoute = FindFastestRoute(flights, "ATL", "SAN").First();

            var route = String.Join(",", fastestRoute.Select(x => String.Format("{0}->{1}", x.FromAirportCode, x.ToAirportCode)));
            var time = fastestRoute.Sum(x => x.FlightDuration);
            Console.WriteLine("Hello World!");
        }
    }
    public class Flight
    {
        public string FlightNumber { get; set; }
        public string FromAirportCode { get; set; }
        public string ToAirportCode { get; set; }
        public float Distance { get; set; }
        public float FlightDuration { get; set; }
    }

    

}
