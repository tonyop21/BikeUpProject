using BikeUp.Factories;
using BikeUp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeUp.Data
{
    public class BikeUpContext : DbContext
    {

        public BikeUpManager Manager { get; set; }

        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public BikeUpContext(DbContextOptions<BikeUpContext> options) : base(options)
        {
            this.Manager = BikeUpManager.GetInstance();
        }

        public void SetManager(BikeUpManager manager)
        {
            Manager = manager;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = BikeUp.db");
        }

        public Customer CreateCustomer(string name)
        {
            Customer customer = Manager.CreateCustomer(name);
            Customers.Add(customer);
            SaveChanges();
            return customer;
        }
        public Bike CreateBike(int capacity, string type)
        {
            Bike bike = Manager.CreateBike(capacity, type);
            Bikes.Add(bike);
            SaveChanges();
            return bike;
        }

        public void ReadAndSetHourlyRates()
        {
            Manager.ReadAndSetHourlyRates(Bikes);
        }

        public void RentBike(int customerId, string bikeType)
        {
            Customer customer = SearchCustomerById(customerId);
            Bike bike = SearchBikeByType(bikeType) == null ? BikeFactory.GetInstance(100, bikeType) : SearchBikeByType(bikeType);
            Manager.RentBike(customer, bike);
            SaveChanges();
        }

        public Bike SearchBikeByType(string type)
        {
            foreach(Bike bike in Bikes)
            {
                if (bike.Type == type && bike.IsAvailable)
                {
                    return bike;
                }
            }
            return null;
        }

        //Return bike using bike id
        public void ReturnBike(int id)
        {
            Bike bike = SearchBikeById(id);
            Customer customer = SearchCustomerByBikeId(id);
            Manager.ReturnBike(bike, customer);
            SaveChanges();
        }

        //Return bike using customer id
        public void ReturnBike(int? id)
        {
            Customer customer = SearchCustomerById((int)id);
            Bike bike = SearchBikeById((int)customer.BikeId);
            Manager.ReturnBike(bike, customer);
            SaveChanges();
        }

        private Customer SearchCustomerByBikeId(int id)
        {
            Customer customer = null;

            foreach (Customer c in Customers)
            {
                if (c.BikeId == id)
                {
                    customer = c;
                    break;
                }
            }
            return customer;
        }

        public Customer SearchCustomerById(int id)
        {
            Customer customer = null;

            foreach(Customer c in Customers)
            {
                if (c.CustomerId == id)
                {
                    customer = c;
                    break;
                }
            }
            return customer;
        }
        public Bike SearchBikeById(int id)
        {
            return Bikes.Find(id);
        }

        public void ClearData()
        {
            Customers.RemoveRange(Customers);
            Bikes.RemoveRange(Bikes);
            SaveChanges();
        }

        public List<Bike> GetAvailableBikes()
        {
            List<Bike> availableBikes = new List<Bike>();

            foreach (Bike bike in Bikes)
            {
                if (bike.IsAvailable)
                {
                    availableBikes.Add(bike);
                }

            }

            return availableBikes;
        }

        public void PrintCustomers()
        {
            foreach (Customer c in Customers)
            {
                Console.WriteLine(c.Name);
            }
        }
        public void PrintBikes()
        {
            foreach (Bike b in Bikes)
            {
                Console.WriteLine(b.BikeId);
            }
        }


    }
}
