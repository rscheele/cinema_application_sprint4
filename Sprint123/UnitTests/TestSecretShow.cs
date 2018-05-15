using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestSecretShow
    {
        [TestMethod]
        public void TestSecretShowDiscount()
        {
            //Arrange
            bool Secret = true;
            decimal Price = 12.00M;
            //Act
            if(Secret == true)
            {
                Price = Price - 2.50M;
            }
            //Assert
            Assert.AreEqual(Price, 9.50M);
        }
    }
}
