using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeUp.DateAux
{
    public class Date
    {

        public static DateTime Round(DateTime dateTime)
        {
            var updated = dateTime.AddMinutes(30);
            return new DateTime(updated.Year, updated.Month, updated.Day,
                                 updated.Hour, 0, 0, dateTime.Kind);
        }

        private Date()
        {

        }
    }
}
