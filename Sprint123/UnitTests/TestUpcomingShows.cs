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
        DateTime now = DateTime.Now;
        DateTime showTimeBegin = DateTime.Now.Date + new TimeSpan(20,40,00);
        DateTime showTimeEnd = DateTime.Now.Date + new TimeSpan(21, 10, 00);

        DateTime showTime2Begin = DateTime.Now.Date + new TimeSpan(20, 25, 00);
        DateTime showTime2End = DateTime.Now.Date + new TimeSpan(21, 25, 00);

        DateTime showTime3Begin = DateTime.Now.Date + new TimeSpan(20, 35, 00);
        DateTime showTime3End = DateTime.Now.Date + new TimeSpan(21, 45, 00);

        //DateTime EndOfDay = DateTime.Today.AddDays(1) + new TimeSpan(02, 00, 00);

        [TestMethod]
        public void TestForUpcomingShows() {
            //arrange
            string showBegin = showTimeBegin.ToString();
            string showEnd = showTimeEnd.ToString();

            string show2Begin = showTime2Begin.ToString();
            string show2End = showTime2End.ToString();

            string show3Begin = showTime3Begin.ToString();
            string show3End = showTime3End.ToString();


            Mock<IShowRepository> mock = new Mock<IShowRepository>();
            mock.Setup(m => m.GetShows()).Returns(new Show[]{
            new Show{ShowID=1,BeginTime=DateTime.Parse(showBegin),EndTime=DateTime.Parse(showEnd), MovieID=4,RoomID=2,NumberofTickets=11}, //3
            new Show{ShowID=2,BeginTime=DateTime.Parse(show2Begin),EndTime=DateTime.Parse(show2End), MovieID=1,RoomID=1,NumberofTickets=10}, //1
            new Show{ShowID=3,BeginTime=DateTime.Parse(show3Begin),EndTime=DateTime.Parse(show3End), MovieID=2,RoomID=2,NumberofTickets=31}, //2
            new Show{ShowID=4,BeginTime=DateTime.Parse("2018-03-29 14:45:00.000"),EndTime=DateTime.Parse("2018-03-29 16:55:00.000"), MovieID=3,RoomID=3,NumberofTickets=25},
            new Show{ShowID=4,BeginTime=DateTime.Parse("2018-06-24 11:45:00.000"),EndTime=DateTime.Parse("2018-06-24 13:55:00.000"), MovieID=2,RoomID=4,NumberofTickets=0},
            });

            Mock<IMovieOverviewRepository> mock2 = new Mock<IMovieOverviewRepository>();
            mock2.Setup(m => m.Movies).Returns(new Movie[] {
            new Movie {MovieID=1, Name = "Darkest Hour", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 120, Is3D = false, MainActors = "Gary Oldman, Lily James", SubActors = "Kristin Scott Thomas", Director = "Joe Wright", LocationID = 1,Genre = "historisch" },
            new Movie {MovieID=2, Name = "Red Sparrow", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 140, Is3D = false, MainActors = "Jennifer Lawerence, Joel Edgerton", SubActors = "Matthias Schoenaerts", Director = "Francis Lawrence", LocationID = 1, Genre = "actie" },
            new Movie {MovieID=3, Name = "Death Wish", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 108, Is3D = true, MainActors = "Bruce Willis", SubActors = "Vincent D'Onofrio, Elisabeth Shue", Director = "Eli Roth", LocationID = 1, Genre = "actie" },
            new Movie {MovieID=4, Name = "Diep in de Zee", Language = "Nederlands", LanguageSub = "Nederlands", Age = 6, MovieType = 2, Length = 91, Is3D = false, MainActors = "Justin Felbinger", SubActors = " ", Director = "Julio Soto Gurpide", LocationID = 1, Genre="kids" },
            });

            UpcomingShowController controller = new UpcomingShowController(mock2.Object, mock.Object);

            var result = controller.Overview() as ViewResult;
            List<Show> showResult = (List<Show>)result.ViewData.Model;

            // Assert -- assert if expected equals given by act.
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(List<Show>));
            Assert.AreEqual("2", showResult[0].ShowID.ToString());
            Assert.AreEqual("3", showResult[1].ShowID.ToString());
            Assert.AreEqual("1", showResult[2].ShowID.ToString());
            //Assert.ThrowsException<ArgumentOutOfRangeException>(() => (showResult[3].ShowID));

        }

        [TestMethod]
        public void TestDetailPageForMovies()
        {
            string showBegin = showTimeBegin.ToString();
            string showEnd = showTimeEnd.ToString();

            string show2Begin = showTime2Begin.ToString();
            string show2End = showTime2End.ToString();

            string show3Begin = showTime3Begin.ToString();
            string show3End = showTime3End.ToString();


            Mock<IShowRepository> mock = new Mock<IShowRepository>();
            mock.Setup(m => m.GetShows()).Returns(new Show[]{
            new Show{ShowID=1,BeginTime=DateTime.Parse(showBegin),EndTime=DateTime.Parse(showEnd), MovieID=4,RoomID=2,NumberofTickets=11}, //3
            new Show{ShowID=2,BeginTime=DateTime.Parse(show2Begin),EndTime=DateTime.Parse(show2End), MovieID=1,RoomID=1,NumberofTickets=10}, //1
            new Show{ShowID=3,BeginTime=DateTime.Parse(show3Begin),EndTime=DateTime.Parse(show3End), MovieID=2,RoomID=2,NumberofTickets=31}, //2
            new Show{ShowID=4,BeginTime=DateTime.Parse("2018-03-29 14:45:00.000"),EndTime=DateTime.Parse("2018-03-29 16:55:00.000"), MovieID=3,RoomID=3,NumberofTickets=25},
            new Show{ShowID=4,BeginTime=DateTime.Parse("2018-06-24 11:45:00.000"),EndTime=DateTime.Parse("2018-06-24 13:55:00.000"), MovieID=2,RoomID=4,NumberofTickets=0},
            });

            Mock<IMovieOverviewRepository> mock2 = new Mock<IMovieOverviewRepository>();
            mock2.Setup(m => m.Movies).Returns(new Movie[] {
            new Movie {MovieID=1, Name = "Darkest Hour", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 120, Is3D = false, MainActors = "Gary Oldman, Lily James", SubActors = "Kristin Scott Thomas", Director = "Joe Wright", LocationID = 1,Genre = "historisch" },
            new Movie {MovieID=2, Name = "Red Sparrow", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 140, Is3D = false, MainActors = "Jennifer Lawerence, Joel Edgerton", SubActors = "Matthias Schoenaerts", Director = "Francis Lawrence", LocationID = 1, Genre = "actie" },
            new Movie {MovieID=3, Name = "Death Wish", Language = "Engels", LanguageSub = "Nederlands", Age = 16, MovieType = 2, Length = 108, Is3D = true, MainActors = "Bruce Willis", SubActors = "Vincent D'Onofrio, Elisabeth Shue", Director = "Eli Roth", LocationID = 1, Genre = "actie" },
            new Movie {MovieID=4, Name = "Diep in de Zee", Language = "Nederlands", LanguageSub = "Nederlands", Age = 6, MovieType = 2, Length = 91, Is3D = false, MainActors = "Justin Felbinger", SubActors = " ", Director = "Julio Soto Gurpide", LocationID = 1, Genre="kids" },
            });

            //arrange
            int showid = 1;
            int showid2 = 2;
            //Act
            UpcomingShowController controller2 = new UpcomingShowController(mock2.Object, mock.Object);
            var result = controller2.ShowDetails(showid) as ViewResult;
            Show show = (Show)result.ViewData.Model;
            var result2 = controller2.ShowDetails(showid2) as ViewResult;
            Show show2 = (Show)result2.ViewData.Model;
            // Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Show));
            Assert.AreEqual("1", show.ShowID.ToString());
            Assert.AreEqual("2", show2.ShowID.ToString());
        }
    }
}
