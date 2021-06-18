using BikeUp.DateAux;
using BikeUp.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BikeUp.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string Name { get; set; }

        public double AmountSpent { get; set; }
        public int NrOfRentedBikes { get; set; }
        public int TotalRentingHours { get; set; }
        public string Phone { get; set; }

        [ForeignKey("Bike")]
        public int? BikeId { get; set; }
        public Bike Bike { get; set; }

        public Customer()
        {
        }

        public Customer(string name)
        {
            Name = name;
        }

        public Customer(string Name, string Phone)
        {
            this.Name = Name;
            this.Phone = Phone;
        }

        public void AddBike(Bike bike)
        {
            this.BikeId = bike.BikeId;
            this.Bike = bike;
            bike.RentDate = DateTime.Now;
            NrOfRentedBikes++;
        }

        public void ReturnBike(Bike bike)
        {
            TimeSpan ts = (TimeSpan)(DateTime.Now - bike.RentDate);
            this.TotalRentingHours += (int)Math.Ceiling(ts.TotalHours);

            Console.WriteLine(FileManager.ReadHourlyRates()[0]);
            bike.SetHourlyRate();
            Console.WriteLine(bike.HourlyRate);
            this.AmountSpent += this.TotalRentingHours * bike.HourlyRate;
            this.AmountSpent = Convert.ToDouble(this.AmountSpent.ToString("N2"));

            Bike = null;
            BikeId = null;
        }

        public override string ToString()
        {
            return $"Name:{Name} // Phone:{Phone}";
        }
    }
}
