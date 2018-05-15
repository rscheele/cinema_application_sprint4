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
        
        // GET: Overview
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
            DateTime minusDateTime = currentDateTime.Add(new TimeSpan(0, 25, 0));
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

        [ChildActionOnly]
        public ActionResult Expected()
        {
            List<Show> allShows = showRepository.GetShows().ToList();
            DateTime now = DateTime.Now;
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysUntilWednesday = ((int)DayOfWeek.Wednesday - (int)now.DayOfWeek + 7) % 7;
            DateTime nextWednesday = now.AddDays(daysUntilWednesday).Date;
            DateTime NextWednesday = nextWednesday.Add(new TimeSpan(23, 59, 59));
            //Three Expected shows --------BEGIN
            if (allShows.Where(s => s.BeginTime > nextWednesday).Count() >= 3)
            {
                string x = allShows.Where(s => s.BeginTime > NextWednesday).ElementAt(0).Movie.Trailer.ToString();
                byte[] x2 = allShows.Where(s => s.BeginTime > NextWednesday).ElementAt(0).Movie.Image;
                string y = allShows.Where(s => s.BeginTime > NextWednesday).ElementAt(1).Movie.Trailer.ToString();
                byte[] y2 = allShows.Where(s => s.BeginTime > NextWednesday).ElementAt(1).Movie.Image;
                string z = allShows.Where(s => s.BeginTime > NextWednesday).ElementAt(2).Movie.Trailer.ToString();
                byte[] z2 = allShows.Where(s => s.BeginTime > NextWednesday).ElementAt(2).Movie.Image;

                ViewBag.trailer1 = x;
                ViewBag.image1 = x2;
                ViewBag.trailer2 = y;
                ViewBag.image2 = y2;
                ViewBag.trailer3 = z;
                ViewBag.image3 = z2;
            }
            //Three Expected shows --------END
            return PartialView("Caroussel");
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
            DateTime dateTime = DateTime.Now;
            DateTime minusDateTime = dateTime.Add(new TimeSpan(0, 25, 0));
            if (minusDateTime > orderedShow.BeginTime)
            {
                return RedirectToAction("NotAvailable");
            }
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