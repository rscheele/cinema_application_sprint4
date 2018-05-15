using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class EmailReservation
    {
        public EmailReservation()
        {

        }
        [Required(ErrorMessage = "Please enter your email address")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Voer een geldig emailadres in")]
        public string EmailAdress { get; set; }
        public long ReservationID { get; set; }
        public bool Paid { get; set; }
        public bool NewsLetter { get; set; }

    }
}