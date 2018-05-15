using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface ITicketRepository
    {
        IEnumerable<Ticket> GetTickets(long ReservationID);
        IEnumerable<Ticket> GetShowTickets(int ShowID);
        IEnumerable<Ticket> GetMovieSecretTickets();
        void UpdateTickets(List<Ticket> Tickets);
        void SaveTickets(List<Ticket> Tickets);
    }
}
