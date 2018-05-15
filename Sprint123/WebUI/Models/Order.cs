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
        public int normalTickets { get; set; }
        public int childTickets { get; set; }
        public int studentTickets { get; set; }
        public int seniorTickets { get; set; }
        public int ladiesTickets { get; set; }
        public long reservationID { get; set; }
        public int showID { get; set; }
    }
}