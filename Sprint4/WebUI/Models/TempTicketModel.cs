using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class TempTicketModel
    {
        public long ReservationID { get; set; }
        public decimal Price { get; set; }
        public string TicketType { get; set; }
        public bool Popcorn { get; set; }
        public bool Glasses { get; set; }
        public bool Is3D { get; set; }
        public bool Vip { get; set; }
    }
}