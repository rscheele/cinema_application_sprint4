using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class TicketController : Controller
    {
        private IShowRepository showRepository;
        private IShowSeatRepository showSeatRepository;
        private ITempTicketRepository tempTicketRepository;
        private ITicketRepository ticketRepository;

        public TicketController(IShowRepository showRepository, IShowSeatRepository showSeatRepository, ITempTicketRepository tempTicketRepository, ITicketRepository ticketRepository)
        {
            this.showRepository = showRepository;
            this.showSeatRepository = showSeatRepository;
            this.tempTicketRepository = tempTicketRepository;
            this.ticketRepository = ticketRepository;
        }

        [HttpGet]
        public ActionResult OrderTickets(long reservationID, int showID)
        {
            // Remove tickets that are still in progress of being bought older than 5 minutes
            DateTime currentDateTime = DateTime.Now;
            DateTime minusDateTime = currentDateTime.Add(new TimeSpan(0, -5, 0));
            List<TempTicket> tempTicketList = tempTicketRepository.GetTempTickets().ToList();
            List<TempTicket> oldTempTickets = new List<TempTicket>();

            foreach (var i in tempTicketList)
            {
                if (i.TimeAdded < minusDateTime)
                {
                    List<ShowSeat> showSeats = showSeatRepository.GetShowSeats(i.ShowID).ToList();
                    foreach (var j in showSeats)
                    {
                        if (i.SeatID == j.SeatID)
                        {
                            j.IsReserved = false;
                            showSeatRepository.UpdateShowSeats(j);
                            break;
                        }
                    }
                    oldTempTickets.Add(i);
                }
            }
            tempTicketRepository.DeleteTempTickets(oldTempTickets);

            Show selectedShow = showRepository.FindShow(showID);
            string soldOut = (string)TempData["SoldOut"];
            List<decimal> tarrifs = calculatePrices(selectedShow);

            // Set reservation ID for order
            Order order = new Order();
            order.ShowID = showID;
            order.ReservationID = reservationID;

            //hide name if movie is secret ----BEGIN
            //Boolean IsSecret = (Boolean)TempData["Secret"];
            bool secret = (bool)TempData["Secret"];
            if (secret != true)
            {
                ViewBag.MovieName = selectedShow.Movie.Name;
            }
            else
            {
                ViewBag.MovieName = "?";
            }
            //hide name if movie is secret ----END
            ViewBag.NormalPrice = tarrifs[0];
            ViewBag.ChildPrice = tarrifs[1];
            ViewBag.StudentPrice = tarrifs[2];
            ViewBag.SeniorPrice = tarrifs[3];
            if (soldOut != null)
            {
                ViewBag.SoldOut = soldOut;
            }

            TempData["Secret"] = secret;
            return View("OrderTickets", order);
        }

        [HttpPost]
        public ActionResult OrderTickets(Order order)
        {
            // Add a maximum of tickets that can be ordered based on database
            Show selectedShow = showRepository.FindShow(order.ShowID);
            List<TempTicket> tempTickets = new List<TempTicket>();
            int bookedTickets = ticketRepository.GetShowTickets(selectedShow.ShowID).Count();
            int reservedTickets = tempTicketRepository.GetTempTicketsShow(selectedShow.ShowID).Count();
            int totalBookedSeats = bookedTickets + reservedTickets;
            int maxSeats = selectedShow.Room.TotalSeats;
            int seatsLeft = maxSeats - totalBookedSeats;
            int max = 10;
            int numberoftickets = 0;
            if (seatsLeft < 10)
            {
                max = seatsLeft;
            }
            TempData["Show"] = selectedShow;

            List<TempTicket> existingTempTickets = tempTicketRepository.GetTempTicketsReservation(order.ReservationID).ToList();
            if (existingTempTickets != null)
            {
                tempTicketRepository.DeleteTempTickets(existingTempTickets);
            }

            //TempData["Order"] = order;
            // Check if there are tickets available
            int ticketcount = order.StudentTickets + order.SeniorTickets + order.NormalTickets + order.ChildTickets;
            if (max == 0)
            {
                TempData["SoldOut"] = "De show is helaas uitverkocht. Er zijn geen tickets meer beschikbaar.";
                return RedirectToAction("OrderTickets", new { order.ReservationID , selectedShow.ShowID});
            }
            else if (ticketcount <= 0 | ticketcount > 10)
            {
                return RedirectToAction("OrderTickets", new { order.ReservationID , selectedShow.ShowID });
            }
            else if (ticketcount > max)
            {
                TempData["SoldOut"] = "De show is bijna uitverkocht, er zijn nog maar " + max + " tickets beschikbaar!";
                return RedirectToAction("OrderTickets", new { order.ReservationID , selectedShow.ShowID });
            }
            else
            {
                List<decimal> tarrifs = calculatePrices(selectedShow);
                // Add normal tickets
                for (int i = 0; i < order.NormalTickets; i++)
                {
                    TempTicket tempTicket = new TempTicket();
                    tempTicket.Price = tarrifs[0];
                    tempTicket.TicketType = "Standaard";
                    tempTicket.ReservationID = order.ReservationID;
                    tempTicket.ShowID = selectedShow.ShowID;
                    tempTicket.TimeAdded = DateTime.Now;
                    tempTicket.Show = selectedShow;
                    tempTicket.IsPaid = false;
                    tempTicket.ShowID = selectedShow.ShowID;
                    tempTickets.Add(tempTicket);
                    numberoftickets++;
                }
                // Add child tickets
                for (int i = 0; i < order.ChildTickets; i++)
                {
                    TempTicket tempTicket = new TempTicket();
                    tempTicket.Price = tarrifs[1];
                    tempTicket.TicketType = "Kind";
                    tempTicket.ReservationID = order.ReservationID;
                    tempTicket.ShowID = selectedShow.ShowID;
                    tempTicket.TimeAdded = DateTime.Now;
                    tempTicket.Show = selectedShow;
                    tempTicket.IsPaid = false;
                    tempTicket.ShowID = selectedShow.ShowID;
                    tempTickets.Add(tempTicket);
                    numberoftickets++;
                }
                // Add student tickets
                for (int i = 0; i < order.StudentTickets; i++)
                {
                    TempTicket tempTicket = new TempTicket();
                    tempTicket.Price = tarrifs[2];
                    tempTicket.TicketType = "Student";
                    tempTicket.ReservationID = order.ReservationID;
                    tempTicket.ShowID = selectedShow.ShowID;
                    tempTicket.TimeAdded = DateTime.Now;
                    tempTicket.Show = selectedShow;
                    tempTicket.IsPaid = false;
                    tempTicket.ShowID = selectedShow.ShowID;
                    tempTickets.Add(tempTicket);
                    numberoftickets++;
                }
                // Add senior tickets
                for (int i = 0; i < order.SeniorTickets; i++)
                {
                    TempTicket tempTicket = new TempTicket();
                    tempTicket.Price = tarrifs[3];
                    tempTicket.TicketType = "Senior";
                    tempTicket.ReservationID = order.ReservationID;
                    tempTicket.ShowID = selectedShow.ShowID;
                    tempTicket.TimeAdded = DateTime.Now;
                    tempTicket.Show = selectedShow;
                    tempTicket.IsPaid = false;
                    tempTicket.ShowID = selectedShow.ShowID;
                    tempTickets.Add(tempTicket);
                    numberoftickets++;
                }
                selectedShow.NumberofTickets = selectedShow.NumberofTickets + numberoftickets;
                tempTicketRepository.SaveTempTickets(tempTickets);
                return RedirectToAction("AddPopcorn", new { order.ReservationID });
            }
        }

        [HttpGet]
        public ActionResult AddPopcorn(long reservationID)
        {
            List<TempTicket> tempTickets = tempTicketRepository.GetTempTicketsReservation(reservationID).ToList();
            List<TempTicketModel> tempTicketModel = new List<TempTicketModel>();
            Show selectedShow = showRepository.FindShow(tempTickets.FirstOrDefault().ShowID);

            foreach (var item in tempTickets)
            {
                TempTicketModel model = new TempTicketModel();
                model.ReservationID = item.ReservationID;
                model.Price = item.Price;
                model.TicketType = item.TicketType;
                model.Is3D = selectedShow.Movie.Is3D;
                tempTicketModel.Add(model);
            }
            return View("AddPopcorn", tempTicketModel);
        }

        [HttpPost]
        public ActionResult AddPopcorn(List<TempTicketModel> tickets)
        {
            List<TempTicket> ticketList = tempTicketRepository.GetTempTicketsReservation(tickets.FirstOrDefault().ReservationID).ToList();
            for (int i = 0; i < tickets.Count; i++)
            {
                if (tickets[i].Popcorn == true)
                {
                    ticketList[i].Price = ticketList[i].Price + 5.00M;
                    ticketList[i].Popcorn = true;
                }
                else
                {
                    ticketList[i].Popcorn = false;
                }
                if (tickets[i].Glasses == true)
                {
                    ticketList[i].Price = ticketList[i].Price + 2.00M;
                    ticketList[i].Glasses = true;
                }
                else
                {
                    ticketList[i].Glasses = false;
                }
            }
            tempTicketRepository.UpdateTempTickets(ticketList);
            return RedirectToAction("SelectSeats", "SeatSelection", new { reservationID = tickets.FirstOrDefault().ReservationID });
        }

        public List<decimal> calculatePrices(Show show)
        {
            decimal normal;
            decimal child;
            decimal student;
            decimal senior;

            bool secret = (bool)TempData["Secret"];

            // Calculate the base price
            if (show.Movie.Length >= 120)
            {
                normal = 10.00M;
            } else
            {
                normal = 9.50M;
            }

            if (secret == true)
            {
                normal = normal - 2.50M;
            }

            // Calculate wether the movie is in 3D
            if (show.Movie.Is3D == true)
            {
                normal = normal + 2.50M;
            }

            // Calculate child tarrif
            if (show.BeginTime.Hour < 18 && show.Movie.Language == "Nederlands")
            {
                child = normal - 1.75M;
            } else
            {
                child = normal;
            }

            // Calculate student tarrif
            if (show.BeginTime.DayOfWeek == DayOfWeek.Monday | show.BeginTime.DayOfWeek == DayOfWeek.Tuesday | show.BeginTime.DayOfWeek == DayOfWeek.Wednesday | show.BeginTime.DayOfWeek == DayOfWeek.Thursday)
            {
                student = normal - 1.50M;
            }
            else
            {
                student = normal;
            }

            // Calculate senior tarrif
            // Holidays NYI
            if (show.BeginTime.DayOfWeek == DayOfWeek.Monday | show.BeginTime.DayOfWeek == DayOfWeek.Tuesday | show.BeginTime.DayOfWeek == DayOfWeek.Wednesday | show.BeginTime.DayOfWeek == DayOfWeek.Thursday)
            {
                senior = normal - 1.50M;
            }
            else
            {
                senior = normal;
            }

            List<decimal> tariffs = new List<decimal>();
            tariffs.Add(normal);
            tariffs.Add(child);
            tariffs.Add(student);
            tariffs.Add(senior);
            TempData["Secret"] = secret;
            return tariffs;
        }

    }
}