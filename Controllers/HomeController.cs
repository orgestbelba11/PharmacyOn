﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PharmacyOn.Data;
using PharmacyOn.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PhamacyOn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Main()
        {
            var data = _context.Products.ToList();
            return View(data);
        }

        public IActionResult ShopSingle(int id)
        {
            var data = _context.Products.Where(s => s.ID == id).ToList();
            return View(data);
        }

        public IActionResult AddToCart(int id)
        {
            var Item = _context.Products.Where(s => s.ID == id).ToList();
            
            var Cart = new ShoppingCart
            {

                UserID = HttpContext.Session.GetString("UserID"),
                ProductName = Item.FirstOrDefault().Name,
                PhotoPath = Item.FirstOrDefault().ImagePath,
                Cost = Item.FirstOrDefault().Price,
                Quantity = 1,
                TotalPrice = Item.FirstOrDefault().Price

            };
            _context.ShoppingCarts.Add(Cart);
            _context.SaveChanges();
            return RedirectToAction("ShopSingle", "Home", new { id });
        }

        public IActionResult Checkout()
        {
            var shoppingCart = _context.ShoppingCarts.Where(s => s.UserID == HttpContext.Session.GetString("UserID")).ToList();
            var user = _context.Users.Where(s => s.ID == HttpContext.Session.GetString("UserID")).ToList();
            dynamic multiplemodels = new ExpandoObject();
            multiplemodels.shoppingCart = shoppingCart;
            multiplemodels.user = user;

            return View(multiplemodels);
        }

        public IActionResult Menu()
        {
            var data = _context.Products.ToList();
            return View(data);
        }

        public IActionResult OrderCompleted()
        {
            var cart = _context.ShoppingCarts.Where(s => s.UserID == HttpContext.Session.GetString("UserID")).ToList();
            var userInfo = _context.Users.Where(s => s.ID == HttpContext.Session.GetString("UserID")).ToList();

            string productsName = "";
            string totalPrices = "";
            string quantities = "";
            string photoPaths = "";

            foreach(var data in cart)
            {
                productsName += (";" + data.ProductName);
                photoPaths += (";" + data.PhotoPath);
                totalPrices += (";" + data.TotalPrice);
                quantities += (";" + data.Quantity);   
            }
            var order = new Order
            {
                UserID = cart.FirstOrDefault().UserID,
                ProductName = productsName,
                Status = "Pending",
                PhotoPath = photoPaths,
                PrescriptionPhotoPath = "",
                Address = userInfo.FirstOrDefault().Address,
                TotalPrice = totalPrices,
                Date = DateTime.Now,

            };
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
