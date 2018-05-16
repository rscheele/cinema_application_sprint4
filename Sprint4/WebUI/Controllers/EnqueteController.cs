using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class EnqueteController : Controller
    {
        private ITicketRepository ticketRepository;
        private IEnqueteResponseRepository enqueteResponseRepository;

        public EnqueteController(IEnqueteResponseRepository enqueteResponseRepository, ITicketRepository ticketRepository)
        {
            this.enqueteResponseRepository = enqueteResponseRepository;
            this.ticketRepository = ticketRepository;
        }

        [HttpGet]
        public ActionResult Enquete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Enquete(EnqueteResponse enqueteResponse)
        {
            Ticket ticket = ticketRepository.GetTicketByCode(enqueteResponse.TicketCode);
            ViewBag.WrongCode = "";
            if (enqueteResponse.TicketCode == null)
            {
                ViewBag.WrongCode = "Voer een ticketcode in.";
            }
            else if (ticket != null)
            {
                EnqueteResponse response = enqueteResponseRepository.FindEnqueteResponse(enqueteResponse.TicketCode);
                if (response != null)
                {
                    ViewBag.WrongCode = "Deze code is al eens gebruikt.";
                }
                else if (response == null)
                {
                    enqueteResponseRepository.SaveEnqueteEnqueteResponse(enqueteResponse);
                    ViewBag.WrongCode = "Je enquete is verstuurd!";
                }
            }
            else if (ticket == null)
            {
                ViewBag.WrongCode = "Deze code is niet geldig.";
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult ViewEnqueteResults()
        {
            List<EnqueteResponse> responses = enqueteResponseRepository.GetAllEnqueteResponses().Reverse().ToList();

            // CALCULATE MEAN SCORE
            int i = 0;
            int totalScore = 0;
            foreach (var item in responses)
            {
                i++;
                totalScore = totalScore + item.UserScore;
            }
            double meanScore = (double)totalScore / (double)i;
            ViewBag.MeanScore = meanScore;

            return View(responses);
        }
    }
}