using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUI.Controllers;
using Domain.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Abstract;
using System.Net.Http;
using Moq;
using System.Web.Mvc;
using Domain.Entities;

namespace UnitTests
{
    [TestClass]
    public class TestReservation
    {
        [TestMethod]
        public void TestViewReservation()
        {
            var movieRepository = new Mock<IMovieOverviewRepository>();
            var showRepository = new Mock<IShowRepository>();
            /*var ticketRepository = new Mock<ITicketRepository>();

            ReservationController controller = new ReservationController(movieRepository.Object, showRepository.Object, ticketRepository.Object);

            // Reservtion page
            ActionResult result = controller.Reservation();

            // No reservation
            ActionResult result2 = controller.Reservation("453873873");*/
        }
    }
}
