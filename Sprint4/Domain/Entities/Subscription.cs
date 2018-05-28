using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Name { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string HouseNumberExtras { get; set; }
        public string HomeTown { get; set; }
        public string ImageTitle { get; set; }
        public string ImagePath { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
