using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BikeUp.Data;
using BikeUp.Models;
using BikeUp.Factories;
using Microsoft.AspNetCore.Http;
using BikeUp.Exceptions;

namespace BikeUp.Controllers
{
    public class BikesController : Controller
    {
        private readonly BikeUpContext _context;

        public BikesController(BikeUpContext context)
        {
            _context = context;
        }

        // GET DashBoard
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.CustomersByHours = await (from cust in _context.Customers
                                              orderby cust.TotalRentingHours descending
                                              select cust).Take(3).ToListAsync();

            ViewBag.CustomersByNrOfBikes = await (from cust in _context.Customers
                                                  orderby cust.NrOfRentedBikes descending
                                                  select cust).Take(3).ToListAsync();

            ViewBag.CustomersByAmountSpent = await (from cust in _context.Customers
                                                    orderby cust.AmountSpent descending
                                                    select cust).Take(3).ToListAsync();

            ViewBag.MostRentedBikes = await (from bike in _context.Bikes
                                             orderby bike.TimesRented descending
                                             select bike).Take(3).ToListAsync();
            return View();
        }



        // GET: Bikes
        public async Task<IActionResult> Index(string available = "", string type = "")
        {
            
            ViewBag.Bikes = new List<Bike>();
            List<int> bikeIds = new List<int>();
            List<Bike> bikes = new List<Bike>();


            foreach (Bike b in _context.Bikes)
            {
                if ((string.IsNullOrWhiteSpace(available) || b.IsAvailable == Convert.ToBoolean(available)) && (string.IsNullOrWhiteSpace(type) || b.Type == type))
                {
                    ViewBag.Bikes.Add(b);
                }
            }

            return View(await _context.Bikes.ToListAsync());
        }

        public IActionResult RedirectToIndex()
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: Bikes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            

            var bike = await _context.Bikes
                .FirstOrDefaultAsync(m => m.BikeId == id);
            if (bike == null)
            {
                return NotFound();
            }
            ViewBag.Bike = bike;

            return View(bike);
        }

        // GET: Bikes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bikes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BikeId,RentDate,Capacity,Type,IsAvailable")] Bike bike)
        {
            if (ModelState.IsValid)
            {
                bike = BikeFactory.GetInstance(bike.Capacity, bike.Type);
                _context.Add(bike);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bike);
        }

        // GET: Bikes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null)
            {
                return NotFound();
            }
            return View(bike);
        }

        // POST: Bikes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BikeId,RentDate,Capacity,Type,IsAvailable,TimesRented")] Bike bike)
        {
            if (id != bike.BikeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bike = _context.SearchBikeById(id);
                    bike.Capacity = Convert.ToDouble(Request.Form["capacity"]);
                    
                    
                    _context.Update(bike);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BikeExists(bike.BikeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bike);
        }

        // GET: Bikes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bike = await _context.Bikes
                .FirstOrDefaultAsync(m => m.BikeId == id);
            if (bike == null)
            {
                return NotFound();
            }
            ViewBag.Bike = bike;

            return View(bike);
        }

        // POST: Bikes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);
            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BikeExists(int id)
        {
            return _context.Bikes.Any(e => e.BikeId == id);
        }

        [HttpGet]
        public IActionResult RentBike()
        {
            List<Customer> availableCustomers = new List<Customer>();

            foreach (Customer c in _context.Customers)
            {
                if (c.BikeId == null)
                {
                    availableCustomers.Add(c);
                }
            }
            ViewData["Customers"] =
                DropDownList<Customer>.LoadItems(
                    availableCustomers, "CustomerId", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult RentBike(IFormCollection form)
        {
            //_context.SetManager(BikeUpManager.GetInstance());

            if (Request.Form["CustomerId"].Count == 0 || string.IsNullOrWhiteSpace(Request.Form["CustomerId"]))
            {
                return RedirectToAction(nameof(NoCustomerSelectedError));
            }
            if (Request.Form["Type"].Count == 0 || string.IsNullOrWhiteSpace(Request.Form["Type"]))
            {
                return RedirectToAction(nameof(NoBikeTypeSelectedError));
            }
            string bikeType = Request.Form["Type"].ToString();
            int customerId = Convert.ToInt32(Request.Form["CustomerId"].ToString());
            try
            {
                _context.RentBike(customerId, bikeType);
            }
            catch (BikeAlreadyRentedException e)
            {
                ViewBag.Exception = e;
                return RedirectToAction(nameof(BikeAlreadyRented));
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public IActionResult BikeAlreadyRented()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NoBikeTypeSelectedError()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BackToReturn()
        {
            return RedirectToAction(nameof(ReturnBike));
        }

        [HttpGet]
        public IActionResult NoCustomerSelectedError()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BackToRent()
        {
            return RedirectToAction(nameof(RentBike));
        }

        [HttpGet]
        public IActionResult ReturnBike()
        {
            List<Bike> unavailableBikes = new List<Bike>();

            foreach (Bike b in _context.Bikes)
            {
                if (!b.IsAvailable)
                {
                    unavailableBikes.Add(b);
                }
            }

            ViewData["Bikes"] =
                DropDownList<Bike>.LoadItems(
                    unavailableBikes, "BikeId", "BikeId");
            return View();
        }

        [HttpGet]
        public IActionResult NoBikeIdSelectedError()
        {

            return View();
        }
        [HttpPost]
        public IActionResult ReturnBike(IFormCollection form)
        {
            //_context.SetManager(BikeUpManager.GetInstance());
            if (string.IsNullOrWhiteSpace(Request.Form["BikeId"]))
            {
                return RedirectToAction(nameof(NoBikeIdSelectedError));
            }

            int bikeId = Convert.ToInt32(Request.Form["BikeId"].ToString());

            _context.ReturnBike(bikeId);

            return RedirectToAction(nameof(Dashboard));
        }


    }
}
