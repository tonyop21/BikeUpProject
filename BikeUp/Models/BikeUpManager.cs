using BikeUp.Factories;
using BikeUp.Files;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeUp.Models
{
    public class BikeUpManager
    {

        private FileManager fileManager = new FileManager();

        private BikeUpManager()
        {
        }

        private static BikeUpManager _instance;

        public static BikeUpManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new BikeUpManager();
                
            }

            return _instance;
        }
        public Customer CreateCustomer(string name)
        {
            return new Customer(name);
        }

        public Bike CreateBike(int capacity, string type)
        {
            return BikeFactory.GetInstance(capacity, type);
        }

        public void RentBike(Customer customer, Bike bike)
        {
            customer.AddBike(bike);
            bike.RentBike(customer);
        }

        public void ReturnBike(Bike bike, Customer customer)
        {
            customer.ReturnBike(bike);
            bike.ReturnBike();
        }

        public void ReadAndSetHourlyRates(DbSet<Bike> bikes)
        {
            double[] hourlyRates = FileManager.ReadHourlyRates();

            foreach (Bike bike in bikes)
            {
                if (bike is ElectricBike)
                {
                    bike.HourlyRate = hourlyRates[0];
                } 
                else if(bike is GasBike)
                {
                    bike.HourlyRate = hourlyRates[1];
                }
            }
        }
    }
}
