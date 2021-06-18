using BikeUp.Data;
using BikeUp.Models;
using BikeUp.Factories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeUp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();


            

            //BikeUpManager manager = BikeUpManager.GetInstance();
            //BikeUpContext context = new BikeUpContext();
            //context.Manager = manager;
            //context.ClearData();
            //Customer customer1 = context.CreateCustomer("Ruca");
            //context.PrintCustomers();
            //Bike bike1 = context.CreateBike(100, BikeType.Electric);
            //context.PrintBikes();

            //context.RentBike(customer1.CustomerId, bike1.BikeId);

            //Console.WriteLine(customer1.BikeId);
            //Console.WriteLine(customer1.Bike.GetType());
            //Console.WriteLine(bike1.IsAvailable);

            //context.ReturnBike(bike1.BikeId);

            //Console.WriteLine(customer1.BikeId==null? "isnull":"has a value");
            //Console.WriteLine(customer1.Bike == null ? "isnull" : "has a value");
            //Console.WriteLine(bike1.IsAvailable);
            //context.ReadAndSetHourlyRates();
            //Console.WriteLine(bike1.HourlyRate);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
