using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShowSeat
    {
        public ShowSeat()
        {

        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SeatID { get; set; }
        public int ShowID { get; set; }
        [NotMapped]
        [ForeignKey("ShowID")]
        public virtual Show Show { get; set; }
        public int RoomID { get; set; }
        [NotMapped]
        [ForeignKey("RoomID")]
        public virtual Room Room { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
        public bool IsTaken { get; set; }
        public bool IsReserved { get; set; }
        public long ReservationID { get; set; }
    }
}
