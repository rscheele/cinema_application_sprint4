using System;
using System.Collections.Generic;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class SecretMoviefunctTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var locations = new List<Location>
            {
            new Location{LocationID=1, Name="Cinemapolis",City="Breda", Rooms=6,TicketPrice=10.00M},
            };

            /*Mock<MovieOverviewRepository> mock = new Mock<MovieOverviewRepository>();
            mock.Setup(f => f.Movies).Returns(new Movie[] {
            new Movie { Name = "Darkest Hour", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 120, Is3D = false, LocationID = 1 },
            new Movie { Name = "Red Sparrow", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 140, Is3D = false, LocationID = 1 },
            new Movie { Name = "Death Wish", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 108, Is3D = true, LocationID = 1 },
            new Movie { Name = "Diep in de Zee", Language = "Nederlands", LanguageSub = "Nederlands", Age = 6, MovieType = 2, Length = 91, Is3D = false, LocationID = 1 },
            new Movie { Name = "Black Panther", Language = "Engels", LanguageSub = "Nederlands", Age = 12, MovieType = 2, Length = 134, Is3D = true, LocationID = 1 },
            new Movie { Name = "Bankier van het Verzet", Language = "Nederlands", LanguageSub = "Nederlands", Age = 12, MovieType = 2, Length = 123, Is3D = false, LocationID = 1 },
            new Movie { Name = "The Shape of Water", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 123, Is3D = false, LocationID = 1 },
            new Movie { Name = "Three BillBoards Outside Ebbing, Missouri", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 115, Is3D = false, LocationID = 1 },
            new Movie { Name = "De grote blije kinderfilm", Language = "Nederlands", LanguageSub = "Nederlands", Age = 4, MovieType = 2, Length = 150, Is3D = false, LocationID = 1 }
            });*/
            
        
            var roomlayouts = new List<RoomLayout>
            {
            new RoomLayout{LayoutID=1, FrontX=20, FrontY=20, BackX=2, BackY=10},
            new RoomLayout{LayoutID=2, FrontX=15, FrontY=15, BackX=1, BackY=10}
            };

            var rooms = new List<Room>
            {
            new Room{RoomID=1,RoomNumber=1,LayoutID=1,LocationID=1},
            new Room{RoomID=2,RoomNumber=2,LayoutID=1,LocationID=1},
            new Room{RoomID=3,RoomNumber=3,LayoutID=2,LocationID=1},
            new Room{RoomID=4,RoomNumber=4,LayoutID=2,LocationID=1},
            new Room{RoomID=5,RoomNumber=5,LayoutID=2,LocationID=1},
            new Room{RoomID=6,RoomNumber=6,LayoutID=2,LocationID=1}
            };

            var seats = new List<Seat>
            {
            new Seat{SeatID=1,RowX=1,RowY=1,SeatNumber=1,RoomID=1}
            };

            var shows = new List<Show>
            {
            new Show{ShowID=1,BeginTime=DateTime.Parse("2018-03-06 16:15:00.000"),EndTime=DateTime.Parse("2018-04-06 18:30:00.000"),MovieID=9,RoomID=2,ChildDiscount=true,StudentDiscount=true,SeniorDiscount=true},
            new Show{ShowID=1,BeginTime=DateTime.Parse("2018-03-06 19:00:00.000"),EndTime=DateTime.Parse("2018-04-06 21:00:00.000"),MovieID=1,RoomID=1,ChildDiscount=false,StudentDiscount=true,SeniorDiscount=true},
            new Show{ShowID=1,BeginTime=DateTime.Parse("2018-03-06 19:00:00.000"),EndTime=DateTime.Parse("2018-04-06 21:00:00.000"),MovieID=2,RoomID=2,ChildDiscount=false,StudentDiscount=true,SeniorDiscount=true},
            new Show{ShowID=1,BeginTime=DateTime.Parse("2018-03-06 19:00:00.000"),EndTime=DateTime.Parse("2018-04-06 21:00:00.000"),MovieID=3,RoomID=3,ChildDiscount=false,StudentDiscount=true,SeniorDiscount=true},
            new Show{ShowID=1,BeginTime=DateTime.Parse("2018-03-06 19:00:00.000"),EndTime=DateTime.Parse("2018-04-06 21:00:00.000"),MovieID=4,RoomID=4,ChildDiscount=false,StudentDiscount=true,SeniorDiscount=true},
            new Show{ShowID=1,BeginTime=DateTime.Parse("2018-03-06 19:00:00.000"),EndTime=DateTime.Parse("2018-04-06 21:00:00.000"),MovieID=5,RoomID=5,ChildDiscount=false,StudentDiscount=true,SeniorDiscount=true},
            new Show{ShowID=1,BeginTime=DateTime.Parse("2018-03-06 19:00:00.000"),EndTime=DateTime.Parse("2018-04-06 21:00:00.000"),MovieID=6,RoomID=6,ChildDiscount=false,StudentDiscount=true,SeniorDiscount=true},
            new Show{ShowID=1,BeginTime=DateTime.Parse("2018-03-06 21:15:00.000"),EndTime=DateTime.Parse("2018-04-06 23:15:00.000"),MovieID=7,RoomID=1,ChildDiscount=false,StudentDiscount=true,SeniorDiscount=true},
            new Show{ShowID=1,BeginTime=DateTime.Parse("2018-03-06 21:15:00.000"),EndTime=DateTime.Parse("2018-04-06 22:30:00.000"),MovieID=8,RoomID=2,ChildDiscount=false,StudentDiscount=true,SeniorDiscount=true}
            };


        }
    }
}
