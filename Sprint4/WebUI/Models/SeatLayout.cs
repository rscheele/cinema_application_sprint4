using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class SeatLayout
    {
        public IEnumerable<ShowSeat> showSeats;

        public int rowCount;

        public List<TempTicket> tickets;
    }
}