using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using System.Net;
using System.Net.Mail;

namespace WebUI.Controllers
{
    public class ReservationController : Controller
    {
        private IMovieOverviewRepository movieRepository;
        private IShowRepository showRepository;
        private ITicketRepository ticketRepository;
        private ITempTicketRepository tempTicketRepository;
        private IShowSeatRepository showSeatRepository;
        private IEmailRepository emailRepository;
        private static Random random = new Random();

        public ReservationController(IMovieOverviewRepository movieRepository, IShowRepository showRepository, ITicketRepository ticketRepository, ITempTicketRepository tempTicketRepository, IShowSeatRepository showSeatRepository, IEmailRepository emailRepository)
        {
            this.movieRepository = movieRepository;
            this.showRepository = showRepository;
            this.ticketRepository = ticketRepository;
            this.tempTicketRepository = tempTicketRepository;
            this.showSeatRepository = showSeatRepository;
            this.emailRepository = emailRepository;
        }
        /*public decimal CalculateTotalPrice(IEnumerable<Ticket> ticket) {
            foreach (var item in ticket)
            {
                return TotalPrice + item.Price;
            }
            return TotalPrice;
        }*/
        // GET: Reservation
        [HttpGet]
        public ActionResult Reservation()
        {
            return View("Reservation");
        }

        [HttpPost]
        public ActionResult Reservation(string reservationID/*, string paytype*/)
        {
            long resID = Convert.ToInt64(reservationID);
            IEnumerable<Ticket> tickets = ticketRepository.GetTickets(resID);

            if (tickets.Count() > 0) { 
                // Add the show to the ticket
                List<Show> allShows = showRepository.GetShows().ToList();
                Show orderedShow = allShows.Find(r => r.ShowID == tickets.First().ShowID);
                foreach (var item in tickets)
                {
                    item.Show = orderedShow;
                }

                if (tickets.First().IsPaid == true)
                {
                    return View("DisplayReservation", tickets);
                }
                else
                {
                    //CalculateTotalPrice(tickets);
                    //ViewBag.price = TotalPrice;
                    return RedirectToAction("Pay", "Pin", new { reservationID = resID /*, paytype = payid */});
                }
            }
            else
            {
                return View("NoReservationFound");
            }
        }

        [HttpGet]
        public ActionResult PrintTickets(long reservationID)
        {
            PinViewModel model = (PinViewModel)TempData["model"];

            if (model.PinValue == "")
            {
                model.IncorrectPinValue = "Vul pincode in";

                TempData["model"] = model;
                return RedirectToAction("Pay", "Pin");
            }

            if (model.PinValue == "0000" | model.PinValue.Length <= 3)
            {
                model.IncorrectPinValue = "Vul een geldige pincode in";

                TempData["model"] = model;
                return RedirectToAction("Pay", "Pin");
            }
            else
            {
                List<TempTicket> tempTickets = tempTicketRepository.GetTempTicketsReservation(reservationID).ToList();
                List<Ticket> tickets = new List<Ticket>();
                if (tempTickets.Count != 0) {
                    IEnumerable<ShowSeat> showSeats = showSeatRepository.GetShowSeatsReservation(tempTickets.FirstOrDefault().ReservationID);
                    foreach (var item in tempTickets)
                    {
                        item.IsPaid = true;
                        foreach (var seat in showSeats)
                        {
                            if (seat.SeatID == item.SeatID)
                            {
                                seat.IsReserved = false;
                                seat.IsTaken = true;
                            }
                        }
                        Ticket ticket = new Ticket();
                        ticket.IsPaid = item.IsPaid;
                        ticket.Popcorn = item.Popcorn;
                        ticket.Price = item.Price;
                        ticket.ReservationID = item.ReservationID;
                        ticket.RowNumber = item.RowNumber;
                        ticket.Seat = item.Seat;
                        ticket.SeatID = item.SeatID;
                        ticket.SeatNumber = item.SeatNumber;
                        ticket.Show = item.Show;
                        ticket.ShowID = item.ShowID;
                        ticket.TicketType = item.TicketType;
                        ticket.Glasses = item.Glasses;
                        ticket.Vip = item.Vip;
                        tickets.Add(ticket);
                    }
                    showSeatRepository.UpdateShowSeats(showSeats.ToList());
                    ticketRepository.SaveTickets(tickets);
                    tempTicketRepository.DeleteTempTicket(tempTickets.FirstOrDefault().ReservationID);
                }
                else
                {
                    tickets = ticketRepository.GetTickets(reservationID).ToList();
                    IEnumerable<ShowSeat> showSeats = showSeatRepository.GetShowSeatsReservation(tickets.FirstOrDefault().ReservationID);
                    foreach (var item in tickets)
                    {
                        item.IsPaid = true;
                        foreach (var seat in showSeats)
                        {
                            if (seat.SeatID == item.SeatID)
                            {
                                seat.IsReserved = false;
                                seat.IsTaken = true;
                            }
                        }
                    }
                    ticketRepository.UpdateTickets(tickets);
                }

                Reservation reservation = new Reservation();
                reservation.reservationID = reservationID;
                return View("Success", reservation);
            }
        }

        [HttpGet]
        public ActionResult PrintSessionTickets(long reservationID)
        {
            List<Ticket> tickets = ticketRepository.GetTickets(reservationID).ToList();
            Show show = showRepository.FindShow(tickets[0].ShowID);
            var pdf = new PrintTickets(tickets, show);
            return pdf.SendPdf();
        }

        [HttpGet]
        public ActionResult PrintReservationTickets(long reservationID)
        {
            List<Ticket> tickets = ticketRepository.GetTickets(reservationID).ToList();
            Show show = showRepository.FindShow(tickets[0].ShowID);
            var pdf = new PrintTickets(tickets, show);
            return pdf.SendPdf();
        }

        [HttpGet]
        public ActionResult DisplayReservation(long reservationID)
        {
            IEnumerable<TempTicket> tempTickets = tempTicketRepository.GetTempTicketsReservation(reservationID);

            // Add the show to the ticket
            Show orderedShow = showRepository.FindShow(tempTickets.FirstOrDefault().ShowID);
            foreach (var item in tempTickets)
            {
                item.Show = orderedShow;
            }
            return View("DisplayTempReservation", tempTickets);
        }

        [HttpGet]
        public ActionResult EmailReservation(long reservationID, bool paid)
        {
            EmailReservation emailReservation = new EmailReservation();
            emailReservation.ReservationID = reservationID;
            emailReservation.Paid = paid;
            return View("EmailReservation", emailReservation);
        }

        [HttpPost]
        public ViewResult EmailReservation(EmailReservation emailReservation)
        {
            if (ModelState.IsValid)
            {
                if (emailReservation.NewsLetter == true)
                {
                    EmailAdress emailAdress = new EmailAdress();
                    emailAdress.Email = emailReservation.EmailAdress;
                    emailRepository.SaveEmailAdress(emailAdress);
                }

                Email(emailReservation.ReservationID, emailReservation.EmailAdress);

                List<TempTicket> tempTickets = tempTicketRepository.GetTempTicketsReservation(emailReservation.ReservationID).ToList();
                List<Ticket> tickets = new List<Ticket>();
                IEnumerable<ShowSeat> showSeats = showSeatRepository.GetShowSeatsReservation(tempTickets.FirstOrDefault().ReservationID);
                foreach (var item in tempTickets)
                {
                    foreach (var seat in showSeats)
                    {
                        if (seat.SeatID == item.SeatID)
                        {
                            seat.IsReserved = false;
                            seat.IsTaken = true;
                        }
                    }
                    Ticket ticket = new Ticket();
                    ticket.TicketCode = RandomString(10);
                    ticket.IsPaid = item.IsPaid;
                    ticket.Popcorn = item.Popcorn;
                    ticket.Price = item.Price;
                    ticket.ReservationID = item.ReservationID;
                    ticket.RowNumber = item.RowNumber;
                    ticket.Seat = item.Seat;
                    ticket.SeatID = item.SeatID;
                    ticket.SeatNumber = item.SeatNumber;
                    ticket.Show = item.Show;
                    ticket.ShowID = item.ShowID;
                    ticket.TicketType = item.TicketType;
                    ticket.Glasses = item.Glasses;
                    ticket.Vip = item.Vip;
                    tickets.Add(ticket);
                }
                showSeatRepository.UpdateShowSeats(showSeats.ToList());
                ticketRepository.SaveTickets(tickets);
                tempTicketRepository.DeleteTempTicket(tempTickets.FirstOrDefault().ReservationID);

                Reservation reservation = new Reservation();
                reservation.reservationID = emailReservation.ReservationID;
                reservation.Paid = emailReservation.Paid;
                return View("Success", reservation);
            }
            else
            {
                return View("EmailReservation", emailReservation);
            }
        }

        // FUNCTION TO GENERATE RANDOM STRING
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpGet]
        public ActionResult SendingEmail(EmailReservation emailReservation)
        {
            return View("SendingEmail", emailReservation);
        }

        private void Email(long reservationID, string emailTo)
        {
            var fromAddress = new MailAddress("avanscinema@gmail.com", "Avans Cinema");
            var toAddress = new MailAddress(emailTo, "To Name");
            const string fromPassword = "avans123";
            string subject = "Je AvansCinema ticket met ID " + reservationID.ToString();
            string body = "Jullie reserveringsnummer is " + reservationID.ToString() + " . Gebruik deze om je tickets op te halen bij de terminal in de bioscoop.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}