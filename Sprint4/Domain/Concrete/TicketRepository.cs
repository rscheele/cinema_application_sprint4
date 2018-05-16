using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class TicketRepository : ITicketRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Ticket> GetMovieSecretTickets()
        {
            return context.Tickets;
        }

        public IEnumerable<Ticket> GetTickets(long ReservationID)
        {
            List<Ticket> list = context.Tickets.Where(x => x.ReservationID == ReservationID).ToList();
            if (list != null)
            {
                return list;
            } else
            {
                return null;
            }
            
        }

        public IEnumerable<Ticket> GetShowTickets(int ShowID)
        {
            List<Ticket> list = context.Tickets.Where(x => x.ShowID == ShowID).ToList();
            if (list != null)
            {
                return list;
            }
            else
            {
                return null;
            }

        }

        public void SaveTickets(List<Ticket> Tickets)
        {
            foreach (var item in Tickets)
            {
                context.Tickets.Add(item);
            }
            context.SaveChanges();
        }

        public void UpdateTickets(List<Ticket> Tickets)
        {
            foreach (var item in Tickets)
            {
                Ticket dbEntry = context.Tickets.Find(item.TicketID);
                if (dbEntry != null)
                {
                    context.Entry(dbEntry).CurrentValues.SetValues(item);
                }
            }
            context.SaveChanges();
        }

        public Ticket GetTicketByCode(string ticketCode)
        {
            Ticket ticket = context.Tickets.Where(x => x.TicketCode == ticketCode).FirstOrDefault();
            if (ticket != null)
            {
                return ticket;
            }
            else
            {
                return null;
            }
        }
    }
        
}
