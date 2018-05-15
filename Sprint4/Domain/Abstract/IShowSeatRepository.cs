using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IShowSeatRepository
    {
        IEnumerable<ShowSeat> GetShowSeats();
        IEnumerable<ShowSeat> GetShowSeats(int? ShowID);
        IEnumerable<ShowSeat> GetShowSeatsReservation(long ReservationID);
        void SaveShowSeats(List<ShowSeat> ShowSeats);
        void UpdateShowSeats(ShowSeat ShowSeat);
        void UpdateShowSeats(List<ShowSeat> ShowSeats);
    }
}
