using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class MovieOverviewRepository : IMovieOverviewRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Movie> Movies{
            get { return context.Movies; }
        }

        public virtual List<Movie> getMovieList()
        {
            List<Movie> list = context.Movies.ToList();
            List<Show> showsList = context.Shows.ToList();
            List<Movie> returnList = new List<Movie>();

            foreach (Movie f in list)
            {
                Show s = showsList.Where(sh => sh.MovieID == f.MovieID && sh.BeginTime >= DateTime.Now).
                    FirstOrDefault();

                if (s != null)
                {
                    returnList.Add(f);
                }
            }

            return returnList;
        }

        public virtual List<Show> getShowList()
        {
            return context.Shows.ToList();
        }

        public List<Show> getShowbyId(int id)
        {
            int i = 0;
            List<Show> showList = getShowList();
            List<Show> showListById = new List<Show>();
            while (i < showList.Count)
            {
                Show currentShow = showList.ElementAt(i);
                if (currentShow.MovieID == id)
                {
                    DateTime currentMovie = currentShow.BeginTime;
                    string hour = currentMovie.Hour.ToString();

                    showListById.Add(currentShow);
                }
                i++;
            }

            if (showListById != null)
            {
                return showListById;
            }
            else
            {
                throw new Exception("Er is een fout.");
            }
        }

        public static DateTime getNextWeekday(DateTime start, DayOfWeek day)
        {
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            DateTime newDate = start.AddDays(daysToAdd);

            return newDate;
        }


        /* 11-3
        public IEnumerable<Movie> GetMovies()
        {
            return context.Movies;
        }*/
    }
}
