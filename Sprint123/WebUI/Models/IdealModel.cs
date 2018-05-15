using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class IdealModel
    {
        [Required(ErrorMessage = "Selecteer uw bank")]
        [Display(Name = "Selecteer uw bank")]
        public Bank Bank { get; set; }
        public string Bankerror { get; set; }
        public long reservationID;
    }

    public enum Bank
    {
        Rabobank = 1,
        ING = 2,
        SNSBank = 3,
        Knab = 4
    }
}