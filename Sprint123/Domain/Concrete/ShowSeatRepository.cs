using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class ShowSeatRepository : IShowSeatRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<ShowSeat> GetShowSeats()
        {
            return context.ShowSeats;
        }

        public IEnumerable<ShowSeat> GetShowSeats(int? ShowID)
        {
            List<ShowSeat> list = context.ShowSeats.Where(x => x.ShowID == ShowID).ToList();
            if (list != null)
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<ShowSeat> GetShowSeatsReservation(long ReservationID)
        {
            List<ShowSeat> list = context.ShowSeats.Where(x => x.ReservationID == ReservationID).ToList();
            if (list != null)
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        public void SaveShowSeats(List<ShowSeat> ShowSeats)
        {
            foreach (var item in ShowSeats)
            {
                context.ShowSeats.Add(item);
            }
            context.SaveChanges();
        }

        public void UpdateShowSeats(List<ShowSeat> ShowSeats)
        {
            foreach (var item in ShowSeats)
            {
                ShowSeat dbEntry = context.ShowSeats.Find(item.SeatID);
                if (dbEntry != null)
                {
                    context.Entry(dbEntry).CurrentValues.SetValues(item);
                }
            }
            context.SaveChanges();
        }

        public void UpdateShowSeats(ShowSeat ShowSeat)
        {
            ShowSeat dbEntry = context.ShowSeats.Find(ShowSeat.SeatID);
            context.Entry(dbEntry).CurrentValues.SetValues(ShowSeat);
            context.SaveChanges();
        }
    }
}
