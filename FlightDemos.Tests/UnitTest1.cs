using FlightRoutes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightDemos.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Write your code here
            List<Flight> flights = Program.LoadFlights(); //Magic method which loads all the flights between the airports

            var fastestRoute = Program.FindFastestRoute(flights, "ATL", "SAN").First();

            var route = String.Join(",", fastestRoute.Select(x => String.Format("{0}->{1}", x.FromAirportCode, x.ToAirportCode)));
            var time = fastestRoute.Sum(x => x.FlightDuration);
            Assert.AreEqual(route, "ATL->COS,COS->OAK,OAK->SAN");
        //    Assert.AreEqual(time, 8.9);

        }

        [TestMethod]
        public void TestMethod2()
        {
            //Write your code here
            List<Flight> flights = Program.LoadFlights(); //Magic method which loads all the flights between the airports

            var fastestRoute = Program.FindFastestRoute(flights, "ATL", "SAN");

            var route = String.Join(",", fastestRoute.First().Select(x => String.Format("{0}->{1}", x.FromAirportCode, x.ToAirportCode)));
            var time = fastestRoute.First().Sum(x => x.FlightDuration);
            Assert.AreEqual(fastestRoute.Count(), 2);
            Assert.AreEqual(route, "ATL->COS,COS->OAK,OAK->SAN");
         //   Assert.AreEqual(time, 8.9);

        }
    }
}
