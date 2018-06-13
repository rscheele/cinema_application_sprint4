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
    public class TestEnquete
    {
        [TestMethod]
        public void TestEnqueteForm()
        {
            // Create test data
            Ticket ticket1 = new Ticket { TicketID = 0, TicketCode = "QW4545AW" };
            Ticket ticket2 = new Ticket { TicketID = 1, TicketCode = "GWEGWEM7" };
            EnqueteResponse enqueteResponse1 = new EnqueteResponse { TicketCode = "GWEGWEM7", UserScore = 5 };
            EnqueteResponse enqueteResponse2 = new EnqueteResponse { TicketCode = "QW4545AW", UserScore = 4 };
            EnqueteResponse enqueteResponse3 = new EnqueteResponse { TicketCode = "QW#T##@T"};
            EnqueteResponse enqueteResponse4 = new EnqueteResponse { TicketCode = null };
            List<EnqueteResponse> responses = new List<EnqueteResponse>();
            responses.Add(enqueteResponse1);
            responses.Add(enqueteResponse2);

            // Create Mock repositories
            Mock<ITicketRepository> mockTicketRepository = new Mock<ITicketRepository>();
            mockTicketRepository.Setup(m => m.GetTicketByCode("QW4545AW")).Returns(ticket1);
            mockTicketRepository.Setup(m => m.GetTicketByCode("GWEGWEM7")).Returns(ticket2);
            mockTicketRepository.Setup(m => m.GetTicketByCode("QW#T##@T")).Returns((Ticket)null);
            Mock<IEnqueteResponseRepository> mockEnqueteResponseRepository = new Mock<IEnqueteResponseRepository>();
            mockEnqueteResponseRepository.Setup(m => m.FindEnqueteResponse("GWEGWEM7")).Returns(enqueteResponse1);
            mockEnqueteResponseRepository.Setup(m => m.GetAllEnqueteResponses()).Returns(responses);

            // Setup fake identity
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockFrontOffice = mocks.Create<IPrincipal>();
            mockFrontOffice.SetupGet(p => p.Identity.Name).Returns("test4@gmail.com");
            mockFrontOffice.Setup(p => p.IsInRole("Admin")).Returns(true);

            // Create mock controller context for authentication
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockFrontOffice.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            // Setup the controller
            EnqueteController enqueteController = new EnqueteController(mockEnqueteResponseRepository.Object, mockTicketRepository.Object) { ControllerContext = mockContext.Object };

            // Check if the correct View is returned when the ticketcode has not been used yet
            var viewResult1 = enqueteController.Enquete(enqueteResponse2) as ViewResult;
            Assert.AreEqual(viewResult1.ViewBag.WrongCode, "Je enquete is verstuurd!");

            // Check if the correct View is returned when the ticketcode has been used
            var viewResult2 = enqueteController.Enquete(enqueteResponse1) as ViewResult;
            Assert.AreEqual(viewResult2.ViewBag.WrongCode, "Deze code is al eens gebruikt.");

            // Check if the correct View is returned when the ticketcode is invalid
            var viewResult3 = enqueteController.Enquete(enqueteResponse3) as ViewResult;
            Assert.AreEqual(viewResult3.ViewBag.WrongCode, "Deze code is niet geldig.");

            // Check if the correct View is returned when no code is filled in
            var viewResult4 = enqueteController.Enquete(enqueteResponse4) as ViewResult;
            Assert.AreEqual(viewResult4.ViewBag.WrongCode, "Voer een ticketcode in.");

            // Check if enquete results are correctly calculated and displayed for the admin
            var viewResult5 = enqueteController.ViewEnqueteResults() as ViewResult;
            Assert.AreEqual(viewResult5.ViewBag.MeanScore, 4.5);
        }
    }
}
