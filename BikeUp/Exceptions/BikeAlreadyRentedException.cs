using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeUp.Exceptions
{
    public class BikeAlreadyRentedException : Exception
    {

        public BikeAlreadyRentedException() : base("The bike you are trying to rent is already in use...")
        {
            
        }
    }
}
