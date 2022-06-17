using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PharmacyOn.Models;
using PharmacyOn.Data;
using Microsoft.AspNetCore.Http;

namespace PharmacyOn.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult LogIn()
        {
            var model = new User();
            return View(model);
        }

        //HttpPost Action to check if the email and password are valid.
        [HttpPost]
        public IActionResult LogIn(User model)
        {
            var data = _context.Users.Where(s => s.Email.Equals(model.Email) && s.Password.Equals(model.Password)).ToList();
            if (data.Count == 1)
            {
                ViewBag.Message += string.Format("Logged in successfully<br />");

                HttpContext.Session.SetString("UserID", data.FirstOrDefault().ID);

                return RedirectToAction("Main", "Home");
            }
            else
            {
                ViewBag.Message += string.Format("Wrong password or username!<br />");
                return View();
            }
        }

        public IActionResult SignUp()
        {
            var model = new User();
            return View(model);
        }

        [HttpPost]
        public IActionResult SignUp(User model)
        {

            var data = _context.Users.Where(s => s.Email.Equals(model.Email)).ToList();

            if (data.Count > 0)
            {
                ViewBag.Message += string.Format("Email already taken!<br />");
                return View();
            }
            else
            {
                var account = new User { 

                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Username = model.Username,
                    Birthday = model.Birthday.ToString(),
                    PersonalID = model.PersonalID,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Email = model.Email, 
                    Password = model.Password,
                    Weight = model.Weight,
                    Height = model.Height,
                    BloodGroup = model.BloodGroup,
                    Allergies = model.Allergies,
                    MedicalConditions = model.MedicalConditions
                  
                };

                _context.Users.Add(account);
                _context.SaveChanges();
                ViewBag.Message += string.Format("Registered Successfuly!<br />");

                return RedirectToAction("LogIn", "Users");
            }      
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LogIn", "Users");
        }

        public IActionResult MyOrders()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                var data = _context.Orders.Where(s => s.UserID == HttpContext.Session.GetString("UserID")).ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("LogIn", "Users");
            }         
        }

        public IActionResult ConfirmRecieved(int? id)
        {
            var order = _context.Orders.Where(s => s.Id == id).FirstOrDefault();
            if (order != null)
            {
                order.Status = "Delivered";
            }
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("MyOrders", "Users");
        }
    }
}
