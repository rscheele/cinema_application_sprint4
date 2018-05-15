using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Show
    {

        public Show()
        {

        }

        [Key]
        public int ShowID { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MovieID { get; set; }
        [ForeignKey("MovieID")]
        public virtual Movie Movie { get; set; }
        public int RoomID { get; set; }
        [ForeignKey("RoomID")]
        public virtual Room Room { get; set; }
        public int NumberofTickets { get; set; }
        // Type 0 = normal show, type 1 = ladies night
        public int ShowType { get; set; }
    }
}
