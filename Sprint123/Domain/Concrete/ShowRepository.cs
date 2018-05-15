using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class ShowRepository : IShowRepository
    {
        private EFDbContext context = new EFDbContext();

        public Show FindShow(int showID)
        {
            Show show = context.Shows.Where(x => x.ShowID == showID).FirstOrDefault();
            if (showID != null)
            {
                return show;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Show> GetShows()
        {
            return context.Shows;
        }

        public void SaveShow(Show show)
        {
            if (show.ShowID == 0)
            {
                context.Shows.Add(show);
            }
            else
            {
                Show dbEntry = context.Shows.Find(show.ShowID);
                if (dbEntry != null)
                {
                    dbEntry.BeginTime = show.BeginTime;
                    dbEntry.EndTime = show.EndTime;
                    dbEntry.MovieID = show.MovieID;
                    dbEntry.RoomID = show.RoomID;
                    dbEntry.NumberofTickets = show.NumberofTickets;
                }
            }
            context.SaveChanges();
        }
    }
}
