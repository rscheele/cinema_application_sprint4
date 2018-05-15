using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class CreditcardModel
    {
        [Required(ErrorMessage = "Voer een geldig creditcardnummer van 8 cijfers in")]
        [MaxLength(8)]
        [MinLength(8)]
        public String Creditcard { get; set; }
        [Required(ErrorMessage = "Voer een maand in")]
        public Month Month { get; set; }
        [Required(ErrorMessage = "Voer een geldig jaar in")]
        [Range(2017, 2117)]
        //[MaxLength(4)]
        public int Year { get; set; }
        [Required(ErrorMessage = "Voer CVC code in(3 cijfers achterop de creditcard).")]
        [MaxLength(3)]
        [MinLength(3)]
        public int CVC { get; set; }
        public long reservationID;
    }

    public enum Month
    {
        januari = 1,
        februari = 2,
        maart = 3,
        april = 4,
        mei = 5,
        juni = 6,
        juli = 7,
        augustus = 8,
        september = 9,
        oktober = 10,
        november = 11,
        december = 12
    }
}