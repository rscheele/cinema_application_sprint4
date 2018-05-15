using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Room
    {

        public Room()
        {
        }

        [Key]
        public int RoomID { get; set; }
        public int RoomNumber { get; set; }
        public int TotalSeats { get; set; }
        public int RowCount { get; set; }
    }
}
