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
                IdealModel idealModel = (IdealModel)TempData["idealModel"];
                if (idealModel == null)
                {
                    idealModel = new IdealModel();
                }
                idealModel.reservationID = reservationID;
                TempData["idealModel"] = idealModel;
                return View("Ideal", idealModel);
            }
            if (paytype == "credit")
            {
                CreditcardModel creditcardModel = (CreditcardModel)TempData["creditcardModel"];
                if (creditcardModel == null)
                {
                    creditcardModel = new CreditcardModel();
                }
                creditcardModel.reservationID = reservationID;
                TempData["creditcardModel"] = creditcardModel;
                return View("Creditcard", creditcardModel);
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

        //[HttpGet]
        public ActionResult FinishIdeal(long reservationID, string paytype, Bank bank)
        {
            IdealModel idealModel = (IdealModel)TempData["idealModel"];
            if (bank == 0)
            {
                idealModel.Bankerror = "Selecteer uw Bank";

                TempData["idealModel"] = idealModel;
                return RedirectToAction("Pay", "Pin", new { reservationID, paytype });
            }
            else
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
                    return RedirectToAction("EmailReservation", "Reservation", new { reservationID, paid });
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

        //[HttpGet]
        public ActionResult FinishCredit(long reservationID, string paytype, long? Creditcard, DateTime? ExpireDate, int? CVC)
        {
            CreditcardModel creditcardModel = (CreditcardModel)TempData["creditcardModel"];
            DateTime now = DateTime.Today;
            if (Creditcard < 1000000000000000 | Creditcard > 9999999999999999 || Creditcard.HasValue != true)
            {
                creditcardModel.Crediterror = "Vul een geldig creditcardnummer(16 cijfers) in.";
                creditcardModel.Expireerror = "";
                creditcardModel.CVCerror = "";
                TempData["creditcardModel"] = creditcardModel;
                return RedirectToAction("Pay", "Pin", new { reservationID, paytype });
            }
            if (CVC < 100 | CVC > 999 || CVC.HasValue != true)
            {
                creditcardModel.CVCerror = "Vul een geldige CVC code in van 3 cijfers.";
                creditcardModel.Expireerror = "";
                creditcardModel.Crediterror = "";

                TempData["creditcardModel"] = creditcardModel;
                return RedirectToAction("Pay", "Pin", new { reservationID, paytype });
            }
            if (ExpireDate.HasValue != true || ExpireDate < now.Date)
            {
                creditcardModel.Expireerror = "Vul een geldige en niet verlopen datum in.";
                creditcardModel.CVCerror = "";
                creditcardModel.Crediterror = "";

                TempData["creditcardModel"] = creditcardModel;
                return RedirectToAction("Pay", "Pin", new { reservationID, paytype });
            }
            else
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
                    return RedirectToAction("EmailReservation", "Reservation", new { reservationID, paid });
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

        [HttpGet]
        public ActionResult FinishPin(long reservationID)
        {
            PinViewModel model = (PinViewModel)TempData["model"];

            if (model.PinValue == "" | model.PinValue == null)
            {
                model.IncorrectPinValue = "Vul pincode in";

                TempData["model"] = model;
                return RedirectToAction("Pay", "Pin", new { reservationID });
            }

            if (model.PinValue == "0000" | model.PinValue.Length <= 3)
            {
                model.IncorrectPinValue = "Vul een geldige pincode in";

                TempData["model"] = model;
                return RedirectToAction("Pay", "Pin", new {reservationID});
            }
            else
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
}