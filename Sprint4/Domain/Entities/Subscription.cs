using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Domain.Entities
{
    public class Subscription
    {
        public Subscription()
        {

        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SubscriptionId { get; set; }
        [Required]
        [DisplayName("Voornaam*")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Je kan alleen letters in je naam gebruiken.")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Straatnaam*")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Je kan alleen letters in je straatnaam gebruiken.")]
        public string Street { get; set; }
        [Required]
        [DisplayName("Huisnummer*")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Je kan alleen cijfers in je huisnummer gebruiken.")]
        public int HouseNumber { get; set; }
        [DisplayName("Huisnummer toevoeging")]
        public string HouseNumberExtras { get; set; }
        [Required]
        [DisplayName("Woonplaats*")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Je kan alleen letters in je woonplaats gebruiken.")]
        public string HomeTown { get; set; }
        [DisplayName("Foto uploaden*")]
        public string ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Geboortedatum*")]
        public DateTime BirthDate { get; set; }
        public DateTime ExpireDate { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email adres*")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{1,4}[-]{0,1}[0-9]{1,10}$", ErrorMessage = "Geen geldig telefoonnummer.")]
        [DisplayName("Telefoonnummer*")]
        public string PhoneNumber { get; set; }
    }
}
