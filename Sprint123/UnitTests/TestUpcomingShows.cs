using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject.Infrastructure.Language;
using WebUI.Controllers;

namespace UnitTests
{
    [TestClass]
    public class TestUpcomingShows
    {
        private IMovieOverviewRepository movieRepository = new MovieOverviewRepository();
       
        DateTime now = DateTime.Now;
        DateTime EndOfDay = DateTime.Today.AddDays(1) + new TimeSpan(02, 00, 00);
        DateTime currentDateTime = DateTime.Now;

        public IMovieOverviewRepository MovieRepository { get => movieRepository; set => movieRepository = value; }

        //string search = "Bruce";
        //string genre = "actie";
        //string genre2 = "kids";
        //int age = 4;

        private bool ShowIDIsEqual(List<int> l1, List<int> l2)
        {
            if (l1.Count != l2.Count)
            {
                return false;
            }

            for (int i = 0; i < l1.Count; i++)
            {
                if (l1[i] != l2[i])
                {
                    return false;
                }
            }

            return true;
        }

        [TestMethod]
        public void TestForUpcomingShows() {
            //IMPORTANT: test only works before 19:15 hours & tested cinema is Breda. !!!
            //arrange
            Mock<IShowRepository> mock = new Mock<IShowRepository>();
            mock.Setup(m => m.GetShows()).Returns(new Show[]{
            new Show{ShowID=1,BeginTime=DateTime.Parse("2018-04-3 14:40:00.000"),EndTime=DateTime.Parse("2018-04-3 16:10:00.000"), /*Movie={MovieID=4, Name = "Diep in de Zee", Language = "Nederlands", LanguageSub = "Nederlands", Age = 6, MovieType = 2, Length = 91, Is3D = false, MainActors = "Justin Felbinger", SubActors = " ", Director = "Julio Soto Gurpide", LocationID = 1, Genre="kids"}*/MovieID=4,RoomID=2,NumberofTickets=11,ChildDiscount=true,StudentDiscount=true,SeniorDiscount=true}, //3
            new Show{ShowID=2,BeginTime=DateTime.Parse("2018-04-3 14:25:00.000"),EndTime=DateTime.Parse("2018-04-3 16:25:00.000"), /*Movie={MovieID=1, Name = "Darkest Hour", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 120, Is3D = false, MainActors = "Gary Oldman, Lily James", SubActors = "Kristin Scott Thomas", Director = "Joe Wright", LocationID = 1,Genre = "historisch"}, */ MovieID=1,RoomID=1,NumberofTickets=10,ChildDiscount=false,StudentDiscount=true,SeniorDiscount=true}, //1
            new Show{ShowID=3,BeginTime=DateTime.Parse("2018-04-3 14:35:00.000"),EndTime=DateTime.Parse("2018-04-3 16:45:00.000"), /*Movie={MovieID=2, Name = "Red Sparrow", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 140, Is3D = false, MainActors = "Jennifer Lawerence, Joel Edgerton", SubActors = "Matthias Schoenaerts", Director = "Francis Lawrence", LocationID = 1, Genre = "actie"  },*/ MovieID=2,RoomID=2,NumberofTickets=3,ChildDiscount=false,StudentDiscount=true,SeniorDiscount=true}, //2
            new Show{ShowID=4,BeginTime=DateTime.Parse("2018-03-29 14:45:00.000"),EndTime=DateTime.Parse("2018-03-29 16:55:00.000"), /*Movie={MovieID=3, Name = "Death Wish", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 108, Is3D = true, MainActors = "Bruce Willis", SubActors = "Vincent D'Onofrio, Elisabeth Shue", Director = "Eli Roth", LocationID = 1, Genre = "actie"},*/  MovieID=3,RoomID=3,NumberofTickets=1,ChildDiscount=false,StudentDiscount=true,SeniorDiscount=true},
            });

            Mock<IMovieOverviewRepository> mock2 = new Mock<IMovieOverviewRepository>();
            mock2.Setup(m => m.Movies).Returns(new Movie[] {
            new Movie {MovieID=1, Name = "Darkest Hour", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 120, Is3D = false, MainActors = "Gary Oldman, Lily James", SubActors = "Kristin Scott Thomas", Director = "Joe Wright", LocationID = 1,Genre = "historisch" },
            new Movie {MovieID=2, Name = "Red Sparrow", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 140, Is3D = false, MainActors = "Jennifer Lawerence, Joel Edgerton", SubActors = "Matthias Schoenaerts", Director = "Francis Lawrence", LocationID = 1, Genre = "actie" },
            new Movie {MovieID=3, Name = "Death Wish", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 108, Is3D = true, MainActors = "Bruce Willis", SubActors = "Vincent D'Onofrio, Elisabeth Shue", Director = "Eli Roth", LocationID = 1, Genre = "actie" },
            new Movie {MovieID=4, Name = "Diep in de Zee", Language = "Nederlands", LanguageSub = "Nederlands", Age = 6, MovieType = 2, Length = 91, Is3D = false, MainActors = "Justin Felbinger", SubActors = " ", Director = "Julio Soto Gurpide", LocationID = 1, Genre="kids" },
            });

            UpcomingShowController controller = new UpcomingShowController(MovieRepository, mock.Object);
            

            int daysUntilWednesday = ((int)DayOfWeek.Wednesday - (int)now.DayOfWeek + 7) % 7;
            DateTime nextWednesday = now.AddDays(daysUntilWednesday);
            DateTime minusDateTime = currentDateTime.Add(new TimeSpan(0, -25, 0));

            List<Show> allShows = mock.Object.GetShows().ToList();
            List<Show> tempShowList = new List<Show>();
            //Act
            //Filter out shows from different location
             List<Show> allThislocationShows = allShows.ToEnumerable().Where(s => s.Movie.LocationID == 1).ToList();
            //Filter out shows from the past
            List<Show> ShowsFromNow = allThislocationShows.ToEnumerable()
                .Where(s => s.BeginTime > now).ToList();
            //Order by show date
            List<Show> ShowsFromNowOrderedByDate = ShowsFromNow.ToEnumerable()
                .OrderBy(s => s.BeginTime).ToList();
            //take shows form current movie week
            List<Show> upcomingShows = ShowsFromNowOrderedByDate.ToEnumerable()
                .Where(s => s.EndTime < nextWednesday).ToList();

            foreach (var i in upcomingShows)
            {
                if (i.BeginTime > minusDateTime)
                {
                    tempShowList.Add(i);
                }
            }
            upcomingShows = tempShowList;

            var result = controller.Overview(1, "Bruce", 0, now) as ViewResult;
            List<Show> showResult = (List<Show>)result.ViewData.Model;

            //List<int> IdOfShows = new List<int>
            //{
            //    showResult[0].ShowID,
            //    showResult[1].ShowID,
            //    showResult[2].ShowID
            //};

            //List<int> ExpectedIDsofShows = new List<int>
            //{
            //    3,
            //    1,
            //    2
            //};
            // Assert -- assert if expected equals given by act.
            Assert.AreEqual("Tomb Raider", showResult[0].Movie.Name);
            //Assert.IsInstanceOfType(result.ViewData.Model, typeof(List<Show>));
            Assert.AreEqual("Overview", result.ViewName);
            //Assert.IsTrue(ShowIDIsEqual(IdOfShows, ExpectedIDsofShows));
        }

        //[TestMethod]
        //public void TestFiltersForMovies()
        //{
        //    //arrange
        //    List<Movie> allMovies = movielist;
        //    String searchString = "xxx";
        //    int? age = 0;
        //    DateTime start;
        //    //Act

        //    //----------Filters BEGIN-------------------
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        List<Show> filteredShows = upcomingShows
        //            .Where(s => s.Movie.Name.Contains(searchString)
        //                || s.Movie.MainActors.Contains(searchString)
        //                || s.Movie.Genre.Contains(searchString)
        //                || s.Movie.MainActors.Contains(searchString)
        //                || s.Movie.SubActors.Contains(searchString)
        //                || s.Movie.Director.Contains(searchString))
        //            .ToList();
        //        if (age != null && age > 0)
        //        {
        //            filteredShows.Where(s => s.Movie.Age == age).ToList();

        //        }
        //        if (start.HasValue == true)
        //        {
        //            DateTime selectedDate = (DateTime)start;
        //            filteredShows.Where(s => s.BeginTime.DayOfYear == selectedDate.DayOfYear).ToList();
        //        }
        //    }

        //    //-----------Filters END-------------------


        //    List<int> IdOfFilteredMovies = new List<int>
        //    {
        //        filteredSearchMovies[0].MovieID,  //id=1
        //    };

        //    List<int> IdOfFilteredMoviesonGenre = new List<int>
        //    {
        //        filteredMoviesonGenre[0].MovieID, //id=1
        //        filteredMoviesonGenre[1].MovieID //id=2

        //    };

        //    List<int> IdOfFilteredMoviesonGenre2 = new List<int>
        //    {
        //        filteredMoviesonGenre2[0].MovieID, //id=4
        //        filteredMoviesonGenre2[1].MovieID //id=5

        //    };

        //    List<int> IdOfFilteredMoviesonAge = new List<int>
        //    {
        //        filteredMoviesonAge[0].MovieID, //id=4
        //        filteredMoviesonAge[1].MovieID //id=5
        //    };

        //    List<int> ExpectedIDsofFilteredMovies = new List<int>
        //    {
        //        1
        //    };

        //    List<int> ExpectedIDsofFilteredMoviesonGenre = new List<int>
        //    {
        //        1,
        //        2
        //    };
        //    List<int> ExpectedIDsofFilteredMoviesonGenre2 = new List<int>
        //    {
        //        4,
        //        5
        //    };
        //    List<int> ExpectedIDsofFilteredMoviesonAge = new List<int>
        //    {
        //        4,
        //        5                
        //    };
        //    // Assert -- assert if expected ids of movies equals given by act.
        //    Assert.IsTrue(ShowIDIsEqual(IdOfFilteredMovies, ExpectedIDsofFilteredMovies));
        //    Assert.IsTrue(ShowIDIsEqual(IdOfFilteredMoviesonGenre, ExpectedIDsofFilteredMoviesonGenre));
        //    Assert.IsTrue(ShowIDIsEqual(IdOfFilteredMoviesonGenre2, ExpectedIDsofFilteredMoviesonGenre2));
        //    Assert.IsTrue(ShowIDIsEqual(IdOfFilteredMoviesonAge, ExpectedIDsofFilteredMoviesonAge));
        //}
    }
}
