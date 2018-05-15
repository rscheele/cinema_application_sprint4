using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Location
    {

        public Location()
        {

        }

        [Key]
        public int LocationID { get; set; }

        public string Name { get; set; }
        public string City { get; set; }
        public int Rooms { get; set; }
        public decimal TicketPrice { get; set; }
        //public decimal ChildDiscount { get; set; }
        //public decimal StudentDiscount { get; set; }
        //public decimal SeniorDiscount { get; set; }
        //public decimal CJPDiscount { get; set; }
    }
}
