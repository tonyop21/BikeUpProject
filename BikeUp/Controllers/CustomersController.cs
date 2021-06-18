using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BikeUp.Data;
using BikeUp.Models;

namespace BikeUp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly BikeUpContext _context;

        public CustomersController(BikeUpContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string searchString)
        {
            var customers = from c in _context.Customers select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.Name.Contains(searchString));
            }
            return View(await customers.Include(c => c.Bike).ToListAsync());
        }

        public IActionResult RedirectToIndex()
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Bike)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["BikeId"] = new SelectList(_context.Bikes, "BikeId", "BikeId");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,Name,Phone,BikeId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BikeId"] = new SelectList(_context.Bikes, "BikeId", "BikeId", customer.BikeId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["BikeId"] = new SelectList(_context.Bikes, "BikeId", "BikeId", customer.BikeId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,Name,Phone,BikeId")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            ViewData["BikeId"] = new SelectList(_context.Bikes, "BikeId", "BikeId", customer.BikeId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Bike)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }




        [HttpGet]
        public IActionResult ReturnBikeByCustomer(int? id)
        {
            

            var customer = _context.Bikes.Find(id);
            
            _context.ReturnBike(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RentBikeByCustomer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Bike)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> RentBikeByCustomer(int id)
        {

            var customer = await _context.Customers
                .Include(c => c.Bike)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(Request.Form["Type"].ToString()))
            {
                return RedirectToAction(nameof(NoBikeTypeSelectedError), customer);
            }

            _context.RentBike(id, Request.Form["Type"].ToString());

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> NoBikeTypeSelectedError(int id)
        {
            //var customer = await _context.Customers
            //    .Include(c => c.Bike)
            //    .FirstOrDefaultAsync(m => m.CustomerId == id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BackToRent(int id)
        {
            //var customer = await _context.Customers
            //    .Include(c => c.Bike)
            //    .FirstOrDefaultAsync(m => m.CustomerId == id);
            return RedirectToAction(nameof(Index)/*, customer.CustomerId*/);
        }
    }
}
