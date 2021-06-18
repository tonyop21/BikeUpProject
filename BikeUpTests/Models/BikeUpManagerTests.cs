using Microsoft.VisualStudio.TestTools.UnitTesting;
using BikeUp.Factories;
using BikeUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeUp.Models.Tests
{
    [TestClass()]
    public class BikeUpManagerTests
    {
        [TestMethod()]
        public void RentBikeTest()
        {

            BikeUpManager manager = BikeUpManager.GetInstance();

            Customer customer1 = new Customer()
            {
                Name = "Johnny",
                Phone = "294332441"
            };

            Bike bike1 = BikeFactory.GetInstance(100, "Gas");

            manager.RentBike(customer1, bike1);

            Assert.AreEqual(customer1.Bike, bike1);
        }

        [TestMethod()]
        public void RentBikeTestNoRental()
        {
            BikeUpManager Manager = BikeUpManager.GetInstance();

            Customer customer1 = new Customer()
            {
                Name = "Paulo Nunes",
                Phone = "+351 923456789"
            };

            Customer customer2 = new Customer()
            {
                Name = "Paula Esteves",
                Phone = "+351 924456789"
            };

            Bike bike1 = BikeFactory.GetInstance(10, "Gas");

            Manager.RentBike(customer1, bike1);

            Assert.ThrowsException<BikeUp.Exceptions.BikeAlreadyRentedException>(() => Manager.RentBike(customer2, bike1));
        }
    }
}