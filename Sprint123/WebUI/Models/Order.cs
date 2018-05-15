using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        public int NormalTickets { get; set; }
        public int ChildTickets { get; set; }
        public int StudentTickets { get; set; }
        public int SeniorTickets { get; set; }
        public int LadiesTickets { get; set; }
        public int KidsTickets { get; set; }
        public long ReservationID { get; set; }
        public int ShowID { get; set; }
    }
}