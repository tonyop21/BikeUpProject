using BikeUp.Exceptions;
using BikeUp.Factories;
using BikeUp.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BikeUp.Models
{
    public class Bike
    {

        [Key]
        public int BikeId { get; set; }
        public DateTime? RentDate { get; set; } 

        [Required]
        public double Capacity { get; set; }

        public string Type { get; set; }
        [NotMapped]
        public double HourlyRate { get; set; }
        public bool IsAvailable { get; set; }
        public Customer Customer { get; set; }

        public int TimesRented { get; set; }


        public Bike()
        {
            IsAvailable = true;
        }

        public Bike(double Capacity)
        {
            IsAvailable = true;
        }

        public void RentBike(Customer customer)
        {
            if (!this.IsAvailable)
            {
                throw new BikeAlreadyRentedException();
            }
            this.Customer = customer;
            this.RentDate = DateTime.Now;
            this.IsAvailable = false;
            this.TimesRented++;
        }

        public void ReturnBike()
        {
            Customer = null;
            IsAvailable = true;
            RentDate = null;
        }

        public void SetHourlyRate()
        {
            double[] hourlyRates = FileManager.ReadHourlyRates();
            this.HourlyRate = this.Type == "Electric" ? hourlyRates[0] : hourlyRates[1]; 
        }

    }

    public class ElectricBike : Bike
    {

        public ElectricBike(double capacity) : base(capacity)
        {
            this.Type = "Electric";
            this.Capacity = 100;
            this.HourlyRate = FileManager.ReadHourlyRates()[0];
        }

        public override string ToString()
        {
            return $"Electric Bike: Capacity={base.Capacity}kWh";
        }

    }

    public class GasBike : Bike
    {
        public GasBike(double capacity) : base(capacity)
        {
            this.Type = "Gas";
            this.Capacity = 300;
            this.HourlyRate = FileManager.ReadHourlyRates()[1];
        }

        public override string ToString()
        {
            return $"Gas Bike: Capacity={base.Capacity}L";
        }

    }
}


