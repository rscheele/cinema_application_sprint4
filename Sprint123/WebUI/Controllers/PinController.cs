using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class PinController : Controller
    {
        private ITempTicketRepository tempTicketRepository;
        private ITicketRepository ticketRepository;

        public PinController(ITempTicketRepository tempTicketRepository, ITicketRepository ticketRepository)
        {
            this.tempTicketRepository = tempTicketRepository;
            this.ticketRepository = ticketRepository;
        }

        // GET: Pin
        public ViewResult Pay(long reservationID, string paytype)
        {
            List<TempTicket> ticketsList = tempTicketRepository.GetTempTicketsReservation(reservationID).ToList();
            //ViewBag.tickets = ticketsList;
            
            if (paytype == "ideal")
            {
                IdealModel model2 = (IdealModel)TempData["model2"];
                if (model2 == null)
                {
                    model2 = new IdealModel();
                }
                model2.reservationID = reservationID;
                TempData["model2"] = model2;
                return View("Ideal", model2);
            }
            if (paytype == "credit")
            {
                CreditcardModel model4 = (CreditcardModel)TempData["model4"];
                if (model4 == null)
                {
                    model4 = new CreditcardModel();
                }
                model4.reservationID = reservationID;
                TempData["model4"] = model4;
                return View("Creditcard", model4);
            }
            else
            {
                PinViewModel model = (PinViewModel)TempData["model"];
                if (model == null)
                {
                    model = new PinViewModel();
                }
                model.reservationID = reservationID;
                TempData["model"] = model;

                return View("Pay", model);
            }
        }

        [HttpGet]
        public ActionResult PinViewAddNumber(String s, long reservationID)
        {
            PinViewModel model = (PinViewModel)TempData["model"];

            model.PinValue += s;

            TempData["model"] = model;
            return RedirectToAction("Pay", new { reservationID });
        }

        [HttpGet]
        public ActionResult PinViewRemoveNumber(long reservationID)
        {
            PinViewModel model = (PinViewModel)TempData["model"];

            model.PinRemoveNumber();

            TempData["model"] = model;
            return RedirectToAction("Pay", new { reservationID });
        }

        //[HttpPost]
        //ActionResult ValidateIdeal(long reservationID)
        //{
        //    IdealModel model = (IdealModel)TempData["model"];
        //    TempData["model"] = model;
        //    if (ModelState.IsValid)
        //    {
        //        return RedirectToAction("Finish", reservationID);
        //    }
        //    else
        //    {
        //        return View("Ideal", new { reservationID });
        //    }
        //}

        //[HttpPost]
        //ActionResult ValidateCredit(long reservationID)
        //{
        //    CreditcardModel model = (CreditcardModel)TempData["model"];
        //    TempData["model"] = model;
        //    if (ModelState.IsValid)
        //    {
        //        return RedirectToAction("Finish", reservationID);
        //    }
        //    else
        //    {
        //        return View("Creditcard", new { reservationID });
        //    }
        //}


        public ActionResult Finish(long reservationID)
        {
            List<TempTicket> tempTickets = tempTicketRepository.GetTempTicketsReservation(reservationID).ToList();
            bool paid = true;
            if (tempTickets.Count > 0)
            {
                foreach (var i in tempTickets)
                {
                    i.IsPaid = true;
                }
                tempTicketRepository.UpdateTempTickets(tempTickets);
                return RedirectToAction("EmailReservation", "Reservation", new { reservationID , paid});
            }
            else
            {
                List<Ticket> tickets = ticketRepository.GetTickets(reservationID).ToList();
                foreach (var i in tickets)
                {
                    i.IsPaid = true;
                }
                ticketRepository.UpdateTickets(tickets);
                return RedirectToAction("PrintReservationTickets", "Reservation", new { reservationID });
            }
        }
    }
}