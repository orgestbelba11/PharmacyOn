using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PharmacyOn.Data;
using PharmacyOn.Models;

namespace PharmacyOn.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ShoppingCartsController> _logger;

        public ShoppingCartsController(ApplicationDbContext context, ILogger<ShoppingCartsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ShoppingCarts
        public IActionResult Index()
        {
            var data = _context.ShoppingCarts.Where(s => s.UserID == HttpContext.Session.GetString("UserID")).ToList();

            return View(data);
        }

        public IActionResult IncreaseQuantity(int? id)
        {
            var Item = _context.ShoppingCarts.Where(s => s.Id == id).FirstOrDefault();

            int quantity = Item.Quantity + 1;

            if(Item != null)
            {
                Item.Quantity = quantity;
                Item.TotalPrice = Item.Cost * quantity;
            }

            _context.Entry(Item).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index", "ShoppingCarts");
        }

        public IActionResult DecreaseQuantity(int? id)
        {
            var Item = _context.ShoppingCarts.Where(s => s.Id == id).FirstOrDefault();

            int quantity = Item.Quantity - 1;

            if (quantity < 1)
            {
                return RedirectToAction("Index", "ShoppingCarts");
            }
            else
            {
                if (Item != null)
                {
                    Item.Quantity = quantity;
                    Item.TotalPrice = Item.Cost * quantity;
                }
                _context.Entry(Item).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index", "ShoppingCarts");
            }

            
        }

        // GET: ShoppingCarts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserID,ProductName,PhotoPath,Cost,Quantity,TotalPrice")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserID,ProductName,PhotoPath,Cost,Quantity,TotalPrice")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.Id))
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
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
            _context.ShoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCarts.Any(e => e.Id == id);
        }
    }
}
