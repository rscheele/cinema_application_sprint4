using Domain.Abstract;
using Domain.Entities;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class UpcomingShowController : Controller
    {
        private IMovieOverviewRepository movieRepository;
        private IShowRepository showRepository;

        public UpcomingShowController(IMovieOverviewRepository movieRepository, IShowRepository showRepository)
        {
            this.movieRepository = movieRepository;
            this.showRepository = showRepository;
        }

        // GET: Overview
        //[HttpGet]
        public ActionResult Overview(int locationid, string searchString, int? age, DateTime? start)
        {

            int Locationid = locationid;
            DateTime now = DateTime.Now;
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysUntilWednesday = ((int)DayOfWeek.Wednesday - (int)now.DayOfWeek + 7) % 7;
            DateTime nextWednesday = now.AddDays(daysUntilWednesday);

            List<Show> allShows = showRepository.GetShows().ToList();
            //Filter out shows from different location and shows that are not of type 0
            List<Show> allThislocationShows = allShows.ToEnumerable().Where(s => s.Movie.LocationID == Locationid && s.ShowType == 0).ToList();

            // Remove shows that start within 25 minutes (Or that are older for that matter)
            DateTime currentDateTime = DateTime.Now;
            DateTime minusDateTime = currentDateTime.Add(new TimeSpan(0, -25, 0));
            List<Show> tempShowList = new List<Show>();

            //Filter out shows from the past
            List<Show> ShowsFromNow = allThislocationShows.ToEnumerable()
                .Where(s => s.BeginTime > now).ToList();
            //Order by show date
            List<Show> ShowsFromNowOrderedByDate = ShowsFromNow.ToEnumerable()
                .OrderBy(s => s.BeginTime).ToList();
            //take shows form current movie week
            List<Show> upcomingShows = ShowsFromNowOrderedByDate.ToEnumerable()
                .Where(s => s.EndTime < nextWednesday).ToList();

            foreach (var i in upcomingShows)
            {
                if (i.BeginTime > minusDateTime)
                {
                    tempShowList.Add(i);
                }
            }
            upcomingShows = tempShowList;
            ViewBag.locid = 1;

            //--------------filters BEGIN  was allShows!!!!--------------------
            if (!String.IsNullOrEmpty(searchString) | age.HasValue | start.HasValue)
            {
                List<Show> filteredShows = allShows.ToList();
                if (!String.IsNullOrEmpty(searchString)) {
                    filteredShows     
                    .Where(s => s.Movie.Name.Contains(searchString)
                            || s.Movie.MainActors.Contains(searchString)
                            || s.Movie.Genre.Contains(searchString)
                            || s.Movie.MainActors.Contains(searchString)
                            || s.Movie.SubActors.Contains(searchString)
                            || s.Movie.Director.Contains(searchString))
                        .ToList();
                }
                if(age != null && age>0 )
                {
                    filteredShows.Where(s => s.Movie.Age == age).ToList();
                }
                if(start.HasValue == true)
                {
                    DateTime selectedDate = (DateTime)start;
                    filteredShows.Where(s => s.BeginTime.DayOfYear == selectedDate.DayOfYear).ToList();
                }
                return View(filteredShows);

            }
            //Three Expected shows --------BEGIN
            /*string x = allShows.Where(s => s.BeginTime > nextWednesday).ElementAt(1).Movie.Trailer.ToString();
            string y = allShows.Where(s => s.BeginTime > nextWednesday).ElementAt(2).Movie.Trailer.ToString();
            string z = allShows.Where(s => s.BeginTime > nextWednesday).ElementAt(3).Movie.Trailer.ToString();
            ViewBag.trailer1 = x;
            ViewBag.trailer2 = y;
            ViewBag.trailer3 = z;*/
            //Three Expected shows --------END
            //return View(upcomingShows);
            return View(allShows);
        }

        // GET: UpcomingShow
        [HttpGet]
        public ActionResult Upcoming(int locationid)
        {
            
            int Locationid = locationid; 
            DateTime now = DateTime.Now;
            DateTime EndOfDay = DateTime.Today.AddDays(1) +new TimeSpan(02,00,00);
        
            List<Show> allShows = showRepository.GetShows().ToList();

            List<Show> allThislocationShows = allShows.ToEnumerable().Where(s => s.Movie.LocationID == Locationid).ToList();
            
            //Filter out shows from the past
            List<Show> ShowsFromNow = allShows.ToEnumerable()
                .Where(s => s.BeginTime > now).ToList();

            //Order by show date
            List<Show> ShowsFromNowOrderedByDate = ShowsFromNow.ToEnumerable()
                .OrderBy(s => s.BeginTime).ToList();

            //take shows form current workday
            List<Show> upcomingShows = ShowsFromNowOrderedByDate.ToEnumerable()
                .Where(s => s.EndTime < EndOfDay).ToList();

            //--secret movie ---
            IEnumerable<Show> showx = allShows;
            //IEnumerable<Show> list = showRepository.GetShows();
            IEnumerable<Show> secretShow = showx.OrderBy(s => s.NumberofTickets).Take(1);
            Show show = secretShow.First();
            String showid = show.ShowID.ToString();
            string begintime = show.BeginTime.ToString();
            string endtime = show.EndTime.ToString();
            string language = show.Movie.Language.ToString();
            string sublanguage = show.Movie.LanguageSub.ToString();
            string length = show.Movie.Length.ToString();
            string room = show.RoomID.ToString();
            int age = show.Movie.Age;

            ViewBag.showid = showid;
            ViewBag.begintime = begintime;
            ViewBag.endtime = endtime;
            ViewBag.threed = show.Movie.Is3D;
            ViewBag.language = language;
            ViewBag.sublanguage = sublanguage;
            ViewBag.length = length;
            ViewBag.age = age;
            ViewBag.room = room;
            //--secret movie ---         
            DateTime today = DateTime.Now;
            var culture = new System.Globalization.CultureInfo("nl-NL");
            var day = culture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);

            string ParsedDayOfWeek = day;

            DateTime time = DateTime.Now;
            string HourOfDay = time.Hour.ToString();
            string MinuteOfDay = time.Minute.ToString();
            string Location = show.Movie.Location.Name;//location name
            
            ViewBag.DayOfWeek = ParsedDayOfWeek;
            ViewBag.HourOfDay = HourOfDay;
            ViewBag.MinuteOfDay = MinuteOfDay;
            ViewBag.Location = Location;
            //will become upcomingShows
            return View(allShows);
            //return View(upcomingShows);
        }

        [HttpGet]
        public ActionResult ShowDetails(int id)
        {
            List<Show> allShows = showRepository.GetShows().ToList();
            Show orderedShow = allShows.Find(r => r.ShowID == id);
            return View("ShowDetails", orderedShow);

        }

        [HttpGet]
        public ActionResult NotAvailable()
        {
            return View("NotAvailable");
        }

        [HttpGet]
        public ActionResult OrderMovie(int id, bool secret)
        {
            List<Show> allShows = showRepository.GetShows().ToList();
            Show orderedShow = allShows.Find(r => r.ShowID == id);
            TempData["Secret"] = secret;
            // Generating reservation ID with datetime and using this as our transaction session ID
            DateTime dateTime = DateTime.Now;
            DateTime minusDateTime = dateTime.Add(new TimeSpan(0, -25, 0));
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
                return RedirectToAction("OrderTickets", "Ticket", new { reservationID, showID = id });
            }
        }
    }
}