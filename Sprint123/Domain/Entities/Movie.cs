using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Movie
    {

        public Movie()
        {

        }
        [Key]
        public int MovieID { get; set; }

        public string Name { get; set; }
        public string Language { get; set; }
        public string LanguageSub { get; set; }
        public int Age { get; set; }
        public int MovieType { get; set; }
        public int Length { get; set; }
        public bool Is3D { get; set; }
        public byte[] Image { get; set; }
        public string Trailer { get; set; }
        public string MainActors { get; set; }
        public string SubActors { get; set; }
        public string IMDB { get; set; }
        public string Website { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }

        public int LocationID { get; set; }

        [ForeignKey("LocationID")]
        public virtual Location Location { get; set; }
    }
}
