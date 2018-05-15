using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;

namespace UnitTests
{
    [TestClass]
    public class TestTickets
    {
        [TestMethod]
        public void SetupRepositories()
        {
            // Show Showseat Ticket Tempticket
            // Shows
            List<Show> shows = new List<Show>();
            Movie movie = new Movie { MovieID = 1, Name = "Darkest Hour", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 120, Is3D = false, Image = null, Trailer = "https://www.youtube.com/embed/LtJ60u7SUSw", MainActors = "Gary Oldman, Lily James", SubActors = "Kristin Scott Thomas", IMDB = "tt4555426", Website = "http://www.imdb.com/title/tt4555426/?ref_=plg_rt_1", Director = "Joe Wright", LocationID = 1, Description = "<p>Slechts een paar dagen nadat hij premier van het Verenigd Koninkrijk is geworden, moet Winston Churchill (Gary Oldman) meteen &eacute;&eacute;n van zijn zwaarste beproevingen doorstaan.</p><p>Hij moet kiezen of hij met Nazi Duitsland over een vredesverdrag wil onderhandelen, of dat hij voor de idealen en de vrijheid van zijn natie wil vechten.</p><p>Terwijl de niet te stoppen Nazi-troepen West-Europa binnenvallen en de dreiging van een invasie voelbaar is, heeft Churchill te maken met een onvoorbereid volk en een sceptische koning. En dan is ook nog zijn eigen politieke partij tegen hem aan het samenspannen.</p><p>Churchill moet zijn moeilijkste dieptepunt overwinnen en een natie achter zich zien te krijgen in een poging de wereldgeschiedenis te veranderen.</p>", Genre = "historisch" };
            Room room = new Room { RoomID = 1, RoomNumber = 1, TotalSeats = 240, RowCount = 12 };
            Show showToAdd = new Show { ShowID = 1, BeginTime = DateTime.Parse("2018-03-29 14:40:00.000"), EndTime = DateTime.Parse("2018-03-29 16:10:00.000"), MovieID = 1, RoomID = 1, NumberofTickets = 11, Movie = movie, Room = room };
            shows.Add(showToAdd);
            Movie movie2 = new Movie { MovieID = 1, Name = "Darkest Hour", Language = "Nederlands", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 110, Is3D = true, Image = null, Trailer = "https://www.youtube.com/embed/LtJ60u7SUSw", MainActors = "Gary Oldman, Lily James", SubActors = "Kristin Scott Thomas", IMDB = "tt4555426", Website = "http://www.imdb.com/title/tt4555426/?ref_=plg_rt_1", Director = "Joe Wright", LocationID = 1, Description = "<p>Slechts een paar dagen nadat hij premier van het Verenigd Koninkrijk is geworden, moet Winston Churchill (Gary Oldman) meteen &eacute;&eacute;n van zijn zwaarste beproevingen doorstaan.</p><p>Hij moet kiezen of hij met Nazi Duitsland over een vredesverdrag wil onderhandelen, of dat hij voor de idealen en de vrijheid van zijn natie wil vechten.</p><p>Terwijl de niet te stoppen Nazi-troepen West-Europa binnenvallen en de dreiging van een invasie voelbaar is, heeft Churchill te maken met een onvoorbereid volk en een sceptische koning. En dan is ook nog zijn eigen politieke partij tegen hem aan het samenspannen.</p><p>Churchill moet zijn moeilijkste dieptepunt overwinnen en een natie achter zich zien te krijgen in een poging de wereldgeschiedenis te veranderen.</p>", Genre = "historisch" };
            Show showToAdd2 = new Show { ShowID = 1, BeginTime = DateTime.Parse("2018-03-29 14:40:00.000"), EndTime = DateTime.Parse("2018-03-29 16:10:00.000"), MovieID = 1, RoomID = 1, NumberofTickets = 11, Movie = movie2, Room = room };
            shows.Add(showToAdd2);
            Mock<IShowRepository> mockShowRepository = new Mock<IShowRepository>();
            mockShowRepository.Setup(s => s.GetShows()).Returns(shows);

            // ShowSeat
            /*List<ShowSeat> showSeats = new List<ShowSeat>();
            int id = 0;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    ShowSeat seat = new ShowSeat { SeatID = id, IsReserved = false, IsTaken = false, ReservationID = 0, Room = room, RoomID = 1, RowNumber = i, SeatNumber = j, ShowID = 1, Show = showToAdd };
                    showSeats.Add(seat);
                    id++;
                }
            }
            for (int i = 121; i < 128; i++)
            {
                showSeats[i].IsReserved = true;
            }
            for (int i = 128; i < 134; i++)
            {
                showSeats[i].IsTaken = true;
            }*/
            Mock<IShowSeatRepository> mockShowSeatRepository = new Mock<IShowSeatRepository>();
            //mockShowSeatRepository.Setup(s => s.GetShowSeats()).Returns(showSeats);

            // TempTicket
            /*List<TempTicket> tempTickets = new List<TempTicket>
            {
                new TempTicket{ID = 1, IsPaid = false, Popcorn = false, Price = 0, ReservationID = 1, RowNumber = 1, Seat = null, SeatID = 1, SeatNumber = 1, Show = showToAdd, ShowID = 1, TicketID = 1, TicketType = "", TimeAdded = DateTime.Now},
                new TempTicket{ID = 1, IsPaid = false, Popcorn = false, Price = 0, ReservationID = 1, RowNumber = 1, Seat = null, SeatID = 1, SeatNumber = 2, Show = showToAdd, ShowID = 1, TicketID = 2, TicketType = "", TimeAdded = DateTime.Now}

            };*/
            Mock<ITempTicketRepository> mockTempTicketRepository = new Mock<ITempTicketRepository>();

            // Ticket
            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();

            // Create the controller
            var controller = new TicketController(mockShowRepository.Object, mockShowSeatRepository.Object, mockTempTicketRepository.Object, mockTicketRepository.Object);

            // Check if prices are calculated right for show 1
            controller.TempData["Secret"] = false;
            List<decimal> result = controller.calculatePrices(showToAdd);

            DayOfWeek today = DateTime.Today.DayOfWeek;
            List<decimal> prices = new List<decimal>();
            if (today == DayOfWeek.Monday | today == DayOfWeek.Tuesday | today == DayOfWeek.Wednesday | today == DayOfWeek.Thursday)
            {
                prices.Add(10.00M);
                prices.Add(10.00M);
                prices.Add(8.50M);
                prices.Add(8.50M);
            } else
            {
                prices.Add(10.00M);
                prices.Add(10.00M);
                prices.Add(10.00M);
                prices.Add(10.00M);
            }

            CollectionAssert.AreEqual(prices, result);

            // Check if prices are calculated right for show 2
            controller.TempData["Secret"] = true;
            List<decimal> result2 = controller.calculatePrices(showToAdd2);

            List<decimal> prices2 = new List<decimal>();
            if (today == DayOfWeek.Monday | today == DayOfWeek.Tuesday | today == DayOfWeek.Wednesday | today == DayOfWeek.Thursday)
            {
                prices2.Add(9.50M);
                prices2.Add(7.75M);
                prices2.Add(8.00M);
                prices2.Add(8.00M);
            }
            else
            {
                prices2.Add(9.50M);
                prices2.Add(7.75M);
                prices2.Add(9.50M);
                prices2.Add(9.50M);
            }

            CollectionAssert.AreEqual(prices2, result2);

            // Test first function in controller
            //var actionResult1 = (ActionResult)controller.OrderTickets(1,1);
        }
    }
}
