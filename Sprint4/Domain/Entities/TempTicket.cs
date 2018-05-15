using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TempTicket
    {
        public TempTicket()
        {

        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public long ReservationID { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
        public DateTime TimeAdded { get; set; }
        public int TicketID { get; set; }
        public decimal Price { get; set; }
        public string TicketType { get; set; }
        public bool IsPaid { get; set; }
        public int SeatID { get; set; }
        [NotMapped]
        [ForeignKey("SeatID")]
        public virtual Seat Seat { get; set; }
        public int ShowID { get; set; }
        [NotMapped]
        [ForeignKey("ShowID")]
        public virtual Show Show { get; set; }
        public bool Popcorn { get; set; }
        public bool Glasses { get; set; }
    }
}
