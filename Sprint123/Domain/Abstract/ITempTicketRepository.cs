using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface ITempTicketRepository
    {
        IEnumerable<TempTicket> GetTempTickets();
        IEnumerable<TempTicket> GetTempTicketsShow(int? ShowID);
        IEnumerable<TempTicket> GetTempTicketsReservation(long ReservationID);
        void SaveTempTickets(List<TempTicket> TempTickets);
        void UpdateTempTickets(List<TempTicket> TempTickets);
        void DeleteTempTicket(long ReservationID);
        void DeleteTempTickets(List<TempTicket> TempTickets);
    }
}
