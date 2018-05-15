using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class Reservation
    {
        public long reservationID { get; set; }
        public bool Paid { get; set; }
    }
}