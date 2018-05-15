using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IMovieOverviewRepository
    {
        List<Movie> getMovieList();
        List<Show> getShowList();
        List<Show> getShowbyId(int id);
       // IEnumerable<Movie> GetMovies(); //11-3
        IEnumerable<Movie> Movies { get; }
    }
}
