using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class CreditcardModel
    {
        [Required(ErrorMessage = "Voer een geldig creditcardnummer van 16 cijfers in")]
        [MaxLength(16)]
        [MinLength(16)]
        public long Creditcard { get; set; }
        public string Crediterror { get; set; }
        [Required(ErrorMessage = "Geef de verloopdatum aan")]
        [DataType(DataType.Date)]
        [Display(Name = "Verloopdatum kaart")]
        public DateTime ExpireDate { get; set; }
        public string Expireerror { get; set; }
        [Required(ErrorMessage = "Voer CVC code in(3 cijfers achterop de creditcard).")]
        [MaxLength(3)]
        [MinLength(3)]
        public int CVC { get; set; }
        public string CVCerror { get; set; }
        public long reservationID;
    }
}