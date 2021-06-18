using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeUp.Controllers
{
    public static class DropDownList<T>
    {
        public static SelectList LoadItems(IList<T> collection, string value, string text)
        {
            return new SelectList(collection, value, text);
        }
    }
}
