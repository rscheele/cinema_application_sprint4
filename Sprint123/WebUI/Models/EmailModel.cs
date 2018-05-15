using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class EmailModel
    {
        [Required(ErrorMessage = "Voer een emailadres in")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Voer een geldig emailadres in")]
        public string EmailAdress { get; set; }
    }
}