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
    public class SpecialsController : Controller
    {
        private IShowRepository showRepository;
        private ITicketRepository ticketRepository;
        private ITempTicketRepository tempTicketRepository;

        public SpecialsController(IShowRepository showRepository, ITicketRepository ticketRepository, ITempTicketRepository tempTicketRepository)
        {
            this.showRepository = showRepository;
            this.ticketRepository = ticketRepository;
            this.tempTicketRepository = tempTicketRepository;
        }

        // GET: Specials
        public ActionResult Specials()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Ladies()
        {
            Show show = showRepository.GetShows()
                .Where(x => x.BeginTime >= DateTime.Now && x.ShowType == 1)
                .OrderBy(x => x.BeginTime)
                .FirstOrDefault();
            return View("Ladies", show);
        }

        [HttpGet]
        public ActionResult LadiesOrder(int id)
        {
            List<Show> allShows = showRepository.GetShows().ToList();
            Show orderedShow = allShows.Find(r => r.ShowID == id);
            // Generating reservation ID with datetime and using this as our transaction session ID
            DateTime dateTime = DateTime.Now;
            DateTime minusDateTime = dateTime.Add(new TimeSpan(0, -25, 0));
            string soldOut = (string)TempData["SoldOut"];
            if (soldOut != null)
            {
                ViewBag.SoldOut = soldOut;
            }

            if (minusDateTime > orderedShow.BeginTime)
            {
                return RedirectToAction("NotAvailable");
            }
            else
            {
                int year = dateTime.Year;
                int doy = dateTime.DayOfYear;
                int hour = dateTime.Hour;
                int minute = dateTime.Minute;
                int ms = dateTime.Millisecond;
                long reservationID = long.Parse(year.ToString() + doy.ToString().PadLeft(3, '0') + hour.ToString().PadLeft(2, '0') + minute.ToString().PadLeft(2, '0') + ms.ToString().PadLeft(3, '0'));
                Order order = new Order
                {
                    reservationID = reservationID,
                    showID = id
                };
                return View("LadiesOrder", order);
            }
        }

        [HttpPost]
        public ActionResult LadiesOrder(Order order)
        {
            // Add a maximum of tickets that can be ordered based on database
            Show selectedShow = showRepository.FindShow(order.showID);
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

            // Check if there are tickets available
            int ticketcount = order.studentTickets + order.seniorTickets + order.normalTickets + order.childTickets;
            if (max == 0)
            {
                TempData["SoldOut"] = "Ladies night is helaas uitverkocht. Er zijn geen tickets meer beschikbaar.";
                return RedirectToAction("LadiesOrder", new { order.reservationID, selectedShow.ShowID });
            }
            else if (ticketcount > max)
            {
                TempData["SoldOut"] = "Ladies night is bijna uitverkocht, er zijn nog maar " + max + " tickets beschikbaar! Wees snel!";
                return RedirectToAction("LadiesOrder", new { order.reservationID, selectedShow.ShowID });
            }
            else
            {
                // Add ladies tickets
                for (int i = 0; i < order.ladiesTickets; i++)
                {
                    TempTicket tempTicket = new TempTicket();
                    tempTicket.Price = 15.45M;
                    tempTicket.TicketType = "Ladies Night";
                    tempTicket.ReservationID = order.reservationID;
                    tempTicket.ShowID = selectedShow.ShowID;
                    tempTicket.TimeAdded = DateTime.Now;
                    tempTicket.Show = selectedShow;
                    tempTicket.Popcorn = true;
                    if (selectedShow.Movie.Is3D == true)
                    {
                        tempTicket.Glasses = true;
                    }
                    else
                    {
                        tempTicket.Glasses = false;
                    }
                    tempTicket.IsPaid = false;
                    tempTicket.ShowID = selectedShow.ShowID;
                    tempTickets.Add(tempTicket);
                    numberoftickets++;
                }
                selectedShow.NumberofTickets = selectedShow.NumberofTickets + numberoftickets;
                tempTicketRepository.SaveTempTickets(tempTickets);
                return RedirectToAction("SelectSeats", "SeatSelection", new { reservationID = tempTickets.FirstOrDefault().ReservationID });
            }
        }
    }
}