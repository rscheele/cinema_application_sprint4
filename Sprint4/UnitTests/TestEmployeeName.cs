using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;

namespace UnitTests
{
    [TestClass]
    public class TestEmployeeName
    {
        [TestMethod]
        public void TestName()
        {
            // Create test data
            Room room = new Room { RoomID = 6, RoomNumber = 1, TotalSeats = 240, RowCount = 12 };
            Movie movie = new Movie { MovieID = 6, Name = "Red Sparrow", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 140, Is3D = true, Image = null, Trailer = "https://www.youtube.com/embed/PmUL6wMpMWw", MainActors = "Jennifer Lawerence, Joel Edgerton", SubActors = "Matthias Schoenaerts", IMDB = "tt2873282", Website = "http://www.imdb.com/title/tt2873282/?ref_=plg_rt_1", Director = "Francis Lawrence", LocationID = 1, Description = "<p>Tegen haar wil wordt Dominika Egorova (Jennifer Lawrence) opgeleid tot een &lsquo;sparrow&rsquo;, een getrainde verleidster van de Russische veiligheidsdienst.</p><p>Dominika leert haar lichaam als een wapen te gebruiken, maar heeft moeite haar zelfbewustzijn te behouden tijdens de onmenselijke training. In een oneerlijk systeem vindt ze haar kracht en ze groeit uit tot &eacute;&eacute;n van de beste aanwinsten van het programma. </p><p>Haar eerste doelwit is Nate Nash (Joel Edgerton), een CIA-agent die verantwoordelijk is voor de meest gevoelige infiltratie in de Russische inlichtingendienst.De twee jonge agenten komen in een neerwaartse spiraal van aantrekkingskracht en bedrog terecht, wat een bedreiging vormt voor hun carri&egrave;re, hun loyaliteit en de veiligheid van beide landen.</p><p>Red Sparrow is gebaseerd op het boek van de voormalige CIA-agent Jason Matthews.</p>", Genre = "actie" };
            Show show = new Show { ShowID = 3, BeginTime = DateTime.Parse("2018-06-12 19:00:00.000"), EndTime = DateTime.Parse("2018-06-12 21:00:00.000"), MovieID = 6, RoomID = 6, NumberofTickets = 0, ShowType = 0, Movie = movie, Room = room };
            Seat seat1 = new Seat { Room = room, RoomID = 6, RowNumber = 5, SeatID = 20, SeatNumber = 4 };
            Seat seat2 = new Seat { Room = room, RoomID = 6, RowNumber = 5, SeatID = 21, SeatNumber = 5 };
            Ticket ticket1 = new Ticket { TicketID = 0, TicketCode = "QW4545AW", ReservationID = 500, ShowID = 3, Glasses = false, IsPaid = true, Popcorn = true, Price = 10.00M, RowNumber = 5, Seat = seat1, SeatID = 20, SeatNumber = 4, Show = show, TicketType = "Kind", Vip = false };
            Ticket ticket2 = new Ticket { TicketID = 0, TicketCode = "JLIKQ456", ReservationID = 500, ShowID = 3, Glasses = false, IsPaid = true, Popcorn = false, Price = 12.00M, RowNumber = 5, Seat = seat2, SeatID = 21, SeatNumber = 4, Show = show, TicketType = "Normaal", Vip = false };
            List<Ticket> tickets = new List<Ticket>();
            tickets.Add(ticket1);
            tickets.Add(ticket2);
            
            // Create Mock repositories
            Mock<IMovieOverviewRepository> mockMovieOverviewRepository = new Mock<IMovieOverviewRepository>();
            Mock<IShowRepository> mockShowRepository = new Mock<IShowRepository>();
            mockShowRepository.Setup(m => m.FindShow(3)).Returns(show);
            Mock<IShowSeatRepository> mockShowSetRepository = new Mock<IShowSeatRepository>();
            Mock<ITempTicketRepository> mockTempTicketRepository = new Mock<ITempTicketRepository>();
            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockTicketRepository.Setup(m => m.GetShowTickets(500)).Returns(tickets);
            Mock<IRoomRepository> mockRoomRepository = new Mock<IRoomRepository>();

            // Setup fake identity
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockFrontOffice = mocks.Create<IPrincipal>();
            mockFrontOffice.SetupGet(p => p.Identity.Name).Returns("Yvon");
            mockFrontOffice.Setup(p => p.IsInRole("Kassa")).Returns(true);

            // Create mock controller context for authentication
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockFrontOffice.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            // Setup the controller
            SalesTicketController salesTicketController = new SalesTicketController(mockMovieOverviewRepository.Object, mockShowRepository.Object, mockShowSetRepository.Object, mockTempTicketRepository.Object, mockTicketRepository.Object, mockRoomRepository.Object) { ControllerContext = mockContext.Object };

            // Find out identity name for printing
            var userName = salesTicketController.HttpContext.User.Identity.Name;
            Assert.AreEqual("Yvon", userName);
        }
    }
}
