using Domain.Abstract;
using Domain.Entities;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class SalesTicketController : Controller
    {
        private IMovieOverviewRepository movieRepository;
        private IShowRepository showRepository;
        private IShowSeatRepository showSeatRepository;
        private ITempTicketRepository tempTicketRepository;
        private ITicketRepository ticketRepository;
        private IRoomRepository roomRepository;
        private static Random random = new Random();

        public SalesTicketController(IMovieOverviewRepository movieRepository, IShowRepository showRepository, IShowSeatRepository showSeatRepository, ITempTicketRepository tempTicketRepository, ITicketRepository ticketRepository, IRoomRepository roomRepository)
        {
            this.movieRepository = movieRepository;
            this.showRepository = showRepository;
            this.showSeatRepository = showSeatRepository;
            this.tempTicketRepository = tempTicketRepository;
            this.ticketRepository = ticketRepository;
            this.roomRepository = roomRepository;
        }

        // METHOD FOR SEARCH FILTERS
        [HttpPost]
        public ActionResult Dofilter(string searchString, int? age, DateTime? start)
        {
            if (start.HasValue == true)
            {
                DateTime selectedDate = (DateTime)start;
                List<Show> filteredShows = showRepository.GetShows().ToList();

                if (!String.IsNullOrEmpty(searchString) && age.HasValue == true)//1
                {
                    List<Show> list = filteredShows.Where(s => s.Movie.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.MainActors.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.Genre.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.MainActors.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.SubActors.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.Director.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                           && s.Movie.Age == age
                           && s.BeginTime.DayOfYear == selectedDate.DayOfYear).ToList();
                    return View("Overview", list);
                }
                else if (!String.IsNullOrEmpty(searchString) && age.HasValue == false)//2
                {
                    List<Show> list = filteredShows.Where(s => s.Movie.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.MainActors.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.Genre.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.MainActors.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.SubActors.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.Director.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            && s.BeginTime.DayOfYear == selectedDate.DayOfYear).ToList();
                    return View("Overview", list);
                }
                else if (String.IsNullOrEmpty(searchString) && age.HasValue == true)//7
                {
                    List<Show> list = filteredShows.Where(s => s.Movie.Age == age
                            && s.BeginTime.DayOfYear == selectedDate.DayOfYear).ToList();
                    return View("Overview", list);
                }
                else if (String.IsNullOrEmpty(searchString) && age.HasValue == false)//3
                {
                    List<Show> list = filteredShows.Where(s => s.BeginTime.DayOfYear == selectedDate.DayOfYear).ToList();
                    return View("Overview", list);
                }
            }

            if (!String.IsNullOrEmpty(searchString) | age.HasValue == true)
            {
                List<Show> filteredShows = showRepository.GetShows().ToList();

                if (!String.IsNullOrEmpty(searchString) && age.HasValue == true)
                { //6
                    List<Show> list = filteredShows.Where(s => s.Movie.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.MainActors.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.Genre.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.SubActors.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            | s.Movie.Director.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                            && s.Movie.Age == age).ToList();
                    return View("Overview", list);
                }
                else if (!String.IsNullOrEmpty(searchString) && age.HasValue == false)//4
                {
                    List<Show> list = filteredShows.Where(s => s.Movie.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                           | s.Movie.MainActors.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                           | s.Movie.Genre.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                           | s.Movie.SubActors.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                           | s.Movie.Director.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    return View("Overview", list);
                }
                else if (age.HasValue == true && String.IsNullOrEmpty(searchString))//5
                {
                    List<Show> list = filteredShows.Where(s => s.Movie.Age == age).ToList();
                    return View("Overview", list);
                }
            }
            return RedirectToAction("Overview");
        }

        // METHOD FOR DISPLAYING OVERVIEW
        //[HttpGet]
        public ActionResult Overview()
        {

            //int Locationid = 1;//locationid
            DateTime now = DateTime.Now;
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysUntilWednesday = ((int)DayOfWeek.Wednesday - (int)now.DayOfWeek + 7) % 7;
            DateTime nextWednesday = now.AddDays(daysUntilWednesday).Date;
            DateTime NextWednesday = nextWednesday.Add(new TimeSpan(23, 59, 59));
            List<Show> allShows = showRepository.GetShows().ToList();
            //Filter out shows from different location and shows that are not of type 0
            //List<Show> allThislocationShows = allShows.ToEnumerable().Where(s => s.Movie.LocationID == Locationid && s.ShowType == 0).ToList();
            DateTime currentDateTime = DateTime.Now;
            DateTime minusDateTime = currentDateTime.Add(new TimeSpan(0, -25, 0));
            List<Show> tempShowList = new List<Show>();

            //Filter out shows from the past or start within 25 minutes and shows that are not of type 0
            List<Show> ShowsFromNow = allShows.ToEnumerable()//was allThislocationShows
                .Where(s => s.BeginTime > minusDateTime && s.ShowType == 0).ToList();
            //Order by show date
            List<Show> ShowsFromNowOrderedByDate = ShowsFromNow.ToEnumerable()
                .OrderBy(s => s.BeginTime).ToList();
            //take shows form current movie week
            List<Show> upcomingShows = ShowsFromNowOrderedByDate.ToEnumerable()
                .Where(s => s.EndTime < NextWednesday).ToList();
            ViewBag.locid = 1;
            //return View(upcomingShows);
            return View(upcomingShows);
        }

        // METHOD FOR ORDERING MOVIE
        [HttpGet]
        public ActionResult OrderMovie(int id)
        {
            List<Show> allShows = showRepository.GetShows().ToList();
            Show orderedShow = allShows.Find(r => r.ShowID == id);
            TempData["Secret"] = false;
            // Generating reservation ID with datetime and using this as our transaction session ID
            DateTime dateTime = DateTime.Now;
            //DateTime minusDateTime = dateTime.Add(new TimeSpan(0, -25, 0));
            // IT SHOULD BE POSSIBLE TO ORDER MOVIES WHEN THEY ARE ALREADY STARTING
            /*if (minusDateTime > orderedShow.BeginTime)
            {
                return RedirectToAction("NotAvailable");
            }
            else
            {*/
            int year = dateTime.Year;
            int doy = dateTime.DayOfYear;
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;
            int ms = dateTime.Millisecond;
            long reservationID = long.Parse(year.ToString() + doy.ToString().PadLeft(3, '0') + hour.ToString().PadLeft(2, '0') + minute.ToString().PadLeft(2, '0') + ms.ToString().PadLeft(3, '0'));
            return RedirectToAction("OrderTickets", "SalesTicket", new { reservationID, showID = id });
            //}
        }

        // GETTER FOR ORDERING TICKETS
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

        // POSTING THE TICKETS TO ORDER
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
                return RedirectToAction("OrderTickets", new { order.ReservationID, selectedShow.ShowID });
            }
            else if (ticketcount <= 0 | ticketcount > 10)
            {
                return RedirectToAction("OrderTickets", new { order.ReservationID, selectedShow.ShowID });
            }
            else if (ticketcount > max)
            {
                TempData["SoldOut"] = "De show is bijna uitverkocht, er zijn nog maar " + max + " tickets beschikbaar!";
                return RedirectToAction("OrderTickets", new { order.ReservationID, selectedShow.ShowID });
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

        // ADDING POPCORN AND 3D GLASSES
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

        // POSTING POPCORN AND 3D GLASSES
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
            return RedirectToAction("SelectSeats", "SalesTicket", new { reservationID = tickets.FirstOrDefault().ReservationID });
        }

        // FUNCTION TO CALCULATE PRICES
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
            }
            else
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
            }
            else
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

        // SEATSELECTION IS DONE AUTOMATICALLY, NO VIEW IS DISPLAYED
        [HttpGet]
        public ActionResult SelectSeats(long reservationID)
        {
            List<TempTicket> ticketList = tempTicketRepository.GetTempTicketsReservation(reservationID).ToList();
            Show show = showRepository.FindShow(ticketList.FirstOrDefault().ShowID);
            Room room = roomRepository.GetRoom(show.RoomID);
            IEnumerable<ShowSeat> showSeats = showSeatRepository.GetShowSeats(show.ShowID);
            int totalTickets = ticketList.Count();
            // Quick and dirty math programming
            double halfwayRaw = room.RowCount / 2;
            int halfway = int.Parse(Math.Ceiling(halfwayRaw).ToString());

            if (ticketList.FirstOrDefault().RowNumber == 0)
            {
                int row = halfway - 1;
                bool fillUp = true;
                for (int i = 1; i <= room.RowCount; i++)
                {
                    if (row < room.RowCount && fillUp == true)
                    {
                        row++;
                    }
                    else
                    {
                        if (row == room.RowCount)
                        {
                            row = halfway;
                            fillUp = false;
                        }
                        row--;
                    }


                    List<ShowSeat> currentRow = new List<ShowSeat>();
                    int count = 0;
                    // Trying to find somehwere where you can all sit next to eachother
                    foreach (var j in showSeats)
                    {
                        if (j.RowNumber == row && j.IsTaken == false && j.IsReserved == false)
                        {
                            currentRow.Add(j);
                            count++;
                        }
                    }
                    if (count >= totalTickets)
                    {
                        List<TempTicket> tempTickets = tempTicketRepository.GetTempTicketsReservation(ticketList.FirstOrDefault().ReservationID).ToList();
                        for (int k = 0; k < totalTickets; k++)
                        {
                            ticketList[k].RowNumber = currentRow[k].RowNumber;
                            ticketList[k].SeatNumber = currentRow[k].SeatNumber;
                            ticketList[k].SeatID = currentRow[k].SeatID;
                            tempTickets[k].RowNumber = currentRow[k].RowNumber;
                            tempTickets[k].SeatNumber = currentRow[k].SeatNumber;
                            tempTickets[k].SeatID = currentRow[k].SeatID;
                            showSeats.Where(x => x.SeatID == currentRow[k].SeatID).FirstOrDefault().IsReserved = true;
                            showSeats.Where(x => x.SeatID == currentRow[k].SeatID).FirstOrDefault().ReservationID = reservationID;
                        }
                        showSeatRepository.UpdateShowSeats(showSeats.ToList());
                        tempTicketRepository.UpdateTempTickets(tempTickets);
                        break;
                    }
                    else if (row == 1)
                    // If there aren't enough seats left where you can sit next to eachother
                    {
                        List<TempTicket> tempTickets = tempTicketRepository.GetTempTicketsReservation(ticketList.FirstOrDefault().ReservationID).ToList();
                        currentRow.Clear();
                        count = 0;
                        foreach (var j in showSeats)
                        {
                            if (j.IsTaken == false && j.IsReserved == false)
                            {
                                currentRow.Add(j);
                                count++;
                                if (count == totalTickets)
                                {
                                    break;
                                }
                            }
                        }

                        for (int k = 0; k < totalTickets; k++)
                        {
                            ticketList[k].RowNumber = currentRow[k].RowNumber;
                            ticketList[k].SeatNumber = currentRow[k].SeatNumber;
                            ticketList[k].SeatID = currentRow[k].SeatID;
                            tempTickets[k].RowNumber = currentRow[k].RowNumber;
                            tempTickets[k].SeatNumber = currentRow[k].SeatNumber;
                            tempTickets[k].SeatID = currentRow[k].SeatID;
                            showSeats.Where(x => x.SeatID == currentRow[k].SeatID).FirstOrDefault().IsReserved = true;
                        }
                        showSeatRepository.UpdateShowSeats(showSeats.ToList());
                        tempTicketRepository.UpdateTempTickets(tempTickets);
                    }
                    currentRow.Clear();
                }
            }

            SeatLayout seatLayout = new SeatLayout();
            seatLayout.showSeats = showSeats;
            seatLayout.rowCount = room.RowCount;
            seatLayout.tickets = ticketList;

            IEnumerable<TempTicket> tempTickets2 = tempTicketRepository.GetTempTicketsReservation(reservationID);
            decimal totalPrice = 0;
            // Add the show to the ticket
            Show orderedShow = showRepository.FindShow(tempTickets2.FirstOrDefault().ShowID);
            foreach (var item in tempTickets2)
            {
                item.Show = orderedShow;
                totalPrice = totalPrice + item.Price;
            }
            SalesTempTicketModel salesTempTicketModel = new SalesTempTicketModel();
            salesTempTicketModel.TempTickets = tempTickets2.ToList();
            salesTempTicketModel.TotalPrice = totalPrice;
            salesTempTicketModel.CurrentPrice = totalPrice;

            TempData["salesTempTicketModel"] = salesTempTicketModel;
            return View("DisplayTempReservation", salesTempTicketModel);
        }

        // ADD DEDUCTION IN DEDUCTION OVERVIEW
        [HttpGet]
        public ViewResult Deduction(string tarrifType,  int tarrifReduction)
        {
            SalesTempTicketModel salesTempTicketModel = (SalesTempTicketModel)TempData["salesTempTicketModel"];
            List<Deduction> deductions = salesTempTicketModel.Deductions;
            Deduction deduction = new Deduction { Name = tarrifType, Amount = tarrifReduction };
            if (deductions == null)
            {
                List<Deduction> newDeductions = new List<Deduction>();
                deduction.Id = 0;
                newDeductions.Add(deduction);
                salesTempTicketModel.Deductions = newDeductions;
            }
            else
            {
                deduction.Id = deductions.ToArray().Length;
                deductions.Add(deduction);
                salesTempTicketModel.Deductions = deductions;
            }
            decimal currentPrice = salesTempTicketModel.CurrentPrice;
            currentPrice = currentPrice - (Decimal)tarrifReduction;
            if (currentPrice < 0)
            {
                currentPrice = 0;
            }
            salesTempTicketModel.CurrentPrice = currentPrice;
            TempData["salesTempTicketModel"] = salesTempTicketModel;
            return View("DisplayTempReservation", salesTempTicketModel);
        }

        // REMOVE DEDUCTION FROM DEDUCTIONS
        [HttpGet]
        public ViewResult RemoveDeduction(int id)
        {
            SalesTempTicketModel salesTempTicketModel = (SalesTempTicketModel)TempData["salesTempTicketModel"];
            List<Deduction> deductions = salesTempTicketModel.Deductions;
            Deduction deduction = deductions.Single(x => x.Id == id);
            deductions.Remove(deduction);
            salesTempTicketModel.Deductions = deductions;
            decimal currentPrice = salesTempTicketModel.TotalPrice;
            foreach (var item in salesTempTicketModel.Deductions)
            {
                currentPrice = currentPrice - item.Amount;
            }
            if (currentPrice < 0)
            {
                currentPrice = 0;
            }
            salesTempTicketModel.CurrentPrice = currentPrice;
            TempData["salesTempTicketModel"] = salesTempTicketModel;
            return View("DisplayTempReservation", salesTempTicketModel);
        }

        // PAY TICKETS AND SAVE THEM IN THE DATABASE
        [HttpGet]
        public ActionResult Pay(long reservationId)
        {
            List<TempTicket> tempTickets = tempTicketRepository.GetTempTicketsReservation(reservationId).ToList();
            List<Ticket> tickets = new List<Ticket>();
            if (tempTickets.Count != 0)
            {
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
            }
            else
            {
                tickets = ticketRepository.GetTickets(reservationId).ToList();
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
            reservation.reservationID = reservationId;
            return View("Success", reservation);
        }

        // SHOW SUCCESFUL PAYMENT VIEW
        public ViewResult Success(Reservation reservation)
        {
            return View(reservation);
        }

        // PRINT THE TICKETS
        [HttpGet]
        public ActionResult PrintSessionTickets(long reservationID)
        {
            List<Ticket> tickets = ticketRepository.GetTickets(reservationID).ToList();
            Show show = showRepository.FindShow(tickets[0].ShowID);
            var pdf = new PrintTickets(tickets, show);
            return pdf.SendPdf();
        }

        // FUNCTION TO GENERATE RANDOM STRING
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}