using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Seat
    {
        public Seat()
        {

        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SeatID { get; set; }
        public int RoomID { get; set; }
        [NotMapped]
        [ForeignKey("RoomID")]
        public virtual Room Room { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
    }
}
