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
                    ReservationID = reservationID,
                    ShowID = id
                };
                return View("LadiesOrder", order);
            }
        }

        [HttpPost]
        public ActionResult LadiesOrder(Order order)
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

            // Check if there are tickets available
            int ticketcount = order.StudentTickets + order.SeniorTickets + order.NormalTickets + order.ChildTickets;
            if (max == 0)
            {
                TempData["SoldOut"] = "Ladies night is helaas uitverkocht. Er zijn geen tickets meer beschikbaar.";
                return RedirectToAction("LadiesOrder", new { order.ReservationID, selectedShow.ShowID });
            }
            else if (ticketcount > max)
            {
                TempData["SoldOut"] = "Ladies night is bijna uitverkocht, er zijn nog maar " + max + " tickets beschikbaar! Wees snel!";
                return RedirectToAction("LadiesOrder", new { order.ReservationID, selectedShow.ShowID });
            }
            else
            {
                // Add ladies tickets
                for (int i = 0; i < order.LadiesTickets; i++)
                {
                    TempTicket tempTicket = new TempTicket();
                    tempTicket.Price = 15.45M;
                    tempTicket.TicketType = "Ladies Night";
                    tempTicket.ReservationID = order.ReservationID;
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

        public ActionResult KidsOverview()
        {
            DateTime now = DateTime.Now;
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysUntilWednesday = ((int)DayOfWeek.Wednesday - (int)now.DayOfWeek + 7) % 7;
            DateTime nextWednesday = now.AddDays(daysUntilWednesday);

            List<Show> allShows = showRepository.GetShows().ToList();
            //Filter out shows from different location and shows that are not of type 0
            List<Show> allThislocationShows = allShows.Where(s => s.Movie.LocationID == 1 && s.ShowType == 0).ToList();

            // Remove shows that start within 25 minutes (Or that are older for that matter)
            DateTime currentDateTime = DateTime.Now;
            DateTime minusDateTime = currentDateTime.Add(new TimeSpan(0, -25, 0));
            List<Show> tempShowList = new List<Show>();

            //Filter out shows from the past
            List<Show> ShowsFromNow = allThislocationShows
                .Where(s => s.BeginTime > now).ToList();
            //Order by show date
            List<Show> ShowsFromNowOrderedByDate = ShowsFromNow
                .OrderBy(s => s.BeginTime).ToList();
            //take shows form current movie week
            List<Show> upcomingShows = ShowsFromNowOrderedByDate
                .Where(s => s.EndTime < nextWednesday && s.Movie.Genre.Contains("kids")).ToList();

            foreach (var i in upcomingShows)
            {
                if (i.BeginTime > minusDateTime)
                {
                    tempShowList.Add(i);
                }
            }
            upcomingShows = tempShowList;
            return View("KidsOverview", upcomingShows);
        }

        [HttpGet]
        public ActionResult KidsDetails(int id)
        {
            List<Show> allShows = showRepository.GetShows().ToList();
            Show orderedShow = allShows.Find(r => r.ShowID == id);
            return View("KidsDetails", orderedShow);
        }

        [HttpGet]
        public ActionResult KidsOrder(int id)
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
                    ReservationID = reservationID,
                    ShowID = id
                };
                return View("KidsOrder", order);
            }
        }

        [HttpPost]
        public ActionResult KidsOrder(Order order)
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

            // Check if there are tickets available
            int ticketcount = order.StudentTickets + order.SeniorTickets + order.NormalTickets + order.ChildTickets;
            if (max == 0)
            {
                TempData["SoldOut"] = "Deze film is helaas uitverkocht. Er zijn geen tickets meer beschikbaar.";
                return RedirectToAction("KidsOrder", new { order.ReservationID, selectedShow.ShowID });
            }
            else if (ticketcount > max)
            {
                TempData["SoldOut"] = "Deze film is bijna uitverkocht, er zijn nog maar " + max + " tickets beschikbaar! Wees snel!";
                return RedirectToAction("KidsOrder", new { order.ReservationID, selectedShow.ShowID });
            }
            else
            {
                // Add ladies tickets
                for (int i = 0; i < order.KidsTickets; i++)
                {
                    TempTicket tempTicket = new TempTicket();
                    tempTicket.Price = 14.45M;
                    tempTicket.TicketType = "Kinderfeestje";
                    tempTicket.ReservationID = order.ReservationID;
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