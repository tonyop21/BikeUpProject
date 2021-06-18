using BikeUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeUp.Factories
{
    public class BikeFactory
    {

        public static Bike GetInstance(double capacity, string type)
        {

            switch (type)
            {
                case "Electric":
                    {
                        return new ElectricBike(capacity);
                    }
                case "Gas":
                    {
                        return new GasBike(capacity);
                    }
                default:
                    return null;
            }
        }
    }
}
