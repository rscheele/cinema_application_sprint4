using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EnqueteResponse
    {
        public EnqueteResponse()
        {

        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ResponseId { get; set; }
        public string TicketCode { get; set; }
        public int UserScore { get; set; }
        public string Comment { get; set; }
    }
}
