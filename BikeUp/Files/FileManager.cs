using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BikeUp.Files
{
    public class FileManager
    {


        public static double[] ReadHourlyRates()
        {
            
            double[] hourlyRates = new double[10];
            int counter = 0;
            using (StreamReader sr = new StreamReader(Path.GetFullPath("Files/hourlyRates.txt")))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    hourlyRates[counter] = Convert.ToDouble(line);
                    counter++;
                }
            }
            //O index 0 é o hourlyRate das eléctricas e o 1 é das gas
            return hourlyRates;
        }
    }
}
