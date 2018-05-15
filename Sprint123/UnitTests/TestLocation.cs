using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestLocation
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            List<Location> loclist = new List<Location>
            {
                new Location
                {
                    LocationID=1,
                    Name="Cinema Breda",
                    City ="Breda",
                    Rooms =6,
                    TicketPrice =10.00M
                },
                new Location
                {
                    LocationID=2,
                    Name="Cinema Eindhoven",
                    City ="Eindhoven",
                    Rooms =6,
                    TicketPrice =10.00M
                },
                new Location
                {
                    LocationID=3,
                    Name="Cinema Rotterdam",
                    City="Rotterdam",
                    Rooms =6,
                    TicketPrice =10.00M
                }
            };
            List<Location> list = loclist;


            //Act
            string location = list[0].Name;
            //Assert
            Assert.AreEqual(location, "Cinema Breda");

        }
    }
}
