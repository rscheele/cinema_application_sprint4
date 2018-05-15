using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class TempTicketRepository : ITempTicketRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<TempTicket> GetTempTickets()
        {
            return context.TempTickets;
        }

        public IEnumerable<TempTicket> GetTempTicketsReservation(long ReservationID)
        {
            List<TempTicket> list = context.TempTickets.Where(x => x.ReservationID == ReservationID).ToList();
            if (list != null)
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<TempTicket> GetTempTicketsShow(int? ShowID)
        {
            List<TempTicket> list = context.TempTickets.Where(x => x.ShowID == ShowID).ToList();
            if (list != null)
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        public void SaveTempTickets(List<TempTicket> TempTickets)
        {
            foreach (var item in TempTickets)
            {
                context.TempTickets.Add(item);
            }
            context.SaveChanges();
        }

        public void UpdateTempTickets(List<TempTicket> TempTickets)
        {
            foreach (var item in TempTickets)
            {
                TempTicket dbEntry = context.TempTickets.Find(item.ID);
                if (dbEntry != null)
                {
                    context.Entry(dbEntry).CurrentValues.SetValues(item);
                }
            }
            context.SaveChanges();
        }

        public void DeleteTempTicket(long ReservationID)
        {
            List<TempTicket> list = context.TempTickets.Where(x => x.ReservationID == ReservationID).ToList();
            foreach (var item in list)
            {
                context.TempTickets.Remove(item);
            }
            context.SaveChanges();
        }

        public void DeleteTempTickets(List<TempTicket> tempTickets)
        {
            context.TempTickets.RemoveRange(tempTickets);
            context.SaveChanges();
        }


    }
}
